using ECA.Business.Queries.Models.Admin;
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
using ECA.Business.Storage;
using ECA.Core.DynamicLinq;
using System.Text;
using System.Xml;
using ECA.Business.Queries.Models.Persons;

namespace ECA.Business.Test.Service.Sevis
{
    [TestClass]
    public partial class SevisBatchProcessingServiceTest
    {
        private TestEcaContext context;
        private SevisBatchProcessingService service;
        private Mock<IDS2019FileProvider> fileProvider;
        private Mock<IFileStorageService> cloudStorageService;
        private Mock<IExchangeVisitorService> exchangeVisitorService;
        private Mock<ISevisBatchProcessingNotificationService> notificationService;
        private Mock<IExchangeVisitorValidationService> exchangeVisitorValidationService;
        private Mock<AbstractValidator<ExchangeVisitor>> validator;
        private int maxCreateExchangeVisitorBatchSize = 10;
        private int maxUpdateExchangeVisitorBatchSize = 10;
        private double numberOfDaysToKeep = 1.0;
        private int downloadCooldownInSeconds = 60;
        private int uploadCooldownInSeconds = 60;
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
            appSettings.Add(AppSettings.DOWNLOAD_COOLDOWN_IN_SECONDS, downloadCooldownInSeconds.ToString());
            appSettings.Add(AppSettings.UPLOAD_COOLDOWN_IN_SECONDS, uploadCooldownInSeconds.ToString());
            appSettings.Add(AppSettings.SEVIS_DS2019_STORAGE_CONTAINER, "ds2019files");

            context = new TestEcaContext();
            exchangeVisitorService = new Mock<IExchangeVisitorService>();
            notificationService = new Mock<ISevisBatchProcessingNotificationService>();
            exchangeVisitorValidationService = new Mock<IExchangeVisitorValidationService>();
            validator = new Mock<AbstractValidator<ExchangeVisitor>>();
            cloudStorageService = new Mock<IFileStorageService>();
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

        private Business.Validation.Sevis.Bio.Person GetPerson(DateTime birthDate, int personId, int participantId)
        {
            var state = "TN";
            var mailAddress = new AddressDTO();
            mailAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            mailAddress.DivisionIso = state;

            var usAddress = new AddressDTO();
            usAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            usAddress.DivisionIso = state;

            var firstName = "first";
            var lastName = "last";
            var passport = "passport";
            var preferred = "preferred";
            var suffix = "Jr.";
            var fullName = new FullName(firstName, lastName, passport, preferred, suffix);

            var birthCity = "birth city";
            var birthCountryCode = "CN";
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

        private string GetXml(SEVISBatchCreateUpdateEV batch)
        {
            using (var textWriter = new StringWriter())
            {
                var serializer = new XmlSerializer(typeof(SEVISBatchCreateUpdateEV));
                serializer.Serialize(textWriter, batch);
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
                DivisionIso = "DC",
                LocationName = "name"
            };
            var exchangeVisitor = new ExchangeVisitor(
                sevisId: null,
                isValidated: false,
                person: GetPerson(DateTime.UtcNow, personId, participantId),
                financialInfo: new Business.Validation.Sevis.Finance.FinancialInfo(true, true, null, null),
                occupationCategoryCode: "99",
                programEndDate: DateTime.Now,
                programStartDate: DateTime.Now,
                dependents: new List<Business.Validation.Sevis.Bio.Dependent>(),
                siteOfActivity: siteOfActivity,
                sevisOrgId: "abcde1234567890");
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
                Assert.AreEqual(firstBatch.BatchId.ToString(), addedCommStatus.BatchId);
                Assert.AreEqual(status.SevisUsername, addedCommStatus.SevisUsername);
                Assert.AreEqual(status.SevisOrgId, addedCommStatus.SevisOrgId);

                Assert.AreEqual(1, context.ExchangeVisitorHistories.Count());
                var firstHistory = context.ExchangeVisitorHistories.First();
                Assert.AreEqual(participantId, firstHistory.ParticipantId);
                Assert.AreEqual(exchangeVisitor.ToJson(), firstHistory.PendingModel);
                Assert.IsNull(firstHistory.LastSuccessfulModel);
                DateTimeOffset.UtcNow.Should().BeCloseTo(firstHistory.RevisedOn, 20000);

            };
            context.Revert();
            var result = service.StageBatches();
            Assert.AreEqual(result.Count * 2, context.SaveChangesCalledCount);
            tester(result);

            context.Revert();
            var resultAsync = await service.StageBatchesAsync();
            tester(resultAsync);
            Assert.AreEqual(result.Count * 4, context.SaveChangesCalledCount);
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
                DivisionIso = "DC",
                LocationName = "name"
            };
            var exchangeVisitor = new ExchangeVisitor(
                sevisId: null,
                isValidated: false,
                person: GetPerson(DateTime.UtcNow, personId, firstParticipantId),
                financialInfo: new Business.Validation.Sevis.Finance.FinancialInfo(true, true, null, null),
                occupationCategoryCode: "99",
                programEndDate: DateTime.Now,
                programStartDate: DateTime.Now,
                dependents: new List<Business.Validation.Sevis.Bio.Dependent>(),
                siteOfActivity: siteOfActivity,
                sevisOrgId: "abcde1234567890");
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
            Assert.AreEqual(result.Count * 2, context.SaveChangesCalledCount);
            tester(result);

            context.Revert();
            var resultAsync = await service.StageBatchesAsync();
            tester(resultAsync);
            Assert.AreEqual(result.Count * 4, context.SaveChangesCalledCount);
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
                DivisionIso = "DC",
                LocationName = "name"
            };
            var exchangeVisitor = new ExchangeVisitor(
                sevisId: null,
                isValidated: false,
                person: GetPerson(DateTime.UtcNow, personId, firstParticipantId),
                financialInfo: new Business.Validation.Sevis.Finance.FinancialInfo(true, true, null, null),
                occupationCategoryCode: "99",
                programEndDate: DateTime.Now,
                programStartDate: DateTime.Now,
                dependents: new List<Business.Validation.Sevis.Bio.Dependent>(),
                siteOfActivity: siteOfActivity,
                sevisOrgId: "abcde1234567890");
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
            Assert.AreEqual(result.Count * 2, context.SaveChangesCalledCount);
            tester(result);

