using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Persons;
using System.Threading.Tasks;
using ECA.Data;
using System.Collections.Generic;
using System.Linq;
using Moq;
using ECA.Business.Sevis.Validation;
using ECA.Business.Validation;
using ECA.Business.Service;

namespace ECA.Business.Test.Service.Persons
{
    [TestClass]
    public class ParticipantPersonSevisServiceTest
    {
        private TestEcaContext context;
        private ParticipantPersonsSevisService sevisService;
        private Mock<ISevisValidator<object, UpdatedParticipantPersonSevisValidationEntity>> sevisValidator;
        private ParticipantPersonService personService;
        private Mock<IBusinessValidator<Object, UpdatedParticipantPersonValidationEntity>> personValidator;

        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
            sevisValidator = new Mock<ISevisValidator<object, UpdatedParticipantPersonSevisValidationEntity>>();
            sevisService = new ParticipantPersonsSevisService(context, sevisValidator.Object);
            personValidator = new Mock<IBusinessValidator<object, UpdatedParticipantPersonValidationEntity>>();
            personService = new ParticipantPersonService(context, personValidator.Object);
        }

        [TestMethod]
        public async Task TestSendToSevis()
        {
            var now = DateTimeOffset.Now;
            var yesterday = now.AddDays(-1.0);

            var status = new ParticipantPersonSevisCommStatus
            {
                Id = 1,
                ParticipantId = 1,
                SevisCommStatusId = SevisCommStatus.InformationRequired.Id,
                AddedOn = yesterday
            };

            var status2 = new ParticipantPersonSevisCommStatus
            {
                Id = 2,
                ParticipantId = 1,
                SevisCommStatusId = SevisCommStatus.ReadyToSubmit.Id,
                AddedOn = now
            };

            context.ParticipantPersonSevisCommStatuses.Add(status);
            context.ParticipantPersonSevisCommStatuses.Add(status2);

            var response = await sevisService.SendToSevis(new int[] { status.ParticipantId });

            Assert.AreEqual(1, response.Length);
            Assert.AreEqual(status.ParticipantId, response[0]);

            var newStatus = context.ParticipantPersonSevisCommStatuses.Where(p => p.ParticipantId == status.ParticipantId)
                .OrderByDescending(o => o.AddedOn)
                .FirstOrDefault();

            Assert.AreEqual(SevisCommStatus.QueuedToSubmit.Id, newStatus.SevisCommStatusId);
        }

        [TestMethod]
        public async Task TestSendToSevis_EmptyArray()
        {
            var response = await sevisService.SendToSevis(new int[] {});
            Assert.AreEqual(0, response.Length);
        }

        [TestMethod]
        public async Task TestSendToSevis_Null()
        {
            var response = await sevisService.SendToSevis(null);
            Assert.AreEqual(0, response.Length);
        }

        [TestMethod]
        public void TestSendToSevis_NullPerson()
        {



        }
        
        [TestMethod]
        public async Task TestSendToSevis_IncorrectStatus()
        {
            var status = new ParticipantPersonSevisCommStatus
            {
                Id = 1,
                ParticipantId = 1,
                SevisCommStatusId = SevisCommStatus.InformationRequired.Id,
                AddedOn = DateTimeOffset.Now
            };

            context.ParticipantPersonSevisCommStatuses.Add(status);

            var response = await sevisService.SendToSevis(new int[] { status.ParticipantId });

            Assert.AreEqual(0, response.Length);
        }

        [TestMethod]
        public async Task TestSevisValidation()
        {
            var updaterId = 2;
            var updater = new User(updaterId);
            var now = DateTimeOffset.Now;
            var yesterday = now.AddDays(-1.0);

            var person = new Person
            {
                PersonId = 1,
                FirstName = "firstName",
                LastName = "lastName"
            };
            var history = new History
            {
                RevisedOn = DateTimeOffset.Now
            };
            var participantPerson = new ParticipantPerson
            {
                ParticipantId = 1,
                SevisId = "N0000000001",
                StudyProject = "studyProject",
                HomeInstitutionAddressId = 3,
                HostInstitutionAddressId = 4,
            };
            var project = new Project
            {
                ProjectId = 1
            };
            var participantType = new ParticipantType
            {
                ParticipantTypeId = ParticipantType.Individual.Id,
                Name = "name"
            };
            var participant = new Participant
            {
                ParticipantId = 1,
                PersonId = person.PersonId,
                Person = person,
                ProjectId = project.ProjectId,
                Project = project,
                ParticipantType = participantType,
                ParticipantTypeId = participantType.ParticipantTypeId,
                History = history
            };
            var status = new ParticipantStatus
            {
                Status = "status",
            };
            var cstatus = new ParticipantPersonSevisCommStatus
            {
                Id = 1,
                ParticipantId = 1,
                SevisCommStatusId = SevisCommStatus.InformationRequired.Id,
                AddedOn = yesterday
            };
            var cstatus2 = new ParticipantPersonSevisCommStatus
            {
                Id = 2,
                ParticipantId = 1,
                SevisCommStatusId = SevisCommStatus.ReadyToSubmit.Id,
                AddedOn = now
            };

            participant.Status = status;
            status.Participants.Add(participant);
            participantPerson.Participant = participant;
            project.Participants.Add(participant);

            context.ParticipantPersonSevisCommStatuses.Add(cstatus);
            context.ParticipantPersonSevisCommStatuses.Add(cstatus2);
            context.ParticipantStatuses.Add(status);
            context.Projects.Add(project);
            context.People.Add(person);
            context.Participants.Add(participant);
            context.ParticipantPersons.Add(participantPerson);

            var updatedPersonSevis = new UpdatedParticipantPersonSevis(
                    updater: updater,
                    participantId: participant.ParticipantId,
                    sevisId: participantPerson.SevisId,
                    fieldOfStudyId: null,
                    positionId: null,
                    programCategoryId: null,
                    isSentToSevisViaRTI: false,
                    isValidatedViaRTI: false,
                    isCancelled: false,
                    isDS2019Printed: false,
                    isNeedsUpdate: false,
                    isDS2019SentToTraveler: false,
                    startDate: null,
                    endDate: null,
                    fundingSponsor: null,
                    fundingPersonal: null,
                    fundingVisGovt: null,
                    fundingVisBNC: null,
                    fundingGovtAgency1: null,
                    govtAgency1Id: 1,
                    govtAgency1OtherName: "Agency 1",
                    fundingGovtAgency2: null,
                    govtAgency2Id: 2,
                    govtAgency2OtherName: "Agency 2",
                    fundingIntlOrg1: null,
                    intlOrg1Id: null,
                    intlOrg1OtherName: "Org 1",
                    fundingIntlOrg2: null,
                    intlOrg2Id: 2,
                    intlOrg2OtherName: "Org 2",
                    fundingOther: null,
                    otherName: null,
                    fundingTotal: null
                );

            var response = await sevisService.UpdateAsync(updatedPersonSevis);
            Assert.IsNotNull(response);
            Assert.AreEqual("Agency Two", response.IntlOrg2OtherName);
        }

    }
}
