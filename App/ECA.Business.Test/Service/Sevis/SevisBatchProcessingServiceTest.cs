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

namespace ECA.Business.Test.Service.Sevis
{
    [TestClass]
    public class SevisBatchProcessingServiceTest
    {
        private TestEcaContext context;
        private SevisBatchProcessingService service;
        private Mock<IExchangeVisitorService> exchangeVisitorService;
        private int sevisBatchSize;
        private int queryBatchSize;
        private string orgId;

        [TestInitialize]
        public void TestInit()
        {
            //we need a sevis batch size that can at least handle one exchange visitor update for the unit tests
            sevisBatchSize = 3;
            queryBatchSize = 1;
            orgId = "Org Id";
            context = new TestEcaContext();
            exchangeVisitorService = new Mock<IExchangeVisitorService>();
            service = new SevisBatchProcessingService(context, exchangeVisitorService.Object, orgId, queryBatchSize, sevisBatchSize, null);
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

        #region Staging
        [TestMethod]
        public async Task TestStageBatches()
        {
            var personId = 10;
            var participantId = 1;
            var projectId = 2;
            var user = new User(1);
            Participant participant = null;
            ParticipantPerson participantPerson = null;
            var readyToSubmitStatus = new ParticipantPersonSevisCommStatus
            {
                Id = 1,
                SevisCommStatusId = SevisCommStatus.ReadyToSubmit.Id,
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
                Assert.AreEqual(1, batches.Count);
                var firstBatch = batches.First();

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
                Assert.AreEqual(user.Id.ToString(), firstBatch.SEVISBatchCreateUpdateEV.userID);
                Assert.AreEqual(orgId, firstBatch.SEVISBatchCreateUpdateEV.BatchHeader.OrgID);
                Assert.IsNotNull(firstBatch.SEVISBatchCreateUpdateEV.BatchHeader.BatchID);

                Assert.AreNotEqual(Guid.Empty, firstBatch.BatchId);
                Guid guid;
                Assert.IsTrue(Guid.TryParse(firstBatch.SEVISBatchCreateUpdateEV.BatchHeader.BatchID, out guid));

                Assert.AreEqual(2, context.ParticipantPersonSevisCommStatuses.Count());
                Assert.IsTrue(Object.ReferenceEquals(readyToSubmitStatus, context.ParticipantPersonSevisCommStatuses.First()));
                var addedCommStatus = context.ParticipantPersonSevisCommStatuses.Last();
                DateTimeOffset.UtcNow.Should().BeCloseTo(addedCommStatus.AddedOn, 20000);
                Assert.AreEqual(participantId, addedCommStatus.ParticipantId);
                Assert.AreEqual(SevisCommStatus.PendingSevisSend.Id, addedCommStatus.SevisCommStatusId);
            };
            context.Revert();
            var result = service.StageBatches(user);
            Assert.AreEqual(1, context.SaveChangesCalledCount);
            tester(result);

            context.Revert();
            var resultAsync = await service.StageBatchesAsync(user);
            tester(resultAsync);
            Assert.AreEqual(2, context.SaveChangesCalledCount);
        }

        [TestMethod]
        public async Task TestStageBatches_ParticipantsDoNotHaveSevisId_MultipleBatches()
        {
            var personId = 10;
            var participantId1 = 1;
            var participantId2 = 2;
            var projectId = 3;
            var user = new User(1);
            Participant participant1 = null;
            ParticipantPerson participantPerson1 = null;
            Participant participant2 = null;
            ParticipantPerson participantPerson2 = null;
            var readyToSubmitStatus1 = new ParticipantPersonSevisCommStatus
            {
                Id = 1,
                SevisCommStatusId = SevisCommStatus.ReadyToSubmit.Id,
                AddedOn = DateTime.UtcNow.AddDays(-2.0)
            };
            var readyToSubmitStatus2 = new ParticipantPersonSevisCommStatus
            {
                Id = 2,
                SevisCommStatusId = SevisCommStatus.ReadyToSubmit.Id,
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
                person: GetPerson(personId, participantId1),
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
                participant1 = new Participant
                {
                    ParticipantId = participantId1,
                    ProjectId = projectId
                };
                participantPerson1 = new ParticipantPerson
                {
                    Participant = participant1,
                    ParticipantId = participantId1
                };
                participant2 = new Participant
                {
                    ParticipantId = participantId2,
                    ProjectId = projectId
                };
                participantPerson2 = new ParticipantPerson
                {
                    Participant = participant2,
                    ParticipantId = participantId2
                };
                readyToSubmitStatus1.ParticipantPerson = participantPerson1;
                readyToSubmitStatus1.ParticipantId = participantId1;
                readyToSubmitStatus2.ParticipantPerson = participantPerson2;
                readyToSubmitStatus2.ParticipantId = participantId2;
                participantPerson1.ParticipantPersonSevisCommStatuses.Add(readyToSubmitStatus1);
                participantPerson2.ParticipantPersonSevisCommStatuses.Add(readyToSubmitStatus2);
                context.Participants.Add(participant1);
                context.Participants.Add(participant2);
                context.ParticipantPersons.Add(participantPerson1);
                context.ParticipantPersons.Add(participantPerson2);
                context.ParticipantPersonSevisCommStatuses.Add(readyToSubmitStatus1);
                context.ParticipantPersonSevisCommStatuses.Add(readyToSubmitStatus2);
            });
            Action<List<StagedSevisBatch>> tester = (batches) =>
            {
                Assert.IsNotNull(batches);
                Assert.AreEqual(2, batches.Count);

                Assert.AreNotEqual(batches.First().BatchId, batches.Last().BatchId);
                Assert.AreNotEqual(Guid.Empty, batches.First().BatchId);
                Assert.AreNotEqual(Guid.Empty, batches.Last().BatchId);
                Assert.AreEqual(2, context.SevisBatchProcessings.Count());
                Assert.AreEqual(1, batches.First().GetExchangeVisitors().Count());
                Assert.AreEqual(1, batches.Last().GetExchangeVisitors().Count());

                Assert.AreEqual(1, batches.First().SEVISBatchCreateUpdateEV.CreateEV.Count());
                Assert.AreEqual(1, batches.Last().SEVISBatchCreateUpdateEV.CreateEV.Count());
            };
            context.Revert();
            var result = service.StageBatches(user);
            tester(result);
            Assert.AreEqual(1, context.SaveChangesCalledCount);

            context.Revert();
            var resultAsync = await service.StageBatchesAsync(user);
            tester(resultAsync);
            Assert.AreEqual(2, context.SaveChangesCalledCount);
        }