            context.Revert();
            var resultAsync = await service.StageBatchesAsync();
            tester(resultAsync);
            Assert.AreEqual(result.Count * 4, context.SaveChangesCalledCount);
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
                DivisionIso = "DC",
                LocationName = "name"
            };
            var exchangeVisitor = new ExchangeVisitor(
                sevisId: null,
                isValidated: false,
                person: GetPerson(DateTime.UtcNow, personId, participantId),
                financialInfo: new Business.Validation.Sevis.Finance.FinancialInfo(true, true, null, null),
                occupationCategoryCode: "99",
                programEndDate: DateTime.Now,
                programStartDate: DateTime.Now,
                dependents: new List<Business.Validation.Sevis.Bio.Dependent>(),
                siteOfActivity: siteOfActivity,
                sevisOrgId: "abcde1234567890");
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
            var sevisId = "sevisid";
            var birthDate = DateTime.UtcNow;
            Participant participant = null;
            ParticipantPerson participantPerson = null;
            ExchangeVisitorHistory exchangeVisitorHistory = null;
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
                DivisionIso = "DC",
                LocationName = "name"
            };
            var exchangeVisitor = new ExchangeVisitor(
                sevisId: sevisId,
                isValidated: false,
                person: GetPerson(birthDate, personId, participantId),
                financialInfo: new Business.Validation.Sevis.Finance.FinancialInfo(true, true, null, null),
                occupationCategoryCode: "99",
                programEndDate: DateTime.Now,
                programStartDate: DateTime.Now,
                dependents: new List<Business.Validation.Sevis.Bio.Dependent>(),
                siteOfActivity: siteOfActivity,
                sevisOrgId: "abcde1234567890");

            var previouslySubmittedExchangeVisitor = new ExchangeVisitor(
                sevisId: sevisId,
                isValidated: false,
                sevisOrgId: "sevisOrgId",
                person: GetPerson(birthDate, personId, participantId),
                financialInfo: new Business.Validation.Sevis.Finance.FinancialInfo(true, true, "1.0", null),
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
                exchangeVisitorHistory = new ExchangeVisitorHistory
                {
                    RevisedOn = DateTimeOffset.UtcNow.AddDays(-1.0),
                    ParticipantId = participantId,
                    LastSuccessfulModel = previouslySubmittedExchangeVisitor.ToJson()
                };
                status.ParticipantPerson = participantPerson;
                status.ParticipantId = participantId;
                participantPerson.ParticipantPersonSevisCommStatuses.Add(status);
                context.Participants.Add(participant);
                context.ParticipantPersons.Add(participantPerson);
                context.ParticipantPersonSevisCommStatuses.Add(status);
                context.ExchangeVisitorHistories.Add(exchangeVisitorHistory);
            });
            Action<List<StagedSevisBatch>> tester = (batches) =>
            {
                Assert.AreEqual(1, exchangeVisitor.GetSEVISEVBatchTypeExchangeVisitor1Collection(status.SevisUsername, previouslySubmittedExchangeVisitor).Count());

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
                Assert.AreEqual(exchangeVisitor.GetSEVISEVBatchTypeExchangeVisitor1Collection(status.SevisUsername, previouslySubmittedExchangeVisitor).Count(), firstBatch.SEVISBatchCreateUpdateEV.UpdateEV.Count());
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
                Assert.AreEqual(firstBatch.BatchId.ToString(), addedCommStatus.BatchId);
                Assert.AreEqual(status.SevisUsername, addedCommStatus.SevisUsername);
                Assert.AreEqual(status.SevisOrgId, addedCommStatus.SevisOrgId);

                Assert.AreEqual(1, context.ExchangeVisitorHistories.Count());
                Assert.AreEqual(previouslySubmittedExchangeVisitor.ToJson(), exchangeVisitorHistory.LastSuccessfulModel);
                Assert.AreEqual(exchangeVisitor.ToJson(), exchangeVisitorHistory.PendingModel);
                DateTimeOffset.UtcNow.Should().BeCloseTo(exchangeVisitorHistory.RevisedOn, 20000);
                Assert.AreEqual(participantId, exchangeVisitorHistory.ParticipantId);
            };
            context.Revert();
            var result = service.StageBatches();
            Assert.AreEqual(result.Count * 2, context.SaveChangesCalledCount);
            tester(result);

            context.Revert();
            var resultAsync = await service.StageBatchesAsync();
            tester(resultAsync);
            Assert.AreEqual(result.Count * 4, context.SaveChangesCalledCount);

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
            var birthDate = DateTime.UtcNow;
            var siteOfActivity = new AddressDTO
            {
                DivisionIso = "DC",
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
                exchangeVisitors.Clear();
                var sevisUsername = "sevis username";
                var now = DateTime.UtcNow;
                for (var i = 0; i < maxCreateExchangeVisitorBatchSize; i++)
                {
                    var participant = new Participant
                    {
                        ParticipantId = i + 1,
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
                        isValidated: false,
                        person: GetPerson(birthDate, personId, participant.ParticipantId),
                        financialInfo: new Business.Validation.Sevis.Finance.FinancialInfo(true, true, null, null),
                        occupationCategoryCode: "99",
                        programEndDate: DateTime.Now,
                        programStartDate: DateTime.Now,
                        dependents: new List<Business.Validation.Sevis.Bio.Dependent>(),
                        siteOfActivity: siteOfActivity,
                        sevisOrgId: "abcde1234567890"
                    );
                    exchangeVisitors.Add(exchangeVisitor);
                }
                for (var i = 0; i < maxUpdateExchangeVisitorBatchSize; i++)
                {
                    var participant = new Participant
                    {
                        ParticipantId = (i + 1) * 100,
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
                        sevisOrgId: "abcde1234567890",
                        isValidated: false,
                        person: GetPerson(birthDate, personId, participant.ParticipantId),
                        financialInfo: new Business.Validation.Sevis.Finance.FinancialInfo(true, true, null, null),
                        occupationCategoryCode: "99",
                        programEndDate: DateTime.Now,
                        programStartDate: DateTime.Now,
                        dependents: new List<Business.Validation.Sevis.Bio.Dependent>(),
                        siteOfActivity: siteOfActivity
                    );

                    var previousExchangeVisitor = new ExchangeVisitor(
                        sevisId: "sevisId",
                        sevisOrgId: "abcde1234567890",
                        isValidated: false,
                        person: GetPerson(birthDate, personId, participant.ParticipantId),
                        financialInfo: new Business.Validation.Sevis.Finance.FinancialInfo(true, true, "1.0", null),
                        occupationCategoryCode: "99",
                        programEndDate: DateTime.Now,
                        programStartDate: DateTime.Now,
                        dependents: new List<Business.Validation.Sevis.Bio.Dependent>(),
                        siteOfActivity: siteOfActivity
                    );
                    exchangeVisitors.Add(exchangeVisitor);
                    //we just need one change/one update message
                    Assert.AreEqual(1, exchangeVisitor.GetSEVISEVBatchTypeExchangeVisitor1Collection(readyToSubmitStatus.SevisUsername, previousExchangeVisitor).Count());
                    var history = new ExchangeVisitorHistory
                    {
                        ParticipantId = participant.ParticipantId,
                        LastSuccessfulModel = previousExchangeVisitor.ToJson()
                    };
                    context.ExchangeVisitorHistories.Add(history);
                }
            });

            Action<List<StagedSevisBatch>> tester = (batches) =>
            {
                Assert.IsNotNull(batches);
                Assert.AreEqual(1, batches.Count);

                var firstBatch = batches.First();
                Assert.AreEqual(maxCreateExchangeVisitorBatchSize, firstBatch.SEVISBatchCreateUpdateEV.CreateEV.Count());
                Assert.AreEqual(maxUpdateExchangeVisitorBatchSize, firstBatch.SEVISBatchCreateUpdateEV.UpdateEV.Count());
                Assert.IsTrue(firstBatch.IsSaved);
            };

            context.Revert();
            var result = service.StageBatches();
            tester(result);
            Assert.AreEqual(result.Count + maxCreateExchangeVisitorBatchSize + maxUpdateExchangeVisitorBatchSize,
                context.SaveChangesCalledCount);

            context.Revert();
            var resultAsync = await service.StageBatchesAsync();
            tester(resultAsync);
            Assert.AreEqual((result.Count + maxCreateExchangeVisitorBatchSize + maxUpdateExchangeVisitorBatchSize) * 2,
                context.SaveChangesCalledCount);

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
            var birthDate = DateTime.UtcNow;
            var siteOfActivity = new AddressDTO
            {
                DivisionIso = "DC",
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
                exchangeVisitors.Clear();
                var sevisUsername = "sevis username";
                var now = DateTime.UtcNow;
                for (var i = 0; i <= maxCreateExchangeVisitorBatchSize; i++)
                {
                    var participant = new Participant
                    {
                        ParticipantId = (i + 1),
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
                        isValidated: false,
                        person: GetPerson(birthDate, personId, participant.ParticipantId),
                        financialInfo: new Business.Validation.Sevis.Finance.FinancialInfo(true, true, null, null),
                        occupationCategoryCode: "99",
                        programEndDate: DateTime.Now,
                        programStartDate: DateTime.Now,
                        dependents: new List<Business.Validation.Sevis.Bio.Dependent>(),
                        siteOfActivity: siteOfActivity,
                        sevisOrgId: "abcde1234567890"
                    );
                    exchangeVisitors.Add(exchangeVisitor);
                }
                for (var i = 0;
                    i <= maxUpdateExchangeVisitorBatchSize;
                    i++)
                {
                    var participant = new Participant
                    {
                        ParticipantId = (i + 1) * 100,
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
                        sevisOrgId: "sevisOrgId",
                        isValidated: false,
                        person: GetPerson(birthDate, personId, participant.ParticipantId),
                        financialInfo: new Business.Validation.Sevis.Finance.FinancialInfo(true, true, null, null),
                        occupationCategoryCode: "99",
                        programEndDate: DateTime.Now,
                        programStartDate: DateTime.Now,
                        dependents: new List<Business.Validation.Sevis.Bio.Dependent>(),
                        siteOfActivity: siteOfActivity);

                    var previousExchangeVisitor = new ExchangeVisitor(
                        sevisId: "sevisId",
                        sevisOrgId: "sevisOrgId",
                        isValidated: false,
                        person: GetPerson(birthDate, personId, participant.ParticipantId),
                        financialInfo: new Business.Validation.Sevis.Finance.FinancialInfo(true, true, "1.0", null),
                        occupationCategoryCode: "99",
                        programEndDate: DateTime.Now,
                        programStartDate: DateTime.Now,
                        dependents: new List<Business.Validation.Sevis.Bio.Dependent>(),
                        siteOfActivity: siteOfActivity
                    );
                    exchangeVisitors.Add(exchangeVisitor);
                    //we just need one change/one update message
                    Assert.AreEqual(1, exchangeVisitor.GetSEVISEVBatchTypeExchangeVisitor1Collection(readyToSubmitStatus.SevisUsername, previousExchangeVisitor).Count());
                    var history = new ExchangeVisitorHistory
                    {
                        ParticipantId = participant.ParticipantId,
                        LastSuccessfulModel = previousExchangeVisitor.ToJson()
                    };
                    context.ExchangeVisitorHistories.Add(history);
                }
            });

            Action<List<StagedSevisBatch>> tester = (batches) =>
            {
                Assert.IsNotNull(batches);
                Assert.AreEqual(3, batches.Count);

                var firstBatch = batches.First();
                Assert.AreEqual(maxCreateExchangeVisitorBatchSize, firstBatch.SEVISBatchCreateUpdateEV.CreateEV.Count());
                Assert.IsNull(firstBatch.SEVISBatchCreateUpdateEV.UpdateEV);
                Assert.IsTrue(firstBatch.IsSaved);

                var secondBatch = batches[1];
                Assert.AreEqual(1, secondBatch.SEVISBatchCreateUpdateEV.CreateEV.Count());
                Assert.AreEqual(maxUpdateExchangeVisitorBatchSize, secondBatch.SEVISBatchCreateUpdateEV.UpdateEV.Count());
                Assert.IsTrue(secondBatch.IsSaved);

                var lastBatch = batches.Last();
                Assert.IsNull(lastBatch.SEVISBatchCreateUpdateEV.CreateEV);
                Assert.AreEqual(1, lastBatch.SEVISBatchCreateUpdateEV.UpdateEV.Count());
                Assert.IsTrue(lastBatch.IsSaved);
            };

            context.Revert();
            var result = service.StageBatches();
            tester(result);
            Assert.AreEqual(result.Count + exchangeVisitors.Count,
                context.SaveChangesCalledCount);

            context.Revert();
            var resultAsync = await service.StageBatchesAsync();
            tester(resultAsync);
            Assert.AreEqual((result.Count + exchangeVisitors.Count) * 2,
                context.SaveChangesCalledCount);

            notificationService.Verify(x => x.NotifyNumberOfParticipantsToStage(It.IsAny<int>()), Times.Exactly(2));
            notificationService.Verify(x => x.NotifyStagedSevisBatchCreated(It.IsAny<StagedSevisBatch>()), Times.Exactly(6));
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
                DivisionIso = "DC",
                LocationName = "name"
            };

            var exchangeVisitor = new ExchangeVisitor(
                sevisId: null,
                isValidated: false,
                person: GetPerson(DateTime.UtcNow, 1, 2),
                financialInfo: new Business.Validation.Sevis.Finance.FinancialInfo(true, true, null, null),
                occupationCategoryCode: "99",
                programEndDate: DateTime.Now,
                programStartDate: DateTime.Now,
                dependents: new List<Business.Validation.Sevis.Bio.Dependent>(),
                siteOfActivity: siteOfActivity,
                sevisOrgId: "abcde1234567890");
            var participant = new SevisGroupedParticipantDTO
            {
                ParticipantId = exchangeVisitor.Person.ParticipantId
            };
            Assert.IsNull(service.GetAccomodatingStagedSevisBatch(batches, participant, exchangeVisitor, null, status.SevisUsername, status.SevisOrgId));
        }

        [TestMethod]
        public void TestGetAccomodatingSevisBatch_SevisBatchIsSaved()
        {
            var sevisOrgId = "orgId";
            var batches = new List<StagedSevisBatch>();
            var sevisUsername = "sevis username";
            batches.Add(new StagedSevisBatch(BatchId.NewBatchId(), sevisUsername, sevisOrgId)
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
                DivisionIso = "DC",
                LocationName = "name"
            };

            var exchangeVisitor = new ExchangeVisitor(
                sevisId: null,
                isValidated: false,
                person: GetPerson(DateTime.UtcNow, 1, 2),
                financialInfo: new Business.Validation.Sevis.Finance.FinancialInfo(true, true, null, null),
                occupationCategoryCode: "99",
                programEndDate: DateTime.Now,
                programStartDate: DateTime.Now,
                dependents: new List<Business.Validation.Sevis.Bio.Dependent>(),
                siteOfActivity: siteOfActivity,
                sevisOrgId: "abcde1234567890");
            var participant = new SevisGroupedParticipantDTO
            {
                ParticipantId = exchangeVisitor.Person.ParticipantId
            };
            Assert.IsNull(service.GetAccomodatingStagedSevisBatch(batches, participant, exchangeVisitor, null, status.SevisUsername, status.SevisOrgId));
        }

        [TestMethod]
        public void TestGetAccomodatingSevisBatch_SevisBatchCanAccomodate()
        {
            var sevisOrgId = "orgId";
            var sevisUsername = "sevis username";
            var batches = new List<StagedSevisBatch>();
            batches.Add(new StagedSevisBatch(BatchId.NewBatchId(), sevisUsername, sevisOrgId)
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
                DivisionIso = "DC",
                LocationName = "name"
            };

            var exchangeVisitor = new ExchangeVisitor(
                sevisId: null,
                isValidated: false,
                person: GetPerson(DateTime.UtcNow, 1, 2),
                financialInfo: new Business.Validation.Sevis.Finance.FinancialInfo(true, true, null, null),
                occupationCategoryCode: "99",
                programEndDate: DateTime.Now,
                programStartDate: DateTime.Now,
                dependents: new List<Business.Validation.Sevis.Bio.Dependent>(),
                siteOfActivity: siteOfActivity,
                sevisOrgId: "abcde1234567890");
            var participant = new SevisGroupedParticipantDTO
            {
                ParticipantId = exchangeVisitor.Person.ParticipantId
            };
            Assert.IsTrue(Object.ReferenceEquals(batches.First(), service.GetAccomodatingStagedSevisBatch(batches, participant, exchangeVisitor, null, status.SevisUsername, status.SevisOrgId)));
        }

        [TestMethod]
        public void TestGetAccomodatingSevisBatch_SevisBatchCanNotAccomodateBySize()
        {
            var sevisUsername = "sevis username";
            var sevisOrgId = "orgId";
            var batches = new List<StagedSevisBatch>();
            batches.Add(new StagedSevisBatch(BatchId.NewBatchId(), sevisUsername, sevisOrgId, 0, 0)
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
                DivisionIso = "DC",
                LocationName = "name"
            };

            var exchangeVisitor = new ExchangeVisitor(
                sevisId: null,
                isValidated: false,
                person: GetPerson(DateTime.UtcNow, 1, 2),
                financialInfo: new Business.Validation.Sevis.Finance.FinancialInfo(true, true, null, null),
                occupationCategoryCode: "99",
                programEndDate: DateTime.Now,
                programStartDate: DateTime.Now,
                dependents: new List<Business.Validation.Sevis.Bio.Dependent>(),
                siteOfActivity: siteOfActivity,
                sevisOrgId: "abcde1234567890");
            var participant = new SevisGroupedParticipantDTO
            {
                ParticipantId = exchangeVisitor.Person.ParticipantId
            };
            Assert.IsNull(service.GetAccomodatingStagedSevisBatch(batches, participant, exchangeVisitor, null, status.SevisUsername, status.SevisOrgId));
        }

        [TestMethod]
        public void TestGetAccomodatingSevisBatch_SevisBatchCanNotAccomodateBySevisUsername()
        {
            var sevisUsername = "sevis username";
            var sevisOrgId = "orgId";
            var batches = new List<StagedSevisBatch>();
            batches.Add(new StagedSevisBatch(BatchId.NewBatchId(), sevisUsername, sevisOrgId, 1, 1)
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
                DivisionIso = "DC",
                LocationName = "name"
            };

            var exchangeVisitor = new ExchangeVisitor(
                sevisId: null,
                isValidated: false,
                person: GetPerson(DateTime.UtcNow, 1, 2),
                sevisOrgId: sevisOrgId,
                financialInfo: new Business.Validation.Sevis.Finance.FinancialInfo(true, true, null, null),
                occupationCategoryCode: "99",
                programEndDate: DateTime.Now,
                programStartDate: DateTime.Now,
                dependents: new List<Business.Validation.Sevis.Bio.Dependent>(),
                siteOfActivity: siteOfActivity);
            var participant = new SevisGroupedParticipantDTO
            {
                ParticipantId = exchangeVisitor.Person.ParticipantId
            };
            Assert.IsNull(service.GetAccomodatingStagedSevisBatch(batches, participant, exchangeVisitor, null, "other user", status.SevisOrgId));
        }

        [TestMethod]
        public void TestGetAccomodatingSevisBatch_SevisBatchCanNotAccomodateBySevisOrgId()
        {
            var sevisUsername = "sevis username";
            var sevisOrgId = "orgId";
            var batches = new List<StagedSevisBatch>();
            batches.Add(new StagedSevisBatch(BatchId.NewBatchId(), sevisUsername, sevisOrgId, 1, 1)
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
                DivisionIso = "DC",
                LocationName = "name"
            };

            var exchangeVisitor = new ExchangeVisitor(
                sevisId: null,
                isValidated: false,
                person: GetPerson(DateTime.UtcNow, 1, 2),
                sevisOrgId: sevisOrgId,
                financialInfo: new Business.Validation.Sevis.Finance.FinancialInfo(true, true, null, null),
                occupationCategoryCode: "99",
                programEndDate: DateTime.Now,
                programStartDate: DateTime.Now,
                dependents: new List<Business.Validation.Sevis.Bio.Dependent>(),
                siteOfActivity: siteOfActivity);
            var participant = new SevisGroupedParticipantDTO
            {
                ParticipantId = exchangeVisitor.Person.ParticipantId
            };
            Assert.IsNull(service.GetAccomodatingStagedSevisBatch(batches, participant, exchangeVisitor, null, status.SevisUsername, "other org"));
        }

        [TestMethod]
        public void TestGetAccomodatingSevisBatch_SevisBatchCanNotAccomodateByPreviouslySubmittedExchangeVisitor()
        {
            var sevisUsername = "sevis username";
            var sevisOrgId = "orgId";
            var batches = new List<StagedSevisBatch>();
            var birthDate = DateTime.UtcNow;
            batches.Add(new StagedSevisBatch(BatchId.NewBatchId(), sevisUsername, sevisOrgId, 1, 1)
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
                DivisionIso = "DC",
                LocationName = "name"
            };

            var exchangeVisitor = new ExchangeVisitor(
                sevisId: null,
                isValidated: false,
                sevisOrgId: "sevisOrgId",
                person: GetPerson(birthDate, 1, 2),
                financialInfo: new Business.Validation.Sevis.Finance.FinancialInfo(true, true, null, null),
                occupationCategoryCode: "99",
                programEndDate: DateTime.Now,
                programStartDate: DateTime.Now,
                dependents: new List<Business.Validation.Sevis.Bio.Dependent>(),
                siteOfActivity: siteOfActivity);

            var otherExchangeVisitor = new ExchangeVisitor(
                sevisId: null,
                isValidated: false,
                sevisOrgId: "sevisOrgId",
                person: GetPerson(birthDate, 1, 2),
                financialInfo: new Business.Validation.Sevis.Finance.FinancialInfo(false, false, null, null),
                occupationCategoryCode: "99",
                programEndDate: DateTime.Now.AddDays(1.0),
                programStartDate: DateTime.Now.AddDays(1.0),
                dependents: new List<Business.Validation.Sevis.Bio.Dependent>(),
                siteOfActivity: siteOfActivity);
            var participant = new SevisGroupedParticipantDTO
            {
                ParticipantId = exchangeVisitor.Person.ParticipantId
            };
            Assert.IsTrue(exchangeVisitor.GetChangeDetail(otherExchangeVisitor).HasChanges());
            Assert.AreEqual(1, exchangeVisitor.GetSEVISEVBatchTypeExchangeVisitor1Collection(sevisUsername, otherExchangeVisitor).Count());
            Assert.IsNull(service.GetAccomodatingStagedSevisBatch(batches, participant, exchangeVisitor, null, status.SevisUsername, "other org"));
        }
        #endregion

        #region GetNextBatchToUpload
        [TestMethod]
        public async Task TestGetNextBatchToUpload_ExceededUploadCooldown()
        {
            using (ShimsContext.Create())
            {
                System.Data.Entity.Fakes.ShimDbFunctions.DiffSecondsNullableOfDateTimeOffsetNullableOfDateTimeOffset = (startDate, endDate) =>
                {
                    if (!startDate.HasValue)
                    {
                        return null;
                    }
                    if (!endDate.HasValue)
                    {
                        return null;
                    }
                    return (int)((endDate.Value - startDate.Value).TotalSeconds);
                };
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
                    LastUploadTry = DateTimeOffset.UtcNow.AddDays(-1.0)
                };
                context.SevisBatchProcessings.Add(model);

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
        }

        [TestMethod]
        public async Task TestGetNextBatchToUpload_UploadCooldownNotYetPassed()
        {
            using (ShimsContext.Create())
            {
                System.Data.Entity.Fakes.ShimDbFunctions.DiffSecondsNullableOfDateTimeOffsetNullableOfDateTimeOffset = (startDate, endDate) =>
                {
                    if (!startDate.HasValue)
                    {
                        return null;
                    }
                    if (!endDate.HasValue)
                    {
                        return null;
                    }
                    return (int)((endDate.Value - startDate.Value).TotalSeconds);
                };
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
                    LastUploadTry = DateTimeOffset.UtcNow.AddDays(-1.0)
                };
                context.SevisBatchProcessings.Add(model);
                Assert.IsNotNull(service.GetNextBatchToUpload());

                model.LastUploadTry = DateTimeOffset.UtcNow;

                Action<SevisBatchProcessingDTO> tester = (dto) =>
                {
                    Assert.IsNull(dto);
                };
                var result = service.GetNextBatchToUpload();
                tester(result);
                var asyncResult = await service.GetNextBatchToUploadAsync();
                tester(asyncResult);
            }
        }

        [TestMethod]
        public async Task TestGetNextBatchToUpload_UploadCooldownValueIsNegative()
        {
            using (ShimsContext.Create())
            {
                System.Data.Entity.Fakes.ShimDbFunctions.DiffSecondsNullableOfDateTimeOffsetNullableOfDateTimeOffset = (startDate, endDate) =>
                {
                    if (!startDate.HasValue)
                    {
                        return null;
                    }
                    if (!endDate.HasValue)
                    {
                        return null;
                    }
                    return (int)((endDate.Value - startDate.Value).TotalSeconds);
                };
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
                    LastUploadTry = DateTimeOffset.UtcNow.AddDays(-1.0)
                };
                context.SevisBatchProcessings.Add(model);

                Assert.IsNotNull(service.GetNextBatchToUpload());
                appSettings[AppSettings.UPLOAD_COOLDOWN_IN_SECONDS] = "-" + appSettings[AppSettings.UPLOAD_COOLDOWN_IN_SECONDS];
                model.LastUploadTry = DateTimeOffset.UtcNow;

                Action<SevisBatchProcessingDTO> tester = (dto) =>
                {
                    Assert.IsNull(dto);
                };
                var result = service.GetNextBatchToUpload();
                tester(result);
                var asyncResult = await service.GetNextBatchToUploadAsync();
                tester(asyncResult);
            }
        }

        [TestMethod]
        public async Task TestGetNextBatchToUpload_HasUploadFailureCode()
        {
            using (ShimsContext.Create())
            {
                System.Data.Entity.Fakes.ShimDbFunctions.DiffSecondsNullableOfDateTimeOffsetNullableOfDateTimeOffset = (startDate, endDate) =>
                {
                    if (!startDate.HasValue)
                    {
                        return null;
                    }
                    if (!endDate.HasValue)
                    {
                        return null;
                    }
                    return (int)((endDate.Value - startDate.Value).TotalSeconds);
                };
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
        public async Task TestGetNextBatchToDownload_DoesNotHaveRetrieveDateAndSubmitDateCooldownHasPassed()
        {
            using (ShimsContext.Create())
            {
                System.Data.Entity.Fakes.ShimDbFunctions.DiffSecondsNullableOfDateTimeOffsetNullableOfDateTimeOffset = (startDate, endDate) =>
                {
                    if (!startDate.HasValue)
                    {
                        return null;
                    }
                    if (!endDate.HasValue)
                    {
                        return null;
                    }
                    return (int)((endDate.Value - startDate.Value).TotalSeconds);
                };
                var model = new SevisBatchProcessing
                {
                    BatchId = "batch id",
                    DownloadDispositionCode = "download code",
                    Id = 1,
                    ProcessDispositionCode = "process code",
                    RetrieveDate = null,
                    SendString = "send string",
                    SubmitDate = DateTimeOffset.UtcNow.AddDays(-1.0),
                    TransactionLogString = "transaction log",
                    UploadDispositionCode = "upload code"
                };
                context.SevisBatchProcessings.Add(model);

                Action<SevisBatchProcessingDTO> tester = (s) =>
                {
                    Assert.IsNotNull(s);
                    Assert.AreEqual(model.BatchId, s.BatchId);
                };
                var result = service.GetNextBatchToDownload();
                tester(result);
                var asyncResult = await service.GetNextBatchToDownloadAsync();
                tester(asyncResult);
            }

        }

        [TestMethod]
        public async Task TestGetNextBatchToDownload_DownloadCooldownHasNotYetPassed()
        {
            using (ShimsContext.Create())
            {
                System.Data.Entity.Fakes.ShimDbFunctions.DiffSecondsNullableOfDateTimeOffsetNullableOfDateTimeOffset = (startDate, endDate) =>
                {
                    if (!startDate.HasValue)
                    {
                        return null;
                    }
                    if (!endDate.HasValue)
                    {
                        return null;
                    }
                    return (int)((endDate.Value - startDate.Value).TotalSeconds);
                };
                var model = new SevisBatchProcessing
                {
                    BatchId = "batch id",
                    DownloadDispositionCode = "download code",
                    Id = 1,
                    ProcessDispositionCode = "process code",
                    RetrieveDate = null,
                    SendString = "send string",
                    SubmitDate = DateTimeOffset.UtcNow.AddDays(-1.0),
                    TransactionLogString = "transaction log",
                    UploadDispositionCode = "upload code",
                    LastDownloadTry = DateTimeOffset.UtcNow
                };
                context.SevisBatchProcessings.Add(model);
                Assert.AreEqual(1, SevisBatchProcessingQueries.CreateGetSevisBatchProcessingDTOsToDownloadQuery(context).Count());

                Action<SevisBatchProcessingDTO> tester = (s) =>
                {
                    Assert.IsNull(s);
                };
                var result = service.GetNextBatchToDownload();
                tester(result);
                var asyncResult = await service.GetNextBatchToDownloadAsync();
                tester(asyncResult);
            }
        }

        [TestMethod]
        public async Task TestGetNextBatchToDownload_SubmitDateCooldownHasNotYetSurpassed()
        {
            using (ShimsContext.Create())
            {
                System.Data.Entity.Fakes.ShimDbFunctions.DiffSecondsNullableOfDateTimeOffsetNullableOfDateTimeOffset = (startDate, endDate) =>
                {
                    if (!startDate.HasValue)
                    {
                        return null;
                    }
                    if (!endDate.HasValue)
                    {
                        return null;
                    }
                    return (int)((endDate.Value - startDate.Value).TotalSeconds);
                };
                var model = new SevisBatchProcessing
                {
                    BatchId = "batch id",
                    DownloadDispositionCode = "download code",
                    Id = 1,
                    ProcessDispositionCode = "process code",
                    RetrieveDate = null,
                    SendString = "send string",
                    SubmitDate = DateTimeOffset.UtcNow.AddDays(-1.0),
                    TransactionLogString = "transaction log",
                    UploadDispositionCode = "upload code",
                    LastDownloadTry = DateTimeOffset.UtcNow.AddDays(-1.0)
                };
                context.SevisBatchProcessings.Add(model);
                Assert.IsNotNull(service.GetNextBatchToDownload());

                model.SubmitDate = DateTimeOffset.UtcNow;
                Action<SevisBatchProcessingDTO> tester = (s) =>
                {
                    Assert.IsNull(s);
                };
                var result = service.GetNextBatchToDownload();
                tester(result);
                var asyncResult = await service.GetNextBatchToDownloadAsync();
                tester(asyncResult);
            }
        }

        [TestMethod]
        public async Task TestGetNextBatchToDownload_DownloadCooldownValueIsNegative()
        {
            using (ShimsContext.Create())
            {
                System.Data.Entity.Fakes.ShimDbFunctions.DiffSecondsNullableOfDateTimeOffsetNullableOfDateTimeOffset = (startDate, endDate) =>
                {
                    if (!startDate.HasValue)
                    {
                        return null;
                    }
                    if (!endDate.HasValue)
                    {
                        return null;
                    }
                    return (int)((endDate.Value - startDate.Value).TotalSeconds);
                };
                var model = new SevisBatchProcessing
                {
                    BatchId = "batch id",
                    DownloadDispositionCode = "download code",
                    Id = 1,
                    ProcessDispositionCode = "process code",
                    RetrieveDate = null,
                    SendString = "send string",
                    SubmitDate = DateTimeOffset.UtcNow.AddDays(-1.0),
                    TransactionLogString = "transaction log",
                    UploadDispositionCode = "upload code",
                    LastDownloadTry = DateTimeOffset.UtcNow.AddDays(-1.0)
                };
                context.SevisBatchProcessings.Add(model);
                Assert.IsNotNull(service.GetNextBatchToDownload());
                appSettings[AppSettings.DOWNLOAD_COOLDOWN_IN_SECONDS] = "-" + appSettings[AppSettings.DOWNLOAD_COOLDOWN_IN_SECONDS];

                model.LastDownloadTry = DateTimeOffset.UtcNow;
                Action<SevisBatchProcessingDTO> tester = (s) =>
                {
                    Assert.IsNull(s);
                };
                var result = service.GetNextBatchToDownload();
                tester(result);
                var asyncResult = await service.GetNextBatchToDownloadAsync();
                tester(asyncResult);
            }
        }

        [TestMethod]
        public async Task TestGetNextBatchToDownload_HasGeneralUploadDownloadFailure()
        {
            using (ShimsContext.Create())
            {
                System.Data.Entity.Fakes.ShimDbFunctions.DiffSecondsNullableOfDateTimeOffsetNullableOfDateTimeOffset = (startDate, endDate) =>
                {
                    if (!startDate.HasValue)
                    {
                        return null;
                    }
                    if (!endDate.HasValue)
                    {
                        return null;
                    }
                    return (int)((endDate.Value - startDate.Value).TotalSeconds);
                };
                var model = new SevisBatchProcessing
                {
                    BatchId = "batch id",
                    DownloadDispositionCode = DispositionCode.GeneralUploadDownloadFailure.Code,
                    Id = 1,
                    ProcessDispositionCode = "process code",
                    RetrieveDate = DateTimeOffset.UtcNow.AddDays(1.0),
                    SendString = "send string",
                    SubmitDate = DateTimeOffset.UtcNow.AddDays(-1.0),
                    TransactionLogString = "transaction log",
                    UploadDispositionCode = "upload code"
                };
                context.SevisBatchProcessings.Add(model);
                Assert.AreEqual(1, SevisBatchProcessingQueries.CreateGetSevisBatchProcessingDTOsToDownloadQuery(context).Count());

                Action<SevisBatchProcessingDTO> tester = (s) =>
                {
                    Assert.IsNotNull(s);
                    Assert.AreEqual(model.BatchId, s.BatchId);
                };
                var result = service.GetNextBatchToDownload();
                tester(result);
                var asyncResult = await service.GetNextBatchToDownloadAsync();
                tester(asyncResult);
            }
        }

        [TestMethod]
        public async Task TestGetNextBatchToDownload_HasBatchNotYetProcessedDownloadCode()
        {
            using (ShimsContext.Create())
            {
                System.Data.Entity.Fakes.ShimDbFunctions.DiffSecondsNullableOfDateTimeOffsetNullableOfDateTimeOffset = (startDate, endDate) =>
                {
                    if (!startDate.HasValue)
                    {
                        return null;
                    }
                    if (!endDate.HasValue)
                    {
                        return null;
                    }
                    return (int)((endDate.Value - startDate.Value).TotalSeconds);
                };
                var model = new SevisBatchProcessing
                {
                    BatchId = "batch id",
                    DownloadDispositionCode = DispositionCode.BatchNotYetProcessed.Code,
                    Id = 1,
                    ProcessDispositionCode = "process code",
                    RetrieveDate = DateTimeOffset.UtcNow.AddDays(1.0),
                    SendString = "send string",
                    SubmitDate = DateTimeOffset.UtcNow.AddDays(-1.0),
                    TransactionLogString = "transaction log",
                    UploadDispositionCode = "upload code"
                };
                context.SevisBatchProcessings.Add(model);
                Assert.AreEqual(1, SevisBatchProcessingQueries.CreateGetSevisBatchProcessingDTOsToDownloadQuery(context).Count());

                Action<SevisBatchProcessingDTO> tester = (s) =>
                {
                    Assert.IsNotNull(s);
                    Assert.AreEqual(model.BatchId, s.BatchId);
                };
                var result = service.GetNextBatchToDownload();
                tester(result);
                var asyncResult = await service.GetNextBatchToDownloadAsync();
                tester(asyncResult);
            }
        }

        [TestMethod]
        public async Task TestGetNextBatchToDownload_DoesNotHaveRecord()
        {
            Action<SevisBatchProcessingDTO> tester = (dto) =>
            {
                Assert.IsNull(dto);
            };
            var result = service.GetNextBatchToDownload();
            tester(result);
            var asyncResult = await service.GetNextBatchToDownloadAsync();
            tester(asyncResult);
        }
        #endregion

        #region Update Participant
        [TestMethod]
        public void TestUpdateParticipant_CreateParticipantOnly_Success()
        {
            var batch = new SevisBatchProcessing
            {
                BatchId = "batchId"
            };
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
                Dependent = null,
                requestID = new RequestId(participant.ParticipantId, RequestIdType.Participant, RequestActionType.Create).ToString()
            };
            var groupedProcessRecord = new GroupedTransactionLogTypeBatchDetailProcess
            {
                IsParticipant = true,
                IsPersonDependent = false,
                ObjectId = participant.ParticipantId,
                Records = new List<TransactionLogTypeBatchDetailProcessRecord>
                {
                    record
                }
            };
            var createUpdateEVBatch = new SEVISBatchCreateUpdateEV
            {

            };

            service.UpdateParticipant(user, participantPerson, new List<PersonDependent>(), createUpdateEVBatch, groupedProcessRecord, batch);
            Assert.AreEqual(1, context.ParticipantPersonSevisCommStatuses.Count());

            var addedStatus = context.ParticipantPersonSevisCommStatuses.First();
            Assert.AreEqual(participant.ParticipantId, addedStatus.ParticipantId);
            Assert.AreEqual(batch.BatchId, addedStatus.BatchId);
            Assert.AreEqual(SevisCommStatus.CreatedByBatch.Id, addedStatus.SevisCommStatusId);
            DateTimeOffset.UtcNow.Should().BeCloseTo(addedStatus.AddedOn, 20000);

            Assert.AreEqual(record.sevisID, participantPerson.SevisId);
            var jsonErrors = JsonConvert.DeserializeObject<List<SimpleSevisBatchErrorResult>>(participantPerson.SevisBatchResult);
            Assert.IsNotNull(jsonErrors);
            Assert.AreEqual(0, jsonErrors.Count);
        }

        [TestMethod]
        public void TestUpdateParticipant_UpdateParticipantOnly_Success()
        {
            var batch = new SevisBatchProcessing
            {
                BatchId = "batchId"
            };
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
                SevisId = "sevis id"
            };
            participantPerson.History.CreatedBy = otherUser.Id;
            participantPerson.History.CreatedOn = yesterday;
            participantPerson.History.RevisedBy = otherUser.Id;
            participantPerson.History.RevisedOn = yesterday;

            participant.ParticipantPerson = participantPerson;
            var record = new TransactionLogTypeBatchDetailProcessRecord
            {
                sevisID = participantPerson.SevisId,
                Result = new ResultType
                {
                    status = true
                },
                Dependent = null,
                requestID = new RequestId(participant.ParticipantId, RequestIdType.Participant, RequestActionType.Update).ToString()
            };
            var groupedProcessRecord = new GroupedTransactionLogTypeBatchDetailProcess
            {
                IsParticipant = true,
                IsPersonDependent = false,
                ObjectId = participant.ParticipantId,
                Records = new List<TransactionLogTypeBatchDetailProcessRecord>
                {
                    record
                }
            };
            var createUpdateEVBatch = new SEVISBatchCreateUpdateEV
            {

            };

            service.UpdateParticipant(user, participantPerson, new List<PersonDependent>(), createUpdateEVBatch, groupedProcessRecord, batch);
            Assert.AreEqual(1, context.ParticipantPersonSevisCommStatuses.Count());

            var addedStatus = context.ParticipantPersonSevisCommStatuses.First();
            Assert.AreEqual(participant.ParticipantId, addedStatus.ParticipantId);
            Assert.AreEqual(batch.BatchId, addedStatus.BatchId);
            Assert.AreEqual(SevisCommStatus.UpdatedByBatch.Id, addedStatus.SevisCommStatusId);
            DateTimeOffset.UtcNow.Should().BeCloseTo(addedStatus.AddedOn, 20000);

            Assert.AreEqual(record.sevisID, participantPerson.SevisId);
            var jsonErrors = JsonConvert.DeserializeObject<List<SimpleSevisBatchErrorResult>>(participantPerson.SevisBatchResult);
            Assert.IsNotNull(jsonErrors);
            Assert.AreEqual(0, jsonErrors.Count);
        }

        [TestMethod]
        public void TestUpdateParticipant_CreateParticipantOnly_Failure()
        {
            var batch = new SevisBatchProcessing
            {
                BatchId = "batchId"
            };
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
                Result = new ResultType
                {
                    status = false,
                    ErrorCode = "error code",
                    ErrorMessage = "error message",
                },
                Dependent = null,
                requestID = new RequestId(participant.ParticipantId, RequestIdType.Participant, RequestActionType.Create).ToString()
            };
            var groupedProcessRecord = new GroupedTransactionLogTypeBatchDetailProcess
            {
                IsParticipant = true,
                IsPersonDependent = false,
                ObjectId = participant.ParticipantId,
                Records = new List<TransactionLogTypeBatchDetailProcessRecord>
                {
                    record
                }
            };
            var createUpdateEVBatch = new SEVISBatchCreateUpdateEV
            {

            };

            service.UpdateParticipant(user, participantPerson, new List<PersonDependent>(), createUpdateEVBatch, groupedProcessRecord, batch);
            Assert.AreEqual(1, context.ParticipantPersonSevisCommStatuses.Count());

            var addedStatus = context.ParticipantPersonSevisCommStatuses.First();
            Assert.AreEqual(participant.ParticipantId, addedStatus.ParticipantId);
            Assert.AreEqual(batch.BatchId, addedStatus.BatchId);
            Assert.AreEqual(SevisCommStatus.InformationRequired.Id, addedStatus.SevisCommStatusId);
            DateTimeOffset.UtcNow.Should().BeCloseTo(addedStatus.AddedOn, 20000);

            Assert.AreEqual(record.sevisID, participantPerson.SevisId);
            var jsonErrors = JsonConvert.DeserializeObject<List<SimpleSevisBatchErrorResult>>(participantPerson.SevisBatchResult);
            Assert.IsNotNull(jsonErrors);
            Assert.AreEqual(1, jsonErrors.Count);
            Assert.AreEqual(record.Result.ErrorCode, jsonErrors.First().ErrorCode);
            Assert.AreEqual(record.Result.ErrorMessage, jsonErrors.First().ErrorMessage);
        }

        [TestMethod]
        public void TestUpdateParticipant_UpdateParticipantOnly_Failure()
        {
            var batch = new SevisBatchProcessing
            {
                BatchId = "batchId"
            };
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
                SevisId = "sevis id"
            };
            participantPerson.History.CreatedBy = otherUser.Id;
            participantPerson.History.CreatedOn = yesterday;
            participantPerson.History.RevisedBy = otherUser.Id;
            participantPerson.History.RevisedOn = yesterday;

            participant.ParticipantPerson = participantPerson;
            var record = new TransactionLogTypeBatchDetailProcessRecord
            {
                sevisID = participantPerson.SevisId,
                Result = new ResultType
                {
                    status = false,
                    ErrorCode = "code",
                    ErrorMessage = "message"
                },
                Dependent = null,
                requestID = new RequestId(participant.ParticipantId, RequestIdType.Participant, RequestActionType.Update).ToString()
            };
            var groupedProcessRecord = new GroupedTransactionLogTypeBatchDetailProcess
            {
                IsParticipant = true,
                IsPersonDependent = false,
                ObjectId = participant.ParticipantId,
                Records = new List<TransactionLogTypeBatchDetailProcessRecord>
                {
                    record
                }
            };
            var createUpdateEVBatch = new SEVISBatchCreateUpdateEV
            {

            };

            service.UpdateParticipant(user, participantPerson, new List<PersonDependent>(), createUpdateEVBatch, groupedProcessRecord, batch);
            Assert.AreEqual(1, context.ParticipantPersonSevisCommStatuses.Count());

            var addedStatus = context.ParticipantPersonSevisCommStatuses.First();
            Assert.AreEqual(participant.ParticipantId, addedStatus.ParticipantId);
            Assert.AreEqual(batch.BatchId, addedStatus.BatchId);
            Assert.AreEqual(SevisCommStatus.InformationRequired.Id, addedStatus.SevisCommStatusId);
            DateTimeOffset.UtcNow.Should().BeCloseTo(addedStatus.AddedOn, 20000);

            Assert.AreEqual(record.sevisID, participantPerson.SevisId);
            var jsonErrors = JsonConvert.DeserializeObject<List<SimpleSevisBatchErrorResult>>(participantPerson.SevisBatchResult);
            Assert.IsNotNull(jsonErrors);
            Assert.AreEqual(1, jsonErrors.Count);
            Assert.AreEqual(record.Result.ErrorCode, jsonErrors.First().ErrorCode);
            Assert.AreEqual(record.Result.ErrorMessage, jsonErrors.First().ErrorMessage);
        }

        [TestMethod]
        public void TestUpdateParticipant_ValidateParticipantOnly_Success()
        {
            var batch = new SevisBatchProcessing
            {
                BatchId = "batchId"
            };
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
                SevisId = "sevis id"
            };
            participantPerson.History.CreatedBy = otherUser.Id;
            participantPerson.History.CreatedOn = yesterday;
            participantPerson.History.RevisedBy = otherUser.Id;
            participantPerson.History.RevisedOn = yesterday;

            participant.ParticipantPerson = participantPerson;
            var record = new TransactionLogTypeBatchDetailProcessRecord
            {
                sevisID = participantPerson.SevisId,
                Result = new ResultType
                {
                    status = true
                },
                Dependent = null,
                requestID = new RequestId(participant.ParticipantId, RequestIdType.Validate, RequestActionType.Update).ToString()
            };
            var groupedProcessRecord = new GroupedTransactionLogTypeBatchDetailProcess
            {
                IsParticipant = true,
                IsPersonDependent = false,
                ObjectId = participant.ParticipantId,
                Records = new List<TransactionLogTypeBatchDetailProcessRecord>
                {
                    record
                }
            };
            var createUpdateEVBatch = new SEVISBatchCreateUpdateEV
            {

            };

            service.UpdateParticipant(user, participantPerson, new List<PersonDependent>(), createUpdateEVBatch, groupedProcessRecord, batch);
            Assert.AreEqual(1, context.ParticipantPersonSevisCommStatuses.Count());

            var addedStatus = context.ParticipantPersonSevisCommStatuses.First();
            Assert.AreEqual(participant.ParticipantId, addedStatus.ParticipantId);
            Assert.AreEqual(batch.BatchId, addedStatus.BatchId);
            Assert.AreEqual(SevisCommStatus.ValidatedByBatch.Id, addedStatus.SevisCommStatusId);
            DateTimeOffset.UtcNow.Should().BeCloseTo(addedStatus.AddedOn, 20000);

            Assert.AreEqual(record.sevisID, participantPerson.SevisId);
            var jsonErrors = JsonConvert.DeserializeObject<List<SimpleSevisBatchErrorResult>>(participantPerson.SevisBatchResult);
            Assert.IsNotNull(jsonErrors);
            Assert.AreEqual(0, jsonErrors.Count);
        }

        [TestMethod]
        public void TestUpdateParticipant_ValidateParticipantOnly_Failure()
        {
            var batch = new SevisBatchProcessing
            {
                BatchId = "batchId"
            };
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
                SevisId = "sevis id"
            };
            participantPerson.History.CreatedBy = otherUser.Id;
            participantPerson.History.CreatedOn = yesterday;
            participantPerson.History.RevisedBy = otherUser.Id;
            participantPerson.History.RevisedOn = yesterday;

            participant.ParticipantPerson = participantPerson;
            var record = new TransactionLogTypeBatchDetailProcessRecord
            {
                sevisID = participantPerson.SevisId,
                Result = new ResultType
                {
                    status = false,
                    ErrorCode = "code",
                    ErrorMessage = "message"
                },
                Dependent = null,
                requestID = new RequestId(participant.ParticipantId, RequestIdType.Validate, RequestActionType.Update).ToString()
            };
            var groupedProcessRecord = new GroupedTransactionLogTypeBatchDetailProcess
            {
                IsParticipant = true,
                IsPersonDependent = false,
                ObjectId = participant.ParticipantId,
                Records = new List<TransactionLogTypeBatchDetailProcessRecord>
                {
                    record
                }
            };
            var createUpdateEVBatch = new SEVISBatchCreateUpdateEV
            {

            };

            service.UpdateParticipant(user, participantPerson, new List<PersonDependent>(), createUpdateEVBatch, groupedProcessRecord, batch);
            Assert.AreEqual(1, context.ParticipantPersonSevisCommStatuses.Count());

            var addedStatus = context.ParticipantPersonSevisCommStatuses.First();
            Assert.AreEqual(participant.ParticipantId, addedStatus.ParticipantId);
            Assert.AreEqual(batch.BatchId, addedStatus.BatchId);
            Assert.AreEqual(SevisCommStatus.NeedsValidationInfo.Id, addedStatus.SevisCommStatusId);
            DateTimeOffset.UtcNow.Should().BeCloseTo(addedStatus.AddedOn, 20000);

            Assert.AreEqual(record.sevisID, participantPerson.SevisId);
            var jsonErrors = JsonConvert.DeserializeObject<List<SimpleSevisBatchErrorResult>>(participantPerson.SevisBatchResult);
            Assert.IsNotNull(jsonErrors);
            Assert.AreEqual(1, jsonErrors.Count);
            Assert.AreEqual(record.Result.ErrorCode, jsonErrors.First().ErrorCode);
            Assert.AreEqual(record.Result.ErrorMessage, jsonErrors.First().ErrorMessage);
        }

        [TestMethod]
        public void TestUpdateParticipant_CreateParticipantAndDependent_Success()
        {
            var batch = new SevisBatchProcessing
            {
                BatchId = "batchId"
            };
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

            var personDependent = new PersonDependent
            {
                DependentId = 20,
            };
            var dependentRecord = new TransactionLogTypeBatchDetailProcessRecordDependent
            {
                dependentSevisID = "dependent sevis id",
            };
            SetUserDefinedFields(dependentRecord, participant.ParticipantId, personDependent.DependentId);
            var record = new TransactionLogTypeBatchDetailProcessRecord
            {
                Result = new ResultType
                {
                    status = true
                },
                Dependent = new List<TransactionLogTypeBatchDetailProcessRecordDependent> { dependentRecord }.ToArray(),
                requestID = new RequestId(participant.ParticipantId, RequestIdType.Participant, RequestActionType.Create).ToString()
            };
            var groupedProcessRecord = new GroupedTransactionLogTypeBatchDetailProcess
            {
                IsParticipant = true,
                IsPersonDependent = false,
                ObjectId = participant.ParticipantId,
                Records = new List<TransactionLogTypeBatchDetailProcessRecord>
                {
                    record
                }
            };
            var createUpdateEVBatch = new SEVISBatchCreateUpdateEV
            {

            };

            service.UpdateParticipant(user, participantPerson, new List<PersonDependent> { personDependent }, createUpdateEVBatch, groupedProcessRecord, batch);
            Assert.AreEqual(1, context.ParticipantPersonSevisCommStatuses.Count());

            var addedStatus = context.ParticipantPersonSevisCommStatuses.First();
            Assert.AreEqual(participant.ParticipantId, addedStatus.ParticipantId);
            Assert.AreEqual(batch.BatchId, addedStatus.BatchId);
            Assert.AreEqual(SevisCommStatus.CreatedByBatch.Id, addedStatus.SevisCommStatusId);
            DateTimeOffset.UtcNow.Should().BeCloseTo(addedStatus.AddedOn, 20000);

            Assert.AreEqual(record.sevisID, participantPerson.SevisId);
            Assert.AreEqual(dependentRecord.dependentSevisID, personDependent.SevisId);

            var jsonErrors = JsonConvert.DeserializeObject<List<SimpleSevisBatchErrorResult>>(participantPerson.SevisBatchResult);
            Assert.IsNotNull(jsonErrors);
            Assert.AreEqual(0, jsonErrors.Count);
        }

        [TestMethod]
        public void TestUpdateParticipant_UpdateParticipantAndCreateDependent_Success()
        {
            var batch = new SevisBatchProcessing
            {
                BatchId = "batchId"
            };
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

            var personDependent = new PersonDependent
            {
                DependentId = 20,
            };
            var dependentRecord = new TransactionLogTypeBatchDetailProcessRecordDependent
            {
                dependentSevisID = "dependent sevis id",
            };
            SetUserDefinedFields(dependentRecord, participant.ParticipantId, personDependent.DependentId);
            var record = new TransactionLogTypeBatchDetailProcessRecord
            {
                Result = new ResultType
                {
                    status = true
                },
                Dependent = new List<TransactionLogTypeBatchDetailProcessRecordDependent> { dependentRecord }.ToArray(),
                requestID = new RequestId(participant.ParticipantId, RequestIdType.Participant, RequestActionType.Update).ToString()
            };
            var groupedProcessRecord = new GroupedTransactionLogTypeBatchDetailProcess
            {
                IsParticipant = true,
                IsPersonDependent = false,
                ObjectId = participant.ParticipantId,
                Records = new List<TransactionLogTypeBatchDetailProcessRecord>
                {
                    record
                }
            };
            var createUpdateEVBatch = new SEVISBatchCreateUpdateEV
            {

            };

            service.UpdateParticipant(user, participantPerson, new List<PersonDependent> { personDependent }, createUpdateEVBatch, groupedProcessRecord, batch);
            Assert.AreEqual(1, context.ParticipantPersonSevisCommStatuses.Count());

            var addedStatus = context.ParticipantPersonSevisCommStatuses.First();
            Assert.AreEqual(participant.ParticipantId, addedStatus.ParticipantId);
            Assert.AreEqual(batch.BatchId, addedStatus.BatchId);
            Assert.AreEqual(SevisCommStatus.UpdatedByBatch.Id, addedStatus.SevisCommStatusId);
            DateTimeOffset.UtcNow.Should().BeCloseTo(addedStatus.AddedOn, 20000);

            Assert.AreEqual(record.sevisID, participantPerson.SevisId);
            Assert.AreEqual(dependentRecord.dependentSevisID, personDependent.SevisId);

            var jsonErrors = JsonConvert.DeserializeObject<List<SimpleSevisBatchErrorResult>>(participantPerson.SevisBatchResult);
            Assert.IsNotNull(jsonErrors);
            Assert.AreEqual(0, jsonErrors.Count);
        }

        [TestMethod]
        public void TestUpdateParticipant_UpdateDependent_Success()
        {
            var batch = new SevisBatchProcessing
            {
                BatchId = "batchId"
            };
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

            var personDependent = new PersonDependent
            {
                DependentId = 20,
                SevisId = "sevis id"
            };

            var record = new TransactionLogTypeBatchDetailProcessRecord
            {
                Result = new ResultType
                {
                    status = true
                },
                sevisID = personDependent.SevisId,
                requestID = new RequestId(personDependent.DependentId, RequestIdType.Dependent, RequestActionType.Update).ToString()
            };

            var groupedProcessRecord = new GroupedTransactionLogTypeBatchDetailProcess
            {
                IsParticipant = false,
                IsPersonDependent = true,
                ObjectId = personDependent.DependentId,
                Records = new List<TransactionLogTypeBatchDetailProcessRecord>
                {
                    record
                }
            };
            var createUpdateEVBatch = new SEVISBatchCreateUpdateEV
            {

            };

            service.UpdateParticipant(user, participantPerson, new List<PersonDependent> { personDependent }, createUpdateEVBatch, groupedProcessRecord, batch);
            Assert.AreEqual(1, context.ParticipantPersonSevisCommStatuses.Count());

            var addedStatus = context.ParticipantPersonSevisCommStatuses.First();
            Assert.AreEqual(participant.ParticipantId, addedStatus.ParticipantId);
            Assert.AreEqual(batch.BatchId, addedStatus.BatchId);
            Assert.AreEqual(SevisCommStatus.UpdatedByBatch.Id, addedStatus.SevisCommStatusId);
            DateTimeOffset.UtcNow.Should().BeCloseTo(addedStatus.AddedOn, 20000);

            var jsonErrors = JsonConvert.DeserializeObject<List<SimpleSevisBatchErrorResult>>(participantPerson.SevisBatchResult);
            Assert.IsNotNull(jsonErrors);
            Assert.AreEqual(0, jsonErrors.Count);

            Assert.AreEqual(record.sevisID, personDependent.SevisId);
            Assert.IsFalse(personDependent.IsSevisDeleted);
            Assert.AreEqual(user.Id, personDependent.History.RevisedBy);
            DateTimeOffset.UtcNow.Should().BeCloseTo(personDependent.History.RevisedOn, 20000);
        }

        [TestMethod]
        public void TestUpdateParticipant_CreateDependent_Success()
        {
            var batch = new SevisBatchProcessing
            {
                BatchId = "batchId"
            };
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

            var personDependent = new PersonDependent
            {
                DependentId = 20,
                SevisId = "sevis id"
            };

            var record = new TransactionLogTypeBatchDetailProcessRecord
            {
                Result = new ResultType
                {
                    status = true
                },
                sevisID = personDependent.SevisId,
                requestID = new RequestId(personDependent.DependentId, RequestIdType.Dependent, RequestActionType.Create).ToString()
            };

            var groupedProcessRecord = new GroupedTransactionLogTypeBatchDetailProcess
            {
                IsParticipant = false,
                IsPersonDependent = true,
                ObjectId = personDependent.DependentId,
                Records = new List<TransactionLogTypeBatchDetailProcessRecord>
                {
                    record
                }
            };
            var createUpdateEVBatch = new SEVISBatchCreateUpdateEV
            {

            };

            service.UpdateParticipant(user, participantPerson, new List<PersonDependent> { personDependent }, createUpdateEVBatch, groupedProcessRecord, batch);
            Assert.AreEqual(1, context.ParticipantPersonSevisCommStatuses.Count());

            var addedStatus = context.ParticipantPersonSevisCommStatuses.First();
            Assert.AreEqual(participant.ParticipantId, addedStatus.ParticipantId);
            Assert.AreEqual(batch.BatchId, addedStatus.BatchId);
            Assert.AreEqual(SevisCommStatus.UpdatedByBatch.Id, addedStatus.SevisCommStatusId);
            DateTimeOffset.UtcNow.Should().BeCloseTo(addedStatus.AddedOn, 20000);

            var jsonErrors = JsonConvert.DeserializeObject<List<SimpleSevisBatchErrorResult>>(participantPerson.SevisBatchResult);
            Assert.IsNotNull(jsonErrors);
            Assert.AreEqual(0, jsonErrors.Count);

            Assert.AreEqual(record.sevisID, personDependent.SevisId);
            Assert.IsFalse(personDependent.IsSevisDeleted);
            Assert.AreEqual(user.Id, personDependent.History.RevisedBy);
            DateTimeOffset.UtcNow.Should().BeCloseTo(personDependent.History.RevisedOn, 20000);
        }

        [TestMethod]
        public void TestUpdateParticipant_UpdateDependent_DependentWasDeleted_Success()
        {
            var batch = new SevisBatchProcessing
            {
                BatchId = "batchId"
            };
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

            var personDependent = new PersonDependent
            {
                DependentId = 20,
                SevisId = "sevis id"
            };

            var record = new TransactionLogTypeBatchDetailProcessRecord
            {
                Result = new ResultType
                {
                    status = true
                },
                sevisID = personDependent.SevisId,
                requestID = new RequestId(personDependent.DependentId, RequestIdType.Dependent, RequestActionType.Update).ToString()
            };

            var groupedProcessRecord = new GroupedTransactionLogTypeBatchDetailProcess
            {
                IsParticipant = false,
                IsPersonDependent = true,
                ObjectId = personDependent.DependentId,
                Records = new List<TransactionLogTypeBatchDetailProcessRecord>
                {
                    record
                }
            };
            var createUpdateEVBatch = new SEVISBatchCreateUpdateEV
            {
                UpdateEV = new List<SEVISEVBatchTypeExchangeVisitor1>
                {
                    new SEVISEVBatchTypeExchangeVisitor1
                    {
                        Item = new SEVISEVBatchTypeExchangeVisitorDependent
                        {
                            Item = new SEVISEVBatchTypeExchangeVisitorDependentDelete
                            {
                                dependentSevisID = personDependent.SevisId
                            }
                        }
                    }

                }.ToArray()
            };

            service.UpdateParticipant(user, participantPerson, new List<PersonDependent> { personDependent }, createUpdateEVBatch, groupedProcessRecord, batch);
            Assert.AreEqual(1, context.ParticipantPersonSevisCommStatuses.Count());

            var addedStatus = context.ParticipantPersonSevisCommStatuses.First();
            Assert.AreEqual(participant.ParticipantId, addedStatus.ParticipantId);
            Assert.AreEqual(batch.BatchId, addedStatus.BatchId);
            Assert.AreEqual(SevisCommStatus.UpdatedByBatch.Id, addedStatus.SevisCommStatusId);
            DateTimeOffset.UtcNow.Should().BeCloseTo(addedStatus.AddedOn, 20000);

            var jsonErrors = JsonConvert.DeserializeObject<List<SimpleSevisBatchErrorResult>>(participantPerson.SevisBatchResult);
            Assert.IsNotNull(jsonErrors);
            Assert.AreEqual(0, jsonErrors.Count);

            Assert.IsTrue(personDependent.IsSevisDeleted);
            Assert.AreEqual(record.sevisID, personDependent.SevisId);
            Assert.AreEqual(user.Id, personDependent.History.RevisedBy);
            DateTimeOffset.UtcNow.Should().BeCloseTo(personDependent.History.RevisedOn, 20000);
        }

        [TestMethod]
        public void TestUpdateParticipant_UpdateDependent_Failure()
        {
            var batch = new SevisBatchProcessing
            {
                BatchId = "batchId"
            };
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

            var personDependent = new PersonDependent
            {
                DependentId = 20,
                SevisId = "sevis id"
            };

            var record = new TransactionLogTypeBatchDetailProcessRecord
            {
                Result = new ResultType
                {
                    status = false,
                    ErrorCode = "code",
                    ErrorMessage = "message"
                },
                sevisID = personDependent.SevisId,
                requestID = new RequestId(participant.ParticipantId, RequestIdType.Dependent, RequestActionType.Update).ToString()
            };

            var groupedProcessRecord = new GroupedTransactionLogTypeBatchDetailProcess
            {
                IsParticipant = false,
                IsPersonDependent = true,
                ObjectId = personDependent.DependentId,
                Records = new List<TransactionLogTypeBatchDetailProcessRecord>
                {
                    record
                }
            };
            var createUpdateEVBatch = new SEVISBatchCreateUpdateEV
            {

            };

            service.UpdateParticipant(user, participantPerson, new List<PersonDependent> { personDependent }, createUpdateEVBatch, groupedProcessRecord, batch);
            Assert.AreEqual(1, context.ParticipantPersonSevisCommStatuses.Count());

            var addedStatus = context.ParticipantPersonSevisCommStatuses.First();
            Assert.AreEqual(participant.ParticipantId, addedStatus.ParticipantId);
            Assert.AreEqual(batch.BatchId, addedStatus.BatchId);
            Assert.AreEqual(SevisCommStatus.InformationRequired.Id, addedStatus.SevisCommStatusId);
            DateTimeOffset.UtcNow.Should().BeCloseTo(addedStatus.AddedOn, 20000);

            var jsonErrors = JsonConvert.DeserializeObject<List<SimpleSevisBatchErrorResult>>(participantPerson.SevisBatchResult);
            Assert.IsNotNull(jsonErrors);
            Assert.AreEqual(1, jsonErrors.Count);
            Assert.AreEqual(record.Result.ErrorCode, jsonErrors.First().ErrorCode);
            Assert.AreEqual(record.Result.ErrorMessage, jsonErrors.First().ErrorMessage);
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
        public void GetSevisBatchResultTypeAsJson_ListHasError()
        {
            var resultType = new ResultType
            {
                ErrorCode = "error code",
                ErrorMessage = "error message",
                status = false,
                statusSpecified = true
            };

            var json = service.GetSevisBatchResultTypeAsJson(new List<ResultType> { resultType });
            Assert.IsNotNull(json);
            var jsonAsArray = JsonConvert.DeserializeObject<List<SimpleSevisBatchErrorResult>>(json);
            Assert.IsNotNull(jsonAsArray);
            Assert.AreEqual(1, jsonAsArray.Count);
            Assert.AreEqual(resultType.ErrorCode, jsonAsArray.First().ErrorCode);
            Assert.AreEqual(resultType.ErrorMessage, jsonAsArray.First().ErrorMessage);
        }

        [TestMethod]
        public void GetSevisBatchResultTypeAsJson_ListDoesNotHaveError()
        {
            var resultType = new ResultType
            {
                status = true,
                statusSpecified = true
            };
            var json = service.GetSevisBatchResultTypeAsJson(new List<ResultType> { resultType });
            Assert.IsNotNull(json);
            var jsonAsArray = JsonConvert.DeserializeObject<List<SimpleSevisBatchErrorResult>>(json);
            Assert.IsNotNull(jsonAsArray);
            Assert.AreEqual(0, jsonAsArray.Count);
        }

        [TestMethod]
        public void TestAddResultTypeSevisCommStatus_IsSuccess_SevisIdNull()
        {
            var batch = new SevisBatchProcessing
            {
                BatchId = "batchId"
            };
            var resultType = new ResultType
            {
                status = true
            };
            var participantPerson = new ParticipantPerson
            {
                ParticipantId = 1,
                SevisId = null
            };
            var requestId = new RequestId(participantPerson.ParticipantId, RequestIdType.Participant, RequestActionType.Create);
            Assert.AreEqual(0, context.ParticipantPersonSevisCommStatuses.Count());

            service.AddResultTypeSevisCommStatus(requestId, resultType, participantPerson, batch);
            Assert.AreEqual(1, context.ParticipantPersonSevisCommStatuses.Count());
            Assert.AreEqual(1, participantPerson.ParticipantPersonSevisCommStatuses.Count());
            Assert.IsTrue(Object.ReferenceEquals(context.ParticipantPersonSevisCommStatuses.First(), participantPerson.ParticipantPersonSevisCommStatuses.First()));

            var firstStatus = context.ParticipantPersonSevisCommStatuses.First();
            Assert.AreEqual(SevisCommStatus.CreatedByBatch.Id, firstStatus.SevisCommStatusId);
            Assert.AreEqual(participantPerson.ParticipantId, firstStatus.ParticipantId);
            DateTimeOffset.UtcNow.Should().BeCloseTo(firstStatus.AddedOn, 20000);
            Assert.AreEqual(batch.BatchId, firstStatus.BatchId);
        }

        [TestMethod]
        public void TestAddResultTypeSevisCommStatus_IsSuccess_SevisIdEmpty()
        {
            var batch = new SevisBatchProcessing
            {
                BatchId = "batchId"
            };
            var resultType = new ResultType
            {
                status = true
            };
            var participantPerson = new ParticipantPerson
            {
                ParticipantId = 1,
                SevisId = String.Empty
            };
            var requestId = new RequestId(participantPerson.ParticipantId, RequestIdType.Participant, RequestActionType.Create);
            Assert.AreEqual(0, context.ParticipantPersonSevisCommStatuses.Count());

            service.AddResultTypeSevisCommStatus(requestId, resultType, participantPerson, batch);
            Assert.AreEqual(1, context.ParticipantPersonSevisCommStatuses.Count());
            Assert.AreEqual(1, participantPerson.ParticipantPersonSevisCommStatuses.Count());
            Assert.IsTrue(Object.ReferenceEquals(context.ParticipantPersonSevisCommStatuses.First(), participantPerson.ParticipantPersonSevisCommStatuses.First()));

            var firstStatus = context.ParticipantPersonSevisCommStatuses.First();
            Assert.AreEqual(SevisCommStatus.CreatedByBatch.Id, firstStatus.SevisCommStatusId);
            Assert.AreEqual(participantPerson.ParticipantId, firstStatus.ParticipantId);
            DateTimeOffset.UtcNow.Should().BeCloseTo(firstStatus.AddedOn, 20000);
            Assert.AreEqual(batch.BatchId, firstStatus.BatchId);
        }

        [TestMethod]
        public void TestAddResultTypeSevisCommStatus_IsSuccess_SevisIdIsWhitespace()
        {
            var batch = new SevisBatchProcessing
            {
                BatchId = "batchId"
            };
            var resultType = new ResultType
            {
                status = true
            };
            var participantPerson = new ParticipantPerson
            {
                ParticipantId = 1,
                SevisId = " "
            };
            var requestId = new RequestId(participantPerson.ParticipantId, RequestIdType.Participant, RequestActionType.Create);
            Assert.AreEqual(0, context.ParticipantPersonSevisCommStatuses.Count());

            service.AddResultTypeSevisCommStatus(requestId, resultType, participantPerson, batch);
            Assert.AreEqual(1, context.ParticipantPersonSevisCommStatuses.Count());
            Assert.AreEqual(1, participantPerson.ParticipantPersonSevisCommStatuses.Count());
            Assert.IsTrue(Object.ReferenceEquals(context.ParticipantPersonSevisCommStatuses.First(), participantPerson.ParticipantPersonSevisCommStatuses.First()));

            var firstStatus = context.ParticipantPersonSevisCommStatuses.First();
            Assert.AreEqual(SevisCommStatus.CreatedByBatch.Id, firstStatus.SevisCommStatusId);
            Assert.AreEqual(participantPerson.ParticipantId, firstStatus.ParticipantId);
            DateTimeOffset.UtcNow.Should().BeCloseTo(firstStatus.AddedOn, 20000);
            Assert.AreEqual(batch.BatchId, firstStatus.BatchId);
        }

        [TestMethod]
        public void TestAddResultTypeSevisCommStatus_IsSuccess_HasSevisId()
        {
            var batch = new SevisBatchProcessing
            {
                BatchId = "batchId"
            };
            var resultType = new ResultType
            {
                status = true
            };
            var participantPerson = new ParticipantPerson
            {
                ParticipantId = 1,
                SevisId = "sevisid"
            };
            Assert.AreEqual(0, context.ParticipantPersonSevisCommStatuses.Count());

            var requestId = new RequestId(participantPerson.ParticipantId, RequestIdType.Participant, RequestActionType.Update);
            service.AddResultTypeSevisCommStatus(requestId, resultType, participantPerson, batch);
            Assert.AreEqual(1, context.ParticipantPersonSevisCommStatuses.Count());
            Assert.AreEqual(1, participantPerson.ParticipantPersonSevisCommStatuses.Count());
            Assert.IsTrue(Object.ReferenceEquals(context.ParticipantPersonSevisCommStatuses.First(), participantPerson.ParticipantPersonSevisCommStatuses.First()));

            var firstStatus = context.ParticipantPersonSevisCommStatuses.First();
            Assert.AreEqual(SevisCommStatus.UpdatedByBatch.Id, firstStatus.SevisCommStatusId);
            Assert.AreEqual(participantPerson.ParticipantId, firstStatus.ParticipantId);
            DateTimeOffset.UtcNow.Should().BeCloseTo(firstStatus.AddedOn, 20000);
            Assert.AreEqual(batch.BatchId, firstStatus.BatchId);
        }

        [TestMethod]
        public void TestAddResultTypeSevisCommStatus_IsSuccess_HasSevisIdAndIsValidateRequest()
        {
            var batch = new SevisBatchProcessing
            {
                BatchId = "batchId"
            };
            var resultType = new ResultType
            {
                status = true
            };
            var participantPerson = new ParticipantPerson
            {
                ParticipantId = 1,
                SevisId = "sevisid"
            };
            Assert.AreEqual(0, context.ParticipantPersonSevisCommStatuses.Count());

            var requestId = new RequestId(participantPerson.ParticipantId, RequestIdType.Validate, RequestActionType.Update);
            service.AddResultTypeSevisCommStatus(requestId, resultType, participantPerson, batch);
            Assert.AreEqual(1, context.ParticipantPersonSevisCommStatuses.Count());
            Assert.AreEqual(1, participantPerson.ParticipantPersonSevisCommStatuses.Count());
            Assert.IsTrue(Object.ReferenceEquals(context.ParticipantPersonSevisCommStatuses.First(), participantPerson.ParticipantPersonSevisCommStatuses.First()));

            var firstStatus = context.ParticipantPersonSevisCommStatuses.First();
            Assert.AreEqual(SevisCommStatus.ValidatedByBatch.Id, firstStatus.SevisCommStatusId);
            Assert.AreEqual(participantPerson.ParticipantId, firstStatus.ParticipantId);
            DateTimeOffset.UtcNow.Should().BeCloseTo(firstStatus.AddedOn, 20000);
            Assert.AreEqual(batch.BatchId, firstStatus.BatchId);
        }

        [TestMethod]
        public void TestAddResultTypeSevisCommStatus_IsErrorAndRequestIdIsNotValidate()
        {
            var batch = new SevisBatchProcessing
            {
                BatchId = "batchId"
            };
            var resultType = new ResultType
            {
                status = false
            };
            var participantPerson = new ParticipantPerson
            {
                ParticipantId = 1
            };
            Assert.AreEqual(0, context.ParticipantPersonSevisCommStatuses.Count());

            var requestId = new RequestId(participantPerson.ParticipantId, RequestIdType.Participant, RequestActionType.Create);
            service.AddResultTypeSevisCommStatus(requestId, resultType, participantPerson, batch);
            Assert.AreEqual(1, context.ParticipantPersonSevisCommStatuses.Count());
            Assert.AreEqual(1, participantPerson.ParticipantPersonSevisCommStatuses.Count());
            Assert.IsTrue(Object.ReferenceEquals(context.ParticipantPersonSevisCommStatuses.First(), participantPerson.ParticipantPersonSevisCommStatuses.First()));

            var firstStatus = context.ParticipantPersonSevisCommStatuses.First();
            Assert.AreEqual(SevisCommStatus.InformationRequired.Id, firstStatus.SevisCommStatusId);
            Assert.AreEqual(participantPerson.ParticipantId, firstStatus.ParticipantId);
            DateTimeOffset.UtcNow.Should().BeCloseTo(firstStatus.AddedOn, 20000);
            Assert.AreEqual(batch.BatchId, firstStatus.BatchId);
        }

        [TestMethod]
        public void TestAddResultTypeSevisCommStatus_IsErrorAndRequestIdIsValidate()
        {
            var batch = new SevisBatchProcessing
            {
                BatchId = "batchId"
            };
            var resultType = new ResultType
            {
                status = false
            };
            var participantPerson = new ParticipantPerson
            {
                ParticipantId = 1
            };
            Assert.AreEqual(0, context.ParticipantPersonSevisCommStatuses.Count());

            var requestId = new RequestId(participantPerson.ParticipantId, RequestIdType.Validate, RequestActionType.Create);
            service.AddResultTypeSevisCommStatus(requestId, resultType, participantPerson, batch);
            Assert.AreEqual(1, context.ParticipantPersonSevisCommStatuses.Count());
            Assert.AreEqual(1, participantPerson.ParticipantPersonSevisCommStatuses.Count());
            Assert.IsTrue(Object.ReferenceEquals(context.ParticipantPersonSevisCommStatuses.First(), participantPerson.ParticipantPersonSevisCommStatuses.First()));

            var firstStatus = context.ParticipantPersonSevisCommStatuses.First();
            Assert.AreEqual(SevisCommStatus.NeedsValidationInfo.Id, firstStatus.SevisCommStatusId);
            Assert.AreEqual(participantPerson.ParticipantId, firstStatus.ParticipantId);
            DateTimeOffset.UtcNow.Should().BeCloseTo(firstStatus.AddedOn, 20000);
            Assert.AreEqual(batch.BatchId, firstStatus.BatchId);
        }

        [TestMethod]
        public async Task TestProcessBatchDetailProcess_NotSuccessCode()
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
            SEVISBatchCreateUpdateEV createUpdateBatch = null;
            ExchangeVisitorHistory history = null;
            var pendingHistoryModel = "pending";
            context.SetupActions.Add(() =>
            {
                createUpdateBatch = new SEVISBatchCreateUpdateEV();
                batch = new SevisBatchProcessing
                {
                    BatchId = "hello",
                    Id = 1,
                    SendString = GetXml(createUpdateBatch)
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
                history = new ExchangeVisitorHistory
                {
                    ParticipantId = participantId,
                    PendingModel = pendingHistoryModel
                };
                context.ExchangeVisitorHistories.Add(history);
                context.ParticipantPersons.Add(participantPerson);
                context.People.Add(person);
                context.SevisBatchProcessings.Add(batch);
            });
            var requestId = new RequestId(participantId, RequestIdType.Participant, RequestActionType.Create);
            var record = new TransactionLogTypeBatchDetailProcessRecord
            {
                requestID = requestId.ToString(),
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
                resultCode = DispositionCode.GeneralUploadDownloadFailure.Code,
                RecordCount = new TransactionLogTypeBatchDetailProcessRecordCount
                {
                    Failure = "1",
                    Success = "2"
                }
            };
            var url = new Uri("http://www.google.com");
            var fileContents = new byte[1] { (byte)1 };
            var fileContentStream = new MemoryStream(fileContents);
            var fileContentStreamAsync = new MemoryStream(fileContents);

            fileProvider.Setup(x => x.GetDS2019FileStream(It.IsAny<RequestId>(), It.IsAny<string>())).Returns(fileContentStream);
            fileProvider.Setup(x => x.GetDS2019FileStreamAsync(It.IsAny<RequestId>(), It.IsAny<string>())).ReturnsAsync(fileContentStreamAsync);
            cloudStorageService.Setup(x => x.UploadBlob(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(url);
            cloudStorageService.Setup(x => x.UploadBlobAsync(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(url);
            Action tester = () =>
            {
                Assert.AreEqual(pendingHistoryModel, history.PendingModel);
                Assert.IsNull(history.LastSuccessfulModel);
            };
            context.Revert();
            service.ProcessBatchDetailProcess(user, processDetail, batch, fileProvider.Object);
            tester();

            context.Revert();
            await service.ProcessBatchDetailProcessAsync(user, processDetail, batch, fileProvider.Object);
            tester();

        }

        [TestMethod]
        public async Task TestProcessBatchDetailProcess_IsParticipantRequest_NoDependents()
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
            SEVISBatchCreateUpdateEV createUpdateBatch = null;
            ExchangeVisitorHistory history = null;
            var pendingHistoryModel = "pending";
            context.SetupActions.Add(() =>
            {
                createUpdateBatch = new SEVISBatchCreateUpdateEV();
                batch = new SevisBatchProcessing
                {
                    BatchId = "hello",
                    Id = 1,
                    SendString = GetXml(createUpdateBatch)
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
                history = new ExchangeVisitorHistory
                {
                    ParticipantId = participantId,
                    PendingModel = pendingHistoryModel
                };
                context.ExchangeVisitorHistories.Add(history);
                context.ParticipantPersons.Add(participantPerson);
                context.People.Add(person);
                context.SevisBatchProcessings.Add(batch);
            });
            var requestId = new RequestId(participantId, RequestIdType.Participant, RequestActionType.Create);
            var record = new TransactionLogTypeBatchDetailProcessRecord
            {
                requestID = requestId.ToString(),
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
                resultCode = DispositionCode.Success.Code,
                RecordCount = new TransactionLogTypeBatchDetailProcessRecordCount
                {
                    Failure = "1",
                    Success = "2"
                }
            };
            var url = new Uri("http://www.google.com");
            var fileContents = new byte[1] { (byte)1 };
            var fileContentStream = new MemoryStream(fileContents);
            var fileContentStreamAsync = new MemoryStream(fileContents);

            fileProvider.Setup(x => x.GetDS2019FileStream(It.IsAny<RequestId>(), It.IsAny<string>())).Returns(fileContentStream);
            fileProvider.Setup(x => x.GetDS2019FileStreamAsync(It.IsAny<RequestId>(), It.IsAny<string>())).ReturnsAsync(fileContentStreamAsync);
            cloudStorageService.Setup(x => x.UploadBlob(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(url);
            cloudStorageService.Setup(x => x.UploadBlobAsync(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(url);
            Action tester = () =>
            {
                Assert.AreEqual(user.Id, participantPerson.History.RevisedBy);
                Assert.AreEqual(otherUser.Id, participantPerson.History.CreatedBy);
                Assert.AreEqual(yesterday, participantPerson.History.CreatedOn);
                DateTimeOffset.UtcNow.Should().BeCloseTo(participantPerson.History.RevisedOn, 20000);
                Assert.AreEqual(sevisId, participantPerson.SevisId);

                Assert.AreEqual(1, context.ParticipantPersonSevisCommStatuses.Count());
                Assert.AreEqual(processDetail.resultCode, batch.ProcessDispositionCode);
                Assert.AreEqual(participantPerson.GetDS2019FileName(), participantPerson.DS2019FileName);

                Assert.IsNull(history.PendingModel);
                Assert.AreEqual(pendingHistoryModel, history.LastSuccessfulModel);
                DateTimeOffset.UtcNow.Should().BeCloseTo(history.RevisedOn, 20000);

                Assert.AreEqual(1, context.ParticipantPersonSevisCommStatuses.Count());
                var firstStatus = context.ParticipantPersonSevisCommStatuses.First();
                Assert.AreEqual(SevisCommStatus.CreatedByBatch.Id, firstStatus.SevisCommStatusId);
                Assert.AreEqual(participantId, firstStatus.ParticipantId);
                DateTimeOffset.UtcNow.Should().BeCloseTo(firstStatus.AddedOn, 20000);
            };
            context.Revert();
            service.ProcessBatchDetailProcess(user, processDetail, batch, fileProvider.Object);
            tester();
            notificationService.Verify(x => x.NotifyFinishedProcessingSevisBatchDetails(It.IsAny<string>(), It.IsAny<DispositionCode>()), Times.Exactly(1));
            notificationService.Verify(x => x.NotifyStartedProcessingSevisBatchDetails(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(1));
            cloudStorageService.Verify(x => x.UploadBlob(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(1));
            fileProvider.Verify(x => x.GetDS2019FileStream(It.IsAny<RequestId>(), It.IsAny<string>()), Times.Exactly(1));

            context.Revert();
            await service.ProcessBatchDetailProcessAsync(user, processDetail, batch, fileProvider.Object);
            tester();
            notificationService.Verify(x => x.NotifyFinishedProcessingSevisBatchDetails(It.IsAny<string>(), It.IsAny<DispositionCode>()), Times.Exactly(2));
            notificationService.Verify(x => x.NotifyStartedProcessingSevisBatchDetails(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(2));
            cloudStorageService.Verify(x => x.UploadBlobAsync(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(1));
            fileProvider.Verify(x => x.GetDS2019FileStreamAsync(It.IsAny<RequestId>(), It.IsAny<string>()), Times.Exactly(1));
        }

        [TestMethod]
        public async Task TestProcessBatchDetailProcess_IsParticipantRequest_HasDependent()
        {
            var sevisId = "sevis id";
            var user = new User(1);
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var otherUser = new User(user.Id + 1);
            Participant participant = null;
            ParticipantPerson participantPerson = null;
            PersonDependent dependent = null;
            Data.Person person = null;
            SevisBatchProcessing batch = null;
            ExchangeVisitorHistory history = null;
            var pendingHistoryModel = "pending";
            var participantId = 1;
            var personId = 2;
            var dependentId = 3;
            SEVISBatchCreateUpdateEV createUpdateBatch = null;
            context.SetupActions.Add(() =>
            {
                createUpdateBatch = new SEVISBatchCreateUpdateEV();
                batch = new SevisBatchProcessing
                {
                    BatchId = "hello",
                    Id = 1,
                    SendString = GetXml(createUpdateBatch)
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

                dependent = new PersonDependent
                {
                    DependentId = dependentId,
                    Person = person,
                    PersonId = person.PersonId
                };
                dependent.History.CreatedBy = otherUser.Id;
                dependent.History.CreatedOn = yesterday;
                dependent.History.RevisedBy = otherUser.Id;
                dependent.History.RevisedOn = yesterday;
                person.Family.Add(dependent);
                history = new ExchangeVisitorHistory
                {
                    ParticipantId = participantId,
                    PendingModel = pendingHistoryModel
                };
                context.ExchangeVisitorHistories.Add(history);
                context.PersonDependents.Add(dependent);
                context.Participants.Add(participant);
                context.ParticipantPersons.Add(participantPerson);
                context.People.Add(person);
                context.SevisBatchProcessings.Add(batch);
            });
            var requestId = new RequestId(participantId, RequestIdType.Participant, RequestActionType.Create);
            var dependentRecord = new TransactionLogTypeBatchDetailProcessRecordDependent
            {
                dependentSevisID = "dependentsevisId",
            };
            SetUserDefinedFields(dependentRecord, participantId, dependentId);
            var record = new TransactionLogTypeBatchDetailProcessRecord
            {
                requestID = requestId.ToString(),
                sevisID = sevisId,
                Result = new ResultType
                {
                    status = true
                },
                Dependent = new TransactionLogTypeBatchDetailProcessRecordDependent[] { dependentRecord },
            };
            SetUserDefinedFields(record, participantId, personId);

            var processDetail = new TransactionLogTypeBatchDetailProcess
            {
                Record = new List<TransactionLogTypeBatchDetailProcessRecord> { record }.ToArray(),
                resultCode = DispositionCode.Success.Code,
                RecordCount = new TransactionLogTypeBatchDetailProcessRecordCount
                {
                    Failure = "1",
                    Success = "2"
                }
            };
            var url = new Uri("http://www.google.com");
            var fileContents = new byte[1] { (byte)1 };
            var fileContentStream = new MemoryStream(fileContents);
            var fileContentStreamAsync = new MemoryStream(fileContents);

            fileProvider.Setup(x => x.GetDS2019FileStream(It.IsAny<RequestId>(), It.IsAny<string>())).Returns(fileContentStream);
            fileProvider.Setup(x => x.GetDS2019FileStreamAsync(It.IsAny<RequestId>(), It.IsAny<string>())).ReturnsAsync(fileContentStreamAsync);
            cloudStorageService.Setup(x => x.UploadBlob(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(url);
            cloudStorageService.Setup(x => x.UploadBlobAsync(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(url);
            Action tester = () =>
            {
                Assert.AreEqual(user.Id, dependent.History.RevisedBy);
                Assert.AreEqual(otherUser.Id, dependent.History.CreatedBy);
                Assert.AreEqual(yesterday, dependent.History.CreatedOn);
                DateTimeOffset.UtcNow.Should().BeCloseTo(dependent.History.RevisedOn, 20000);
                dependent.SevisId = dependentRecord.dependentSevisID;
                dependent.IsSevisDeleted = false;

                Assert.IsNull(history.PendingModel);
                Assert.AreEqual(pendingHistoryModel, history.LastSuccessfulModel);
                DateTimeOffset.UtcNow.Should().BeCloseTo(history.RevisedOn, 20000);

                Assert.AreEqual(1, context.ParticipantPersonSevisCommStatuses.Count());
                var firstStatus = context.ParticipantPersonSevisCommStatuses.First();
                Assert.AreEqual(SevisCommStatus.CreatedByBatch.Id, firstStatus.SevisCommStatusId);
                Assert.AreEqual(participantId, firstStatus.ParticipantId);
                DateTimeOffset.UtcNow.Should().BeCloseTo(firstStatus.AddedOn, 20000);
            };
            context.Revert();
            service.ProcessBatchDetailProcess(user, processDetail, batch, fileProvider.Object);
            tester();
            notificationService.Verify(x => x.NotifyFinishedProcessingSevisBatchDetails(It.IsAny<string>(), It.IsAny<DispositionCode>()), Times.Exactly(1));
            notificationService.Verify(x => x.NotifyStartedProcessingSevisBatchDetails(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(1));
            cloudStorageService.Verify(x => x.UploadBlob(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(2));
            fileProvider.Verify(x => x.GetDS2019FileStream(It.IsAny<RequestId>(), It.IsAny<string>()), Times.Exactly(2));

            context.Revert();
            await service.ProcessBatchDetailProcessAsync(user, processDetail, batch, fileProvider.Object);
            tester();
            notificationService.Verify(x => x.NotifyFinishedProcessingSevisBatchDetails(It.IsAny<string>(), It.IsAny<DispositionCode>()), Times.Exactly(2));
            notificationService.Verify(x => x.NotifyStartedProcessingSevisBatchDetails(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(2));
            cloudStorageService.Verify(x => x.UploadBlobAsync(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(2));
            fileProvider.Verify(x => x.GetDS2019FileStreamAsync(It.IsAny<RequestId>(), It.IsAny<string>()), Times.Exactly(2));
        }

        [TestMethod]
        public async Task TestProcessBatchDetailProcess_IsDependentRequest()
        {
            using (ShimsContext.Create())
            {
                var sevisId = "sevis id";
                var user = new User(1);
                var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
                var otherUser = new User(user.Id + 1);
                var dependentId = 1;
                var participantId = 100;
                SevisBatchProcessing batch = null;
                SEVISBatchCreateUpdateEV createUpdateBatch = null;
                PersonDependent dependent = null;
                ECA.Data.Person person = null;
                ParticipantPerson participantPerson = null;
                Participant participant = null;
                var pendingHistoryModel = "pending";
                ExchangeVisitorHistory history = null;
                context.SetupActions.Add(() =>
                {
                    person = new ECA.Data.Person
                    {
                        PersonId = 100,
                    };
                    participant = new Participant
                    {
                        ParticipantId = participantId,
                        Person = person,
                        PersonId = person.PersonId
                    };
                    participantPerson = new ParticipantPerson
                    {
                        ParticipantId = participantId,
                        Participant = participant
                    };
                    participant.ParticipantPerson = participantPerson;
                    dependent = new PersonDependent
                    {
                        Person = person,
                        PersonId = person.PersonId,
                        DependentId = dependentId,
                        SevisId = sevisId
                    };
                    person.Family.Add(dependent);
                    dependent.History.CreatedBy = otherUser.Id;
                    dependent.History.CreatedOn = yesterday;
                    dependent.History.RevisedBy = otherUser.Id;
                    dependent.History.RevisedOn = yesterday;
                    createUpdateBatch = new SEVISBatchCreateUpdateEV();
                    batch = new SevisBatchProcessing
                    {
                        BatchId = "hello",
                        Id = 1,
                        SendString = GetXml(createUpdateBatch)
                    };
                    history = new ExchangeVisitorHistory
                    {
                        ParticipantId = participantId,
                        PendingModel = pendingHistoryModel
                    };
                    context.ExchangeVisitorHistories.Add(history);
                    context.PersonDependents.Add(dependent);
                    context.ParticipantPersons.Add(participantPerson);
                    context.SevisBatchProcessings.Add(batch);
                    context.Participants.Add(participant);
                    context.People.Add(person);
                });
                ECA.Business.Queries.Persons.Fakes.ShimPersonQueries.CreateGetSimplePersonDTOsQueryEcaContext = (ctx) =>
                {
                    var peopleDtos = new List<SimplePersonDTO>();
                    peopleDtos.Add(new SimplePersonDTO
                    {
                        PersonId = person.PersonId,
                        ParticipantId = participantId
                    });
                    return peopleDtos.AsQueryable();
                };
                System.Data.Entity.Fakes.ShimQueryableExtensions.FirstOrDefaultAsyncOf1IQueryableOfM0<SimplePersonDTO>((src) =>
                {
                    return Task<SimplePersonDTO>.FromResult(src.FirstOrDefault());
                });
                var requestId = new RequestId(dependentId, RequestIdType.Dependent, RequestActionType.Update);
                var record = new TransactionLogTypeBatchDetailProcessRecord
                {
                    requestID = requestId.ToString(),
                    sevisID = sevisId,
                    Result = new ResultType
                    {
                        status = true
                    },
                };
                SetUserDefinedFields(record, participantId, dependentId);

                var processDetail = new TransactionLogTypeBatchDetailProcess
                {
                    Record = new List<TransactionLogTypeBatchDetailProcessRecord> { record }.ToArray(),
                    resultCode = DispositionCode.Success.Code,
                    RecordCount = new TransactionLogTypeBatchDetailProcessRecordCount
                    {
                        Failure = "1",
                        Success = "2"
                    }
                };
                var url = new Uri("http://www.google.com");
                var fileContents = new byte[1] { (byte)1 };
                var fileContentStream = new MemoryStream(fileContents);
                var fileContentStreamAsync = new MemoryStream(fileContents);

                fileProvider.Setup(x => x.GetDS2019FileStream(It.IsAny<RequestId>(), It.IsAny<string>())).Returns(fileContentStream);
                fileProvider.Setup(x => x.GetDS2019FileStreamAsync(It.IsAny<RequestId>(), It.IsAny<string>())).ReturnsAsync(fileContentStreamAsync);
                cloudStorageService.Setup(x => x.UploadBlob(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(url);
                cloudStorageService.Setup(x => x.UploadBlobAsync(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(url);
                Action tester = () =>
                {
                    Assert.AreEqual(user.Id, dependent.History.RevisedBy);
                    Assert.AreEqual(otherUser.Id, dependent.History.CreatedBy);
                    Assert.AreEqual(yesterday, dependent.History.CreatedOn);
                    DateTimeOffset.UtcNow.Should().BeCloseTo(dependent.History.RevisedOn, 20000);
                    Assert.AreEqual(sevisId, dependent.SevisId);

                    Assert.IsNull(history.PendingModel);
                    Assert.AreEqual(pendingHistoryModel, history.LastSuccessfulModel);
                    DateTimeOffset.UtcNow.Should().BeCloseTo(history.RevisedOn, 20000);
                };
                context.Revert();
                service.ProcessBatchDetailProcess(user, processDetail, batch, fileProvider.Object);
                tester();
                notificationService.Verify(x => x.NotifyFinishedProcessingSevisBatchDetails(It.IsAny<string>(), It.IsAny<DispositionCode>()), Times.Exactly(1));
                notificationService.Verify(x => x.NotifyStartedProcessingSevisBatchDetails(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(1));
                cloudStorageService.Verify(x => x.UploadBlob(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(1));
                fileProvider.Verify(x => x.GetDS2019FileStream(It.IsAny<RequestId>(), It.IsAny<string>()), Times.Exactly(1));

                context.Revert();
                await service.ProcessBatchDetailProcessAsync(user, processDetail, batch, fileProvider.Object);
                tester();
                notificationService.Verify(x => x.NotifyFinishedProcessingSevisBatchDetails(It.IsAny<string>(), It.IsAny<DispositionCode>()), Times.Exactly(2));
                notificationService.Verify(x => x.NotifyStartedProcessingSevisBatchDetails(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(2));
                cloudStorageService.Verify(x => x.UploadBlobAsync(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(1));
                fileProvider.Verify(x => x.GetDS2019FileStreamAsync(It.IsAny<RequestId>(), It.IsAny<string>()), Times.Exactly(1));
            }
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
            ExchangeVisitorHistory history = null;
            Data.Person person = null;
            SevisBatchProcessing batch = null;
            var participantId = 1;
            var personId = 2;
            SEVISBatchCreateUpdateEV createUpdateBatch = null;
            context.SetupActions.Add(() =>
            {
                createUpdateBatch = new SEVISBatchCreateUpdateEV();
                batch = new SevisBatchProcessing
                {
                    BatchId = "hello",
                    Id = 1,
                    SendString = GetXml(createUpdateBatch)
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

                history = new ExchangeVisitorHistory
                {
                    ParticipantId = participantId
                };
                context.ExchangeVisitorHistories.Add(history);
                context.Participants.Add(participant);
                context.ParticipantPersons.Add(participantPerson);
                context.People.Add(person);
                context.SevisBatchProcessings.Add(batch);
            });

            var record = new TransactionLogTypeBatchDetailProcessRecord
            {
                sevisID = sevisId,
                requestID = new RequestId(participantId, RequestIdType.Participant, RequestActionType.Create).ToString(),
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
            Action<RequestId, string> fileProviderCallback = (reqId, sevId) =>
            {
                Assert.AreEqual(record.requestID, reqId.ToString());
                Assert.AreEqual(sevisId, sevId);
            };
            var fileContentStream = new MemoryStream();
            var fileContentStreamAsync = new MemoryStream();

            fileProvider.Setup(x => x.GetDS2019FileStream(It.IsAny<RequestId>(), It.IsAny<string>()))
                .Returns(fileContentStream).Callback(fileProviderCallback);
            fileProvider.Setup(x => x.GetDS2019FileStreamAsync(It.IsAny<RequestId>(), It.IsAny<string>()))
                .Returns(Task.FromResult<Stream>(fileContentStreamAsync)).Callback(fileProviderCallback);

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
            ExchangeVisitorHistory history = null;
            Data.Person person = null;
            SevisBatchProcessing batch = null;
            var participantId = 1;
            var personId = 2;
            SEVISBatchCreateUpdateEV createUpdateBatch = null;

            context.SetupActions.Add(() =>
            {
                createUpdateBatch = new SEVISBatchCreateUpdateEV();
                batch = new SevisBatchProcessing
                {
                    BatchId = "hello",
                    Id = 1,
                    SendString = GetXml(createUpdateBatch)
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

                history = new ExchangeVisitorHistory
                {
                    ParticipantId = participantId
                };
                context.ExchangeVisitorHistories.Add(history);
                context.Participants.Add(participant);
                context.ParticipantPersons.Add(participantPerson);
                context.People.Add(person);
                context.SevisBatchProcessings.Add(batch);
            });

            var record = new TransactionLogTypeBatchDetailProcessRecord
            {
                sevisID = sevisId,
                requestID = new RequestId(participantId, RequestIdType.Participant, RequestActionType.Create).ToString(),
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
            var url = new Uri("http://www.google.com");
            var fileContents = new byte[1] { (byte)1 };
            var fileContentStream = new MemoryStream(fileContents);
            var fileContentStreamAsync = new MemoryStream(fileContents);

            fileProvider.Setup(x => x.GetDS2019FileStream(It.IsAny<RequestId>(), It.IsAny<string>())).Returns(fileContentStream);
            fileProvider.Setup(x => x.GetDS2019FileStreamAsync(It.IsAny<RequestId>(), It.IsAny<string>())).ReturnsAsync(fileContentStreamAsync);
            cloudStorageService.Setup(x => x.UploadBlob(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(url);
            cloudStorageService.Setup(x => x.UploadBlobAsync(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(url);

            Action<Stream, string, string, string> cloudStorageCallback = (s, contentType, fName, containerName) =>
            {
                Assert.AreEqual(participantPerson.GetDS2019FileName(), fName);
                Assert.AreEqual(SevisBatchProcessingService.DS2019_CONTENT_TYPE, contentType);
                Assert.IsNotNull(s);
                Assert.AreEqual(settings.DS2019FileStorageContainer, containerName);
            };
            cloudStorageService.Setup(x => x.UploadBlob(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(url)
                .Callback(cloudStorageCallback);

            cloudStorageService.Setup(x => x.UploadBlobAsync(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult<Uri>(url))
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
            SEVISBatchCreateUpdateEV createUpdateBatch = null;
            context.SetupActions.Add(() =>
            {
                createUpdateBatch = new SEVISBatchCreateUpdateEV();
                batch = new SevisBatchProcessing
                {
                    BatchId = "hello",
                    Id = 1,
                    SendString = GetXml(createUpdateBatch)
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
            cloudStorageService.Verify(x => x.UploadBlob(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(0));
            fileProvider.Verify(x => x.GetDS2019FileStream(It.IsAny<RequestId>(), It.IsAny<string>()), Times.Exactly(0));

            context.Revert();
            await service.ProcessBatchDetailProcessAsync(user, null, batch, fileProvider.Object);
            tester();
            notificationService.Verify(x => x.NotifyFinishedProcessingSevisBatchDetails(It.IsAny<string>(), It.IsAny<DispositionCode>()), Times.Exactly(0));
            notificationService.Verify(x => x.NotifyStartedProcessingSevisBatchDetails(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never());
            cloudStorageService.Verify(x => x.UploadBlobAsync(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(0));
            fileProvider.Verify(x => x.GetDS2019FileStreamAsync(It.IsAny<RequestId>(), It.IsAny<string>()), Times.Exactly(0));
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
            SEVISBatchCreateUpdateEV createUpdateBatch = null;
            context.SetupActions.Add(() =>
            {
                createUpdateBatch = new SEVISBatchCreateUpdateEV();
                batch = new SevisBatchProcessing
                {
                    BatchId = "hello",
                    Id = 1,
                    SendString = GetXml(createUpdateBatch)
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
            cloudStorageService.Verify(x => x.UploadBlob(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(0));
            fileProvider.Verify(x => x.GetDS2019FileStream(It.IsAny<RequestId>(), It.IsAny<string>()), Times.Exactly(0));

            context.Revert();
            await service.ProcessBatchDetailProcessAsync(user, processDetail, batch, fileProvider.Object);
            tester();
            notificationService.Verify(x => x.NotifyFinishedProcessingSevisBatchDetails(It.IsAny<string>(), It.IsAny<DispositionCode>()), Times.Exactly(2));
            notificationService.Verify(x => x.NotifyStartedProcessingSevisBatchDetails(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(2));
            cloudStorageService.Verify(x => x.UploadBlobAsync(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(0));
            fileProvider.Verify(x => x.GetDS2019FileStreamAsync(It.IsAny<RequestId>(), It.IsAny<string>()), Times.Exactly(0));
        }

        [TestMethod]
        public void TestProcessDownload_IsNotSuccessCode()
        {
            var sevisBatch = new SevisBatchProcessing
            {
                DownloadTries = 0
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
            Assert.AreEqual(1, sevisBatch.DownloadTries);
            Assert.IsNotNull(sevisBatch.LastDownloadTry);
            DateTimeOffset.UtcNow.Should().BeCloseTo(sevisBatch.LastDownloadTry.Value, 20000);
            notificationService.Verify(x => x.NotifyDownloadedBatchProcessed(It.IsAny<string>(), It.IsAny<DispositionCode>()), Times.Exactly(1));
        }

        [TestMethod]
        public void TestProcessDownload_IsSuccessCode()
        {
            var sevisBatch = new SevisBatchProcessing
            {
                DownloadTries = 0
            };
            var downloadDetail = new TransactionLogTypeBatchDetailDownload
            {
                resultCode = DispositionCode.Success.Code,
            };

            service.ProcessDownload(downloadDetail, sevisBatch);
            DateTimeOffset.UtcNow.Should().BeCloseTo(sevisBatch.RetrieveDate.Value, 20000);
            Assert.AreEqual(downloadDetail.resultCode, sevisBatch.DownloadDispositionCode);
            Assert.IsNull(sevisBatch.UploadDispositionCode);
            Assert.IsNull(sevisBatch.ProcessDispositionCode);
            Assert.AreEqual(0, sevisBatch.DownloadTries);
            Assert.IsNull(sevisBatch.LastDownloadTry);
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
            ParticipantPersonSevisCommStatus pendingSevisSend = null;

            context.SetupActions.Add(() =>
            {
                person = new ParticipantPerson
                {
                    ParticipantId = participantId
                };
                pendingSevisSend = new ParticipantPersonSevisCommStatus
                {
                    BatchId = batchId,
                    AddedOn = DateTimeOffset.UtcNow,
                    ParticipantId = participantId,
                    ParticipantPerson = person,
                    SevisCommStatusId = SevisCommStatus.PendingSevisSend.Id
                };
                person.ParticipantPersonSevisCommStatuses.Add(pendingSevisSend);
                sevisBatch = new SevisBatchProcessing
                {
                    BatchId = batchId,
                    UploadTries = 0,
                };
                context.ParticipantPersons.Add(person);
                context.SevisBatchProcessings.Add(sevisBatch);
                context.ParticipantPersonSevisCommStatuses.Add(pendingSevisSend);
            });
            Action tester = () =>
            {
                Assert.AreEqual(today, sevisBatch.SubmitDate);
                Assert.AreEqual(uploadDetail.resultCode, sevisBatch.UploadDispositionCode);
                Assert.IsNull(sevisBatch.DownloadDispositionCode);
                Assert.IsNull(sevisBatch.ProcessDispositionCode);
                Assert.AreEqual(2, context.ParticipantPersonSevisCommStatuses.Count());
                Assert.IsTrue(Object.ReferenceEquals(pendingSevisSend, context.ParticipantPersonSevisCommStatuses.First()));
                Assert.IsFalse(Object.ReferenceEquals(pendingSevisSend, context.ParticipantPersonSevisCommStatuses.Last()));

                var addedCommStatus = context.ParticipantPersonSevisCommStatuses.Last();
                Assert.AreEqual(participantId, addedCommStatus.ParticipantId);
                Assert.AreEqual(batchId, addedCommStatus.BatchId);
                DateTimeOffset.UtcNow.Should().BeCloseTo(addedCommStatus.AddedOn, 20000);
                Assert.AreEqual(SevisCommStatus.SentByBatch.Id, addedCommStatus.SevisCommStatusId);

                Assert.IsNull(sevisBatch.LastUploadTry);
                Assert.AreEqual(0, sevisBatch.UploadTries);
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
        public async Task TestProcessUpload_CheckOtherParticipantsAreNotIncluded()
        {
            var participantId = 1;
            var batchId = "batchId";
            var otherBatchId = "other batchId";
            SevisBatchProcessing sevisBatch = null;
            SevisBatchProcessing otherSevisBatch = null;
            var today = DateTime.UtcNow;
            var uploadDetail = new TransactionLogTypeBatchDetailUpload
            {
                resultCode = DispositionCode.Success.Code,
                dateTimeStamp = today
            };
            ParticipantPerson person = null;
            ParticipantPerson otherPerson = null;
            ParticipantPersonSevisCommStatus pendingSevisSend = null;
            ParticipantPersonSevisCommStatus otherPendingSevisSend = null;

            context.SetupActions.Add(() =>
            {
                person = new ParticipantPerson
                {
                    ParticipantId = participantId
                };
                otherPerson = new ParticipantPerson
                {
                    ParticipantId = person.ParticipantId + 1
                };
                pendingSevisSend = new ParticipantPersonSevisCommStatus
                {
                    BatchId = batchId,
                    AddedOn = DateTimeOffset.UtcNow,
                    ParticipantId = participantId,
                    ParticipantPerson = person,
                    SevisCommStatusId = SevisCommStatus.PendingSevisSend.Id
                };
                otherPendingSevisSend = new ParticipantPersonSevisCommStatus
                {
                    BatchId = otherBatchId,
                    AddedOn = DateTimeOffset.UtcNow,
                    ParticipantId = otherPerson.ParticipantId,
                    ParticipantPerson = otherPerson,
                    SevisCommStatusId = SevisCommStatus.PendingSevisSend.Id
                };
                person.ParticipantPersonSevisCommStatuses.Add(pendingSevisSend);
                otherPerson.ParticipantPersonSevisCommStatuses.Add(otherPendingSevisSend);
                sevisBatch = new SevisBatchProcessing
                {
                    BatchId = batchId,
                    UploadTries = 0,
                };
                otherSevisBatch = new SevisBatchProcessing
                {
                    BatchId = otherBatchId,
                    UploadTries = 0,
                };
                context.ParticipantPersons.Add(person);
                context.ParticipantPersons.Add(otherPerson);
                context.SevisBatchProcessings.Add(sevisBatch);
                context.SevisBatchProcessings.Add(otherSevisBatch);
                context.ParticipantPersonSevisCommStatuses.Add(pendingSevisSend);
                context.ParticipantPersonSevisCommStatuses.Add(otherPendingSevisSend);
            });
            Action tester = () =>
            {
                Assert.AreEqual(1, otherPerson.ParticipantPersonSevisCommStatuses.Count());
                Assert.AreEqual(3, context.ParticipantPersonSevisCommStatuses.Count());
                Assert.IsTrue(Object.ReferenceEquals(pendingSevisSend, context.ParticipantPersonSevisCommStatuses.First()));
                Assert.IsTrue(Object.ReferenceEquals(otherPendingSevisSend, context.ParticipantPersonSevisCommStatuses.ToList()[1]));

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
        public async Task TestProcessUpload_ParticipantAlreadyHasSentByBatchStatus()
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
            ParticipantPersonSevisCommStatus informationRequired = null;

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

                informationRequired = new ParticipantPersonSevisCommStatus
                {
                    BatchId = batchId,
                    AddedOn = DateTimeOffset.UtcNow.AddDays(1.0),
                    ParticipantId = participantId,
                    ParticipantPerson = person,
                    SevisCommStatusId = SevisCommStatus.InformationRequired.Id
                };
                person.ParticipantPersonSevisCommStatuses.Add(informationRequired);
                sevisBatch = new SevisBatchProcessing
                {
                    BatchId = batchId,
                    UploadTries = 0,
                };
                context.ParticipantPersonSevisCommStatuses.Add(sentByBatch);
                context.ParticipantPersonSevisCommStatuses.Add(informationRequired);
                context.ParticipantPersons.Add(person);
                context.SevisBatchProcessings.Add(sevisBatch);
            });
            Action tester = () =>
            {
                Assert.AreEqual(2, context.ParticipantPersonSevisCommStatuses.Count());
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
                    BatchId = batchId,
                    UploadTries = 0
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
                Assert.AreEqual(1, sevisBatch.UploadTries);
                Assert.IsNotNull(sevisBatch.LastUploadTry);
                DateTimeOffset.UtcNow.Should().BeCloseTo(sevisBatch.LastUploadTry.Value, 20000);
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
        public async Task TestProcessUpload_UploadDispostionCodeDenotesInvalidXml()
        {
            var participantId = 1;
            var batchId = "batchId";
            SevisBatchProcessing sevisBatch = null;
            var today = DateTime.UtcNow;
            var code = DispositionCode.InvalidXml;
            var uploadDetail = new TransactionLogTypeBatchDetailUpload
            {
                resultCode = code.Code,
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
                    BatchId = batchId,
                    UploadTries = 0
                };
                context.ParticipantPersons.Add(person);
                context.SevisBatchProcessings.Add(sevisBatch);
            });
            Action tester = () =>
            {
                Assert.AreEqual(0, context.SevisBatchProcessings.Count());
                Assert.AreEqual(1, context.CancelledSevisBatchProcessings.Count());
                var first = context.CancelledSevisBatchProcessings.First();
                Assert.AreEqual(code.Description, first.Reason);
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
        public async Task TestProcessUpload_UploadDispostionCodeDenotesDuplicateBatchId()
        {
            var participantId = 1;
            var batchId = "batchId";
            SevisBatchProcessing sevisBatch = null;
            var today = DateTime.UtcNow;
            var code = DispositionCode.DuplicateBatchId;
            var uploadDetail = new TransactionLogTypeBatchDetailUpload
            {
                resultCode = code.Code,
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
                    BatchId = batchId,
                    UploadTries = 0
                };
                context.ParticipantPersons.Add(person);
                context.SevisBatchProcessings.Add(sevisBatch);
            });
            Action tester = () =>
            {
                Assert.AreEqual(0, context.SevisBatchProcessings.Count());
                Assert.AreEqual(1, context.CancelledSevisBatchProcessings.Count());
                var first = context.CancelledSevisBatchProcessings.First();
                Assert.AreEqual(code.Description, first.Reason);
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
        public async Task TestProcessUpload_UploadDispostionCodeDenotesDocumentNameInvalid()
        {
            var participantId = 1;
            var batchId = "batchId";
            SevisBatchProcessing sevisBatch = null;
            var today = DateTime.UtcNow;
            var code = DispositionCode.DocumentNameInvalid;
            var uploadDetail = new TransactionLogTypeBatchDetailUpload
            {
                resultCode = code.Code,
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
                    BatchId = batchId,
                    UploadTries = 0
                };
                context.ParticipantPersons.Add(person);
                context.SevisBatchProcessings.Add(sevisBatch);
            });
            Action tester = () =>
            {
                Assert.AreEqual(0, context.SevisBatchProcessings.Count());
                Assert.AreEqual(1, context.CancelledSevisBatchProcessings.Count());
                var first = context.CancelledSevisBatchProcessings.First();
                Assert.AreEqual(code.Description, first.Reason);
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
        public async Task TestProcessUpload_UploadDispostionCodeDenotesInvalidOrganizationInformation()
        {
            var participantId = 1;
            var batchId = "batchId";
            SevisBatchProcessing sevisBatch = null;
            var today = DateTime.UtcNow;
            var code = DispositionCode.InvalidOrganizationInformation;
            var uploadDetail = new TransactionLogTypeBatchDetailUpload
            {
                resultCode = code.Code,
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
                    BatchId = batchId,
                    UploadTries = 0
                };
                context.ParticipantPersons.Add(person);
                context.SevisBatchProcessings.Add(sevisBatch);
            });
            Action tester = () =>
            {
                Assert.AreEqual(0, context.SevisBatchProcessings.Count());
                Assert.AreEqual(1, context.CancelledSevisBatchProcessings.Count());
                var first = context.CancelledSevisBatchProcessings.First();
                Assert.AreEqual(code.Description, first.Reason);
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
        public async Task TestProcessUpload_UploadDispostionCodeDenotesMalformedXml()
        {
            var participantId = 1;
            var batchId = "batchId";
            SevisBatchProcessing sevisBatch = null;
            var today = DateTime.UtcNow;
            var code = DispositionCode.MalformedXml;
            var uploadDetail = new TransactionLogTypeBatchDetailUpload
            {
                resultCode = code.Code,
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
                    BatchId = batchId,
                    UploadTries = 0
                };
                context.ParticipantPersons.Add(person);
                context.SevisBatchProcessings.Add(sevisBatch);
            });
            Action tester = () =>
            {
                Assert.AreEqual(0, context.SevisBatchProcessings.Count());
                Assert.AreEqual(1, context.CancelledSevisBatchProcessings.Count());
                var first = context.CancelledSevisBatchProcessings.First();
                Assert.AreEqual(code.Description, first.Reason);
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
            Action a = () => service.ProcessTransactionLog(user, batchId, xml, fileProvider.Object);
            Func<Task> f = () => service.ProcessTransactionLogAsync(user, batchId, xml, fileProvider.Object);
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
            PersonDependent dependent = null;
            Data.Person person = null;
            SevisBatchProcessing batch = null;
            ExchangeVisitorHistory history = null;
            var participantId = 1;
            var personId = 2;
            var personDependentId = 12;
            SEVISBatchCreateUpdateEV createUpdateBatch = null;
            RequestId requestId = new RequestId(participantId, RequestIdType.Participant, RequestActionType.Create);
            context.SetupActions.Add(() =>
            {
                createUpdateBatch = new SEVISBatchCreateUpdateEV();
                batch = new SevisBatchProcessing
                {
                    BatchId = batchId,
                    Id = 1,
                    SendString = GetXml(createUpdateBatch)
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
                dependent = new PersonDependent
                {
                    DependentId = personDependentId
                };
                person.Family.Add(dependent);
                dependent.Person = person;
                participant.Person = person;
                participant.PersonId = person.PersonId;
                history = new ExchangeVisitorHistory
                {
                    ParticipantId = participantId
                };
                context.ExchangeVisitorHistories.Add(history);
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
                Assert.IsNotNull(batch.TransactionLogString);
            };
            var dependentRecord = new TransactionLogTypeBatchDetailProcessRecordDependent
            {
                dependentSevisID = "sevis id",
            };
            SetUserDefinedFields(dependentRecord, participantId, personDependentId);
            var record = new TransactionLogTypeBatchDetailProcessRecord
            {
                sevisID = sevisId,
                requestID = requestId.ToString(),
                Result = new ResultType
                {
                    status = true,
                    statusSpecified = true
                },
                Dependent = new List<TransactionLogTypeBatchDetailProcessRecordDependent>
                {
                    dependentRecord
                }.ToArray()
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
            service.ProcessTransactionLog(user, batchId, xml, fileProvider.Object);
            tester();
            Assert.AreEqual(1, context.SaveChangesCalledCount);

            context.Revert();
            await service.ProcessTransactionLogAsync(user, batchId, xml, fileProvider.Object);
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
                Assert.IsNotNull(batch.TransactionLogString);
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
            service.ProcessTransactionLog(user, batchId, xml, fileProvider.Object);
            tester();
            Assert.AreEqual(1, context.SaveChangesCalledCount);

            context.Revert();
            await service.ProcessTransactionLogAsync(user, batchId, xml, fileProvider.Object);
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
                Assert.IsNotNull(batch.TransactionLogString);
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
            service.ProcessTransactionLog(user, batchId, xml, fileProvider.Object);
            tester();
            Assert.AreEqual(1, context.SaveChangesCalledCount);

            context.Revert();
            await service.ProcessTransactionLogAsync(user, batchId, xml, fileProvider.Object);
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
        public async Task TestSaveDS2019Form()
        {
            var sevisId = "sevisId";
            var participantPerson = new ParticipantPerson
            {
                SevisId = sevisId,
                ParticipantId = 1
            };
            var fileContents = new byte[1] { (byte)1 };
            var memoryStream = new MemoryStream();
            var memoryStreamAsync = new MemoryStream();
            memoryStream.Read(fileContents, 0, fileContents.Length);
            memoryStreamAsync.Read(fileContents, 0, fileContents.Length);
            Action<Stream, string, string, string> cloudStorageCallback = (s, contentType, fName, containerName) =>
            {
                Assert.AreEqual(participantPerson.GetDS2019FileName(), fName);
                Assert.AreEqual(SevisBatchProcessingService.DS2019_CONTENT_TYPE, contentType);
                Assert.IsNotNull(s);
                Assert.AreEqual(settings.DS2019FileStorageContainer, containerName);
            };
            Action<string> tester = (s) =>
            {
                Assert.AreEqual(participantPerson.GetDS2019FileName(), s);
            };
            var url = new Uri("http://www.google.com");
            cloudStorageService.Setup(x => x.UploadBlob(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(url)
                .Callback(cloudStorageCallback);
            cloudStorageService.Setup(x => x.UploadBlobAsync(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(url)
                .Callback(cloudStorageCallback);
            var fileName = service.SaveDS2019Form(participantPerson, memoryStream);
            tester(fileName);

            var fileNameAsync = await service.SaveDS2019FormAsync(participantPerson, memoryStreamAsync);
            tester(fileNameAsync);
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
            var disposableService = new Mock<IFileStorageService>();
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

        #region Error Handling
        [TestMethod]
        public async Task TestHandleFailedUploadBatch()
        {
            var batchId = 1;
            SevisBatchProcessing batch = null;
            context.SetupActions.Add(() =>
            {
                batch = new SevisBatchProcessing
                {
                    Id = batchId
                };
                context.SevisBatchProcessings.Add(batch);
            });
            var expection = new Exception();
            Action tester = () =>
            {
                Assert.AreEqual(1, batch.UploadTries);
                Assert.IsNotNull(batch.LastUploadTry);
                DateTimeOffset.UtcNow.Should().BeCloseTo(batch.LastUploadTry.Value, 20000);
            };
            context.Revert();
            service.HandleFailedUploadBatch(batchId, expection);
            tester();
            Assert.AreEqual(1, context.SaveChangesCalledCount);

            context.Revert();
            await service.HandleFailedUploadBatchAsync(batchId, expection);
            tester();
            Assert.AreEqual(2, context.SaveChangesCalledCount);
        }

        [TestMethod]
        public async Task TestHandleFailedUploadBatch_BatchDoesNotExist()
        {
            var batchId = 1;
            var message = String.Format("The SEVIS batch processing record with the batch id [{0}] was not found.", batchId);
            Action a = () => service.HandleFailedUploadBatch(batchId, new Exception());
            Func<Task> f = () => service.HandleFailedUploadBatchAsync(batchId, new Exception());
            a.ShouldThrow<ModelNotFoundException>().WithMessage(message);
            f.ShouldThrow<ModelNotFoundException>().WithMessage(message);
        }

        [TestMethod]
        public async Task TestHandleFailedDownloadBatch()
        {
            var batchId = 1;
            SevisBatchProcessing batch = null;
            context.SetupActions.Add(() =>
            {
                batch = new SevisBatchProcessing
                {
                    Id = batchId
                };
                context.SevisBatchProcessings.Add(batch);
            });
            var expection = new Exception();
            Action tester = () =>
            {
                Assert.AreEqual(1, batch.DownloadTries);
                Assert.IsNotNull(batch.LastDownloadTry);
                DateTimeOffset.UtcNow.Should().BeCloseTo(batch.LastDownloadTry.Value, 20000);
            };
            context.Revert();
            service.HandleFailedDownloadBatch(batchId, expection);
            tester();
            Assert.AreEqual(1, context.SaveChangesCalledCount);

            context.Revert();
            await service.HandleFailedDownloadBatchAsync(batchId, expection);
            tester();
            Assert.AreEqual(2, context.SaveChangesCalledCount);
        }

        [TestMethod]
        public async Task TestHandleFailedDownloadBatch_BatchDoesNotExist()
        {
            var batchId = 1;
            var message = String.Format("The SEVIS batch processing record with the batch id [{0}] was not found.", batchId);
            Action a = () => service.HandleFailedDownloadBatch(batchId, new Exception());
            Func<Task> f = () => service.HandleFailedDownloadBatchAsync(batchId, new Exception());
            a.ShouldThrow<ModelNotFoundException>().WithMessage(message);
            f.ShouldThrow<ModelNotFoundException>().WithMessage(message);
        }
        #endregion

        #region Cancel
        [TestMethod]
        public async Task TestCancel()
        {
            using (ShimsContext.Create())
            {


                var isSevisBatchResultModified = false;
                var reason = "reason";
                var sevisOrgId = "org Id";
                var sevisUsername = "username";
                var batchId = "batchId";
                var participantId = 1;
                SevisBatchProcessing batch = null;
                ParticipantPerson participantPerson = null;
                ParticipantPersonSevisCommStatus participantPersonSevisCommStatus = null;
                var dbPropertyEntry = new System.Data.Entity.Infrastructure.Fakes.ShimDbPropertyEntry<ParticipantPerson, string>
                {
                    IsModifiedSetBoolean = (v) =>
                    {
                        isSevisBatchResultModified = v;
                    }
                };
                var entry = new System.Data.Entity.Infrastructure.Fakes.ShimDbEntityEntry<ParticipantPerson>();
                entry.PropertyOf1ExpressionOfFuncOfT0M0<string>((exp) =>
                {
                    return dbPropertyEntry;
                });
                System.Data.Entity.Fakes.ShimDbContext.AllInstances.EntryOf1M0<ParticipantPerson>((ctx, p) =>
                {
                    return entry;
                });
                context.SetupActions.Add(() =>
                {
                    participantPerson = new ParticipantPerson
                    {
                        ParticipantId = participantId
                    };
                    batch = new SevisBatchProcessing
                    {
                        BatchId = batchId,
                        Id = 1,
                        SevisOrgId = sevisOrgId,
                        SevisUsername = sevisUsername,
                        DownloadDispositionCode = "download code",
                        DownloadTries = 2,
                        LastDownloadTry = DateTimeOffset.UtcNow.AddDays(1.0),
                        LastUploadTry = DateTimeOffset.UtcNow.AddDays(2.0),
                        ProcessDispositionCode = "process code",
                        RetrieveDate = DateTimeOffset.UtcNow.AddDays(3.0),
                        SendString = "send string",
                        SubmitDate = DateTime.UtcNow.AddDays(4.0),
                        TransactionLogString = "transaction log",
                        UploadDispositionCode = "upload code",
                        UploadTries = 3
                    };
                    participantPersonSevisCommStatus = new ParticipantPersonSevisCommStatus
                    {
                        BatchId = batchId,
                        ParticipantId = participantId,
                        ParticipantPerson = participantPerson
                    };
                    participantPerson.ParticipantPersonSevisCommStatuses.Add(participantPersonSevisCommStatus);

                    context.ParticipantPersons.Add(participantPerson);
                    context.SevisBatchProcessings.Add(batch);
                    context.ParticipantPersonSevisCommStatuses.Add(participantPersonSevisCommStatus);
                });



                Action tester = () =>
                {
                    Assert.AreEqual(0, context.SevisBatchProcessings.Count());
                    Assert.AreEqual(1, context.CancelledSevisBatchProcessings.Count());
                    Assert.AreEqual(2, context.ParticipantPersonSevisCommStatuses.Count());
                    Assert.AreEqual(1, context.ParticipantPersonSevisCommStatuses.Where(x => x.SevisCommStatusId == SevisCommStatus.BatchCancelledBySystem.Id).Count());

                    //we want to find the attached participant person and be sure that one changed.
                    Assert.IsTrue(isSevisBatchResultModified);
                    Assert.AreEqual(2, context.ParticipantPersons.Count());
                    var attached = context.ParticipantPersons.Last();
                    Assert.IsFalse(Object.ReferenceEquals(participantPerson, attached));
                    Assert.AreEqual(participantId, attached.ParticipantId);
                    Assert.IsNotNull(attached.SevisBatchResult);
                    Assert.AreEqual(service.GetBatchCancelledBySystemAsSevisBatchResultJsonString(reason), attached.SevisBatchResult);

                    var addedStatus = context.ParticipantPersonSevisCommStatuses.Where(x => x.SevisCommStatusId == SevisCommStatus.BatchCancelledBySystem.Id).First();
                    Assert.AreEqual(batchId, addedStatus.BatchId);
                    Assert.AreEqual(sevisUsername, addedStatus.SevisUsername);
                    Assert.AreEqual(sevisOrgId, addedStatus.SevisOrgId);
                    Assert.AreEqual(participantId, addedStatus.ParticipantId);
                    DateTimeOffset.UtcNow.Should().BeCloseTo(addedStatus.AddedOn, 20000);

                    var addedCancelledBatch = context.CancelledSevisBatchProcessings.First();
                    Assert.AreEqual(batch.BatchId, addedCancelledBatch.BatchId);
                    Assert.AreEqual(batch.SevisOrgId, addedCancelledBatch.SevisOrgId);
                    Assert.AreEqual(batch.SevisUsername, addedCancelledBatch.SevisUsername);
                    Assert.AreEqual(batch.DownloadDispositionCode, addedCancelledBatch.DownloadDispositionCode);
                    Assert.AreEqual(batch.DownloadTries, addedCancelledBatch.DownloadTries);
                    Assert.AreEqual(batch.LastDownloadTry, addedCancelledBatch.LastDownloadTry);
                    Assert.AreEqual(batch.LastUploadTry, addedCancelledBatch.LastUploadTry);
                    Assert.AreEqual(batch.ProcessDispositionCode, addedCancelledBatch.ProcessDispositionCode);
                    Assert.AreEqual(batch.RetrieveDate, addedCancelledBatch.RetrieveDate);
                    Assert.AreEqual(batch.SendString, addedCancelledBatch.SendString);
                    Assert.AreEqual(batch.SubmitDate, addedCancelledBatch.SubmitDate);
                    Assert.AreEqual(batch.TransactionLogString, addedCancelledBatch.TransactionLogString);
                    Assert.AreEqual(batch.UploadDispositionCode, addedCancelledBatch.UploadDispositionCode);
                    Assert.AreEqual(batch.UploadTries, addedCancelledBatch.UploadTries);
                    Assert.AreEqual(reason, addedCancelledBatch.Reason);
                    DateTimeOffset.UtcNow.Should().BeCloseTo(addedCancelledBatch.CancelledOn, 20000);
                };
                context.Revert();
                service.Cancel(batch, reason);
                tester();
                notificationService.Verify(x => x.NotifyCancelledSevisBatch(It.IsAny<string>(), It.IsAny<string>()), Times.Once());

                context.Revert();
                await service.CancelAsync(batch, reason);
                tester();
                notificationService.Verify(x => x.NotifyCancelledSevisBatch(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(2));
            }
        }
        #endregion

        #region DeserializeTransactionLogType
        [TestMethod]
        public void TestDeserializeTransactionLogType()
        {
            var transactionLog = new TransactionLogType
            {
                BatchHeader = new TransactionLogTypeBatchHeader
                {
                    BatchID = "batchId",
                    OrgID = "or"
                },
                BatchDetail = new TransactionLogTypeBatchDetail
                {
                    Download = new TransactionLogTypeBatchDetailDownload
                    {
                        resultCode = DispositionCode.Success.Code
                    },
                    Upload = new TransactionLogTypeBatchDetailUpload
                    {
                        dateTimeStamp = DateTime.UtcNow,
                        FileName = "filename",
                        resultCode = DispositionCode.BatchNotYetProcessed.Code,

                    },
                    Process = new TransactionLogTypeBatchDetailProcess
                    {
                        dateTimeStamp = DateTime.UtcNow.AddDays(1.0),
                        resultCode = DispositionCode.DuplicateBatchId.Code,
                        RecordCount = new TransactionLogTypeBatchDetailProcessRecordCount
                        {
                            Failure = "1",
                            Success = "2",
                            Total = "3"
                        },
                        Record = new List<TransactionLogTypeBatchDetailProcessRecord>
                        {
                            new TransactionLogTypeBatchDetailProcessRecord
                            {

                            }
                        }.ToArray()
                    }
                }
            };

            var xml = GetXml(transactionLog);
            var instance = service.DeserializeTransactionLogType(xml);
            Assert.IsNotNull(instance);
        }

        [TestMethod]
        public void TestDeserializeTransactionLogType_HasPhysicalCorrectedAddress()
        {
            var transactionLog = new TransactionLogType
            {
                BatchHeader = new TransactionLogTypeBatchHeader
                {
                    BatchID = "batchId",
                    OrgID = "or"
                },
                BatchDetail = new TransactionLogTypeBatchDetail
                {
                    Download = new TransactionLogTypeBatchDetailDownload
                    {
                        resultCode = DispositionCode.Success.Code
                    },
                    Upload = new TransactionLogTypeBatchDetailUpload
                    {
                        dateTimeStamp = DateTime.UtcNow,
                        FileName = "filename",
                        resultCode = DispositionCode.BatchNotYetProcessed.Code,

                    },
                    Process = new TransactionLogTypeBatchDetailProcess
                    {
                        dateTimeStamp = DateTime.UtcNow.AddDays(1.0),
                        resultCode = DispositionCode.DuplicateBatchId.Code,
                        RecordCount = new TransactionLogTypeBatchDetailProcessRecordCount
                        {
                            Failure = "1",
                            Success = "2",
                            Total = "3"
                        },
                        Record = new List<TransactionLogTypeBatchDetailProcessRecord>
                        {
                            new TransactionLogTypeBatchDetailProcessRecord
                            {
                                PhysicalCorrectedAddress = new Business.Sevis.Model.TransLog.USAddrDoctorResponseType
                                {

                                }
                            }
                        }.ToArray()
                    }
                }
            };
            Assert.IsNotNull(transactionLog.BatchDetail.Process.Record.First().PhysicalCorrectedAddress);
            var xml = GetXml(transactionLog);
            var instance = service.DeserializeTransactionLogType(xml);
            Assert.IsNotNull(instance);
            Assert.IsNull(instance.BatchDetail.Process.Record.First().PhysicalCorrectedAddress);

        }
        #endregion

        #region History
        [TestMethod]
        public void TestPromotePendingModelToLastSuccessfulModel()
        {
            var value = "pending";
            var history = new ExchangeVisitorHistory
            {
                PendingModel = value
            };
            service.PromotePendingModelToLastSuccessfulModel(history);
            Assert.AreEqual(value, history.LastSuccessfulModel);
            Assert.IsNull(history.PendingModel);
            DateTimeOffset.UtcNow.Should().BeCloseTo(history.RevisedOn, 20000);
        }

        [TestMethod]
        public async Task TestAddOrUpdateStagedExchangeVisitorHistory_CheckAddsExchangeVisitorHistory()
        {
            var personId = 10;
            var participantId = 1;
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
                DivisionIso = "DC",
                LocationName = "name"
            };
            var exchangeVisitor = new ExchangeVisitor(
                sevisId: null,
                isValidated: false,
                sevisOrgId: "sevisOrgId",
                person: GetPerson(DateTime.UtcNow, personId, participantId),
                financialInfo: new Business.Validation.Sevis.Finance.FinancialInfo(true, true, null, null),
                occupationCategoryCode: "99",
                programEndDate: DateTime.Now,
                programStartDate: DateTime.Now,
                dependents: new List<Business.Validation.Sevis.Bio.Dependent>(),
                siteOfActivity: siteOfActivity);

            Action tester = () =>
            {
                Assert.AreEqual(1, context.ExchangeVisitorHistories.Count());
                var firstHistory = context.ExchangeVisitorHistories.First();
                Assert.AreEqual(exchangeVisitor.ToJson(), firstHistory.PendingModel);
                Assert.AreEqual(exchangeVisitor.Person.ParticipantId, firstHistory.ParticipantId);
                DateTimeOffset.UtcNow.Should().BeCloseTo(firstHistory.RevisedOn, 20000);
            };
            context.Revert();
            service.AddOrUpdateStagedExchangeVisitorHistory(exchangeVisitor);
            tester();

            context.Revert();
            await service.AddOrUpdateStagedExchangeVisitorHistoryAsync(exchangeVisitor);
            tester();
        }

        [TestMethod]
        public async Task TestAddOrUpdateStagedExchangeVisitorHistory_CheckUpdatesExistingExchangeVisitorHistory()
        {
            var personId = 10;
            var participantId = 1;
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
                DivisionIso = "DC",
                LocationName = "name"
            };
            var exchangeVisitor = new ExchangeVisitor(
                sevisId: null,
                isValidated: false,
                sevisOrgId: "sevisOrgId",
                person: GetPerson(DateTime.UtcNow, personId, participantId),
                financialInfo: new Business.Validation.Sevis.Finance.FinancialInfo(true, true, null, null),
                occupationCategoryCode: "99",
                programEndDate: DateTime.Now,
                programStartDate: DateTime.Now,
                dependents: new List<Business.Validation.Sevis.Bio.Dependent>(),
                siteOfActivity: siteOfActivity);

            var history = new ExchangeVisitorHistory
            {
                ParticipantId = exchangeVisitor.Person.ParticipantId
            };
            context.SetupActions.Add(() =>
            {
                context.ExchangeVisitorHistories.Add(history);
            });
            Action tester = () =>
            {
                Assert.AreEqual(1, context.ExchangeVisitorHistories.Count());
                var firstHistory = context.ExchangeVisitorHistories.First();
                Assert.IsTrue(Object.ReferenceEquals(history, context.ExchangeVisitorHistories.First()));
                Assert.AreEqual(exchangeVisitor.ToJson(), firstHistory.PendingModel);
                Assert.AreEqual(exchangeVisitor.Person.ParticipantId, firstHistory.ParticipantId);
                DateTimeOffset.UtcNow.Should().BeCloseTo(firstHistory.RevisedOn, 20000);
            };
            context.Revert();
            service.AddOrUpdateStagedExchangeVisitorHistory(exchangeVisitor);
            tester();

            context.Revert();
            await service.AddOrUpdateStagedExchangeVisitorHistoryAsync(exchangeVisitor);
            tester();
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
