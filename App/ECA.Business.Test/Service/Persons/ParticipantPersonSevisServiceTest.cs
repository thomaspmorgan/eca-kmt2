using ECA.Business.Service.Persons;
using FluentAssertions;
using ECA.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using ECA.Business.Service;
using ECA.Core.Exceptions;
using Moq;

namespace ECA.Business.Test.Service.Persons
{
    [TestClass]
    public class ParticipantPersonSevisServiceTest
    {
        private TestEcaContext context;
        private ParticipantPersonsSevisService sevisService;

        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
            sevisService = new ParticipantPersonsSevisService(context, null);
        }

        [TestMethod]
        public async Task TestSendToSevis()
        {
            var now = DateTimeOffset.Now;
            var yesterday = now.AddDays(-1.0);
            var projectId = 1;
            ParticipantPersonSevisCommStatus status = null;
            ParticipantPersonSevisCommStatus status2 = null;
            var participant = new Participant
            {
                ProjectId = projectId,
                ParticipantId = 10
            };
            var participantPerson = new ParticipantPerson
            {
                ParticipantId = participant.ParticipantId,
                Participant = participant,
            };
            ParticipantsToBeSentToSevis model = null;
            context.SetupActions.Add(() =>
            {
                status = new ParticipantPersonSevisCommStatus
                {
                    Id = 1,
                    ParticipantId = participant.ParticipantId,
                    SevisCommStatusId = SevisCommStatus.InformationRequired.Id,
                    AddedOn = yesterday,
                    ParticipantPerson = participantPerson,
                };
                status2 = new ParticipantPersonSevisCommStatus
                {
                    Id = 2,
                    ParticipantId = participant.ParticipantId,
                    SevisCommStatusId = SevisCommStatus.ReadyToSubmit.Id,
                    AddedOn = now,
                    ParticipantPerson = participantPerson,
                };
                context.ParticipantPersons.Add(participantPerson);
                context.Participants.Add(participant);
                context.ParticipantPersonSevisCommStatuses.Add(status);
                context.ParticipantPersonSevisCommStatuses.Add(status2);
                model = new ParticipantsToBeSentToSevis(
                    user: new User(1),
                    projectId: projectId,
                    participantIds: new int[] { status.ParticipantId },
                    sevisUsername: "sevis username",
                    sevisOrgId: "sevis org id");
            });

            Action beforeTester = () =>
            {
                Assert.AreEqual(2, context.ParticipantPersonSevisCommStatuses.Count());
            };
            Action<IEnumerable<int>> afterTester = (returnedParticipantIds) =>
            {
                Assert.AreEqual(3, context.ParticipantPersonSevisCommStatuses.Count());
                var addedStatus = context.ParticipantPersonSevisCommStatuses.Last();
                Assert.AreEqual(SevisCommStatus.QueuedToSubmit.Id, addedStatus.SevisCommStatusId);
                Assert.AreEqual(participant.ParticipantId, addedStatus.ParticipantId);
                Assert.AreEqual(model.SevisUsername, addedStatus.SevisUsername);
                Assert.AreEqual(model.SevisOrgId, addedStatus.SevisOrgId);
                DateTimeOffset.Now.Should().BeCloseTo(addedStatus.AddedOn, 20000);

                CollectionAssert.AreEqual(new List<int> { participant.ParticipantId }, returnedParticipantIds.ToList());
            };
            context.Revert();
            beforeTester();
            var response = await sevisService.SendToSevisAsync(model);
            afterTester(response);

            context.Revert();
            beforeTester();
            response = sevisService.SendToSevis(model);
            afterTester(response);

        }

        [TestMethod]
        public async Task TestSendToSevis_EmptyArray()
        {
            var now = DateTimeOffset.Now;
            var yesterday = now.AddDays(-1.0);
            var projectId = 1;

            var participant = new Participant
            {
                ProjectId = projectId,
                ParticipantId = 10
            };
            var participantPerson = new ParticipantPerson
            {
                ParticipantId = participant.ParticipantId,
                Participant = participant,
            };
            ParticipantsToBeSentToSevis model = null;
            context.SetupActions.Add(() =>
            {
                context.ParticipantPersons.Add(participantPerson);
                context.Participants.Add(participant);
                model = new ParticipantsToBeSentToSevis(
                    user: new User(1),
                    projectId: projectId,
                    participantIds: new int[] { },
                    sevisUsername: "sevis username",
                    sevisOrgId: "sevis org id");
            });
            
            Action beforeTester = () =>
            {
                Assert.AreEqual(0, context.ParticipantPersonSevisCommStatuses.Count());
            };
            Action<IEnumerable<int>> afterTester = (returnedParticipantIds) =>
            {
                Assert.AreEqual(0, context.ParticipantPersonSevisCommStatuses.Count());
                Assert.AreEqual(0, returnedParticipantIds.Count());
            };
            context.Revert();
            beforeTester();
            var response = await sevisService.SendToSevisAsync(model);
            afterTester(response);

            context.Revert();
            beforeTester();
            response = sevisService.SendToSevis(model);
            afterTester(response);
        }