        [TestMethod]
        public async Task TestStageBatches_ParticipantsHaveSevisId_MultipleBatches()
        {
            var sevisId = "sevisId";
            var personId = 10;
            var participantId1 = 1;
            var participantId2 = 2;
            var projectId = 3;
            var user = new User(1);
            Participant participant1 = null;
            ParticipantPerson participantPerson1 = null;
            Participant participant2 = null;
            ParticipantPerson participantPerson2 = null;
            var readyToSubmitStatus1 = new ParticipantPersonSevisCommStatus
            {
                Id = 1,
                SevisCommStatusId = SevisCommStatus.ReadyToSubmit.Id,
                AddedOn = DateTime.UtcNow.AddDays(-2.0)
            };
            var readyToSubmitStatus2 = new ParticipantPersonSevisCommStatus
            {
                Id = 2,
                SevisCommStatusId = SevisCommStatus.ReadyToSubmit.Id,
                AddedOn = DateTime.UtcNow.AddDays(-1.0)
            };

            var siteOfActivity = new AddressDTO
            {
                Division = "DC",
                LocationName = "name"
            };
            var exchangeVisitor = new ExchangeVisitor(
                user: user,
                sevisId: sevisId,
                person: GetPerson(personId, participantId1),
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
                participant1 = new Participant
                {
                    ParticipantId = participantId1,
                    ProjectId = projectId,
                };
                participantPerson1 = new ParticipantPerson
                {
                    Participant = participant1,
                    ParticipantId = participantId1,
                    SevisId = sevisId
                };
                participant2 = new Participant
                {
                    ParticipantId = participantId2,
                    ProjectId = projectId
                };
                participantPerson2 = new ParticipantPerson
                {
                    Participant = participant2,
                    ParticipantId = participantId2,
                    SevisId = sevisId
                };
                readyToSubmitStatus1.ParticipantPerson = participantPerson1;
                readyToSubmitStatus1.ParticipantId = participantId1;
                readyToSubmitStatus2.ParticipantPerson = participantPerson2;
                readyToSubmitStatus2.ParticipantId = participantId2;
                participantPerson1.ParticipantPersonSevisCommStatuses.Add(readyToSubmitStatus1);
                participantPerson2.ParticipantPersonSevisCommStatuses.Add(readyToSubmitStatus2);
                context.Participants.Add(participant1);
                context.Participants.Add(participant2);
                context.ParticipantPersons.Add(participantPerson1);
                context.ParticipantPersons.Add(participantPerson2);
                context.ParticipantPersonSevisCommStatuses.Add(readyToSubmitStatus1);
                context.ParticipantPersonSevisCommStatuses.Add(readyToSubmitStatus2);
            });
            Action<List<StagedSevisBatch>> tester = (batches) =>
            {
                Assert.IsNotNull(batches);
                Assert.AreEqual(2, batches.Count);

                Assert.AreEqual(0, batches.First().SEVISBatchCreateUpdateEV.CreateEV.Count());
                Assert.IsTrue(0 < batches.First().SEVISBatchCreateUpdateEV.UpdateEV.Count());

                Assert.AreEqual(0, batches.Last().SEVISBatchCreateUpdateEV.CreateEV.Count());
                Assert.IsTrue(0 < batches.Last().SEVISBatchCreateUpdateEV.UpdateEV.Count());
            };
            context.Revert();
            var result = service.StageBatches(user);
            tester(result);
            Assert.AreEqual(1, context.SaveChangesCalledCount);

