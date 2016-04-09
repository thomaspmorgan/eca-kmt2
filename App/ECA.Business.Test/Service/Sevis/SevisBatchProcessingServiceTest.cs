﻿using ECA.Business.Queries.Models.Admin;
using FluentAssertions;
using ECA.Business.Queries.Sevis;
using ECA.Business.Service;
using ECA.Business.Service.Admin;
using ECA.Business.Service.Persons;
using ECA.Business.Service.Sevis;
using ECA.Business.Validation.Sevis;
using ECA.Business.Validation.Sevis.Bio;
using ECA.Data;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using ECA.Business.Queries.Models.Sevis;
using ECA.Core.Exceptions;
using FluentValidation;
using FluentValidation.Results;
using ECA.Business.Sevis.Model.TransLog;
using System.IO;
using System.Xml.Serialization;
using ECA.Business.Sevis.Model;
using Newtonsoft.Json;
using System.Reflection;
using ECA.Core.Settings;
using System.Collections.Specialized;
using System.Configuration;

namespace ECA.Business.Test.Service.Sevis
{
    [TestClass]
    public class SevisBatchProcessingServiceTest
    {
        private TestEcaContext context;
        private SevisBatchProcessingService service;
        private Mock<IDS2019FileProvider> fileProvider;
        private Mock<IDummyCloudStorage> cloudStorageService;
        private Mock<IExchangeVisitorService> exchangeVisitorService;
        private Mock<ISevisBatchProcessingNotificationService> notificationService;
        private Mock<IExchangeVisitorValidationService> exchangeVisitorValidationService;
        private Mock<AbstractValidator<ExchangeVisitor>> validator;
        private int maxCreateExchangeVisitorBatchSize = 10;
        private int maxUpdateExchangeVisitorBatchSize = 10;
        private double numberOfDaysToKeep = 1.0;
        private NameValueCollection appSettings;
        private ConnectionStringSettingsCollection connectionStrings;
        private AppSettings settings;

        [TestInitialize]
        public void TestInit()
        {
            appSettings = new NameValueCollection();
            connectionStrings = new ConnectionStringSettingsCollection();
            settings = new AppSettings(appSettings, connectionStrings);

            appSettings.Add(AppSettings.NUMBER_OF_DAYS_TO_KEEP_PROCESSED_SEVIS_BATCH_RECORDS, numberOfDaysToKeep.ToString());

            context = new TestEcaContext();
            exchangeVisitorService = new Mock<IExchangeVisitorService>();
            notificationService = new Mock<ISevisBatchProcessingNotificationService>();
            exchangeVisitorValidationService = new Mock<IExchangeVisitorValidationService>();
            validator = new Mock<AbstractValidator<ExchangeVisitor>>();
            cloudStorageService = new Mock<IDummyCloudStorage>();
            fileProvider = new Mock<IDS2019FileProvider>();

            exchangeVisitorValidationService.Setup(x => x.GetValidator()).Returns(validator.Object);
            service = new SevisBatchProcessingService(
                context: context,
                appSettings: settings,
                cloudStorageService: cloudStorageService.Object,
                exchangeVisitorService: exchangeVisitorService.Object,
                notificationService: notificationService.Object,
                exchangeVisitorValidationService: exchangeVisitorValidationService.Object,
                maxCreateExchangeVisitorRecordsPerBatch: maxCreateExchangeVisitorBatchSize,
                maxUpdateExchangeVisitorRecordsPerBatch: maxUpdateExchangeVisitorBatchSize);
        }

        private Business.Validation.Sevis.Bio.Person GetPerson(int personId, int participantId)
        {
            var state = "TN";
            var mailAddress = new AddressDTO();
            mailAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            mailAddress.Division = state;

            var usAddress = new AddressDTO();
            usAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            usAddress.Division = state;

            var firstName = "first";
            var lastName = "last";
            var passport = "passport";
            var preferred = "preferred";
            var suffix = "Jr.";
            var fullName = new FullName(firstName, lastName, passport, preferred, suffix);

            var birthCity = "birth city";
            var birthCountryCode = "CN";
            var birthDate = DateTime.UtcNow;
            var citizenshipCountryCode = "FR";
            var email = "someone@isp.com";
            var gender = Gender.SEVIS_MALE_GENDER_CODE_VALUE;
            var permanentResidenceCountryCode = "MX";
            var phone = "123-456-7890";
            short positionCode = 120;
            var printForm = true;
            var BirthCountryReasonId = BirthCountryReason.BornToForeignDiplomat.Id;
            var remarks = "remarks";
            var programCataegoryCode = "1D";

            var subjectFieldCode = "01.0102";
            var subjectField = new SubjectField(subjectFieldCode, null, null, null);

            var person = new Business.Validation.Sevis.Bio.Person(
                fullName,
                birthCity,
                birthCountryCode,
                birthDate,
                citizenshipCountryCode,
                email,
                gender,
                permanentResidenceCountryCode,
                phone,
                remarks,
                positionCode.ToString(),
                programCataegoryCode,
                subjectField,
                mailAddress,
                usAddress,
                printForm,
                personId,
                participantId);
            return person;
        }

        private string GetXml(TransactionLogType transactionLog)
        {
            using (var textWriter = new StringWriter())
            {
                var serializer = new XmlSerializer(typeof(TransactionLogType));
                serializer.Serialize(textWriter, transactionLog);
                var xml = textWriter.ToString();
                return xml;
            }
        }

        #region Constructor
        [TestMethod]
        public void TestConstructor()
        {
            var instance = new SevisBatchProcessingService(
                context: context,
                appSettings: settings,
                cloudStorageService: cloudStorageService.Object,
                exchangeVisitorService: exchangeVisitorService.Object,
                notificationService: notificationService.Object,
                exchangeVisitorValidationService: exchangeVisitorValidationService.Object,
                maxCreateExchangeVisitorRecordsPerBatch: maxCreateExchangeVisitorBatchSize,
                maxUpdateExchangeVisitorRecordsPerBatch: maxUpdateExchangeVisitorBatchSize);

            Assert.AreEqual(maxCreateExchangeVisitorBatchSize, instance.MaxCreateExchangeVisitorRecordsPerBatch);
            Assert.AreEqual(maxUpdateExchangeVisitorBatchSize, instance.MaxUpdateExchangeVisitorRecordsPerBatch);
        }

        [TestMethod]
        public void TestConstructor_CheckDefaults()
        {
            var instance = new SevisBatchProcessingService(
                context: context,
                appSettings: settings,
                cloudStorageService: cloudStorageService.Object,
                exchangeVisitorService: exchangeVisitorService.Object,
                notificationService: notificationService.Object,
                exchangeVisitorValidationService: exchangeVisitorValidationService.Object);

            Assert.AreEqual(StagedSevisBatch.MAX_CREATE_EXCHANGE_VISITOR_RECORDS_PER_BATCH_DEFAULT, instance.MaxCreateExchangeVisitorRecordsPerBatch);
            Assert.AreEqual(StagedSevisBatch.MAX_UPDATE_EXCHANGE_VISITOR_RECORD_PER_BATCH_DEFAULT, instance.MaxUpdateExchangeVisitorRecordsPerBatch);
        }
        #endregion

