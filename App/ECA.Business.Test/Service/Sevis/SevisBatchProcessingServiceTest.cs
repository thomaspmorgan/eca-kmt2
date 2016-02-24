using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Sevis;
using System.Xml.Linq;
using ECA.Data;
using ECA.Business.Service.Persons;
using ECA.Business.Service;
using System.Collections.Generic;
using System.Linq;

namespace ECA.Business.Test.Service.Sevis
{
    [TestClass]
    public class SevisBatchProcessingServiceTest
    {
        private TestEcaContext context;
        private SevisBatchProcessingService service;
        private ParticipantService participantService;
        private ParticipantPersonsSevisService participantPersonService;

        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
            participantService = new ParticipantService(context, null);
            participantPersonService = new ParticipantPersonsSevisService(context, null);
            service = new SevisBatchProcessingService(context, participantService, participantPersonService, null);
        }

        [TestMethod]
        public void TestSevisBatchProcessing_Create()
        {
            var sevisBatchProcessing = service.Create();
            Assert.IsTrue(sevisBatchProcessing.BatchId == 0);
        }

        [TestMethod]
        public void TestSevisBatchProcessing_XML_Get()
        {
            var sevisBatchProcessing = service.Create();
            var xml = sevisBatchProcessing.SendXml;
        }
        
        [TestMethod]
        public void TestSevisBatchProcessing_XML_SetGet()
        {
            var sevisBatchProcessing = service.Create();
            string testXmlString = "<root><e1>test</e1><e2>test2</e2></root>";
            sevisBatchProcessing.SendXml = XElement.Parse(testXmlString);
            string outXmlString = sevisBatchProcessing.SendXml.ToString(SaveOptions.DisableFormatting);
            Assert.AreEqual(testXmlString, outXmlString);

            sevisBatchProcessing.TransactionLogXml = XElement.Parse(testXmlString);
            outXmlString = sevisBatchProcessing.SendXml.ToString(SaveOptions.DisableFormatting);
            Assert.AreEqual(testXmlString, outXmlString);
        }

        [TestMethod]
        public void TestSevisBatchProcessing_GetById()
        {
            var sbp1 = new ECA.Data.SevisBatchProcessing
            {
                BatchId = 1,
                SendXml = XElement.Parse("<root><e1>teste1</e1></root>")
            };

            context.SevisBatchProcessings.Add(sbp1);

            var sbpDTO = service.GetById(1);

            Assert.AreEqual(sbpDTO.BatchId, sbp1.BatchId);
            Assert.AreEqual(sbpDTO.SendXml.ToString(), sbp1.SendXml.ToString());
        }

        [TestMethod]
        public void TestSevisBatchProcessing_SaveBatchResult()
        {
            var user = new User(1);
            var sbp1 = new ECA.Data.SevisBatchProcessing
            {
                BatchId = 1,
                SubmitDate = new DateTimeOffset(new DateTime(2016, 02, 02)),
                RetrieveDate = new DateTimeOffset(new DateTime(2016, 02, 04)),
                SendXml = XElement.Parse(@"<root></root>"),
                TransactionLogXml = XElement.Parse(@"<Root><Process><Record sevisID='N0000000001' requestID='123' userID='1'><Result status='0'><ErrorCode>S1056</ErrorCode><ErrorMessage>Invalid student visa type for this action</ErrorMessage></Result></Record></Process></Root>")
            };
            ParticipantType individual = new ParticipantType
            {
                IsPerson = true,
                Name = ParticipantType.Individual.Value,
                ParticipantTypeId = ParticipantType.Individual.Id
            };
            ParticipantStatus status = new ParticipantStatus
            {
                ParticipantStatusId = ParticipantStatus.Active.Id,
                Status = ParticipantStatus.Active.Value
            };
            var participantPerson = new ParticipantPerson
            {
                ParticipantId = 123,
                SevisId = "N0000000001"
            };
            ParticipantPersonSevisCommStatus sevisCommStatus = new ParticipantPersonSevisCommStatus
            {
                Id = 1,
                AddedOn = DateTimeOffset.Now,
                ParticipantId = 123,
                ParticipantPerson = participantPerson,
                SevisCommStatusId = SevisCommStatus.ReadyToSubmit.Id
            };
            List<ParticipantPersonSevisCommStatus> sevisCommStatuses = new List<ParticipantPersonSevisCommStatus>();
            sevisCommStatuses.Add(sevisCommStatus);
            participantPerson.ParticipantPersonSevisCommStatuses = sevisCommStatuses;
            var project = new Project
            {
                ProjectId = 1
            };
            var participant = new Participant
            {
                ParticipantId = participantPerson.ParticipantId,
                ProjectId = project.ProjectId,
                Project = project,
                ParticipantStatusId = status.ParticipantStatusId,
                ParticipantTypeId = individual.ParticipantTypeId
            };
            participantPerson.Participant = participant;
            project.Participants.Add(participant);

            context.SevisBatchProcessings.Add(sbp1);
            context.Projects.Add(project);
            context.ParticipantStatuses.Add(status);
            context.ParticipantTypes.Add(individual);
            context.ParticipantPersonSevisCommStatuses.Add(sevisCommStatus);
            context.Participants.Add(participant);
            context.ParticipantPersons.Add(participantPerson);

            var updates = service.UpdateParticipantPersonSevisBatchStatusAsync(user, 1);

            var resultsDTO = updates.Result;

            Assert.IsTrue(resultsDTO.Count() == 1);
            Assert.IsTrue(resultsDTO.Select(x => x.SevisCommStatus).FirstOrDefault() == SevisCommStatus.BatchRequestUnsuccessful.Value);
        }


    }
}