            context.Revert();
            var resultAsync = await service.StageBatchesAsync(user);
            tester(resultAsync);
            Assert.AreEqual(2, context.SaveChangesCalledCount);
        }

        [TestMethod]
        public async Task TestStageBatches_NoReadyToSubmitParticipants()
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

        //[TestMethod]
        //public void TestSevisBatchProcessing_Create()
        //{
        //    var sevisBatchProcessing = service.Create();
        //    Assert.IsTrue(sevisBatchProcessing.BatchId == 0);
        //}

        //[TestMethod]
        //public void TestSevisBatchProcessing_XML_Get()
        //{
        //    var sevisBatchProcessing = service.Create();
        //    var xml = sevisBatchProcessing.SendXml;
        //}

        //[TestMethod]
        //public void TestSevisBatchProcessing_XML_SetGet()
        //{
        //    var sevisBatchProcessing = service.Create();
        //    string testXmlString = "<root><e1>test</e1><e2>test2</e2></root>";
        //    sevisBatchProcessing.SendXml = XElement.Parse(testXmlString);
        //    string outXmlString = sevisBatchProcessing.SendXml.ToString(SaveOptions.DisableFormatting);
        //    Assert.AreEqual(testXmlString, outXmlString);

        //    sevisBatchProcessing.TransactionLogXml = XElement.Parse(testXmlString);
        //    outXmlString = sevisBatchProcessing.SendXml.ToString(SaveOptions.DisableFormatting);
        //    Assert.AreEqual(testXmlString, outXmlString);
        //}

        //[TestMethod]
        //public void TestSevisBatchProcessing_GetById()
        //{
        //    var sbp1 = new ECA.Data.SevisBatchProcessing
        //    {
        //        BatchId = 1,
        //        SendXml = XElement.Parse("<root><e1>teste1</e1></root>")
        //    };

        //    context.SevisBatchProcessings.Add(sbp1);

        //    var sbpDTO = service.GetById(1);

        //    Assert.AreEqual(sbpDTO.BatchId, sbp1.BatchId);
        //    Assert.AreEqual(sbpDTO.SendXml.ToString(), sbp1.SendXml.ToString());
        //}

        //[TestMethod]
        //public void TestSevisBatchProcessing_GetSevisBatchProcessingDTOsForUpload()
        //{
        //    var sbp1 = new ECA.Data.SevisBatchProcessing
        //    {
        //        BatchId = 1,
        //        SendXml = XElement.Parse("<root><e1>teste1</e1></root>")
        //    };

        //    var sbp2 = new ECA.Data.SevisBatchProcessing
        //    {
        //        BatchId = 2,
        //        SendXml = XElement.Parse("<root><e1>teste1</e1></root>")
        //    };

        //    var sbp3 = new ECA.Data.SevisBatchProcessing
        //    {
        //        BatchId = 3,
        //        SendXml = XElement.Parse("<root><e1>teste1</e1></root>"),
        //        SubmitDate = new DateTime(2012, 12, 31)
        //    };
        //    context.SevisBatchProcessings.Add(sbp1);
        //    context.SevisBatchProcessings.Add(sbp2);
        //    context.SevisBatchProcessings.Add(sbp3);

        //    var sbpDTOs = service.GetSevisBatchesToUpload();

        //    Assert.IsTrue(sbpDTOs.Count() == 2);
        //    Assert.AreEqual(sbpDTOs.ElementAt(0).BatchId, sbp1.BatchId);
        //    Assert.AreEqual(sbpDTOs.ElementAt(1).BatchId, sbp2.BatchId);
        //}