        #region Staging
        [TestMethod]
        public async Task TestStageBatches_OneExchangeVisitor_DoesNotHaveSevisId()
        {
            var personId = 10;
            var participantId = 1;
            var projectId = 2;
            Participant participant = null;
            ParticipantPerson participantPerson = null;
            var status = new ParticipantPersonSevisCommStatus
            {
                Id = 1,
                SevisCommStatusId = SevisCommStatus.QueuedToSubmit.Id,
                AddedOn = DateTime.UtcNow.AddDays(-1.0),
                SevisUsername = "sevis username",
                SevisOrgId = "sevis org id"
            };

            var siteOfActivity = new AddressDTO
            {
                Division = "DC",
                LocationName = "name"
            };
            var exchangeVisitor = new ExchangeVisitor(
                sevisId: null,
                person: GetPerson(personId, participantId),
                financialInfo: new Business.Validation.Sevis.Finance.FinancialInfo(true, true, null, null),
                occupationCategoryCode: "99",
                programEndDate: DateTime.Now,
                programStartDate: DateTime.Now,
                dependents: new List<Business.Validation.Sevis.Bio.Dependent>(),
                siteOfActivity: siteOfActivity);
            exchangeVisitorService.Setup(x => x.GetExchangeVisitor(It.IsAny<int>(), It.IsAny<int>())).Returns(exchangeVisitor);
            exchangeVisitorService.Setup(x => x.GetExchangeVisitorAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(exchangeVisitor);
            validator.Setup(x => x.Validate(It.IsAny<ExchangeVisitor>())).Returns(new FluentValidation.Results.ValidationResult());

            context.SetupActions.Add(() =>
            {
                participant = new Participant
                {
                    ParticipantId = participantId,
                    ProjectId = projectId
                };
                participantPerson = new ParticipantPerson
                {
                    Participant = participant,
                    ParticipantId = participantId
                };
                status.ParticipantPerson = participantPerson;
                status.ParticipantId = participantId;
                participantPerson.ParticipantPersonSevisCommStatuses.Add(status);
                context.Participants.Add(participant);
                context.ParticipantPersons.Add(participantPerson);
                context.ParticipantPersonSevisCommStatuses.Add(status);
            });
            Action<List<StagedSevisBatch>> tester = (batches) =>
            {
                Assert.IsNotNull(batches);
                Assert.AreEqual(1, batches.Count);
                var firstBatch = batches.First();

                Assert.IsTrue(firstBatch.IsSaved);

                Assert.AreEqual(1, firstBatch.GetExchangeVisitors().Count());
                Assert.IsTrue(Object.ReferenceEquals(exchangeVisitor, firstBatch.GetExchangeVisitors().First()));

                Assert.IsNotNull(firstBatch.SevisBatchProcessing);
                Assert.AreEqual(1, context.SevisBatchProcessings.Count());
                Assert.IsTrue(Object.ReferenceEquals(firstBatch.SevisBatchProcessing, context.SevisBatchProcessings.First()));
                Assert.IsNotNull(firstBatch.SevisBatchProcessing.SendString);
                Assert.AreNotEqual(Guid.Empty, firstBatch.SevisBatchProcessing.BatchId);
                Assert.IsNull(firstBatch.SevisBatchProcessing.RetrieveDate);
                Assert.IsNull(firstBatch.SevisBatchProcessing.SubmitDate);

                Assert.IsNotNull(firstBatch.SEVISBatchCreateUpdateEV.BatchHeader);
                Assert.AreEqual(1, firstBatch.SEVISBatchCreateUpdateEV.CreateEV.Count());
                Assert.IsNull(firstBatch.SEVISBatchCreateUpdateEV.UpdateEV);
                Assert.AreEqual(status.SevisUsername, firstBatch.SEVISBatchCreateUpdateEV.userID);
                Assert.AreEqual(status.SevisOrgId, firstBatch.SEVISBatchCreateUpdateEV.BatchHeader.OrgID);
                Assert.IsNotNull(firstBatch.SEVISBatchCreateUpdateEV.BatchHeader.BatchID);

                Assert.IsNotNull(firstBatch.BatchId);
                Assert.AreEqual(2, context.ParticipantPersonSevisCommStatuses.Count());
                Assert.IsTrue(Object.ReferenceEquals(status, context.ParticipantPersonSevisCommStatuses.First()));
                var addedCommStatus = context.ParticipantPersonSevisCommStatuses.Last();
                DateTimeOffset.UtcNow.Should().BeCloseTo(addedCommStatus.AddedOn, 20000);
                Assert.AreEqual(participantId, addedCommStatus.ParticipantId);
                Assert.AreEqual(SevisCommStatus.PendingSevisSend.Id, addedCommStatus.SevisCommStatusId);
                Assert.AreEqual(firstBatch.BatchId, addedCommStatus.BatchId);
                Assert.AreEqual(status.SevisUsername, addedCommStatus.SevisUsername);
                Assert.AreEqual(status.SevisOrgId, addedCommStatus.SevisOrgId);
            };
            context.Revert();
            var result = service.StageBatches();
            Assert.AreEqual(result.Count, context.SaveChangesCalledCount);
            tester(result);

            context.Revert();
            var resultAsync = await service.StageBatchesAsync();
            tester(resultAsync);
            Assert.AreEqual(result.Count * 2, context.SaveChangesCalledCount);
            notificationService.Verify(x => x.NotifyNumberOfParticipantsToStage(It.IsAny<int>()), Times.Exactly(2));
            notificationService.Verify(x => x.NotifyStagedSevisBatchCreated(It.IsAny<StagedSevisBatch>()), Times.Exactly(2));
            notificationService.Verify(x => x.NotifyStagedSevisBatchesFinished(It.IsAny<List<StagedSevisBatch>>()), Times.Exactly(2));
            notificationService.Verify(x => x.NotifyInvalidExchangeVisitor(It.IsAny<ExchangeVisitor>()), Times.Never());
        }

        [TestMethod]
        public async Task TestStageBatches_CommStatusesHaveDifferentSevisOrgId() 
        {
            var personId = 10;
            var firstParticipantId = 1;
            var secondParticipantId = 2;
            var projectId = 2;
            Participant firstParticipant = null;
            ParticipantPerson firstParticipantPerson = null;
            Participant secondParticipant = null;
            ParticipantPerson secondParticipantPerson = null;
            var firstStatus = new ParticipantPersonSevisCommStatus
            {
                Id = 1,
                SevisCommStatusId = SevisCommStatus.QueuedToSubmit.Id,
                AddedOn = DateTime.UtcNow.AddDays(-1.0),
                SevisUsername = "sevis username",
                SevisOrgId = "abc"
            };

            var secondStatus = new ParticipantPersonSevisCommStatus
            {
                Id = 2,
                SevisCommStatusId = SevisCommStatus.QueuedToSubmit.Id,
                AddedOn = DateTime.UtcNow.AddDays(-2.0),
                SevisUsername = "sevis username",
                SevisOrgId = "xyz"
            };

            var siteOfActivity = new AddressDTO
            {
                Division = "DC",
                LocationName = "name"
            };
            var exchangeVisitor = new ExchangeVisitor(
                sevisId: null,
                person: GetPerson(personId, firstParticipantId),
                financialInfo: new Business.Validation.Sevis.Finance.FinancialInfo(true, true, null, null),
                occupationCategoryCode: "99",
                programEndDate: DateTime.Now,
                programStartDate: DateTime.Now,
                dependents: new List<Business.Validation.Sevis.Bio.Dependent>(),
                siteOfActivity: siteOfActivity);
            exchangeVisitorService.Setup(x => x.GetExchangeVisitor(It.IsAny<int>(), It.IsAny<int>())).Returns(exchangeVisitor);
            exchangeVisitorService.Setup(x => x.GetExchangeVisitorAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(exchangeVisitor);
            validator.Setup(x => x.Validate(It.IsAny<ExchangeVisitor>())).Returns(new FluentValidation.Results.ValidationResult());

            context.SetupActions.Add(() =>
            {
                firstParticipant = new Participant
                {
                    ParticipantId = firstParticipantId,
                    ProjectId = projectId
                };
                firstParticipantPerson = new ParticipantPerson
                {
                    Participant = firstParticipant,
                    ParticipantId = firstParticipantId
                };
                secondParticipant = new Participant
                {
                    ParticipantId = secondParticipantId,
                    ProjectId = projectId
                };
                secondParticipantPerson = new ParticipantPerson
                {
                    Participant = secondParticipant,
                    ParticipantId = secondParticipantId
                };
                firstStatus.ParticipantPerson = firstParticipantPerson;
                firstStatus.ParticipantId = firstParticipantId;
                secondStatus.ParticipantPerson = secondParticipantPerson;
                secondStatus.ParticipantId = firstParticipantId;
                firstParticipantPerson.ParticipantPersonSevisCommStatuses.Add(firstStatus);
                secondParticipantPerson.ParticipantPersonSevisCommStatuses.Add(secondStatus);
                context.Participants.Add(firstParticipant);
                context.Participants.Add(secondParticipant);
                context.ParticipantPersons.Add(firstParticipantPerson);
                context.ParticipantPersons.Add(secondParticipantPerson);
                context.ParticipantPersonSevisCommStatuses.Add(firstStatus);
                context.ParticipantPersonSevisCommStatuses.Add(secondStatus);
            });
            Action<List<StagedSevisBatch>> tester = (batches) =>
            {
                Assert.AreEqual(firstStatus.SevisUsername, secondStatus.SevisUsername);
                Assert.AreNotEqual(firstStatus.SevisOrgId, secondStatus.SevisOrgId);
                Assert.IsNotNull(batches);
                Assert.AreEqual(2, batches.Count);
                batches = batches.OrderByDescending(x => x.SEVISBatchCreateUpdateEV.userID).ToList();
                var firstBatch = batches.First();
                Assert.AreEqual(firstStatus.SevisOrgId, firstBatch.SEVISBatchCreateUpdateEV.BatchHeader.OrgID);
                Assert.AreEqual(firstStatus.SevisUsername, firstBatch.SEVISBatchCreateUpdateEV.userID);
                var lastBatch = batches.Last();
                Assert.AreEqual(secondStatus.SevisOrgId, lastBatch.SEVISBatchCreateUpdateEV.BatchHeader.OrgID);
                Assert.AreEqual(secondStatus.SevisUsername, lastBatch.SEVISBatchCreateUpdateEV.userID);

            };
            context.Revert();
            var result = service.StageBatches();
            Assert.AreEqual(result.Count, context.SaveChangesCalledCount);
            tester(result);

            context.Revert();
            var resultAsync = await service.StageBatchesAsync();
            tester(resultAsync);
            Assert.AreEqual(result.Count * 2, context.SaveChangesCalledCount);
            notificationService.Verify(x => x.NotifyNumberOfParticipantsToStage(It.IsAny<int>()), Times.Exactly(2));
            notificationService.Verify(x => x.NotifyStagedSevisBatchCreated(It.IsAny<StagedSevisBatch>()), Times.Exactly(4));
            notificationService.Verify(x => x.NotifyStagedSevisBatchesFinished(It.IsAny<List<StagedSevisBatch>>()), Times.Exactly(2));
            notificationService.Verify(x => x.NotifyInvalidExchangeVisitor(It.IsAny<ExchangeVisitor>()), Times.Never());
        }

        [TestMethod]
        public async Task TestStageBatches_CommStatusesHaveDifferentSevisUsername()
        {
            var personId = 10;
            var firstParticipantId = 1;
            var secondParticipantId = 2;
            var projectId = 2;
            Participant firstParticipant = null;
            ParticipantPerson firstParticipantPerson = null;
            Participant secondParticipant = null;
            ParticipantPerson secondParticipantPerson = null;
            var firstStatus = new ParticipantPersonSevisCommStatus
            {
                Id = 1,
                SevisCommStatusId = SevisCommStatus.QueuedToSubmit.Id,
                AddedOn = DateTime.UtcNow.AddDays(-1.0),
                SevisUsername = "sevis username",
                SevisOrgId = "abc"
            };

            var secondStatus = new ParticipantPersonSevisCommStatus
            {
                Id = 2,
                SevisCommStatusId = SevisCommStatus.QueuedToSubmit.Id,
                AddedOn = DateTime.UtcNow.AddDays(-2.0),
                SevisUsername = "other username",
                SevisOrgId = "abc"
            };

            var siteOfActivity = new AddressDTO
            {
                Division = "DC",
                LocationName = "name"
            };
            var exchangeVisitor = new ExchangeVisitor(
                sevisId: null,
                person: GetPerson(personId, firstParticipantId),
                financialInfo: new Business.Validation.Sevis.Finance.FinancialInfo(true, true, null, null),
                occupationCategoryCode: "99",
                programEndDate: DateTime.Now,
                programStartDate: DateTime.Now,
                dependents: new List<Business.Validation.Sevis.Bio.Dependent>(),
                siteOfActivity: siteOfActivity);
            exchangeVisitorService.Setup(x => x.GetExchangeVisitor(It.IsAny<int>(), It.IsAny<int>())).Returns(exchangeVisitor);
            exchangeVisitorService.Setup(x => x.GetExchangeVisitorAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(exchangeVisitor);
            validator.Setup(x => x.Validate(It.IsAny<ExchangeVisitor>())).Returns(new FluentValidation.Results.ValidationResult());

            context.SetupActions.Add(() =>
            {
                firstParticipant = new Participant
                {
                    ParticipantId = firstParticipantId,
                    ProjectId = projectId
                };
                firstParticipantPerson = new ParticipantPerson
                {
                    Participant = firstParticipant,
                    ParticipantId = firstParticipantId
                };
                secondParticipant = new Participant
                {
                    ParticipantId = secondParticipantId,
                    ProjectId = projectId
                };
                secondParticipantPerson = new ParticipantPerson
                {
                    Participant = secondParticipant,
                    ParticipantId = secondParticipantId
                };
                firstStatus.ParticipantPerson = firstParticipantPerson;
                firstStatus.ParticipantId = firstParticipantId;
                secondStatus.ParticipantPerson = secondParticipantPerson;
                secondStatus.ParticipantId = firstParticipantId;
                firstParticipantPerson.ParticipantPersonSevisCommStatuses.Add(firstStatus);
                secondParticipantPerson.ParticipantPersonSevisCommStatuses.Add(secondStatus);
                context.Participants.Add(firstParticipant);
                context.Participants.Add(secondParticipant);
                context.ParticipantPersons.Add(firstParticipantPerson);
                context.ParticipantPersons.Add(secondParticipantPerson);
                context.ParticipantPersonSevisCommStatuses.Add(firstStatus);
                context.ParticipantPersonSevisCommStatuses.Add(secondStatus);
            });
            Action<List<StagedSevisBatch>> tester = (batches) =>
            {
                Assert.AreNotEqual(firstStatus.SevisUsername, secondStatus.SevisUsername);
                Assert.AreEqual(firstStatus.SevisOrgId, secondStatus.SevisOrgId);
                Assert.IsNotNull(batches);
                Assert.AreEqual(2, batches.Count);
                batches = batches.OrderByDescending(x => x.SEVISBatchCreateUpdateEV.userID).ToList();
                var firstBatch = batches.First();
                Assert.AreEqual(firstStatus.SevisOrgId, firstBatch.SEVISBatchCreateUpdateEV.BatchHeader.OrgID);
                Assert.AreEqual(firstStatus.SevisUsername, firstBatch.SEVISBatchCreateUpdateEV.userID);
                var lastBatch = batches.Last();
                Assert.AreEqual(secondStatus.SevisOrgId, lastBatch.SEVISBatchCreateUpdateEV.BatchHeader.OrgID);
                Assert.AreEqual(secondStatus.SevisUsername, lastBatch.SEVISBatchCreateUpdateEV.userID);

            };
            context.Revert();
            var result = service.StageBatches();
            Assert.AreEqual(result.Count, context.SaveChangesCalledCount);
            tester(result);

            context.Revert();
            var resultAsync = await service.StageBatchesAsync();
            tester(resultAsync);
            Assert.AreEqual(result.Count * 2, context.SaveChangesCalledCount);
            notificationService.Verify(x => x.NotifyNumberOfParticipantsToStage(It.IsAny<int>()), Times.Exactly(2));
            notificationService.Verify(x => x.NotifyStagedSevisBatchCreated(It.IsAny<StagedSevisBatch>()), Times.Exactly(4));
            notificationService.Verify(x => x.NotifyStagedSevisBatchesFinished(It.IsAny<List<StagedSevisBatch>>()), Times.Exactly(2));
            notificationService.Verify(x => x.NotifyInvalidExchangeVisitor(It.IsAny<ExchangeVisitor>()), Times.Never());
        }

        [TestMethod]
        public async Task TestStageBatches_ExchangeVisitorIsNotValid()
        {
            var personId = 10;
            var participantId = 1;
            var projectId = 2;
            Participant participant = null;
            ParticipantPerson participantPerson = null;
            var status = new ParticipantPersonSevisCommStatus
            {
                Id = 1,
                SevisCommStatusId = SevisCommStatus.QueuedToSubmit.Id,
                AddedOn = DateTime.UtcNow.AddDays(-1.0),
                SevisUsername = "sevis username",
                SevisOrgId = "sevis org id"
            };

            var siteOfActivity = new AddressDTO
            {
                Division = "DC",
                LocationName = "name"
            };
            var exchangeVisitor = new ExchangeVisitor(
                sevisId: null,
                person: GetPerson(personId, participantId),
                financialInfo: new Business.Validation.Sevis.Finance.FinancialInfo(true, true, null, null),
                occupationCategoryCode: "99",
                programEndDate: DateTime.Now,
                programStartDate: DateTime.Now,
                dependents: new List<Business.Validation.Sevis.Bio.Dependent>(),
                siteOfActivity: siteOfActivity);
            exchangeVisitorService.Setup(x => x.GetExchangeVisitor(It.IsAny<int>(), It.IsAny<int>())).Returns(exchangeVisitor);
            exchangeVisitorService.Setup(x => x.GetExchangeVisitorAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(exchangeVisitor);

            var validationFailures = new List<ValidationFailure>();
            validationFailures.Add(new ValidationFailure("property", "error"));

            validator.Setup(x => x.Validate(It.IsAny<ExchangeVisitor>()))
                .Returns(new FluentValidation.Results.ValidationResult(validationFailures));

            context.SetupActions.Add(() =>
            {
                participant = new Participant
                {
                    ParticipantId = participantId,
                    ProjectId = projectId
                };
                participantPerson = new ParticipantPerson
                {
                    Participant = participant,
                    ParticipantId = participantId
                };
                status.ParticipantPerson = participantPerson;
                status.ParticipantId = participantId;
                participantPerson.ParticipantPersonSevisCommStatuses.Add(status);
                context.Participants.Add(participant);
                context.ParticipantPersons.Add(participantPerson);
                context.ParticipantPersonSevisCommStatuses.Add(status);
            });
            Action<List<StagedSevisBatch>> tester = (batches) =>
            {
                Assert.IsNotNull(batches);
                Assert.AreEqual(0, batches.Count);
                Assert.AreEqual(0, context.SevisBatchProcessings.Count());
            };
            context.Revert();
            var result = service.StageBatches();
            Assert.AreEqual(result.Count, context.SaveChangesCalledCount);
            tester(result);
            exchangeVisitorValidationService.Verify(x => x.RunParticipantSevisValidation(It.IsAny<int>(), It.IsAny<int>()), Times.Once());
            exchangeVisitorValidationService.Verify(x => x.SaveChanges(), Times.Once());

            context.Revert();
            var resultAsync = await service.StageBatchesAsync();
            tester(resultAsync);
            Assert.AreEqual(result.Count * 2, context.SaveChangesCalledCount);
            exchangeVisitorValidationService.Verify(x => x.RunParticipantSevisValidationAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once());
            exchangeVisitorValidationService.Verify(x => x.SaveChangesAsync(), Times.Once());
            notificationService.Verify(x => x.NotifyNumberOfParticipantsToStage(It.IsAny<int>()), Times.Exactly(2));
            notificationService.Verify(x => x.NotifyStagedSevisBatchCreated(It.IsAny<StagedSevisBatch>()), Times.Never());
            notificationService.Verify(x => x.NotifyInvalidExchangeVisitor(It.IsAny<ExchangeVisitor>()), Times.Exactly(2));
            notificationService.Verify(x => x.NotifyStagedSevisBatchesFinished(It.IsAny<List<StagedSevisBatch>>()), Times.Exactly(2));
        }

        [TestMethod]
        public async Task TestStageBatches_OneExchangeVisitor_HasSevisId()
        {
            var personId = 10;
            var participantId = 1;
            var projectId = 2;
            Participant participant = null;
            ParticipantPerson participantPerson = null;
            var status = new ParticipantPersonSevisCommStatus
            {
                Id = 1,
                SevisCommStatusId = SevisCommStatus.QueuedToSubmit.Id,
                AddedOn = DateTime.UtcNow.AddDays(-1.0),
                SevisOrgId = "sevis org Id",
                SevisUsername = "sevis user"
            };

            var siteOfActivity = new AddressDTO
            {
                Division = "DC",
                LocationName = "name"
            };
            var exchangeVisitor = new ExchangeVisitor(
                sevisId: "sevisid",
                person: GetPerson(personId, participantId),
                financialInfo: new Business.Validation.Sevis.Finance.FinancialInfo(true, true, null, null),
                occupationCategoryCode: "99",
                programEndDate: DateTime.Now,
                programStartDate: DateTime.Now,
                dependents: new List<Business.Validation.Sevis.Bio.Dependent>(),
                siteOfActivity: siteOfActivity);
            exchangeVisitorService.Setup(x => x.GetExchangeVisitor(It.IsAny<int>(), It.IsAny<int>())).Returns(exchangeVisitor);
            exchangeVisitorService.Setup(x => x.GetExchangeVisitorAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(exchangeVisitor);
            validator.Setup(x => x.Validate(It.IsAny<ExchangeVisitor>())).Returns(new FluentValidation.Results.ValidationResult());
            context.SetupActions.Add(() =>
            {
                participant = new Participant
                {
                    ParticipantId = participantId,
                    ProjectId = projectId
                };
                participantPerson = new ParticipantPerson
                {
                    Participant = participant,
                    ParticipantId = participantId
                };
                status.ParticipantPerson = participantPerson;
                status.ParticipantId = participantId;
                participantPerson.ParticipantPersonSevisCommStatuses.Add(status);
                context.Participants.Add(participant);
                context.ParticipantPersons.Add(participantPerson);
                context.ParticipantPersonSevisCommStatuses.Add(status);
            });
            Action<List<StagedSevisBatch>> tester = (batches) =>
            {
                Assert.IsNotNull(batches);
                Assert.AreEqual(1, batches.Count);
                var firstBatch = batches.First();

                Assert.IsTrue(firstBatch.IsSaved);

                Assert.AreEqual(1, firstBatch.GetExchangeVisitors().Count());
                Assert.IsTrue(Object.ReferenceEquals(exchangeVisitor, firstBatch.GetExchangeVisitors().First()));

                Assert.IsNotNull(firstBatch.SevisBatchProcessing);
                Assert.AreEqual(1, context.SevisBatchProcessings.Count());
                Assert.IsTrue(Object.ReferenceEquals(firstBatch.SevisBatchProcessing, context.SevisBatchProcessings.First()));
                Assert.IsNotNull(firstBatch.SevisBatchProcessing.SendString);
                Assert.AreNotEqual(Guid.Empty, firstBatch.SevisBatchProcessing.BatchId);
                Assert.IsNull(firstBatch.SevisBatchProcessing.RetrieveDate);
                Assert.IsNull(firstBatch.SevisBatchProcessing.SubmitDate);

                Assert.IsNotNull(firstBatch.SEVISBatchCreateUpdateEV.BatchHeader);
                Assert.IsNull(firstBatch.SEVISBatchCreateUpdateEV.CreateEV);
                Assert.AreEqual(exchangeVisitor.GetSEVISEVBatchTypeExchangeVisitor1Collection(status.SevisUsername).Count(), firstBatch.SEVISBatchCreateUpdateEV.UpdateEV.Count());
                Assert.AreEqual(status.SevisUsername, firstBatch.SEVISBatchCreateUpdateEV.userID);
                Assert.AreEqual(status.SevisOrgId, firstBatch.SEVISBatchCreateUpdateEV.BatchHeader.OrgID);
                Assert.IsNotNull(firstBatch.SEVISBatchCreateUpdateEV.BatchHeader.BatchID);

                Assert.IsNotNull(firstBatch.BatchId);

                Assert.AreEqual(2, context.ParticipantPersonSevisCommStatuses.Count());
                Assert.IsTrue(Object.ReferenceEquals(status, context.ParticipantPersonSevisCommStatuses.First()));
                var addedCommStatus = context.ParticipantPersonSevisCommStatuses.Last();
                DateTimeOffset.UtcNow.Should().BeCloseTo(addedCommStatus.AddedOn, 20000);
                Assert.AreEqual(participantId, addedCommStatus.ParticipantId);
                Assert.AreEqual(SevisCommStatus.PendingSevisSend.Id, addedCommStatus.SevisCommStatusId);
                Assert.AreEqual(firstBatch.BatchId, addedCommStatus.BatchId);
                Assert.AreEqual(status.SevisUsername, addedCommStatus.SevisUsername);
                Assert.AreEqual(status.SevisOrgId, addedCommStatus.SevisOrgId);
            };
            context.Revert();
            var result = service.StageBatches();
            Assert.AreEqual(result.Count, context.SaveChangesCalledCount);
            tester(result);

            context.Revert();
            var resultAsync = await service.StageBatchesAsync();
            tester(resultAsync);
            Assert.AreEqual(result.Count * 2, context.SaveChangesCalledCount);

            notificationService.Verify(x => x.NotifyNumberOfParticipantsToStage(It.IsAny<int>()), Times.Exactly(2));
            notificationService.Verify(x => x.NotifyStagedSevisBatchCreated(It.IsAny<StagedSevisBatch>()), Times.Exactly(2));
            notificationService.Verify(x => x.NotifyStagedSevisBatchesFinished(It.IsAny<List<StagedSevisBatch>>()), Times.Exactly(2));
            notificationService.Verify(x => x.NotifyInvalidExchangeVisitor(It.IsAny<ExchangeVisitor>()), Times.Never());
        }

        [TestMethod]
        public async Task TestStageBatches_HasMaxCreateAndUpdateExchangeVisitors()
        {
            var personId = 10;
            var projectId = 500;
            var siteOfActivity = new AddressDTO
            {
                Division = "DC",
                LocationName = "name"
            };
            var exchangeVisitors = new List<ExchangeVisitor>();
            Func<int, int, ExchangeVisitor> getExchangeVisitor = (projId, partId) =>
            {
                return exchangeVisitors.Where(x => x.Person.ParticipantId == partId).First();
            };
            Func<int, int, Task<ExchangeVisitor>> getExchangeVisitorAync = (projId, partId) =>
            {
                return Task.FromResult<ExchangeVisitor>(getExchangeVisitor(projId, partId));
            };

            exchangeVisitorService
                .Setup(x => x.GetExchangeVisitor(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(getExchangeVisitor);
            exchangeVisitorService
                .Setup(x => x.GetExchangeVisitorAsync(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(getExchangeVisitorAync);
            validator.Setup(x => x.Validate(It.IsAny<ExchangeVisitor>())).Returns(new FluentValidation.Results.ValidationResult());
            context.SetupActions.Add(() =>
            {
                var sevisUsername = "sevis username";
                var now = DateTime.UtcNow;
                for (var i = 0; i < maxCreateExchangeVisitorBatchSize; i++)
                {
                    var participant = new Participant
                    {
                        ParticipantId = i,
                        ProjectId = projectId,
                    };
                    var participantPerson = new ParticipantPerson
                    {
                        ParticipantId = participant.ParticipantId,
                        Participant = participant
                    };
                    participant.ParticipantPerson = participantPerson;
                    var readyToSubmitStatus = new ParticipantPersonSevisCommStatus
                    {
                        Id = participant.ParticipantId,
                        AddedOn = now,
                        ParticipantId = participant.ParticipantId,
                        SevisCommStatusId = SevisCommStatus.QueuedToSubmit.Id,
                        ParticipantPerson = participantPerson,
                        SevisUsername = sevisUsername,
                        SevisOrgId = "sevis org id"
                    };
                    participantPerson.ParticipantPersonSevisCommStatuses.Add(readyToSubmitStatus);
                    context.Participants.Add(participant);
                    context.ParticipantPersons.Add(participantPerson);
                    context.ParticipantPersonSevisCommStatuses.Add(readyToSubmitStatus);

                    var exchangeVisitor = new ExchangeVisitor(
                        sevisId: null,
                        person: GetPerson(personId, participant.ParticipantId),
                        financialInfo: new Business.Validation.Sevis.Finance.FinancialInfo(true, true, null, null),
                        occupationCategoryCode: "99",
                        programEndDate: DateTime.Now,
                        programStartDate: DateTime.Now,
                        dependents: new List<Business.Validation.Sevis.Bio.Dependent>(),
                        siteOfActivity: siteOfActivity
                    );
                    exchangeVisitors.Add(exchangeVisitor);
                }
                for (var i = 1;
                    i <= maxUpdateExchangeVisitorBatchSize / exchangeVisitors.First().GetSEVISEVBatchTypeExchangeVisitor1Collection(sevisUsername).Count();
                    i++)
                {
                    var participant = new Participant
                    {
                        ParticipantId = i * 100,
                        ProjectId = projectId,
                    };
                    var participantPerson = new ParticipantPerson
                    {
                        ParticipantId = participant.ParticipantId,
                        Participant = participant
                    };
                    participant.ParticipantPerson = participantPerson;
                    var readyToSubmitStatus = new ParticipantPersonSevisCommStatus
                    {
                        Id = participant.ParticipantId,
                        AddedOn = now,
                        ParticipantId = participant.ParticipantId,
                        SevisCommStatusId = SevisCommStatus.QueuedToSubmit.Id,
                        ParticipantPerson = participantPerson,
                        SevisUsername = sevisUsername,
                        SevisOrgId = "sevis org id"
                    };
                    participantPerson.ParticipantPersonSevisCommStatuses.Add(readyToSubmitStatus);
                    context.Participants.Add(participant);
                    context.ParticipantPersons.Add(participantPerson);
                    context.ParticipantPersonSevisCommStatuses.Add(readyToSubmitStatus);

                    var exchangeVisitor = new ExchangeVisitor(
                        sevisId: "sevisId",
                        person: GetPerson(personId, participant.ParticipantId),
                        financialInfo: new Business.Validation.Sevis.Finance.FinancialInfo(true, true, null, null),
                        occupationCategoryCode: "99",
                        programEndDate: DateTime.Now,
                        programStartDate: DateTime.Now,
                        dependents: new List<Business.Validation.Sevis.Bio.Dependent>(),
                        siteOfActivity: siteOfActivity
                    );
                    exchangeVisitors.Add(exchangeVisitor);
                }
            });

            Action<List<StagedSevisBatch>> tester = (batches) =>
            {
                Assert.IsNotNull(batches);
                Assert.AreEqual(1, batches.Count);

                var firstBatch = batches.First();
                Assert.AreEqual(10, firstBatch.SEVISBatchCreateUpdateEV.CreateEV.Count());
                Assert.IsTrue(0 < firstBatch.SEVISBatchCreateUpdateEV.UpdateEV.Count());
                Assert.IsTrue(firstBatch.IsSaved);
            };

            context.Revert();
            var result = service.StageBatches();
            tester(result);
            Assert.AreEqual(result.Count, context.SaveChangesCalledCount);

            context.Revert();
            var resultAsync = await service.StageBatchesAsync();
            tester(resultAsync);
            Assert.AreEqual(result.Count * 2, context.SaveChangesCalledCount);

            notificationService.Verify(x => x.NotifyNumberOfParticipantsToStage(It.IsAny<int>()), Times.Exactly(2));
            notificationService.Verify(x => x.NotifyStagedSevisBatchCreated(It.IsAny<StagedSevisBatch>()), Times.Exactly(2));
            notificationService.Verify(x => x.NotifyStagedSevisBatchesFinished(It.IsAny<List<StagedSevisBatch>>()), Times.Exactly(2));
            notificationService.Verify(x => x.NotifyInvalidExchangeVisitor(It.IsAny<ExchangeVisitor>()), Times.Never());
        }

        [TestMethod]
        public async Task TestStageBatches_HasMoreThanMaxCreateAndUpdateExchangeVisitors()
        {
            var personId = 10;
            var projectId = 500;
            var siteOfActivity = new AddressDTO
            {
                Division = "DC",
                LocationName = "name"
            };
            var exchangeVisitors = new List<ExchangeVisitor>();
            Func<int, int, ExchangeVisitor> getExchangeVisitor = (projId, partId) =>
            {
                return exchangeVisitors.Where(x => x.Person.ParticipantId == partId).First();
            };
            Func<int, int, Task<ExchangeVisitor>> getExchangeVisitorAync = (projId, partId) =>
            {
                return Task.FromResult<ExchangeVisitor>(getExchangeVisitor(projId, partId));
            };

            exchangeVisitorService
                .Setup(x => x.GetExchangeVisitor(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(getExchangeVisitor);
            exchangeVisitorService
                .Setup(x => x.GetExchangeVisitorAsync(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(getExchangeVisitorAync);
            validator.Setup(x => x.Validate(It.IsAny<ExchangeVisitor>())).Returns(new FluentValidation.Results.ValidationResult());
            context.SetupActions.Add(() =>
            {
                var sevisUsername = "sevis username";
                var now = DateTime.UtcNow;
                for (var i = 0; i < maxCreateExchangeVisitorBatchSize; i++)
                {
                    var participant = new Participant
                    {
                        ParticipantId = i,
                        ProjectId = projectId,
                    };
                    var participantPerson = new ParticipantPerson
                    {
                        ParticipantId = participant.ParticipantId,
                        Participant = participant
                    };
                    participant.ParticipantPerson = participantPerson;
                    var readyToSubmitStatus = new ParticipantPersonSevisCommStatus
                    {
                        Id = participant.ParticipantId,
                        AddedOn = now,
                        ParticipantId = participant.ParticipantId,
                        SevisCommStatusId = SevisCommStatus.QueuedToSubmit.Id,
                        ParticipantPerson = participantPerson,
                        SevisUsername = sevisUsername,
                        SevisOrgId = "sevis org id"
                    };
                    participantPerson.ParticipantPersonSevisCommStatuses.Add(readyToSubmitStatus);
                    context.Participants.Add(participant);
                    context.ParticipantPersons.Add(participantPerson);
                    context.ParticipantPersonSevisCommStatuses.Add(readyToSubmitStatus);

                    var exchangeVisitor = new ExchangeVisitor(
                        sevisId: null,
                        person: GetPerson(personId, participant.ParticipantId),
                        financialInfo: new Business.Validation.Sevis.Finance.FinancialInfo(true, true, null, null),
                        occupationCategoryCode: "99",
                        programEndDate: DateTime.Now,
                        programStartDate: DateTime.Now,
                        dependents: new List<Business.Validation.Sevis.Bio.Dependent>(),
                        siteOfActivity: siteOfActivity
                    );
                    exchangeVisitors.Add(exchangeVisitor);
                }
                for (var i = 1;
                    i <= maxUpdateExchangeVisitorBatchSize;
                    i++)
                {
                    var participant = new Participant
                    {
                        ParticipantId = i * 100,
                        ProjectId = projectId,
                    };
                    var participantPerson = new ParticipantPerson
                    {
                        ParticipantId = participant.ParticipantId,
                        Participant = participant
                    };
                    participant.ParticipantPerson = participantPerson;
                    var readyToSubmitStatus = new ParticipantPersonSevisCommStatus
                    {
                        Id = participant.ParticipantId,
                        AddedOn = now,
                        ParticipantId = participant.ParticipantId,
                        SevisCommStatusId = SevisCommStatus.QueuedToSubmit.Id,
                        ParticipantPerson = participantPerson,
                        SevisUsername = sevisUsername,
                        SevisOrgId = "sevis org id"
                    };
                    participantPerson.ParticipantPersonSevisCommStatuses.Add(readyToSubmitStatus);
                    context.Participants.Add(participant);
                    context.ParticipantPersons.Add(participantPerson);
                    context.ParticipantPersonSevisCommStatuses.Add(readyToSubmitStatus);

                    var exchangeVisitor = new ExchangeVisitor(
                        sevisId: "sevisId",
                        person: GetPerson(personId, participant.ParticipantId),
                        financialInfo: new Business.Validation.Sevis.Finance.FinancialInfo(true, true, null, null),
                        occupationCategoryCode: "99",
                        programEndDate: DateTime.Now,
                        programStartDate: DateTime.Now,
                        dependents: new List<Business.Validation.Sevis.Bio.Dependent>(),
                        siteOfActivity: siteOfActivity
                    );
                    exchangeVisitors.Add(exchangeVisitor);
                }
            });
            var numberOfUpdateRecordsPerExchangeVisitor = 3;
            Action<List<StagedSevisBatch>> tester = (batches) =>
            {
                var expectedBatchCount = 4;
                Assert.IsNotNull(batches);
                Assert.AreEqual(expectedBatchCount, batches.Count);

                for (var i = 0; i < expectedBatchCount; i++)
                {
                    var batch = batches[i];
                    if (i == expectedBatchCount - 1)
                    {
                        Assert.IsNull(batch.SEVISBatchCreateUpdateEV.CreateEV);
                        Assert.AreEqual(numberOfUpdateRecordsPerExchangeVisitor, batch.SEVISBatchCreateUpdateEV.UpdateEV.Count());
                    }
                    if (i == 0)
                    {
                        Assert.AreEqual(maxCreateExchangeVisitorBatchSize, batch.SEVISBatchCreateUpdateEV.CreateEV.Count());
                    }
                    if (i > 0 && i < expectedBatchCount - 1)
                    {
                        Assert.IsNull(batch.SEVISBatchCreateUpdateEV.CreateEV);
                        Assert.AreEqual(numberOfUpdateRecordsPerExchangeVisitor * 3, batch.SEVISBatchCreateUpdateEV.UpdateEV.Count());
                    }
                }
            };

            context.Revert();
            var result = service.StageBatches();
            tester(result);
            Assert.AreEqual(result.Count, context.SaveChangesCalledCount);

            context.Revert();
            var resultAsync = await service.StageBatchesAsync();
            tester(resultAsync);
            Assert.AreEqual(result.Count * 2, context.SaveChangesCalledCount);

            notificationService.Verify(x => x.NotifyNumberOfParticipantsToStage(It.IsAny<int>()), Times.Exactly(2));
            notificationService.Verify(x => x.NotifyStagedSevisBatchCreated(It.IsAny<StagedSevisBatch>()), Times.Exactly(8));
            notificationService.Verify(x => x.NotifyStagedSevisBatchesFinished(It.IsAny<List<StagedSevisBatch>>()), Times.Exactly(2));
            notificationService.Verify(x => x.NotifyInvalidExchangeVisitor(It.IsAny<ExchangeVisitor>()), Times.Never());
        }


        [TestMethod]
        public async Task TestStageBatches_NoQueuedToSubmitParticipants()
        {
            using (ShimsContext.Create())
            {
                var participantId = 1;
                var projectId = 2;
                Participant participant = null;
                ParticipantPerson participantPerson = null;
                var readyToSubmitStatus = new ParticipantPersonSevisCommStatus
                {
                    Id = 1,
                    SevisCommStatusId = SevisCommStatus.PendingSevisSend.Id,
                    AddedOn = DateTime.UtcNow.AddDays(-1.0)
                };

                context.SetupActions.Add(() =>
                {
                    participant = new Participant
                    {
                        ParticipantId = participantId,
                        ProjectId = projectId
                    };
                    participantPerson = new ParticipantPerson
                    {
                        Participant = participant,
                        ParticipantId = participantId
                    };
                    readyToSubmitStatus.ParticipantPerson = participantPerson;
                    readyToSubmitStatus.ParticipantId = participantId;
                    participantPerson.ParticipantPersonSevisCommStatuses.Add(readyToSubmitStatus);
                    context.Participants.Add(participant);
                    context.ParticipantPersons.Add(participantPerson);
                    context.ParticipantPersonSevisCommStatuses.Add(readyToSubmitStatus);
                });
                Action<List<StagedSevisBatch>> tester = (batches) =>
                {
                    Assert.IsNotNull(batches);
                    Assert.AreEqual(0, batches.Count);

                    Assert.AreEqual(1, context.ParticipantPersonSevisCommStatuses.Count());
                    Assert.IsTrue(Object.ReferenceEquals(readyToSubmitStatus, context.ParticipantPersonSevisCommStatuses.First()));
                };
                context.Revert();
                var result = service.StageBatches();
                tester(result);
                Assert.AreEqual(0, context.SaveChangesCalledCount);

                context.Revert();
                var resultAsync = await service.StageBatchesAsync();
                tester(resultAsync);
                Assert.AreEqual(0, context.SaveChangesCalledCount);

                notificationService.Verify(x => x.NotifyNumberOfParticipantsToStage(It.IsAny<int>()), Times.Exactly(2));
                notificationService.Verify(x => x.NotifyStagedSevisBatchCreated(It.IsAny<StagedSevisBatch>()), Times.Never());
                notificationService.Verify(x => x.NotifyStagedSevisBatchesFinished(It.IsAny<List<StagedSevisBatch>>()), Times.Exactly(2));
                notificationService.Verify(x => x.NotifyInvalidExchangeVisitor(It.IsAny<ExchangeVisitor>()), Times.Never());
            }
        }
        #endregion

        #region GetAccomodatingSevisBatch
        [TestMethod]
        public void TestGetAccomodatingSevisBatch_NoSevisBatches()
        {
            var batches = new List<StagedSevisBatch>();
            var status = new ParticipantPersonSevisCommStatus
            {
                Id = 1,
                SevisCommStatusId = SevisCommStatus.QueuedToSubmit.Id,
                AddedOn = DateTime.UtcNow.AddDays(-1.0),
                SevisUsername = "sevis username",
                SevisOrgId = "sevis org id"
            };

            var siteOfActivity = new AddressDTO
            {
                Division = "DC",
                LocationName = "name"
            };

            var exchangeVisitor = new ExchangeVisitor(
                sevisId: null,
                person: GetPerson(1, 2),
                financialInfo: new Business.Validation.Sevis.Finance.FinancialInfo(true, true, null, null),
                occupationCategoryCode: "99",
                programEndDate: DateTime.Now,
                programStartDate: DateTime.Now,
                dependents: new List<Business.Validation.Sevis.Bio.Dependent>(),
                siteOfActivity: siteOfActivity);
            Assert.IsNull(service.GetAccomodatingStagedSevisBatch(batches, exchangeVisitor, status.SevisUsername, status.SevisOrgId));
        }

        [TestMethod]
        public void TestGetAccomodatingSevisBatch_SevisBatchIsSaved()
        {
            var sevisOrgId = "orgId";
            var batches = new List<StagedSevisBatch>();
            var sevisUsername = "sevis username";
            batches.Add(new StagedSevisBatch(Guid.NewGuid(), sevisUsername, sevisOrgId)
            {
                IsSaved = true
            });
            var status = new ParticipantPersonSevisCommStatus
            {
                Id = 1,
                SevisCommStatusId = SevisCommStatus.QueuedToSubmit.Id,
                AddedOn = DateTime.UtcNow.AddDays(-1.0)
            };

            var siteOfActivity = new AddressDTO
            {
                Division = "DC",
                LocationName = "name"
            };

            var exchangeVisitor = new ExchangeVisitor(
                sevisId: null,
                person: GetPerson(1, 2),
                financialInfo: new Business.Validation.Sevis.Finance.FinancialInfo(true, true, null, null),
                occupationCategoryCode: "99",
                programEndDate: DateTime.Now,
                programStartDate: DateTime.Now,
                dependents: new List<Business.Validation.Sevis.Bio.Dependent>(),
                siteOfActivity: siteOfActivity);
            Assert.IsNull(service.GetAccomodatingStagedSevisBatch(batches, exchangeVisitor, status.SevisUsername, status.SevisOrgId));
        }

        [TestMethod]
        public void TestGetAccomodatingSevisBatch_SevisBatchCanAccomodate()
        {
            var sevisOrgId = "orgId";
            var sevisUsername = "sevis username";
            var batches = new List<StagedSevisBatch>();
            batches.Add(new StagedSevisBatch(Guid.NewGuid(), sevisUsername, sevisOrgId)
            {
                IsSaved = false
            });
            var status = new ParticipantPersonSevisCommStatus
            {
                Id = 1,
                SevisCommStatusId = SevisCommStatus.QueuedToSubmit.Id,
                AddedOn = DateTime.UtcNow.AddDays(-1.0),
                SevisOrgId = sevisOrgId,
                SevisUsername = sevisUsername
            };

            var siteOfActivity = new AddressDTO
            {
                Division = "DC",
                LocationName = "name"
            };

            var exchangeVisitor = new ExchangeVisitor(
                sevisId: null,
                person: GetPerson(1, 2),
                financialInfo: new Business.Validation.Sevis.Finance.FinancialInfo(true, true, null, null),
                occupationCategoryCode: "99",
                programEndDate: DateTime.Now,
                programStartDate: DateTime.Now,
                dependents: new List<Business.Validation.Sevis.Bio.Dependent>(),
                siteOfActivity: siteOfActivity);
            Assert.IsTrue(Object.ReferenceEquals(batches.First(), service.GetAccomodatingStagedSevisBatch(batches, exchangeVisitor, status.SevisUsername, status.SevisOrgId)));
        }

        [TestMethod]
        public void TestGetAccomodatingSevisBatch_SevisBatchCanNotAccomodateBySize()
        {
            var sevisUsername = "sevis username";
            var sevisOrgId = "orgId";
            var batches = new List<StagedSevisBatch>();
            batches.Add(new StagedSevisBatch(Guid.NewGuid(), sevisUsername, sevisOrgId, 0, 0)
            {
                IsSaved = false,
            });
            var status = new ParticipantPersonSevisCommStatus
            {
                Id = 1,
                SevisCommStatusId = SevisCommStatus.QueuedToSubmit.Id,
                AddedOn = DateTime.UtcNow.AddDays(-1.0)
            };

            var siteOfActivity = new AddressDTO
            {
                Division = "DC",
                LocationName = "name"
            };

            var exchangeVisitor = new ExchangeVisitor(
                sevisId: null,
                person: GetPerson(1, 2),
                financialInfo: new Business.Validation.Sevis.Finance.FinancialInfo(true, true, null, null),
                occupationCategoryCode: "99",
                programEndDate: DateTime.Now,
                programStartDate: DateTime.Now,
                dependents: new List<Business.Validation.Sevis.Bio.Dependent>(),
                siteOfActivity: siteOfActivity);
            Assert.IsNull(service.GetAccomodatingStagedSevisBatch(batches, exchangeVisitor, status.SevisUsername, status.SevisOrgId));
        }

        [TestMethod]
        public void TestGetAccomodatingSevisBatch_SevisBatchCanNotAccomodateBySevisUsername()
        {
            var sevisUsername = "sevis username";
            var sevisOrgId = "orgId";
            var batches = new List<StagedSevisBatch>();
            batches.Add(new StagedSevisBatch(Guid.NewGuid(), sevisUsername, sevisOrgId, 1, 1)
            {
                IsSaved = false,
            });
            var status = new ParticipantPersonSevisCommStatus
            {
                Id = 1,
                SevisCommStatusId = SevisCommStatus.QueuedToSubmit.Id,
                AddedOn = DateTime.UtcNow.AddDays(-1.0),
                SevisUsername = sevisUsername,
                SevisOrgId = sevisOrgId
            };

            var siteOfActivity = new AddressDTO
            {
                Division = "DC",
                LocationName = "name"
            };

            var exchangeVisitor = new ExchangeVisitor(
                sevisId: null,
                person: GetPerson(1, 2),
                financialInfo: new Business.Validation.Sevis.Finance.FinancialInfo(true, true, null, null),
                occupationCategoryCode: "99",
                programEndDate: DateTime.Now,
                programStartDate: DateTime.Now,
                dependents: new List<Business.Validation.Sevis.Bio.Dependent>(),
                siteOfActivity: siteOfActivity);
            Assert.IsNull(service.GetAccomodatingStagedSevisBatch(batches, exchangeVisitor, "other user", status.SevisOrgId));
        }

        [TestMethod]
        public void TestGetAccomodatingSevisBatch_SevisBatchCanNotAccomodateBySevisOrgId()
        {
            var sevisUsername = "sevis username";
            var sevisOrgId = "orgId";
            var batches = new List<StagedSevisBatch>();
            batches.Add(new StagedSevisBatch(Guid.NewGuid(), sevisUsername, sevisOrgId, 1, 1)
            {
                IsSaved = false,
            });
            var status = new ParticipantPersonSevisCommStatus
            {
                Id = 1,
                SevisCommStatusId = SevisCommStatus.QueuedToSubmit.Id,
                AddedOn = DateTime.UtcNow.AddDays(-1.0),
                SevisUsername = sevisUsername,
                SevisOrgId = sevisOrgId
            };

            var siteOfActivity = new AddressDTO
            {
                Division = "DC",
                LocationName = "name"
            };

            var exchangeVisitor = new ExchangeVisitor(
                sevisId: null,
                person: GetPerson(1, 2),
                financialInfo: new Business.Validation.Sevis.Finance.FinancialInfo(true, true, null, null),
                occupationCategoryCode: "99",
                programEndDate: DateTime.Now,
                programStartDate: DateTime.Now,
                dependents: new List<Business.Validation.Sevis.Bio.Dependent>(),
                siteOfActivity: siteOfActivity);
            Assert.IsNull(service.GetAccomodatingStagedSevisBatch(batches, exchangeVisitor, status.SevisUsername, "other org"));
        }
        #endregion

        #region GetNextBatchToUpload
        [TestMethod]
        public async Task TestGetNextBatchToUpload_HasRecord()
        {
            var model = new SevisBatchProcessing
            {
                BatchId = "batch id",
                DownloadDispositionCode = "download code",
                Id = 1,
                ProcessDispositionCode = "process code",
                RetrieveDate = DateTimeOffset.UtcNow.AddDays(1.0),
                SendString = "send string",
                SubmitDate = null,
                TransactionLogString = "transaction log",
            };
            context.SevisBatchProcessings.Add(model);
            Assert.AreEqual(1, SevisBatchProcessingQueries.CreateGetSevisBatchProcessingDTOsToUploadQuery(context).Count());

            Action<SevisBatchProcessingDTO> tester = (dto) =>
            {
                Assert.IsNotNull(dto);
                Assert.AreEqual(model.Id, dto.Id);
            };
            var result = service.GetNextBatchToUpload();
            tester(result);
            var asyncResult = await service.GetNextBatchToUploadAsync();
            tester(asyncResult);
        }

        [TestMethod]
        public async Task TestGetNextBatchToUpload_HasUploadFailureCode()
        {
            var model = new SevisBatchProcessing
            {
                BatchId = "batch id",
                DownloadDispositionCode = "download code",
                Id = 1,
                ProcessDispositionCode = "process code",
                RetrieveDate = DateTimeOffset.UtcNow.AddDays(1.0),
                SendString = "send string",
                SubmitDate = DateTimeOffset.UtcNow,
                TransactionLogString = "transaction log",
                UploadDispositionCode = DispositionCode.GeneralUploadDownloadFailure.Code
            };
            context.SevisBatchProcessings.Add(model);
            Assert.AreEqual(1, SevisBatchProcessingQueries.CreateGetSevisBatchProcessingDTOsToUploadQuery(context).Count());

            Action<SevisBatchProcessingDTO> tester = (dto) =>
            {
                Assert.IsNotNull(dto);
                Assert.AreEqual(model.Id, dto.Id);
            };
            var result = service.GetNextBatchToUpload();
            tester(result);
            var asyncResult = await service.GetNextBatchToUploadAsync();
            tester(asyncResult);
        }

        [TestMethod]
        public async Task TestGetNextBatchToUpload_HasBatchNeverSubmittedFailureCode()
        {
            var model = new SevisBatchProcessing
            {
                BatchId = "batch id",
                DownloadDispositionCode = DispositionCode.BatchNeverSubmitted.Code,
                Id = 1,
                ProcessDispositionCode = "process code",
                RetrieveDate = DateTimeOffset.UtcNow.AddDays(1.0),
                SendString = "send string",
                SubmitDate = DateTimeOffset.UtcNow,
                TransactionLogString = "transaction log",
            };
            context.SevisBatchProcessings.Add(model);
            Assert.AreEqual(1, SevisBatchProcessingQueries.CreateGetSevisBatchProcessingDTOsToUploadQuery(context).Count());

            Action<SevisBatchProcessingDTO> tester = (dto) =>
            {
                Assert.IsNotNull(dto);
                Assert.AreEqual(model.Id, dto.Id);
            };
            var result = service.GetNextBatchToUpload();
            tester(result);
            var asyncResult = await service.GetNextBatchToUploadAsync();
            tester(asyncResult);
        }

        [TestMethod]
        public async Task TestGetNextBatchToUpload_DoesNotHaveRecord()
        {

            Action<SevisBatchProcessingDTO> tester = (dto) =>
            {
                Assert.IsNull(dto);
            };
            var result = service.GetNextBatchToUpload();
            tester(result);
            var asyncResult = await service.GetNextBatchToUploadAsync();
            tester(asyncResult);
        }
        #endregion

        #region GetNextByBatchIdToDownload

        [TestMethod]
        public async Task TestGetNextBatchByBatchIdToDownload_DoesNotHaveRetrieveDate()
        {
            var model = new SevisBatchProcessing
            {
                BatchId = "batch id",
                DownloadDispositionCode = "download code",
                Id = 1,
                ProcessDispositionCode = "process code",
                RetrieveDate = null,
                SendString = "send string",
                SubmitDate = DateTimeOffset.UtcNow,
                TransactionLogString = "transaction log",
                UploadDispositionCode = "upload code"
            };
            context.SevisBatchProcessings.Add(model);
            Assert.AreEqual(1, SevisBatchProcessingQueries.CreateGetSevisBatchProcessingDTOsToDownloadQuery(context).Count());

            Action<string> tester = (s) =>
            {
                Assert.IsNotNull(s);
                Assert.AreEqual(model.BatchId, s);
            };
            var result = service.GetNextBatchByBatchIdToDownload();
            tester(result);
            var asyncResult = await service.GetNextBatchByBatchIdToDownloadAsync();
            tester(asyncResult);
        }

        [TestMethod]
        public async Task TestGetNextBatchByBatchIdToDownload_HasGeneralUploadDownloadFailure()
        {
            var model = new SevisBatchProcessing
            {
                BatchId = "batch id",
                DownloadDispositionCode = DispositionCode.GeneralUploadDownloadFailure.Code,
                Id = 1,
                ProcessDispositionCode = "process code",
                RetrieveDate = DateTimeOffset.UtcNow.AddDays(1.0),
                SendString = "send string",
                SubmitDate = DateTimeOffset.UtcNow,
                TransactionLogString = "transaction log",
                UploadDispositionCode = "upload code"
            };
            context.SevisBatchProcessings.Add(model);
            Assert.AreEqual(1, SevisBatchProcessingQueries.CreateGetSevisBatchProcessingDTOsToDownloadQuery(context).Count());

            Action<string> tester = (s) =>
            {
                Assert.IsNotNull(s);
                Assert.AreEqual(model.BatchId, s);
            };
            var result = service.GetNextBatchByBatchIdToDownload();
            tester(result);
            var asyncResult = await service.GetNextBatchByBatchIdToDownloadAsync();
            tester(asyncResult);
        }

        [TestMethod]
        public async Task TestGetNextBatchByBatchIdToDownload_HasBatchNotYetProcessedDownloadCode()
        {
            var model = new SevisBatchProcessing
            {
                BatchId = "batch id",
                DownloadDispositionCode = DispositionCode.BatchNotYetProcessed.Code,
                Id = 1,
                ProcessDispositionCode = "process code",
                RetrieveDate = DateTimeOffset.UtcNow.AddDays(1.0),
                SendString = "send string",
                SubmitDate = DateTimeOffset.UtcNow,
                TransactionLogString = "transaction log",
                UploadDispositionCode = "upload code"
            };
            context.SevisBatchProcessings.Add(model);
            Assert.AreEqual(1, SevisBatchProcessingQueries.CreateGetSevisBatchProcessingDTOsToDownloadQuery(context).Count());

            Action<string> tester = (s) =>
            {
                Assert.IsNotNull(s);
                Assert.AreEqual(model.BatchId, s);
            };
            var result = service.GetNextBatchByBatchIdToDownload();
            tester(result);
            var asyncResult = await service.GetNextBatchByBatchIdToDownloadAsync();
            tester(asyncResult);
        }

        [TestMethod]
        public async Task TestGetNextBatchByBatchIdToDownload_DoesNotHaveRecord()
        {
            Action<SevisBatchProcessingDTO> tester = (dto) =>
            {
                Assert.IsNull(dto);
            };
            var result = service.GetNextBatchToUpload();
            tester(result);
            var asyncResult = await service.GetNextBatchToUploadAsync();
            tester(asyncResult);
        }
        #endregion

        #region Process Transaction Log
        [TestMethod]
        public void GetSevisBatchResultTypeAsJson_HasError()
        {
            var resultType = new ResultType
            {
                ErrorCode = "error code",
                ErrorMessage = "error message",
                status = false,
                statusSpecified = true
            };
            var json = service.GetSevisBatchResultTypeAsJson(resultType);
            Assert.IsNotNull(json);
            var jsonAsArray = JsonConvert.DeserializeObject<List<SimpleSevisBatchErrorResult>>(json);
            Assert.IsNotNull(jsonAsArray);
            Assert.AreEqual(1, jsonAsArray.Count);
            Assert.AreEqual(resultType.ErrorCode, jsonAsArray.First().ErrorCode);
            Assert.AreEqual(resultType.ErrorMessage, jsonAsArray.First().ErrorMessage);
        }

        [TestMethod]
        public void GetSevisBatchResultTypeAsJson_DoesNotHaveError()
        {
            var resultType = new ResultType
            {
                status = true,
                statusSpecified = true
            };
            var json = service.GetSevisBatchResultTypeAsJson(resultType);
            Assert.IsNotNull(json);
            var jsonAsArray = JsonConvert.DeserializeObject<List<SimpleSevisBatchErrorResult>>(json);
            Assert.IsNotNull(jsonAsArray);
            Assert.AreEqual(0, jsonAsArray.Count);
        }

        [TestMethod]
        public void TestAddResultTypeSevisCommStatus_IsSuccess()
        {
            var resultType = new ResultType
            {
                status = true
            };
            var participantPerson = new ParticipantPerson
            {
                ParticipantId = 1
            };
            Assert.AreEqual(0, context.ParticipantPersonSevisCommStatuses.Count());

            service.AddResultTypeSevisCommStatus(resultType, participantPerson);
            Assert.AreEqual(1, context.ParticipantPersonSevisCommStatuses.Count());
            Assert.AreEqual(1, participantPerson.ParticipantPersonSevisCommStatuses.Count());
            Assert.IsTrue(Object.ReferenceEquals(context.ParticipantPersonSevisCommStatuses.First(), participantPerson.ParticipantPersonSevisCommStatuses.First()));

            var firstStatus = context.ParticipantPersonSevisCommStatuses.First();
            Assert.AreEqual(SevisCommStatus.CreatedByBatch.Id, firstStatus.SevisCommStatusId);
            Assert.AreEqual(participantPerson.ParticipantId, firstStatus.ParticipantId);
            DateTimeOffset.UtcNow.Should().BeCloseTo(firstStatus.AddedOn, 20000);
        }

        [TestMethod]
        public void TestAddResultTypeSevisCommStatus_IsError()
        {
            var resultType = new ResultType
            {
                status = false
            };
            var participantPerson = new ParticipantPerson
            {
                ParticipantId = 1
            };
            Assert.AreEqual(0, context.ParticipantPersonSevisCommStatuses.Count());

            service.AddResultTypeSevisCommStatus(resultType, participantPerson);
            Assert.AreEqual(1, context.ParticipantPersonSevisCommStatuses.Count());
            Assert.AreEqual(1, participantPerson.ParticipantPersonSevisCommStatuses.Count());
            Assert.IsTrue(Object.ReferenceEquals(context.ParticipantPersonSevisCommStatuses.First(), participantPerson.ParticipantPersonSevisCommStatuses.First()));

            var firstStatus = context.ParticipantPersonSevisCommStatuses.First();
            Assert.AreEqual(SevisCommStatus.InformationRequired.Id, firstStatus.SevisCommStatusId);
            Assert.AreEqual(participantPerson.ParticipantId, firstStatus.ParticipantId);
            DateTimeOffset.UtcNow.Should().BeCloseTo(firstStatus.AddedOn, 20000);
        }

        [TestMethod]
        public void TestUpdateDependent_CheckProperties()
        {
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var otherUserId = 2;
            var user = new User(1);
            var dependentRecord = new TransactionLogTypeBatchDetailProcessRecordDependent
            {
                dependentSevisID = "sevis id"
            };
            var personDependent = new PersonDependent
            {

            };
            personDependent.History.CreatedBy = otherUserId;
            personDependent.History.CreatedOn = yesterday;
            personDependent.History.RevisedBy = otherUserId;
            personDependent.History.RevisedOn = yesterday;

            service.UpdateDependent(user, dependentRecord, personDependent);
            Assert.AreEqual(dependentRecord.dependentSevisID, personDependent.SevisId);
            Assert.AreEqual(yesterday, personDependent.History.CreatedOn);
            Assert.AreEqual(otherUserId, personDependent.History.CreatedBy);
            Assert.AreEqual(user.Id, personDependent.History.RevisedBy);
            DateTimeOffset.UtcNow.Should().BeCloseTo(personDependent.History.RevisedOn, 20000);
        }

        [TestMethod]
        public void TestUpdateDependent_RecordIsNull()
        {
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var otherUserId = 2;
            var user = new User(1);
            var personDependent = new PersonDependent
            {

            };
            personDependent.History.CreatedBy = otherUserId;
            personDependent.History.CreatedOn = yesterday;
            personDependent.History.RevisedBy = otherUserId;
            personDependent.History.RevisedOn = yesterday;

            service.UpdateDependent(user, null, personDependent);
            Assert.AreEqual(yesterday, personDependent.History.CreatedOn);
            Assert.AreEqual(yesterday, personDependent.History.RevisedOn);
            Assert.AreEqual(otherUserId, personDependent.History.CreatedBy);
            Assert.AreEqual(otherUserId, personDependent.History.RevisedBy);
        }

        [TestMethod]
        public void TestUpdateDependent_DependentIsNull()
        {
            var user = new User(1);
            var dependentRecord = new TransactionLogTypeBatchDetailProcessRecordDependent
            {
                dependentSevisID = "sevis id"
            };
            service.UpdateDependent(user, dependentRecord, null);
        }

        [TestMethod]
        public void TestUpdateParticipant_IsSuccess()
        {
            var user = new User(1);
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var otherUser = new User(user.Id + 1);
            var participant = new Participant
            {
                ParticipantId = 1
            };
            var participantPerson = new ParticipantPerson
            {
                ParticipantId = participant.ParticipantId,
                Participant = participant,
                SevisBatchResult = "sevis batch result",
            };
            participantPerson.History.CreatedBy = otherUser.Id;
            participantPerson.History.CreatedOn = yesterday;
            participantPerson.History.RevisedBy = otherUser.Id;
            participantPerson.History.RevisedOn = yesterday;

            participant.ParticipantPerson = participantPerson;
            var record = new TransactionLogTypeBatchDetailProcessRecord
            {
                sevisID = "sevis Id",
                Result = new ResultType
                {
                    status = true
                },
                Dependent = null
            };

            service.UpdateParticipant(user, participantPerson, new List<PersonDependent>(), record);
            Assert.AreEqual(record.sevisID, participantPerson.SevisId);            
            Assert.AreEqual(yesterday, participantPerson.History.CreatedOn);
            Assert.AreEqual(otherUser.Id, participantPerson.History.CreatedBy);
            Assert.AreEqual(user.Id, participantPerson.History.RevisedBy);
            DateTimeOffset.UtcNow.Should().BeCloseTo(participantPerson.History.RevisedOn, 20000);

            Assert.AreEqual(1, participantPerson.ParticipantPersonSevisCommStatuses.Count());
            var firstStatus = participantPerson.ParticipantPersonSevisCommStatuses.First();
            Assert.AreEqual(SevisCommStatus.CreatedByBatch.Id, firstStatus.SevisCommStatusId);
            firstStatus.AddedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, 20000);

            Assert.IsNotNull(participantPerson.SevisBatchResult);
            var jsonAsArray = JsonConvert.DeserializeObject<List<SimpleSevisBatchErrorResult>>(participantPerson.SevisBatchResult);
            Assert.IsNotNull(jsonAsArray);
            Assert.AreEqual(0, jsonAsArray.Count);
        }

        [TestMethod]
        public void TestUpdateParticipant_IsError()
        {
            var user = new User(1);
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var otherUser = new User(user.Id + 1);
            var participant = new Participant
            {
                ParticipantId = 1
            };
            var participantPerson = new ParticipantPerson
            {
                ParticipantId = participant.ParticipantId,
                Participant = participant,
                SevisBatchResult = "sevis batch result",
            };
            participantPerson.History.CreatedBy = otherUser.Id;
            participantPerson.History.CreatedOn = yesterday;
            participantPerson.History.RevisedBy = otherUser.Id;
            participantPerson.History.RevisedOn = yesterday;

            participant.ParticipantPerson = participantPerson;
            var record = new TransactionLogTypeBatchDetailProcessRecord
            {
                sevisID = "sevis Id",
                Result = new ResultType
                {
                    status = false
                },
                Dependent = null
            };

            service.UpdateParticipant(user, participantPerson, new List<PersonDependent>(), record);
            Assert.IsNull(participantPerson.SevisId);

            Assert.AreEqual(yesterday, participantPerson.History.CreatedOn);
            Assert.AreEqual(otherUser.Id, participantPerson.History.CreatedBy);
            Assert.AreEqual(user.Id, participantPerson.History.RevisedBy);
            DateTimeOffset.UtcNow.Should().BeCloseTo(participantPerson.History.RevisedOn, 20000);

            Assert.AreEqual(1, participantPerson.ParticipantPersonSevisCommStatuses.Count());
            var firstStatus = participantPerson.ParticipantPersonSevisCommStatuses.First();
            Assert.AreEqual(SevisCommStatus.InformationRequired.Id, firstStatus.SevisCommStatusId);
            firstStatus.AddedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, 20000);

            Assert.IsNotNull(participantPerson.SevisBatchResult);
            var json = JsonConvert.DeserializeObject<List<SimpleSevisBatchErrorResult>>(participantPerson.SevisBatchResult);
            Assert.AreEqual(1, json.Count);
        }

        [TestMethod]
        public void TestUpdateParticipant_CheckDependentHasOneDependent_SuccessfulSevisRequest()
        {
            var user = new User(1);
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var otherUser = new User(user.Id + 1);

            var person = new Data.Person
            {
                PersonId = 10,

            };
            var participant = new Participant
            {
                ParticipantId = 1
            };
            var participantPerson = new ParticipantPerson
            {
                ParticipantId = participant.ParticipantId,
                Participant = participant,
                SevisBatchResult = "sevis batch result",
            };
            participantPerson.History.CreatedBy = otherUser.Id;
            participantPerson.History.CreatedOn = yesterday;
            participantPerson.History.RevisedBy = otherUser.Id;
            participantPerson.History.RevisedOn = yesterday;
            participant.ParticipantPerson = participantPerson;
            var personDependent = new PersonDependent
            {
                DependentId = 250,
                Person = person,
                PersonId = person.PersonId,
                SevisId = null
            };
            personDependent.History.CreatedBy = otherUser.Id;
            personDependent.History.CreatedOn = yesterday;
            personDependent.History.RevisedBy = otherUser.Id;
            personDependent.History.RevisedOn = yesterday;
            var processedDepenent = new TransactionLogTypeBatchDetailProcessRecordDependent
            {
                dependentSevisID = "dep sevis id",
            };
            SetUserDefinedFields(processedDepenent, participant.ParticipantId, personDependent.DependentId);
            var record = new TransactionLogTypeBatchDetailProcessRecord
            {
                sevisID = "sevis Id",
                Result = new ResultType
                {
                    status = true
                },
                Dependent = new List<TransactionLogTypeBatchDetailProcessRecordDependent>
                {
                    processedDepenent
                }.ToArray()
            };

            service.UpdateParticipant(user, participantPerson, new List<PersonDependent> { personDependent }, record);
            Assert.AreEqual(yesterday, personDependent.History.CreatedOn);
            Assert.AreEqual(otherUser.Id, personDependent.History.CreatedBy);
            Assert.AreEqual(user.Id, personDependent.History.RevisedBy);
            DateTimeOffset.UtcNow.Should().BeCloseTo(personDependent.History.RevisedOn, 20000);
            Assert.AreEqual(processedDepenent.dependentSevisID, personDependent.SevisId);
        }

        [TestMethod]
        public void TestUpdateParticipant_MultipleDependents_SuccessfulSevisRequest()
        {
            var user = new User(1);
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var otherUser = new User(user.Id + 1);

            var person = new Data.Person
            {
                PersonId = 10,

            };
            var participant = new Participant
            {
                ParticipantId = 1
            };
            var participantPerson = new ParticipantPerson
            {
                ParticipantId = participant.ParticipantId,
                Participant = participant,
                SevisBatchResult = "sevis batch result",
            };
            participantPerson.History.CreatedBy = otherUser.Id;
            participantPerson.History.CreatedOn = yesterday;
            participantPerson.History.RevisedBy = otherUser.Id;
            participantPerson.History.RevisedOn = yesterday;
            participant.ParticipantPerson = participantPerson;
            var personDependentToUpdate = new PersonDependent
            {
                DependentId = 250,
                Person = person,
                PersonId = person.PersonId,
                SevisId = null
            };
            personDependentToUpdate.History.CreatedBy = otherUser.Id;
            personDependentToUpdate.History.CreatedOn = yesterday;
            personDependentToUpdate.History.RevisedBy = otherUser.Id;
            personDependentToUpdate.History.RevisedOn = yesterday;
            var otherPersonDependent = new PersonDependent
            {
                DependentId = 251
            };
            var processedDepenent = new TransactionLogTypeBatchDetailProcessRecordDependent
            {
                dependentSevisID = "dep sevis id",
            };
            SetUserDefinedFields(processedDepenent, participant.ParticipantId, personDependentToUpdate.DependentId);

            var record = new TransactionLogTypeBatchDetailProcessRecord
            {
                sevisID = "sevis Id",
                Result = new ResultType
                {
                    status = true
                },
                Dependent = new List<TransactionLogTypeBatchDetailProcessRecordDependent>
                {
                    processedDepenent
                }.ToArray()
            };

            service.UpdateParticipant(user, participantPerson, new List<PersonDependent> { personDependentToUpdate, otherPersonDependent }, record);

            Assert.IsNull(otherPersonDependent.SevisId);
            Assert.AreEqual(processedDepenent.dependentSevisID, personDependentToUpdate.SevisId);
        }

        [TestMethod]
        public async Task TestProcessBatchDetailProcess_HasDS2019File()
        {
            var sevisId = "sevis id";
            var user = new User(1);
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var otherUser = new User(user.Id + 1);
            Participant participant = null;
            ParticipantPerson participantPerson = null;
            Data.Person person = null;
            SevisBatchProcessing batch = null;
            var participantId = 1;
            var personId = 2;
            context.SetupActions.Add(() =>
            {
                batch = new SevisBatchProcessing
                {
                    BatchId = "hello",
                    Id = 1
                };
                participant = new Participant
                {
                    ParticipantId = participantId
                };
                participantPerson = new ParticipantPerson
                {
                    ParticipantId = participant.ParticipantId,
                    Participant = participant,
                    SevisBatchResult = "sevis batch result",
                };
                participantPerson.History.CreatedBy = otherUser.Id;
                participantPerson.History.CreatedOn = yesterday;
                participantPerson.History.RevisedBy = otherUser.Id;
                participantPerson.History.RevisedOn = yesterday;

                participant.ParticipantPerson = participantPerson;
                person = new Data.Person
                {
                    PersonId = personId
                };
                participant.Person = person;
                participant.PersonId = person.PersonId;
                context.Participants.Add(participant);
                context.ParticipantPersons.Add(participantPerson);
                context.People.Add(person);
                context.SevisBatchProcessings.Add(batch);
            });

            var record = new TransactionLogTypeBatchDetailProcessRecord
            {
                sevisID = sevisId,
                Result = new ResultType
                {
                    status = true
                },
                Dependent = null,
            };
            SetUserDefinedFields(record, participantId, personId);
            var processDetail = new TransactionLogTypeBatchDetailProcess
            {
                Record = new List<TransactionLogTypeBatchDetailProcessRecord> { record }.ToArray(),
                resultCode = DispositionCode.BusinessRuleViolations.Code,
                RecordCount = new TransactionLogTypeBatchDetailProcessRecordCount
                {
                    Failure = "1",
                    Success = "2"
                }
            };
            var url = "url";
            var fileContents = new byte[1] { (byte)1 };
            fileProvider.Setup(x => x.GetDS2019File(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())).Returns(fileContents);
            fileProvider.Setup(x => x.GetDS2019FileAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(fileContents);
            cloudStorageService.Setup(x => x.SaveFile(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<string>())).Returns(url);
            cloudStorageService.Setup(x => x.SaveFileAsync(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<string>())).ReturnsAsync(url);
            Action tester = () =>
            {
                Assert.AreEqual(sevisId, participantPerson.SevisId);
                Assert.AreEqual(1, context.ParticipantPersonSevisCommStatuses.Count());
                Assert.AreEqual(processDetail.resultCode, batch.ProcessDispositionCode);
                Assert.AreEqual(url, participantPerson.DS2019FileUrl);
            };
            context.Revert();
            service.ProcessBatchDetailProcess(user, processDetail, batch, fileProvider.Object);
            tester();
            notificationService.Verify(x => x.NotifyFinishedProcessingSevisBatchDetails(It.IsAny<string>(), It.IsAny<DispositionCode>()), Times.Exactly(1));
            notificationService.Verify(x => x.NotifyStartedProcessingSevisBatchDetails(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(1));
            cloudStorageService.Verify(x => x.SaveFile(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<string>()), Times.Exactly(1));
            fileProvider.Verify(x => x.GetDS2019File(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(1));

            context.Revert();
            await service.ProcessBatchDetailProcessAsync(user, processDetail, batch, fileProvider.Object);
            tester();
            notificationService.Verify(x => x.NotifyFinishedProcessingSevisBatchDetails(It.IsAny<string>(), It.IsAny<DispositionCode>()), Times.Exactly(2));
            notificationService.Verify(x => x.NotifyStartedProcessingSevisBatchDetails(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(2));
            cloudStorageService.Verify(x => x.SaveFileAsync(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<string>()), Times.Exactly(1));
            fileProvider.Verify(x => x.GetDS2019FileAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(1));
        }


        [TestMethod]
        public async Task TestProcessBatchDetailProcess_DS2019FileIsEmpty()
        {
            var sevisId = "sevis id";
            var user = new User(1);
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var otherUser = new User(user.Id + 1);
            Participant participant = null;
            ParticipantPerson participantPerson = null;
            Data.Person person = null;
            SevisBatchProcessing batch = null;
            var participantId = 1;
            var personId = 2;
            context.SetupActions.Add(() =>
            {
                batch = new SevisBatchProcessing
                {
                    BatchId = "hello",
                    Id = 1
                };
                participant = new Participant
                {
                    ParticipantId = participantId
                };
                participantPerson = new ParticipantPerson
                {
                    ParticipantId = participant.ParticipantId,
                    Participant = participant,
                    SevisBatchResult = "sevis batch result",
                };
                participantPerson.History.CreatedBy = otherUser.Id;
                participantPerson.History.CreatedOn = yesterday;
                participantPerson.History.RevisedBy = otherUser.Id;
                participantPerson.History.RevisedOn = yesterday;

                participant.ParticipantPerson = participantPerson;
                person = new Data.Person
                {
                    PersonId = personId
                };
                participant.Person = person;
                participant.PersonId = person.PersonId;
                context.Participants.Add(participant);
                context.ParticipantPersons.Add(participantPerson);
                context.People.Add(person);
                context.SevisBatchProcessings.Add(batch);
            });

            var record = new TransactionLogTypeBatchDetailProcessRecord
            {
                sevisID = sevisId,
                Result = new ResultType
                {
                    status = true
                },
                Dependent = null,
            };
            SetUserDefinedFields(record, participantId, personId);
            var processDetail = new TransactionLogTypeBatchDetailProcess
            {
                Record = new List<TransactionLogTypeBatchDetailProcessRecord> { record }.ToArray(),
                resultCode = DispositionCode.BusinessRuleViolations.Code,
                RecordCount = new TransactionLogTypeBatchDetailProcessRecordCount
                {
                    Failure = "1",
                    Success = "2"
                }
            };
            var fileContents = new byte[0];
            fileProvider.Setup(x => x.GetDS2019File(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())).Returns(fileContents);
            fileProvider.Setup(x => x.GetDS2019FileAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(fileContents);

            Action tester = () =>
            {
                Assert.IsNull(participantPerson.DS2019FileUrl);
            };
            context.Revert();
            service.ProcessBatchDetailProcess(user, processDetail, batch, fileProvider.Object);
            tester();
            cloudStorageService.Verify(x => x.SaveFile(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<string>()), Times.Exactly(0));
            fileProvider.Verify(x => x.GetDS2019File(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(1));

            context.Revert();
            await service.ProcessBatchDetailProcessAsync(user, processDetail, batch, fileProvider.Object);
            tester();
            cloudStorageService.Verify(x => x.SaveFileAsync(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<string>()), Times.Exactly(0));
            fileProvider.Verify(x => x.GetDS2019FileAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(1));
        }

        [TestMethod]
        public async Task TestProcessBatchDetailProcess_DS2019FileIsNull()
        {
            var sevisId = "sevis id";
            var user = new User(1);
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var otherUser = new User(user.Id + 1);
            Participant participant = null;
            ParticipantPerson participantPerson = null;
            Data.Person person = null;
            SevisBatchProcessing batch = null;
            var participantId = 1;
            var personId = 2;
            context.SetupActions.Add(() =>
            {
                batch = new SevisBatchProcessing
                {
                    BatchId = "hello",
                    Id = 1
                };
                participant = new Participant
                {
                    ParticipantId = participantId
                };
                participantPerson = new ParticipantPerson
                {
                    ParticipantId = participant.ParticipantId,
                    Participant = participant,
                    SevisBatchResult = "sevis batch result",
                };
                participantPerson.History.CreatedBy = otherUser.Id;
                participantPerson.History.CreatedOn = yesterday;
                participantPerson.History.RevisedBy = otherUser.Id;
                participantPerson.History.RevisedOn = yesterday;

                participant.ParticipantPerson = participantPerson;
                person = new Data.Person
                {
                    PersonId = personId
                };
                participant.Person = person;
                participant.PersonId = person.PersonId;
                context.Participants.Add(participant);
                context.ParticipantPersons.Add(participantPerson);
                context.People.Add(person);
                context.SevisBatchProcessings.Add(batch);
            });

            var record = new TransactionLogTypeBatchDetailProcessRecord
            {
                sevisID = sevisId,
                Result = new ResultType
                {
                    status = true
                },
                Dependent = null,
            };
            SetUserDefinedFields(record, participantId, personId);
            var processDetail = new TransactionLogTypeBatchDetailProcess
            {
                Record = new List<TransactionLogTypeBatchDetailProcessRecord> { record }.ToArray(),
                resultCode = DispositionCode.BusinessRuleViolations.Code,
                RecordCount = new TransactionLogTypeBatchDetailProcessRecordCount
                {
                    Failure = "1",
                    Success = "2"
                }
            };
            byte[] fileContents = null;
            fileProvider.Setup(x => x.GetDS2019File(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())).Returns(fileContents);
            fileProvider.Setup(x => x.GetDS2019FileAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(fileContents);

            Action tester = () =>
            {
                Assert.IsNull(participantPerson.DS2019FileUrl);
            };
            context.Revert();
            service.ProcessBatchDetailProcess(user, processDetail, batch, fileProvider.Object);
            tester();
            cloudStorageService.Verify(x => x.SaveFile(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<string>()), Times.Exactly(0));
            fileProvider.Verify(x => x.GetDS2019File(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(1));

            context.Revert();
            await service.ProcessBatchDetailProcessAsync(user, processDetail, batch, fileProvider.Object);
            tester();
            cloudStorageService.Verify(x => x.SaveFileAsync(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<string>()), Times.Exactly(0));
            fileProvider.Verify(x => x.GetDS2019FileAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(1));
        }

        [TestMethod]
        public async Task TestProcessBatchDetailProcess_CheckFileProviderCallback()
        {
            var sevisId = "sevis id";
            var user = new User(1);
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var otherUser = new User(user.Id + 1);
            Participant participant = null;
            ParticipantPerson participantPerson = null;
            Data.Person person = null;
            SevisBatchProcessing batch = null;
            var participantId = 1;
            var personId = 2;
            context.SetupActions.Add(() =>
            {
                batch = new SevisBatchProcessing
                {
                    BatchId = "hello",
                    Id = 1
                };
                participant = new Participant
                {
                    ParticipantId = participantId
                };
                participantPerson = new ParticipantPerson
                {
                    ParticipantId = participant.ParticipantId,
                    Participant = participant,
                    SevisBatchResult = "sevis batch result",
                };
                participantPerson.History.CreatedBy = otherUser.Id;
                participantPerson.History.CreatedOn = yesterday;
                participantPerson.History.RevisedBy = otherUser.Id;
                participantPerson.History.RevisedOn = yesterday;

                participant.ParticipantPerson = participantPerson;
                person = new Data.Person
                {
                    PersonId = personId
                };
                participant.Person = person;
                participant.PersonId = person.PersonId;
                context.Participants.Add(participant);
                context.ParticipantPersons.Add(participantPerson);
                context.People.Add(person);
                context.SevisBatchProcessings.Add(batch);
            });

            var record = new TransactionLogTypeBatchDetailProcessRecord
            {
                sevisID = sevisId,
                Result = new ResultType
                {
                    status = true
                },
                Dependent = null,
            };
            SetUserDefinedFields(record, participantId, personId);
            var processDetail = new TransactionLogTypeBatchDetailProcess
            {
                Record = new List<TransactionLogTypeBatchDetailProcessRecord> { record }.ToArray(),
                resultCode = DispositionCode.BusinessRuleViolations.Code,
                RecordCount = new TransactionLogTypeBatchDetailProcessRecordCount
                {
                    Failure = "1",
                    Success = "2"
                }
            };
            Action<int, string, string> fileProviderCallback = (partId, batId, sevId) =>
            {
                Assert.AreEqual(participantId, partId);
                Assert.AreEqual(batch.BatchId, batId);
                Assert.AreEqual(sevisId, sevId);
            };
            fileProvider.Setup(x => x.GetDS2019File(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new byte[0]).Callback(fileProviderCallback);
            fileProvider.Setup(x => x.GetDS2019FileAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult<byte[]>(new byte[0])).Callback(fileProviderCallback);

            context.Revert();
            service.ProcessBatchDetailProcess(user, processDetail, batch, fileProvider.Object);

            context.Revert();
            await service.ProcessBatchDetailProcessAsync(user, processDetail, batch, fileProvider.Object);

        }

        [TestMethod]
        public async Task TestProcessBatchDetailProcess_CheckCloudStorageCallback()
        {
            var sevisId = "sevis id";
            var user = new User(1);
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var otherUser = new User(user.Id + 1);
            Participant participant = null;
            ParticipantPerson participantPerson = null;
            Data.Person person = null;
            SevisBatchProcessing batch = null;
            var participantId = 1;
            var personId = 2;
            context.SetupActions.Add(() =>
            {
                batch = new SevisBatchProcessing
                {
                    BatchId = "hello",
                    Id = 1
                };
                participant = new Participant
                {
                    ParticipantId = participantId
                };
                participantPerson = new ParticipantPerson
                {
                    ParticipantId = participant.ParticipantId,
                    Participant = participant,
                    SevisBatchResult = "sevis batch result",
                };
                participantPerson.History.CreatedBy = otherUser.Id;
                participantPerson.History.CreatedOn = yesterday;
                participantPerson.History.RevisedBy = otherUser.Id;
                participantPerson.History.RevisedOn = yesterday;

                participant.ParticipantPerson = participantPerson;
                person = new Data.Person
                {
                    PersonId = personId
                };
                participant.Person = person;
                participant.PersonId = person.PersonId;
                context.Participants.Add(participant);
                context.ParticipantPersons.Add(participantPerson);
                context.People.Add(person);
                context.SevisBatchProcessings.Add(batch);
            });

            var record = new TransactionLogTypeBatchDetailProcessRecord
            {
                sevisID = sevisId,
                Result = new ResultType
                {
                    status = true
                },
                Dependent = null,
            };
            SetUserDefinedFields(record, participantId, personId);
            var processDetail = new TransactionLogTypeBatchDetailProcess
            {
                Record = new List<TransactionLogTypeBatchDetailProcessRecord> { record }.ToArray(),
                resultCode = DispositionCode.BusinessRuleViolations.Code,
                RecordCount = new TransactionLogTypeBatchDetailProcessRecordCount
                {
                    Failure = "1",
                    Success = "2"
                }
            };
            var url = "url";
            var fileContents = new byte[1] { (byte)1 };
            fileProvider.Setup(x => x.GetDS2019File(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())).Returns(fileContents);
            fileProvider.Setup(x => x.GetDS2019FileAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(fileContents);
            cloudStorageService.Setup(x => x.SaveFile(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<string>())).Returns(url);
            cloudStorageService.Setup(x => x.SaveFileAsync(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<string>())).ReturnsAsync(url);

            Action<string, byte[], string> cloudStorageCallback = (fName, fContents, contentType) =>
            {
                Assert.AreEqual(SevisBatchProcessingService.GetDS2019FileName(participantId, sevisId), fName);
                Assert.AreEqual(SevisBatchProcessingService.DS2019_CONTENT_TYPE, contentType);
            };
            cloudStorageService.Setup(x => x.SaveFile(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<string>()))
                .Returns(url)
                .Callback(cloudStorageCallback);

            cloudStorageService.Setup(x => x.SaveFileAsync(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<string>()))
                .Returns(Task.FromResult<string>(url))
                .Callback(cloudStorageCallback);

            context.Revert();
            service.ProcessBatchDetailProcess(user, processDetail, batch, fileProvider.Object);

            context.Revert();
            await service.ProcessBatchDetailProcessAsync(user, processDetail, batch, fileProvider.Object);

        }

        [TestMethod]
        public async Task TestProcessBatchDetailProcess_ProcessIsNull()
        {
            var user = new User(1);
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var otherUser = new User(user.Id + 1);
            Participant participant = null;
            ParticipantPerson participantPerson = null;
            Data.Person person = null;
            SevisBatchProcessing batch = null;

            var participantId = 1;
            var personId = 2;
            context.SetupActions.Add(() =>
            {
                batch = new SevisBatchProcessing
                {
                    BatchId = "hello",
                    Id = 1
                };
                participant = new Participant
                {
                    ParticipantId = participantId
                };
                participantPerson = new ParticipantPerson
                {
                    ParticipantId = participant.ParticipantId,
                    Participant = participant,
                    SevisBatchResult = "sevis batch result",
                };
                participantPerson.History.CreatedBy = otherUser.Id;
                participantPerson.History.CreatedOn = yesterday;
                participantPerson.History.RevisedBy = otherUser.Id;
                participantPerson.History.RevisedOn = yesterday;

                participant.ParticipantPerson = participantPerson;
                person = new Data.Person
                {
                    PersonId = personId
                };
                participant.Person = person;
                participant.PersonId = person.PersonId;
                context.Participants.Add(participant);
                context.ParticipantPersons.Add(participantPerson);
                context.People.Add(person);
                context.SevisBatchProcessings.Add(batch);
            });
            Action tester = () =>
            {
                Assert.IsNull(participantPerson.SevisId);
                Assert.AreEqual(0, context.ParticipantPersonSevisCommStatuses.Count());
            };

            context.Revert();
            service.ProcessBatchDetailProcess(user, null, batch, fileProvider.Object);
            tester();
            notificationService.Verify(x => x.NotifyFinishedProcessingSevisBatchDetails(It.IsAny<string>(), It.IsAny<DispositionCode>()), Times.Exactly(0));
            notificationService.Verify(x => x.NotifyStartedProcessingSevisBatchDetails(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never());
            cloudStorageService.Verify(x => x.SaveFile(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<string>()), Times.Exactly(0));
            fileProvider.Verify(x => x.GetDS2019File(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(0));

            context.Revert();
            await service.ProcessBatchDetailProcessAsync(user, null, batch, fileProvider.Object);
            tester();
            notificationService.Verify(x => x.NotifyFinishedProcessingSevisBatchDetails(It.IsAny<string>(), It.IsAny<DispositionCode>()), Times.Exactly(0));
            notificationService.Verify(x => x.NotifyStartedProcessingSevisBatchDetails(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never());
            cloudStorageService.Verify(x => x.SaveFileAsync(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<string>()), Times.Exactly(0));
            fileProvider.Verify(x => x.GetDS2019FileAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(0));
        }

        [TestMethod]
        public async Task TestProcessBatchDetailProcess_NoRecords()
        {
            var user = new User(1);
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var otherUser = new User(user.Id + 1);
            Participant participant = null;
            ParticipantPerson participantPerson = null;
            Data.Person person = null;
            SevisBatchProcessing batch = null;

            var participantId = 1;
            var personId = 2;
            context.SetupActions.Add(() =>
            {
                batch = new SevisBatchProcessing
                {
                    BatchId = "hello",
                    Id = 1
                };
                participant = new Participant
                {
                    ParticipantId = participantId
                };
                participantPerson = new ParticipantPerson
                {
                    ParticipantId = participant.ParticipantId,
                    Participant = participant,
                    SevisBatchResult = "sevis batch result",
                };
                participantPerson.History.CreatedBy = otherUser.Id;
                participantPerson.History.CreatedOn = yesterday;
                participantPerson.History.RevisedBy = otherUser.Id;
                participantPerson.History.RevisedOn = yesterday;

                participant.ParticipantPerson = participantPerson;
                person = new Data.Person
                {
                    PersonId = personId
                };
                participant.Person = person;
                participant.PersonId = person.PersonId;
                context.Participants.Add(participant);
                context.ParticipantPersons.Add(participantPerson);
                context.People.Add(person);
                context.SevisBatchProcessings.Add(batch);
            });
            Action tester = () =>
            {
                Assert.IsNull(participantPerson.SevisId);
                Assert.AreEqual(0, context.ParticipantPersonSevisCommStatuses.Count());
            };

            var processDetail = new TransactionLogTypeBatchDetailProcess
            {
                Record = new List<TransactionLogTypeBatchDetailProcessRecord>().ToArray(),
                resultCode = DispositionCode.BusinessRuleViolations.Code,
                RecordCount = new TransactionLogTypeBatchDetailProcessRecordCount
                {
                    Failure = "1",
                    Success = "2"
                }
            };
            context.Revert();
            service.ProcessBatchDetailProcess(user, processDetail, batch, fileProvider.Object);
            tester();
            notificationService.Verify(x => x.NotifyFinishedProcessingSevisBatchDetails(It.IsAny<string>(), It.IsAny<DispositionCode>()), Times.Exactly(1));
            notificationService.Verify(x => x.NotifyStartedProcessingSevisBatchDetails(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(1));
            cloudStorageService.Verify(x => x.SaveFile(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<string>()), Times.Exactly(0));
            fileProvider.Verify(x => x.GetDS2019File(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(0));

            context.Revert();
            await service.ProcessBatchDetailProcessAsync(user, processDetail, batch, fileProvider.Object);
            tester();
            notificationService.Verify(x => x.NotifyFinishedProcessingSevisBatchDetails(It.IsAny<string>(), It.IsAny<DispositionCode>()), Times.Exactly(2));
            notificationService.Verify(x => x.NotifyStartedProcessingSevisBatchDetails(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(2));
            cloudStorageService.Verify(x => x.SaveFileAsync(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<string>()), Times.Exactly(0));
            fileProvider.Verify(x => x.GetDS2019FileAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(0));
        }

        [TestMethod]
        public void TestProcessDownload()
        {
            var sevisBatch = new SevisBatchProcessing
            {

            };
            var downloadDetail = new TransactionLogTypeBatchDetailDownload
            {
                resultCode = DispositionCode.BusinessRuleViolations.Code,
            };

            service.ProcessDownload(downloadDetail, sevisBatch);
            DateTimeOffset.UtcNow.Should().BeCloseTo(sevisBatch.RetrieveDate.Value, 20000);
            Assert.AreEqual(downloadDetail.resultCode, sevisBatch.DownloadDispositionCode);
            Assert.IsNull(sevisBatch.UploadDispositionCode);
            Assert.IsNull(sevisBatch.ProcessDispositionCode);
            notificationService.Verify(x => x.NotifyDownloadedBatchProcessed(It.IsAny<string>(), It.IsAny<DispositionCode>()), Times.Exactly(1));
        }

        [TestMethod]
        public void TestProcessDownload_NullTransactionLogTypeBatchDetailDownload()
        {
            var sevisBatch = new SevisBatchProcessing
            {

            };

            service.ProcessDownload(null, sevisBatch);
            Assert.IsNull(sevisBatch.RetrieveDate);
            Assert.IsNull(sevisBatch.DownloadDispositionCode);
            Assert.IsNull(sevisBatch.UploadDispositionCode);
            Assert.IsNull(sevisBatch.ProcessDispositionCode);
        }

        [TestMethod]
        public async Task TestProcessUpload()
        {
            var participantId = 1;
            var batchId = "batchId";
            SevisBatchProcessing sevisBatch = null;
            var today = DateTime.UtcNow;
            var uploadDetail = new TransactionLogTypeBatchDetailUpload
            {
                resultCode = DispositionCode.Success.Code,
                dateTimeStamp = today
            };
            ParticipantPerson person = null;
            ParticipantPersonSevisCommStatus sentByBatch = null;

            context.SetupActions.Add(() =>
            {
                person = new ParticipantPerson
                {
                    ParticipantId = participantId
                };
                sentByBatch = new ParticipantPersonSevisCommStatus
                {
                    BatchId = batchId,
                    AddedOn = DateTimeOffset.UtcNow,
                    ParticipantId = participantId,
                    ParticipantPerson = person,
                    SevisCommStatusId = SevisCommStatus.SentByBatch.Id
                };
                person.ParticipantPersonSevisCommStatuses.Add(sentByBatch);
                sevisBatch = new SevisBatchProcessing
                {
                    BatchId = batchId
                };
                context.ParticipantPersonSevisCommStatuses.Add(sentByBatch);
                context.ParticipantPersons.Add(person);
                context.SevisBatchProcessings.Add(sevisBatch);
            });
            Action tester = () =>
            {
                Assert.AreEqual(today, sevisBatch.SubmitDate);
                Assert.AreEqual(uploadDetail.resultCode, sevisBatch.UploadDispositionCode);
                Assert.IsNull(sevisBatch.DownloadDispositionCode);
                Assert.IsNull(sevisBatch.ProcessDispositionCode);
                Assert.AreEqual(2, context.ParticipantPersonSevisCommStatuses.Count());

                var addedCommStatus = context.ParticipantPersonSevisCommStatuses.Last();
                Assert.AreEqual(participantId, addedCommStatus.ParticipantId);
                Assert.AreEqual(batchId, addedCommStatus.BatchId);
                DateTimeOffset.UtcNow.Should().BeCloseTo(addedCommStatus.AddedOn, 20000);
                Assert.AreEqual(SevisCommStatus.SentByBatch.Id, addedCommStatus.SevisCommStatusId);
            };
            context.Revert();
            service.ProcessUpload(uploadDetail, sevisBatch);
            tester();
            notificationService.Verify(x => x.NotifyUploadedBatchProcessed(It.IsAny<string>(), It.IsAny<DispositionCode>()), Times.Exactly(1));

            context.Revert();
            await service.ProcessUploadAsync(uploadDetail, sevisBatch);
            tester();
            notificationService.Verify(x => x.NotifyUploadedBatchProcessed(It.IsAny<string>(), It.IsAny<DispositionCode>()), Times.Exactly(2));
        }

        [TestMethod]
        public async Task TestProcessUpload_DispositionCodeIsNotSuccess()
        {
            var participantId = 1;
            var batchId = "batchId";
            SevisBatchProcessing sevisBatch = null;
            var today = DateTime.UtcNow;
            var uploadDetail = new TransactionLogTypeBatchDetailUpload
            {
                resultCode = DispositionCode.BusinessRuleViolations.Code,
                dateTimeStamp = today
            };
            ParticipantPerson person = null;


            context.SetupActions.Add(() =>
            {
                person = new ParticipantPerson
                {
                    ParticipantId = participantId
                };
                sevisBatch = new SevisBatchProcessing
                {
                    BatchId = batchId
                };
                context.ParticipantPersons.Add(person);
                context.SevisBatchProcessings.Add(sevisBatch);
            });
            Action tester = () =>
            {
                Assert.AreEqual(today, sevisBatch.SubmitDate);
                Assert.AreEqual(uploadDetail.resultCode, sevisBatch.UploadDispositionCode);
                Assert.IsNull(sevisBatch.DownloadDispositionCode);
                Assert.IsNull(sevisBatch.ProcessDispositionCode);
                Assert.AreEqual(0, context.ParticipantPersonSevisCommStatuses.Count());
            };
            context.Revert();
            service.ProcessUpload(uploadDetail, sevisBatch);
            tester();
            notificationService.Verify(x => x.NotifyUploadedBatchProcessed(It.IsAny<string>(), It.IsAny<DispositionCode>()), Times.Exactly(1));

            context.Revert();
            await service.ProcessUploadAsync(uploadDetail, sevisBatch);
            tester();
            notificationService.Verify(x => x.NotifyUploadedBatchProcessed(It.IsAny<string>(), It.IsAny<DispositionCode>()), Times.Exactly(2));
        }

        [TestMethod]
        public void TestProcessUpload_NullTransactionLogTypeBatchDetailUpload()
        {
            var sevisBatch = new SevisBatchProcessing
            {

            };
            service.ProcessUpload(null, sevisBatch);
            Assert.IsNull(sevisBatch.SubmitDate);
            Assert.IsNull(sevisBatch.DownloadDispositionCode);
            Assert.IsNull(sevisBatch.UploadDispositionCode);
            Assert.IsNull(sevisBatch.ProcessDispositionCode);
        }

        [TestMethod]
        public async Task TestProcessTransactionLog_SevisBatchProcessingDoesNotExist()
        {
            var batchId = "hello";
            var user = new User(1);

            var message = String.Format("The SEVIS batch processing record with the batch id [{0}] was not found.", batchId);
            var transactionLog = new TransactionLogType
            {
                BatchHeader = new TransactionLogTypeBatchHeader
                {
                    BatchID = batchId
                },
            };
            var xml = GetXml(transactionLog);
            Action a = () => service.ProcessTransactionLog(user, xml, fileProvider.Object);
            Func<Task> f = () => service.ProcessTransactionLogAsync(user, xml, fileProvider.Object);
            a.ShouldThrow<ModelNotFoundException>().WithMessage(message);
            f.ShouldThrow<ModelNotFoundException>().WithMessage(message);
        }

        [TestMethod]
        public async Task TestProcessTransactionLog_HasProcessRecords()
        {
            var batchId = "hello";
            var sevisId = "sevis id";
            var user = new User(1);
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var otherUser = new User(user.Id + 1);
            Participant participant = null;
            ParticipantPerson participantPerson = null;
            Data.Person person = null;
            SevisBatchProcessing batch = null;

            var participantId = 1;
            var personId = 2;
            context.SetupActions.Add(() =>
            {
                batch = new SevisBatchProcessing
                {
                    BatchId = batchId,
                    Id = 1
                };
                participant = new Participant
                {
                    ParticipantId = participantId
                };
                participantPerson = new ParticipantPerson
                {
                    ParticipantId = participant.ParticipantId,
                    Participant = participant,
                    SevisBatchResult = "sevis batch result",
                };
                participantPerson.History.CreatedBy = otherUser.Id;
                participantPerson.History.CreatedOn = yesterday;
                participantPerson.History.RevisedBy = otherUser.Id;
                participantPerson.History.RevisedOn = yesterday;

                participant.ParticipantPerson = participantPerson;
                person = new Data.Person
                {
                    PersonId = personId
                };
                participant.Person = person;
                participant.PersonId = person.PersonId;
                context.Participants.Add(participant);
                context.ParticipantPersons.Add(participantPerson);
                context.People.Add(person);
                context.SevisBatchProcessings.Add(batch);
            });
            Action tester = () =>
            {
                Assert.AreEqual(1, context.SevisBatchProcessings.Count());
                Assert.AreEqual(sevisId, participantPerson.SevisId);
                Assert.AreEqual(1, context.ParticipantPersonSevisCommStatuses.Count());
            };
            var record = new TransactionLogTypeBatchDetailProcessRecord
            {
                sevisID = sevisId,
                Result = new ResultType
                {
                    status = true,
                    statusSpecified = true
                }
            };
            SetUserDefinedFields(record, participantId, personId);
            var processDetail = new TransactionLogTypeBatchDetailProcess
            {
                Record = new List<TransactionLogTypeBatchDetailProcessRecord> { record }.ToArray(),
                resultCode = DispositionCode.DocumentNameInvalid.Code
            };
            var transactionLog = new TransactionLogType
            {
                BatchHeader = new TransactionLogTypeBatchHeader
                {
                    BatchID = batchId
                },
                BatchDetail = new TransactionLogTypeBatchDetail
                {
                    Process = processDetail
                }
            };
            var xml = GetXml(transactionLog);

            context.Revert();
            service.ProcessTransactionLog(user, xml, fileProvider.Object);
            tester();
            Assert.AreEqual(1, context.SaveChangesCalledCount);

            context.Revert();
            await service.ProcessTransactionLogAsync(user, xml, fileProvider.Object);
            tester();
            Assert.AreEqual(2, context.SaveChangesCalledCount);
        }

        [TestMethod]
        public async Task TestProcessTransactionLog_HasDownloadRecord()
        {
            var batchId = "hello";
            var user = new User(1);
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var otherUser = new User(user.Id + 1);
            var downloadDispositionCode = DispositionCode.DuplicateBatchId.Code;
            SevisBatchProcessing batch = null;
            context.SetupActions.Add(() =>
            {
                batch = new SevisBatchProcessing
                {
                    BatchId = batchId,
                    Id = 1
                };
                context.SevisBatchProcessings.Add(batch);
            });
            Action tester = () =>
            {
                DateTimeOffset.UtcNow.Should().BeCloseTo(batch.RetrieveDate.Value, 20000);
                Assert.AreEqual(downloadDispositionCode, batch.DownloadDispositionCode);
            };

            var transactionLog = new TransactionLogType
            {
                BatchHeader = new TransactionLogTypeBatchHeader
                {
                    BatchID = batchId
                },
                BatchDetail = new TransactionLogTypeBatchDetail
                {
                    Download = new TransactionLogTypeBatchDetailDownload
                    {
                        resultCode = downloadDispositionCode
                    }
                }
            };
            var xml = GetXml(transactionLog);

            context.Revert();
            service.ProcessTransactionLog(user, xml, fileProvider.Object);
            tester();
            Assert.AreEqual(1, context.SaveChangesCalledCount);

            context.Revert();
            await service.ProcessTransactionLogAsync(user, xml, fileProvider.Object);
            tester();
            Assert.AreEqual(2, context.SaveChangesCalledCount);
        }

        [TestMethod]
        public async Task TestProcessTransactionLog_HasUploadRecord()
        {
            var yesterDay = DateTime.UtcNow.AddDays(-1.0);
            var batchId = "hello";
            var user = new User(1);
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var otherUser = new User(user.Id + 1);
            var dispositionCode = DispositionCode.DuplicateBatchId.Code;
            SevisBatchProcessing batch = null;
            context.SetupActions.Add(() =>
            {
                batch = new SevisBatchProcessing
                {
                    BatchId = batchId,
                    Id = 1
                };
                context.SevisBatchProcessings.Add(batch);
            });
            Action tester = () =>
            {
                Assert.AreEqual(yesterDay, batch.SubmitDate);
                Assert.AreEqual(dispositionCode, batch.UploadDispositionCode);
            };

            var transactionLog = new TransactionLogType
            {
                BatchHeader = new TransactionLogTypeBatchHeader
                {
                    BatchID = batchId
                },
                BatchDetail = new TransactionLogTypeBatchDetail
                {
                    Upload = new TransactionLogTypeBatchDetailUpload
                    {
                        dateTimeStamp = yesterDay,
                        resultCode = dispositionCode
                    }
                }
            };
            var xml = GetXml(transactionLog);

            context.Revert();
            service.ProcessTransactionLog(user, xml, fileProvider.Object);
            tester();
            Assert.AreEqual(1, context.SaveChangesCalledCount);

            context.Revert();
            await service.ProcessTransactionLogAsync(user, xml, fileProvider.Object);
            tester();
            Assert.AreEqual(2, context.SaveChangesCalledCount);
        }
        #endregion

        #region Delete
        [TestMethod]
        public async Task TestDeleteProcessedBatches()
        {
            var numberToCreate = SevisBatchProcessingService.QUERY_BATCH_SIZE + 1;
            var processingDate = DateTime.UtcNow.AddDays(-2.0 * numberOfDaysToKeep);
            context.SetupActions.Add(() =>
            {
                for (var i = 0; i < numberToCreate; i++)
                {
                    var batch = new SevisBatchProcessing
                    {
                        Id = i,
                        RetrieveDate = processingDate,
                        DownloadDispositionCode = DispositionCode.Success.Code,
                        UploadDispositionCode = DispositionCode.Success.Code,
                        ProcessDispositionCode = DispositionCode.Success.Code
                    };
                    context.SevisBatchProcessings.Add(batch);
                }
            });
            Action tester = () =>
            {
                Assert.AreEqual(0, context.SevisBatchProcessings.Count());
            };
            context.Revert();
            service.DeleteProcessedBatches();
            tester();
            Assert.AreEqual(2, context.SaveChangesCalledCount);
            notificationService.Verify(x => x.NotifyDeletedSevisBatchProcessing(It.IsAny<int>(), It.IsAny<string>()), Times.Exactly(numberToCreate));

            context.Revert();
            await service.DeleteProcessedBatchesAsync();
            tester();
            Assert.AreEqual(4, context.SaveChangesCalledCount);
            notificationService.Verify(x => x.NotifyDeletedSevisBatchProcessing(It.IsAny<int>(), It.IsAny<string>()), Times.Exactly(numberToCreate * 2));
        }

        [TestMethod]
        public async Task TestDeleteProcessedBatches_NumberOfDaysToKeepIsNegative()
        {
            numberOfDaysToKeep = -1.0 * numberOfDaysToKeep;
            appSettings[AppSettings.NUMBER_OF_DAYS_TO_KEEP_PROCESSED_SEVIS_BATCH_RECORDS] = numberOfDaysToKeep.ToString();
            var processingDate = DateTime.UtcNow.AddDays(2.0 * numberOfDaysToKeep);
            context.SetupActions.Add(() =>
            {
                for (var i = 0; i < SevisBatchProcessingService.QUERY_BATCH_SIZE + 1; i++)
                {
                    var batch = new SevisBatchProcessing
                    {
                        Id = i,
                        RetrieveDate = processingDate,
                        DownloadDispositionCode = DispositionCode.Success.Code,
                        UploadDispositionCode = DispositionCode.Success.Code,
                        ProcessDispositionCode = DispositionCode.Success.Code
                    };
                    context.SevisBatchProcessings.Add(batch);
                }
            });
            Action tester = () =>
            {
                Assert.AreEqual(0, context.SevisBatchProcessings.Count());
            };
            context.Revert();
            service.DeleteProcessedBatches();
            tester();
            Assert.AreEqual(2, context.SaveChangesCalledCount);

            context.Revert();
            await service.DeleteProcessedBatchesAsync();
            tester();
            Assert.AreEqual(4, context.SaveChangesCalledCount);
        }

        [TestMethod]
        public async Task TestDeleteProcessedBatches_NoBatches()
        {
            Action tester = () =>
            {
                Assert.AreEqual(0, context.SevisBatchProcessings.Count());
            };
            context.Revert();
            service.DeleteProcessedBatches();
            tester();
            Assert.AreEqual(0, context.SaveChangesCalledCount);

            context.Revert();
            await service.DeleteProcessedBatchesAsync();
            tester();
            Assert.AreEqual(0, context.SaveChangesCalledCount);
        }

        #endregion

        #region DS2019

        [TestMethod]
        public void TestGetDS2019FileName()
        {
            var participantId = 1;
            var sevisId = "sevisId";
            Assert.AreEqual(string.Format("{0}_{1}.{2}", participantId, sevisId, "pdf"), SevisBatchProcessingService.GetDS2019FileName(participantId, sevisId));
        }

        [TestMethod]
        public async Task TestSaveDS2019Form()
        {
            var participantId = 1;
            var sevisId = "sevisId";
            var fileContents = new byte[1] { (byte)1 };
            Action<string, byte[], string> cloudStorageCallback = (fName, fContents, contentType) =>
            {
                Assert.AreEqual(SevisBatchProcessingService.GetDS2019FileName(participantId, sevisId), fName);
                Assert.AreEqual(SevisBatchProcessingService.DS2019_CONTENT_TYPE, contentType);
                CollectionAssert.AreEqual(fileContents, fContents);
            };
            var url = "url";
            cloudStorageService.Setup(x => x.SaveFile(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<string>()))
                .Returns(url)
                .Callback(cloudStorageCallback);
            cloudStorageService.Setup(x => x.SaveFileAsync(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<string>()))
                .ReturnsAsync(url)
                .Callback(cloudStorageCallback);
            service.SaveDS2019Form(participantId, sevisId, fileContents);
            await service.SaveDS2019FormAsync(participantId, sevisId, fileContents);
        }
        #endregion

        #region Dispose
        [TestMethod]
        public void TestDispose_Context()
        {
            var serviceToDispose = new SevisBatchProcessingService(
                context: context,
                appSettings: settings,
                cloudStorageService: cloudStorageService.Object,
                exchangeVisitorService: exchangeVisitorService.Object,
                notificationService: notificationService.Object,
                exchangeVisitorValidationService: exchangeVisitorValidationService.Object,
                maxCreateExchangeVisitorRecordsPerBatch: maxCreateExchangeVisitorBatchSize,
                maxUpdateExchangeVisitorRecordsPerBatch: maxUpdateExchangeVisitorBatchSize);

            var contextField = typeof(SevisBatchProcessingService).GetProperty("Context", BindingFlags.NonPublic | BindingFlags.Instance);
            var contextValue = contextField.GetValue(serviceToDispose);
            Assert.IsNotNull(contextField);
            Assert.IsNotNull(contextValue);

            serviceToDispose.Dispose();
            contextValue = contextField.GetValue(serviceToDispose);
            Assert.IsNull(contextValue);
        }

        [TestMethod]
        public void TestDispose_CloudStorageService()
        {
            var disposableService = new Mock<IDummyCloudStorage>();
            var disposable = disposableService.As<IDisposable>();

            var serviceToDispose = new SevisBatchProcessingService(
                context: context,
                appSettings: settings,
                cloudStorageService: disposableService.Object,
                exchangeVisitorService: exchangeVisitorService.Object,
                notificationService: notificationService.Object,
                exchangeVisitorValidationService: exchangeVisitorValidationService.Object,
                maxCreateExchangeVisitorRecordsPerBatch: maxCreateExchangeVisitorBatchSize,
                maxUpdateExchangeVisitorRecordsPerBatch: maxUpdateExchangeVisitorBatchSize);

            var serviceField = typeof(SevisBatchProcessingService).GetField("cloudStorageService", BindingFlags.NonPublic | BindingFlags.Instance);
            var serviceValue = serviceField.GetValue(serviceToDispose);
            Assert.IsNotNull(serviceField);
            Assert.IsNotNull(serviceValue);

            serviceToDispose.Dispose();
            serviceValue = serviceField.GetValue(serviceToDispose);
            Assert.IsNull(serviceValue);
            disposable.Verify(x => x.Dispose(), Times.Once());
        }

        [TestMethod]
        public void TestDispose_ExchangeVisitorService()
        {
            var disposableService = new Mock<IExchangeVisitorService>();
            var disposable = disposableService.As<IDisposable>();

            var serviceToDispose = new SevisBatchProcessingService(
                context: context,
                appSettings: settings,
                cloudStorageService: cloudStorageService.Object,
                exchangeVisitorService: disposableService.Object,
                notificationService: notificationService.Object,
                exchangeVisitorValidationService: exchangeVisitorValidationService.Object,
                maxCreateExchangeVisitorRecordsPerBatch: maxCreateExchangeVisitorBatchSize,
                maxUpdateExchangeVisitorRecordsPerBatch: maxUpdateExchangeVisitorBatchSize);

            var serviceField = typeof(SevisBatchProcessingService).GetField("exchangeVisitorService", BindingFlags.NonPublic | BindingFlags.Instance);
            var serviceValue = serviceField.GetValue(serviceToDispose);
            Assert.IsNotNull(serviceField);
            Assert.IsNotNull(serviceValue);

            serviceToDispose.Dispose();
            serviceValue = serviceField.GetValue(serviceToDispose);
            Assert.IsNull(serviceValue);
            disposable.Verify(x => x.Dispose(), Times.Once());
        }

        [TestMethod]
        public void TestDispose_ExchangeVisitorValidationService()
        {
            var disposableService = new Mock<IExchangeVisitorValidationService>();
            var disposable = disposableService.As<IDisposable>();

            var serviceToDispose = new SevisBatchProcessingService(
                context: context,
                appSettings: settings,
                cloudStorageService: cloudStorageService.Object,
                exchangeVisitorService: exchangeVisitorService.Object,
                notificationService: notificationService.Object,
                exchangeVisitorValidationService: disposableService.Object,
                maxCreateExchangeVisitorRecordsPerBatch: maxCreateExchangeVisitorBatchSize,
                maxUpdateExchangeVisitorRecordsPerBatch: maxUpdateExchangeVisitorBatchSize);

            var serviceField = typeof(SevisBatchProcessingService).GetField("exchangeVisitorValidationService", BindingFlags.NonPublic | BindingFlags.Instance);
            var serviceValue = serviceField.GetValue(serviceToDispose);
            Assert.IsNotNull(serviceField);
            Assert.IsNotNull(serviceValue);

            serviceToDispose.Dispose();
            serviceValue = serviceField.GetValue(serviceToDispose);
            Assert.IsNull(serviceValue);
            disposable.Verify(x => x.Dispose(), Times.Once());
        }

        [TestMethod]
        public void TestDispose_NotificationService()
        {
            var disposableService = new Mock<ISevisBatchProcessingNotificationService>();
            var disposable = disposableService.As<IDisposable>();

            var serviceToDispose = new SevisBatchProcessingService(
                context: context,
                appSettings: settings,
                cloudStorageService: cloudStorageService.Object,
                exchangeVisitorService: exchangeVisitorService.Object,
                notificationService: disposableService.Object,
                exchangeVisitorValidationService: exchangeVisitorValidationService.Object,
                maxCreateExchangeVisitorRecordsPerBatch: maxCreateExchangeVisitorBatchSize,
                maxUpdateExchangeVisitorRecordsPerBatch: maxUpdateExchangeVisitorBatchSize);

            var serviceField = typeof(SevisBatchProcessingService).GetField("notificationService", BindingFlags.NonPublic | BindingFlags.Instance);
            var serviceValue = serviceField.GetValue(serviceToDispose);
            Assert.IsNotNull(serviceField);
            Assert.IsNotNull(serviceValue);

            serviceToDispose.Dispose();
            serviceValue = serviceField.GetValue(serviceToDispose);
            Assert.IsNull(serviceValue);
            disposable.Verify(x => x.Dispose(), Times.Once());
        }

        [TestMethod]
        public void TestDispose_NoDisposableServices()
        {

            var serviceToDispose = new SevisBatchProcessingService(
                context: context,
                appSettings: settings,
                cloudStorageService: cloudStorageService.Object,
                exchangeVisitorService: exchangeVisitorService.Object,
                notificationService: notificationService.Object,
                exchangeVisitorValidationService: exchangeVisitorValidationService.Object,
                maxCreateExchangeVisitorRecordsPerBatch: maxCreateExchangeVisitorBatchSize,
                maxUpdateExchangeVisitorRecordsPerBatch: maxUpdateExchangeVisitorBatchSize);

            var serviceNames = new List<string>
            {
                "notificationService",
                "exchangeVisitorValidationService",
                "exchangeVisitorService",
                "cloudStorageService"
            };
            foreach (var name in serviceNames)
            {
                var serviceField = typeof(SevisBatchProcessingService).GetField(name, BindingFlags.NonPublic | BindingFlags.Instance);
                var serviceValue = serviceField.GetValue(serviceToDispose);
                Assert.IsNotNull(serviceField);
                Assert.IsNotNull(serviceValue);

                serviceToDispose.Dispose();
                serviceValue = serviceField.GetValue(serviceToDispose);
                Assert.IsNotNull(serviceValue);
            }
        }
        #endregion


        public void SetUserDefinedFields(TransactionLogTypeBatchDetailProcessRecord record, int participantId, int personId)
        {
            record.UserDefinedA = participantId.ToString();
            record.UserDefinedB = "B" + personId.ToString();
        }

        public void SetUserDefinedFields(TransactionLogTypeBatchDetailProcessRecordDependent record, int participantId, int personId)
        {
            record.UserDefinedA = participantId.ToString();
            record.UserDefinedB = "B" + personId.ToString();
        }
    }
}
