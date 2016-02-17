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
            sevisService = new ParticipantPersonsSevisService(context);
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
                DateTimeOffset.Now.Should().BeCloseTo(addedStatus.AddedOn, 20000);

                CollectionAssert.AreEqual(new List<int> { participant.ParticipantId }, returnedParticipantIds.ToList());
            };
            context.Revert();
            beforeTester();
            var response = await sevisService.SendToSevisAsync(projectId, new int[] { status.ParticipantId });
            afterTester(response);

            context.Revert();
            beforeTester();
            response = sevisService.SendToSevis(projectId, new int[] { status.ParticipantId });
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
            context.SetupActions.Add(() =>
            {
                context.ParticipantPersons.Add(participantPerson);
                context.Participants.Add(participant);
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
            var response = await sevisService.SendToSevisAsync(projectId, new int[] { });
            afterTester(response);

            context.Revert();
            beforeTester();
            response = sevisService.SendToSevis(projectId, new int[] { });
            afterTester(response);
        }

        [TestMethod]
        public async Task TestSendToSevis_Null()
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
            var response = await sevisService.SendToSevisAsync(projectId, null);
            afterTester(response);

            context.Revert();
            beforeTester();
            response = sevisService.SendToSevis(projectId, null);
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
            var response = await sevisService.SendToSevisAsync(projectId, new int[] { status.ParticipantId });
            afterTester(response);

            context.Revert();
            beforeTester();
            response = sevisService.SendToSevis(projectId, new int[] { status.ParticipantId });
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
            var response = await sevisService.SendToSevisAsync(projectId + 1, new int[] { status.ParticipantId });
            afterTester(response);

            context.Revert();
            beforeTester();
            response = sevisService.SendToSevis(projectId + 1, new int[] { status.ParticipantId });
            afterTester(response);
        }

        [TestMethod]
        public async Task TestUpdateParticipantPersonSevisCommStatus_ParticipantDoesNotExist()
        {
            var participantId = 1;
            var projectId = 2;
            var user = new User(3);
            Assert.AreEqual(0, context.Participants.Count());

            var message = String.Format("The model of type [{0}] with id [{1}] was not found.", typeof(Participant).Name, participantId);
            Action a = () => sevisService.UpdateParticipantPersonSevisCommStatus(user, projectId, participantId, null);
            Func<Task> f = () => sevisService.UpdateParticipantPersonSevisCommStatusAsync(user, projectId, participantId, null);
            a.ShouldThrow<ModelNotFoundException>().WithMessage(message);
            f.ShouldThrow<ModelNotFoundException>().WithMessage(message);
        }

        [TestMethod]
        public async Task TestUpdateParticipantPersonSevisCommStatus_ParticipantDoesNotBelongToProject()
        {
            var participantId = 1;
            var projectId = 2;
            var user = new User(3);
            var project = new Project
            {
                ProjectId = projectId,
            };
            var participant = new Participant
            {
                ParticipantId = participantId,
                Project = project,
                ProjectId = project.ProjectId
            };
            Assert.AreEqual(0, context.Participants.Count());
            context.Participants.Add(participant);
            context.Projects.Add(project);

            var message = String.Format("The user with id [{0}] attempted to validate a participant with id [{1}] and project id [{2}] but should have been denied access.",
                        user.Id,
                        participant.ParticipantId,
                        projectId + 1);

            Action a = () => sevisService.UpdateParticipantPersonSevisCommStatus(user, projectId + 1, participantId, null);
            Func<Task> f = () => sevisService.UpdateParticipantPersonSevisCommStatusAsync(user, projectId + 1, participantId, null);
            a.ShouldThrow<BusinessSecurityException>().WithMessage(message);
            f.ShouldThrow<BusinessSecurityException>().WithMessage(message);
        }
    }
}