        //[TestMethod]
        //public void TestSevisBatchProcessing_SaveBatchResult()
        //{
        //    var user = new User(1);
        //    var sbp1 = new ECA.Data.SevisBatchProcessing
        //    {
        //        BatchId = 1,
        //        SubmitDate = DateTimeOffset.Now,
        //        RetrieveDate = DateTimeOffset.Now,
        //        SendXml = XElement.Parse(@"<root></root>"),
        //        TransactionLogXml = XElement.Parse(@"<Root><Process><Record sevisID='N0000000001' requestID='123' userID='1'><Result status='0'><ErrorCode>S1056</ErrorCode><ErrorMessage>Invalid student visa type for this action</ErrorMessage></Result><Result status='0'><ErrorCode>S1048</ErrorCode><ErrorMessage>School Code is missing</ErrorMessage></Result></Record></Process></Root>")
        //    };
        //    ParticipantType participantType = new ParticipantType
        //    {
        //        IsPerson = true,
        //        Name = ParticipantType.Individual.Value,
        //        ParticipantTypeId = ParticipantType.Individual.Id
        //    };
        //    ParticipantStatus status = new ParticipantStatus
        //    {
        //        ParticipantStatusId = ParticipantStatus.Active.Id,
        //        Status = ParticipantStatus.Active.Value
        //    };
        //    var gender = new Gender
        //    {
        //        GenderId = Gender.Male.Id,
        //        GenderName = Gender.Male.Value
        //    };
        //    var person = new Person
        //    {
        //        PersonId = 1,
        //        Gender = gender,
        //        GenderId = gender.GenderId,
        //        FirstName = "first",
        //        LastName = "last",
        //        FullName = "full name"
        //    };
        //    var participantPerson = new ParticipantPerson
        //    {
        //        ParticipantId = 123,
        //        SevisId = "N0000000001"
        //    };
        //    ParticipantPersonSevisCommStatus sevisCommStatus = new ParticipantPersonSevisCommStatus
        //    {
        //        Id = 1,
        //        AddedOn = DateTimeOffset.Now,
        //        ParticipantId = 123,
        //        ParticipantPerson = participantPerson,
        //        SevisCommStatusId = SevisCommStatus.ReadyToSubmit.Id
        //    };
        //    List<ParticipantPersonSevisCommStatus> sevisCommStatuses = new List<ParticipantPersonSevisCommStatus>();
        //    sevisCommStatuses.Add(sevisCommStatus);
        //    participantPerson.ParticipantPersonSevisCommStatuses = sevisCommStatuses;
        //    var project = new Project
        //    {
        //        ProjectId = 1
        //    };
        //    var history = new History
        //    {
        //        RevisedOn = DateTimeOffset.Now
        //    };
        //    var participant = new Participant
        //    {
        //        ParticipantId = participantPerson.ParticipantId,
        //        Person = person,
        //        PersonId = person.PersonId,
        //        ProjectId = project.ProjectId,
        //        Project = project,
        //        ParticipantStatusId = status.ParticipantStatusId,
        //        ParticipantType = participantType,
        //        ParticipantTypeId = participantType.ParticipantTypeId,
        //        ParticipantPerson = participantPerson,
        //        History = history,
        //        Status = status,
        //        StatusDate = DateTimeOffset.Now
        //    };
        //    participantPerson.Participant = participant;
        //    project.Participants.Add(participant);

        //    context.SevisBatchProcessings.Add(sbp1);
        //    context.Projects.Add(project);
        //    context.ParticipantStatuses.Add(status);
        //    context.ParticipantTypes.Add(participantType);
        //    context.Genders.Add(gender);
        //    context.People.Add(person);
        //    context.ParticipantPersonSevisCommStatuses.Add(sevisCommStatus);
        //    context.Participants.Add(participant);
        //    context.ParticipantPersons.Add(participantPerson);

        //    var updates = service.UpdateParticipantPersonSevisBatchStatusAsync(user, 1);

        //    var resultsDTO = updates.Result;

        //    Assert.IsTrue(resultsDTO.Count() == 1);
        //    Assert.IsTrue(resultsDTO.Select(x => x.SevisCommStatus).FirstOrDefault() == SevisCommStatus.BatchRequestUnsuccessful.Value);
        //}


        //[TestMethod]
        //public void TestSevisBatchProcessing_GetById_Null()
        //{
        //    var sbp1 = new ECA.Data.SevisBatchProcessing
        //    {
        //        BatchId = 1,
        //        SendXml = null
        //    };

        //    context.SevisBatchProcessings.Add(sbp1);

        //    var sbpDTO = service.GetById(1);

        //    Assert.AreEqual(sbpDTO.BatchId, sbp1.BatchId);
        //    Assert.AreNotEqual(sbpDTO.SendXml, sbp1.SendXml);
        //}

    }
}