        [TestMethod]
        public async Task TestSendToSevis_NullParticipantIds()
        {
            var now = DateTimeOffset.Now;
            var yesterday = now.AddDays(-1.0);
            var projectId = 1;

            var participant = new Participant
            {
                ProjectId = projectId,
                ParticipantId = 10
            };
            var participantPerson = new ParticipantPerson
            {
                ParticipantId = participant.ParticipantId,
                Participant = participant,
            };
            context.SetupActions.Add(() =>
            {
                context.ParticipantPersons.Add(participantPerson);
                context.Participants.Add(participant);
            });
            var model = new ParticipantsToBeSentToSevis(
                user: new User(1),
                projectId: projectId,
                participantIds: null,
                sevisUsername: "sevis username",
                sevisOrgId: "sevis org id");
            Action beforeTester = () =>
            {
                Assert.AreEqual(0, context.ParticipantPersonSevisCommStatuses.Count());
            };
            Action<IEnumerable<int>> afterTester = (returnedParticipantIds) =>
            {
                Assert.AreEqual(0, context.ParticipantPersonSevisCommStatuses.Count());
                Assert.AreEqual(0, returnedParticipantIds.Count());
            };
            context.Revert();
            beforeTester();
            var response = await sevisService.SendToSevisAsync(model);
            afterTester(response);

            context.Revert();
            beforeTester();
            response = sevisService.SendToSevis(model);
            afterTester(response);
        }


        [TestMethod]
        public async Task TestSendToSevis_IncorrectStatus()
        {
            var now = DateTimeOffset.Now;
            var yesterday = now.AddDays(-1.0);
            var projectId = 1;
            ParticipantPersonSevisCommStatus status = null;
            var participant = new Participant
            {
                ProjectId = projectId,
                ParticipantId = 10
            };
            var participantPerson = new ParticipantPerson
            {
                ParticipantId = participant.ParticipantId,
                Participant = participant,
            };
            ParticipantsToBeSentToSevis model = null;
            context.SetupActions.Add(() =>
            {
                status = new ParticipantPersonSevisCommStatus
                {
                    Id = 1,
                    ParticipantId = participant.ParticipantId,
                    SevisCommStatusId = SevisCommStatus.InformationRequired.Id,
                    AddedOn = yesterday,
                    ParticipantPerson = participantPerson,
                };
                context.ParticipantPersons.Add(participantPerson);
                context.Participants.Add(participant);
                context.ParticipantPersonSevisCommStatuses.Add(status);
                model = new ParticipantsToBeSentToSevis(
                    user: new User(1),
                    projectId: projectId,
                    participantIds: new int[] { status.ParticipantId },
                    sevisUsername: "sevis username",
                    sevisOrgId: "sevis org id");
            });
            Action beforeTester = () =>
            {
                Assert.AreEqual(1, context.ParticipantPersonSevisCommStatuses.Count());
            };
            Action<IEnumerable<int>> afterTester = (returnedParticipantIds) =>
            {
                Assert.AreEqual(1, context.ParticipantPersonSevisCommStatuses.Count());
                Assert.AreEqual(0, returnedParticipantIds.Count());
            };
            context.Revert();
            beforeTester();
            var response = await sevisService.SendToSevisAsync(model);
            afterTester(response);

            context.Revert();
            beforeTester();
            response = sevisService.SendToSevis(model);
            afterTester(response);
        }

        [TestMethod]
        public async Task TestSendToSevis_ParticipantDoesNotBelongToProject()
        {
            var now = DateTimeOffset.Now;
            var yesterday = now.AddDays(-1.0);
            var projectId = 1;
            ParticipantPersonSevisCommStatus status = null;
            var participant = new Participant
            {
                ProjectId = projectId,
                ParticipantId = 10
            };
            var participantPerson = new ParticipantPerson
            {
                ParticipantId = participant.ParticipantId,
                Participant = participant,
            };
            ParticipantsToBeSentToSevis model = null;
            context.SetupActions.Add(() =>
            {
                status = new ParticipantPersonSevisCommStatus
                {
                    Id = 1,
                    ParticipantId = participant.ParticipantId,
                    SevisCommStatusId = SevisCommStatus.ReadyToSubmit.Id,
                    AddedOn = yesterday,
                    ParticipantPerson = participantPerson,
                };
                context.ParticipantPersons.Add(participantPerson);
                context.Participants.Add(participant);
                context.ParticipantPersonSevisCommStatuses.Add(status);
                model = new ParticipantsToBeSentToSevis(
                    user: new User(1),
                    projectId: projectId + 1,
                    participantIds: new int[] { status.ParticipantId },
                    sevisUsername: "sevis username",
                    sevisOrgId: "sevis org id");
            });
            
            Action beforeTester = () =>
            {
                Assert.AreEqual(1, context.ParticipantPersonSevisCommStatuses.Count());
            };
            Action<IEnumerable<int>> afterTester = (returnedParticipantIds) =>
            {
                Assert.AreEqual(1, context.ParticipantPersonSevisCommStatuses.Count());
                Assert.AreEqual(0, returnedParticipantIds.Count());
            };
            context.Revert();
            beforeTester();
            var response = await sevisService.SendToSevisAsync(model);
            afterTester(response);

            context.Revert();
            beforeTester();
            response = sevisService.SendToSevis(model);
            afterTester(response);
        }
    }
}
