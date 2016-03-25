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

namespace ECA.Business.Test.Service.Sevis
{
    [TestClass]
    public class SevisBatchProcessingServiceTest
    {
        private TestEcaContext context;
        private SevisBatchProcessingService service;
        private Mock<IExchangeVisitorService> exchangeVisitorService;
        private Mock<ISevisBatchProcessingNotificationService> notificationService;
        private Mock<IExchangeVisitorValidationService> exchangeVisitorValidationService;
        private Mock<AbstractValidator<ExchangeVisitor>> validator;
        private int maxCreateExchangeVisitorBatchSize = 10;
        private int maxUpdateExchangeVisitorBatchSize = 10;
        private string orgId;

        [TestInitialize]
        public void TestInit()
        {
            orgId = "Org Id";
            context = new TestEcaContext();
            exchangeVisitorService = new Mock<IExchangeVisitorService>();
            notificationService = new Mock<ISevisBatchProcessingNotificationService>();
            exchangeVisitorValidationService = new Mock<IExchangeVisitorValidationService>();
            validator = new Mock<AbstractValidator<ExchangeVisitor>>();

            exchangeVisitorValidationService.Setup(x => x.GetValidator()).Returns(validator.Object);
            service = new SevisBatchProcessingService(
                context: context,
                exchangeVisitorService: exchangeVisitorService.Object,
                notificationService: notificationService.Object,
                exchangeVisitorValidationService: exchangeVisitorValidationService.Object,
                sevisOrgId: orgId,
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
            var birthCountryReason = "reason";
            var remarks = "remarks";
            var programCataegoryCode = "1D";

            var subjectFieldCode = "01.0102";
            var subjectField = new SubjectField(subjectFieldCode, null, null, null);

            var person = new Business.Validation.Sevis.Bio.Person(
                fullName,
                birthCity,
                birthCountryCode,
                birthCountryReason,
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

        private TransactionLogType GetTransactionLogType(string xml)
        {
            using (var memoryStream = new MemoryStream())
            using (var stringReader = new StringReader(xml))
            {
                var serializer = new XmlSerializer(typeof(TransactionLogType));
                var transactionLogType = (TransactionLogType)serializer.Deserialize(stringReader);
                return transactionLogType;
            }
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
                exchangeVisitorService: exchangeVisitorService.Object,
                exchangeVisitorValidationService: exchangeVisitorValidationService.Object,
                sevisOrgId: orgId,
                notificationService: notificationService.Object,
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
                exchangeVisitorService: exchangeVisitorService.Object,
                exchangeVisitorValidationService: exchangeVisitorValidationService.Object,
                notificationService: notificationService.Object,
                sevisOrgId: orgId);

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
            var user = new User(1);
            Participant participant = null;
            ParticipantPerson participantPerson = null;
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
                user: user,
                sevisId: null,
                person: GetPerson(personId, participantId),
                financialInfo: new Business.Validation.Sevis.Finance.FinancialInfo(true, true, null, null),
                occupationCategoryCode: "99",
                programEndDate: DateTime.Now,
                programStartDate: DateTime.Now,
                dependents: new List<Business.Validation.Sevis.Bio.Dependent>(),
                siteOfActivity: siteOfActivity);
            exchangeVisitorService.Setup(x => x.GetExchangeVisitor(It.IsAny<User>(), It.IsAny<int>(), It.IsAny<int>())).Returns(exchangeVisitor);
            exchangeVisitorService.Setup(x => x.GetExchangeVisitorAsync(It.IsAny<User>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(exchangeVisitor);
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
                Assert.AreEqual(0, firstBatch.SEVISBatchCreateUpdateEV.UpdateEV.Count());
                Assert.AreEqual(user.Id.ToString(), firstBatch.SEVISBatchCreateUpdateEV.userID);
                Assert.AreEqual(orgId, firstBatch.SEVISBatchCreateUpdateEV.BatchHeader.OrgID);
                Assert.IsNotNull(firstBatch.SEVISBatchCreateUpdateEV.BatchHeader.BatchID);

                Assert.IsNotNull(firstBatch.BatchId);
                Assert.AreEqual(2, context.ParticipantPersonSevisCommStatuses.Count());
                Assert.IsTrue(Object.ReferenceEquals(status, context.ParticipantPersonSevisCommStatuses.First()));
                var addedCommStatus = context.ParticipantPersonSevisCommStatuses.Last();
                DateTimeOffset.UtcNow.Should().BeCloseTo(addedCommStatus.AddedOn, 20000);
                Assert.AreEqual(participantId, addedCommStatus.ParticipantId);
                Assert.AreEqual(SevisCommStatus.PendingSevisSend.Id, addedCommStatus.SevisCommStatusId);
            };
            context.Revert();
            var result = service.StageBatches(user);
            Assert.AreEqual(result.Count, context.SaveChangesCalledCount);
            tester(result);

            context.Revert();
            var resultAsync = await service.StageBatchesAsync(user);
            tester(resultAsync);
            Assert.AreEqual(result.Count * 2, context.SaveChangesCalledCount);
            notificationService.Verify(x => x.NotifyNumberOfParticipantsToStage(It.IsAny<int>()), Times.Exactly(2));
            notificationService.Verify(x => x.NotifyStagedSevisBatchCreated(It.IsAny<StagedSevisBatch>()), Times.Exactly(2));
            notificationService.Verify(x => x.NotifyStagedSevisBatchesFinished(It.IsAny<List<StagedSevisBatch>>()), Times.Exactly(2));
            notificationService.Verify(x => x.NotifyInvalidExchangeVisitor(It.IsAny<ExchangeVisitor>()), Times.Never());
        }

        [TestMethod]
        public async Task TestStageBatches_ExchangeVisitorIsNotValid()
        {
            var personId = 10;
            var participantId = 1;
            var projectId = 2;
            var user = new User(1);
            Participant participant = null;
            ParticipantPerson participantPerson = null;
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
                user: user,
                sevisId: null,
                person: GetPerson(personId, participantId),
                financialInfo: new Business.Validation.Sevis.Finance.FinancialInfo(true, true, null, null),
                occupationCategoryCode: "99",
                programEndDate: DateTime.Now,
                programStartDate: DateTime.Now,
                dependents: new List<Business.Validation.Sevis.Bio.Dependent>(),
                siteOfActivity: siteOfActivity);
            exchangeVisitorService.Setup(x => x.GetExchangeVisitor(It.IsAny<User>(), It.IsAny<int>(), It.IsAny<int>())).Returns(exchangeVisitor);
            exchangeVisitorService.Setup(x => x.GetExchangeVisitorAsync(It.IsAny<User>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(exchangeVisitor);

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
            var result = service.StageBatches(user);
            Assert.AreEqual(result.Count, context.SaveChangesCalledCount);
            tester(result);
            exchangeVisitorValidationService.Verify(x => x.RunParticipantSevisValidation(It.IsAny<User>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once());

            context.Revert();
            var resultAsync = await service.StageBatchesAsync(user);
            tester(resultAsync);
            Assert.AreEqual(result.Count * 2, context.SaveChangesCalledCount);
            exchangeVisitorValidationService.Verify(x => x.RunParticipantSevisValidationAsync(It.IsAny<User>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once());
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
            var user = new User(1);
            Participant participant = null;
            ParticipantPerson participantPerson = null;
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
                user: user,
                sevisId: "sevisid",
                person: GetPerson(personId, participantId),
                financialInfo: new Business.Validation.Sevis.Finance.FinancialInfo(true, true, null, null),
                occupationCategoryCode: "99",
                programEndDate: DateTime.Now,
                programStartDate: DateTime.Now,
                dependents: new List<Business.Validation.Sevis.Bio.Dependent>(),
                siteOfActivity: siteOfActivity);
            exchangeVisitorService.Setup(x => x.GetExchangeVisitor(It.IsAny<User>(), It.IsAny<int>(), It.IsAny<int>())).Returns(exchangeVisitor);
            exchangeVisitorService.Setup(x => x.GetExchangeVisitorAsync(It.IsAny<User>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(exchangeVisitor);
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
                Assert.AreEqual(0, firstBatch.SEVISBatchCreateUpdateEV.CreateEV.Count());
                Assert.AreEqual(exchangeVisitor.GetSEVISEVBatchTypeExchangeVisitor1Collection().Count(), firstBatch.SEVISBatchCreateUpdateEV.UpdateEV.Count());
                Assert.AreEqual(user.Id.ToString(), firstBatch.SEVISBatchCreateUpdateEV.userID);
                Assert.AreEqual(orgId, firstBatch.SEVISBatchCreateUpdateEV.BatchHeader.OrgID);
                Assert.IsNotNull(firstBatch.SEVISBatchCreateUpdateEV.BatchHeader.BatchID);

                Assert.IsNotNull(firstBatch.BatchId);

                Assert.AreEqual(2, context.ParticipantPersonSevisCommStatuses.Count());
                Assert.IsTrue(Object.ReferenceEquals(status, context.ParticipantPersonSevisCommStatuses.First()));
                var addedCommStatus = context.ParticipantPersonSevisCommStatuses.Last();
                DateTimeOffset.UtcNow.Should().BeCloseTo(addedCommStatus.AddedOn, 20000);
                Assert.AreEqual(participantId, addedCommStatus.ParticipantId);
                Assert.AreEqual(SevisCommStatus.PendingSevisSend.Id, addedCommStatus.SevisCommStatusId);
            };
            context.Revert();
            var result = service.StageBatches(user);
            Assert.AreEqual(result.Count, context.SaveChangesCalledCount);
            tester(result);

            context.Revert();
            var resultAsync = await service.StageBatchesAsync(user);
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
            var user = new User(1);
            var siteOfActivity = new AddressDTO
            {
                Division = "DC",
                LocationName = "name"
            };
            var exchangeVisitors = new List<ExchangeVisitor>();
            Func<User, int, int, ExchangeVisitor> getExchangeVisitor = (u, projId, partId) =>
            {
                return exchangeVisitors.Where(x => x.Person.ParticipantId == partId).First();
            };
            Func<User, int, int, Task<ExchangeVisitor>> getExchangeVisitorAync = (u, projId, partId) =>
            {
                return Task.FromResult<ExchangeVisitor>(getExchangeVisitor(u, projId, partId));
            };

            exchangeVisitorService
                .Setup(x => x.GetExchangeVisitor(It.IsAny<User>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(getExchangeVisitor);
            exchangeVisitorService
                .Setup(x => x.GetExchangeVisitorAsync(It.IsAny<User>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(getExchangeVisitorAync);
            validator.Setup(x => x.Validate(It.IsAny<ExchangeVisitor>())).Returns(new FluentValidation.Results.ValidationResult());
            context.SetupActions.Add(() =>
            {
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
                    };
                    participantPerson.ParticipantPersonSevisCommStatuses.Add(readyToSubmitStatus);
                    context.Participants.Add(participant);
                    context.ParticipantPersons.Add(participantPerson);
                    context.ParticipantPersonSevisCommStatuses.Add(readyToSubmitStatus);

                    var exchangeVisitor = new ExchangeVisitor(
                        user: user,
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
                    i <= maxUpdateExchangeVisitorBatchSize / exchangeVisitors.First().GetSEVISEVBatchTypeExchangeVisitor1Collection().Count();
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
                    };
                    participantPerson.ParticipantPersonSevisCommStatuses.Add(readyToSubmitStatus);
                    context.Participants.Add(participant);
                    context.ParticipantPersons.Add(participantPerson);
                    context.ParticipantPersonSevisCommStatuses.Add(readyToSubmitStatus);

                    var exchangeVisitor = new ExchangeVisitor(
                        user: user,
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
            var result = service.StageBatches(user);
            tester(result);
            Assert.AreEqual(result.Count, context.SaveChangesCalledCount);

            context.Revert();
            var resultAsync = await service.StageBatchesAsync(user);
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
            var user = new User(1);
            var siteOfActivity = new AddressDTO
            {
                Division = "DC",
                LocationName = "name"
            };
            var exchangeVisitors = new List<ExchangeVisitor>();
            Func<User, int, int, ExchangeVisitor> getExchangeVisitor = (u, projId, partId) =>
            {
                return exchangeVisitors.Where(x => x.Person.ParticipantId == partId).First();
            };
            Func<User, int, int, Task<ExchangeVisitor>> getExchangeVisitorAync = (u, projId, partId) =>
            {
                return Task.FromResult<ExchangeVisitor>(getExchangeVisitor(u, projId, partId));
            };

            exchangeVisitorService
                .Setup(x => x.GetExchangeVisitor(It.IsAny<User>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(getExchangeVisitor);
            exchangeVisitorService
                .Setup(x => x.GetExchangeVisitorAsync(It.IsAny<User>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(getExchangeVisitorAync);
            validator.Setup(x => x.Validate(It.IsAny<ExchangeVisitor>())).Returns(new FluentValidation.Results.ValidationResult());
            context.SetupActions.Add(() =>
            {
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
                    };
                    participantPerson.ParticipantPersonSevisCommStatuses.Add(readyToSubmitStatus);
                    context.Participants.Add(participant);
                    context.ParticipantPersons.Add(participantPerson);
                    context.ParticipantPersonSevisCommStatuses.Add(readyToSubmitStatus);

                    var exchangeVisitor = new ExchangeVisitor(
                        user: user,
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
                    };
                    participantPerson.ParticipantPersonSevisCommStatuses.Add(readyToSubmitStatus);
                    context.Participants.Add(participant);
                    context.ParticipantPersons.Add(participantPerson);
                    context.ParticipantPersonSevisCommStatuses.Add(readyToSubmitStatus);

                    var exchangeVisitor = new ExchangeVisitor(
                        user: user,
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
                        Assert.AreEqual(0, batch.SEVISBatchCreateUpdateEV.CreateEV.Count());
                        Assert.AreEqual(numberOfUpdateRecordsPerExchangeVisitor, batch.SEVISBatchCreateUpdateEV.UpdateEV.Count());
                    }
                    if (i == 0)
                    {
                        Assert.AreEqual(maxCreateExchangeVisitorBatchSize, batch.SEVISBatchCreateUpdateEV.CreateEV.Count());
                    }
                    if (i > 0 && i < expectedBatchCount - 1)
                    {
                        Assert.AreEqual(0, batch.SEVISBatchCreateUpdateEV.CreateEV.Count());
                        Assert.AreEqual(numberOfUpdateRecordsPerExchangeVisitor * 3, batch.SEVISBatchCreateUpdateEV.UpdateEV.Count());
                    }
                }
            };

            context.Revert();
            var result = service.StageBatches(user);
            tester(result);
            Assert.AreEqual(result.Count, context.SaveChangesCalledCount);

            context.Revert();
            var resultAsync = await service.StageBatchesAsync(user);
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
                var user = new User(1);
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
                var result = service.StageBatches(user);
                tester(result);
                Assert.AreEqual(0, context.SaveChangesCalledCount);

                context.Revert();
                var resultAsync = await service.StageBatchesAsync(user);
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
                AddedOn = DateTime.UtcNow.AddDays(-1.0)
            };

            var siteOfActivity = new AddressDTO
            {
                Division = "DC",
                LocationName = "name"
            };

            var exchangeVisitor = new ExchangeVisitor(
                user: new User(1),
                sevisId: null,
                person: GetPerson(1, 2),
                financialInfo: new Business.Validation.Sevis.Finance.FinancialInfo(true, true, null, null),
                occupationCategoryCode: "99",
                programEndDate: DateTime.Now,
                programStartDate: DateTime.Now,
                dependents: new List<Business.Validation.Sevis.Bio.Dependent>(),
                siteOfActivity: siteOfActivity);
            Assert.IsNull(service.GetAccomodatingStagedSevisBatch(batches, exchangeVisitor));
        }

        [TestMethod]
        public void TestGetAccomodatingSevisBatch_SevisBatchIsSaved()
        {
            var batches = new List<StagedSevisBatch>();
            batches.Add(new StagedSevisBatch(Guid.NewGuid(), new User(1), "orgId")
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
                user: new User(1),
                sevisId: null,
                person: GetPerson(1, 2),
                financialInfo: new Business.Validation.Sevis.Finance.FinancialInfo(true, true, null, null),
                occupationCategoryCode: "99",
                programEndDate: DateTime.Now,
                programStartDate: DateTime.Now,
                dependents: new List<Business.Validation.Sevis.Bio.Dependent>(),
                siteOfActivity: siteOfActivity);
            Assert.IsNull(service.GetAccomodatingStagedSevisBatch(batches, exchangeVisitor));
        }

        [TestMethod]
        public void TestGetAccomodatingSevisBatch_SevisBatchCanAccomodate()
        {
            var batches = new List<StagedSevisBatch>();
            batches.Add(new StagedSevisBatch(Guid.NewGuid(), new User(1), "orgId")
            {
                IsSaved = false
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
                user: new User(1),
                sevisId: null,
                person: GetPerson(1, 2),
                financialInfo: new Business.Validation.Sevis.Finance.FinancialInfo(true, true, null, null),
                occupationCategoryCode: "99",
                programEndDate: DateTime.Now,
                programStartDate: DateTime.Now,
                dependents: new List<Business.Validation.Sevis.Bio.Dependent>(),
                siteOfActivity: siteOfActivity);
            Assert.IsTrue(Object.ReferenceEquals(batches.First(), service.GetAccomodatingStagedSevisBatch(batches, exchangeVisitor)));
        }

        [TestMethod]
        public void TestGetAccomodatingSevisBatch_SevisBatchCanNotAccomodate()
        {
            var batches = new List<StagedSevisBatch>();
            batches.Add(new StagedSevisBatch(Guid.NewGuid(), new User(1), "orgId", 0, 0)
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
                user: new User(1),
                sevisId: null,
                person: GetPerson(1, 2),
                financialInfo: new Business.Validation.Sevis.Finance.FinancialInfo(true, true, null, null),
                occupationCategoryCode: "99",
                programEndDate: DateTime.Now,
                programStartDate: DateTime.Now,
                dependents: new List<Business.Validation.Sevis.Bio.Dependent>(),
                siteOfActivity: siteOfActivity);
            Assert.IsNull(service.GetAccomodatingStagedSevisBatch(batches, exchangeVisitor));
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
                UploadDispositionCode = "upload code"
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

        [TestMethod]
        public async Task TestGetNextBatchByBatchIdToDownload_HasRecord()
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

        #region Update
        [TestMethod]
        public async Task TestBatchHasBeenSent()
        {
            var id = 1;
            SevisBatchProcessing batch = null;

            context.SetupActions.Add(() =>
            {
                batch = new SevisBatchProcessing
                {
                    Id = id,
                };
                context.SevisBatchProcessings.Add(batch);
            });
            Action tester = () =>
            {
                Assert.AreEqual(1, context.SevisBatchProcessings.Count());
                Assert.IsTrue(Object.ReferenceEquals(batch, context.SevisBatchProcessings.First()));
                DateTimeOffset.UtcNow.Should().BeCloseTo(batch.SubmitDate.Value, 20000);
            };
            context.Revert();
            service.BatchHasBeenSent(id);
            tester();

            context.Revert();
            await service.BatchHasBeenSentAsync(id);
            tester();
        }

        [TestMethod]
        public async Task TestBatchHasBeenSent_ModelDoesNotExist()
        {
            var id = 1;
            var message = String.Format("The SEVIS batch processing record with the batch id [{0}] was not found.", id);
            Action a = () => service.BatchHasBeenSent(id);
            Func<Task> f = () => service.BatchHasBeenSentAsync(id);
            a.ShouldThrow<ModelNotFoundException>().WithMessage(message);
            f.ShouldThrow<ModelNotFoundException>().WithMessage(message);
        }

        [TestMethod]
        public async Task TestBatchHasBeenRetrieved()
        {
            var id = 1;
            var batchId = "batch Id";
            var transactionLog = new TransactionLogType
            {
                BatchHeader = new TransactionLogTypeBatchHeader
                {
                    BatchID = batchId
                }
            };
            SevisBatchProcessing instance = null;
            context.SetupActions.Add(() =>
            {
                instance = new SevisBatchProcessing
                {
                    BatchId = batchId,
                    Id = id
                };
                context.SevisBatchProcessings.Add(instance);
            });
            var xml = GetXml(transactionLog);
            Action tester = () =>
            {
                Assert.AreEqual(instance.TransactionLogString, xml);
                DateTimeOffset.UtcNow.Should().BeCloseTo(instance.RetrieveDate.Value, 20000);
            };

            context.Revert();
            service.BatchHasBeenRetrieved(xml);
            tester();

            context.Revert();
            await service.BatchHasBeenRetrievedAsync(xml);
            tester();
        }

        [TestMethod]
        public async Task TestBatchHasBeenRetrieved_BatchDoesNotExist()
        {
            var batchId = "batch Id";
            var transactionLog = new TransactionLogType
            {
                BatchHeader = new TransactionLogTypeBatchHeader
                {
                    BatchID = batchId
                }
            };
            var xml = GetXml(transactionLog);
            var message = String.Format("The SEVIS batch processing record with the batch id [{0}] was not found.", batchId);
            Action a = () => service.BatchHasBeenRetrieved(xml);
            Func<Task> f = () => service.BatchHasBeenRetrievedAsync(xml);
            a.ShouldThrow<ModelNotFoundException>().WithMessage(message);
            f.ShouldThrow<ModelNotFoundException>().WithMessage(message);
        }
        #endregion

        #region Process Transaction Log

        #endregion
    }
}
