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

namespace ECA.Business.Test.Service.Sevis
{
    [TestClass]
    public class SevisBatchProcessingServiceTest
    {
        private TestEcaContext context;
        private SevisBatchProcessingService service;
        private Mock<IExchangeVisitorService> exchangeVisitorService;
        private int maxCreateExchangeVisitorBatchSize = 10;
        private int maxUpdateExchangeVisitorBatchSize = 10;
        private string orgId;

        [TestInitialize]
        public void TestInit()
        {
            orgId = "Org Id";
            context = new TestEcaContext();
            exchangeVisitorService = new Mock<IExchangeVisitorService>();
            service = new SevisBatchProcessingService(
                context: context,
                exchangeVisitorService: exchangeVisitorService.Object,
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

        #region Constructor
        [TestMethod]
        public void TestConstructor()
        {
            var instance = new SevisBatchProcessingService(
                context: context,
                exchangeVisitorService: exchangeVisitorService.Object,
                sevisOrgId: orgId,
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

                Assert.AreNotEqual(Guid.Empty, firstBatch.BatchId);
                Guid guid;
                Assert.IsTrue(Guid.TryParse(firstBatch.SEVISBatchCreateUpdateEV.BatchHeader.BatchID, out guid));

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

                Assert.AreNotEqual(Guid.Empty, firstBatch.BatchId);
                Guid guid;
                Assert.IsTrue(Guid.TryParse(firstBatch.SEVISBatchCreateUpdateEV.BatchHeader.BatchID, out guid));

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
        }


        //[TestMethod]
        //public async Task TestStageBatches_ParticipantsDoNotHaveSevisId_MultipleBatches()
        //{
        //    string sevisId = null;
        //    var personId = 10;
        //    var projectId = 500;
        //    var user = new User(1);
        //    var siteOfActivity = new AddressDTO
        //    {
        //        Division = "DC",
        //        LocationName = "name"
        //    };
        //    var exchangeVisitors = new List<ExchangeVisitor>();
        //    Func<User, int, int, ExchangeVisitor> getExchangeVisitor = (u, projId, partId) =>
        //    {
        //        return exchangeVisitors.Where(x => x.Person.ParticipantId == partId).First();
        //    };
        //    Func<User, int, int, Task<ExchangeVisitor>> getExchangeVisitorAync = (u, projId, partId) =>
        //    {
        //        return Task.FromResult<ExchangeVisitor>(getExchangeVisitor(u, projId, partId));
        //    };

        //    exchangeVisitorService
        //        .Setup(x => x.GetExchangeVisitor(It.IsAny<User>(), It.IsAny<int>(), It.IsAny<int>()))
        //        .Returns(getExchangeVisitor);
        //    exchangeVisitorService
        //        .Setup(x => x.GetExchangeVisitorAsync(It.IsAny<User>(), It.IsAny<int>(), It.IsAny<int>()))
        //        .Returns(getExchangeVisitorAync);

        //    context.SetupActions.Add(() =>
        //    {
        //        var now = DateTime.UtcNow;
        //        for (var i = 0; i < StagedSevisBatch.MAX_CREATE_EXCHANGE_VISITORS + 1; i++)
        //        {
        //            var participant = new Participant
        //            {
        //                ParticipantId = i,
        //                ProjectId = projectId,
        //            };
        //            var participantPerson = new ParticipantPerson
        //            {
        //                ParticipantId = participant.ParticipantId,
        //                Participant = participant
        //            };
        //            participant.ParticipantPerson = participantPerson;
        //            var readyToSubmitStatus = new ParticipantPersonSevisCommStatus
        //            {
        //                Id = i,
        //                AddedOn = now,
        //                ParticipantId = participant.ParticipantId,
        //                SevisCommStatusId = SevisCommStatus.QueuedToSubmit.Id,
        //                ParticipantPerson = participantPerson,
        //            };
        //            participantPerson.ParticipantPersonSevisCommStatuses.Add(readyToSubmitStatus);
        //            context.Participants.Add(participant);
        //            context.ParticipantPersons.Add(participantPerson);
        //            context.ParticipantPersonSevisCommStatuses.Add(readyToSubmitStatus);

        //            var exchangeVisitor = new ExchangeVisitor(
        //                user: user,
        //                sevisId: sevisId,
        //                person: GetPerson(personId, participant.ParticipantId),
        //                financialInfo: new Business.Validation.Sevis.Finance.FinancialInfo(true, true, null, null),
        //                occupationCategoryCode: "99",
        //                programEndDate: DateTime.Now,
        //                programStartDate: DateTime.Now,
        //                dependents: new List<Business.Validation.Sevis.Bio.Dependent>(),
        //                siteOfActivity: siteOfActivity
        //            );
        //            exchangeVisitors.Add(exchangeVisitor);
        //        }
        //    });
        //    Action<List<StagedSevisBatch>> tester = (batches) =>
        //    {
        //        Assert.IsNotNull(batches);
        //        Assert.AreEqual(2, batches.Count);

        //        Assert.IsTrue(batches.First().IsSaved);
        //        Assert.IsTrue(batches.Last().IsSaved);

        //        Assert.AreNotEqual(batches.First().BatchId, batches.Last().BatchId);
        //        Assert.AreNotEqual(Guid.Empty, batches.First().BatchId);
        //        Assert.AreNotEqual(Guid.Empty, batches.Last().BatchId);
        //        Assert.AreEqual(2, context.SevisBatchProcessings.Count());
        //        Assert.AreEqual(250, batches.First().GetExchangeVisitors().Count());
        //        Assert.AreEqual(1, batches.Last().GetExchangeVisitors().Count());

        //        Assert.AreEqual(250, batches.First().SEVISBatchCreateUpdateEV.CreateEV.Count());
        //        Assert.AreEqual(1, batches.Last().SEVISBatchCreateUpdateEV.CreateEV.Count());
        //    };
        //    participantBatchSize = 300;
        //    service = new SevisBatchProcessingService(context, exchangeVisitorService.Object, orgId, participantBatchSize, null);

        //    context.Revert();
        //    var result = service.StageBatches(user);
        //    tester(result);
        //    Assert.AreEqual(result.Count, context.SaveChangesCalledCount);

        //    context.Revert();
        //    var resultAsync = await service.StageBatchesAsync(user);
        //    tester(resultAsync);
        //    Assert.AreEqual(result.Count * 2, context.SaveChangesCalledCount);
        //}

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
                    if(i > 0 && i < expectedBatchCount - 1)
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
    }
}
