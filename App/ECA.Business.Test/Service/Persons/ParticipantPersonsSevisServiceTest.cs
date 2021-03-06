﻿using ECA.Business.Service.Persons;
using FluentAssertions;
using ECA.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using ECA.Business.Service;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Business.Queries.Models.Persons;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using ECA.Core.DynamicLinq.Filter;
using ECA.Core.Exceptions;
using ECA.Business.Queries.Models.Persons.ExchangeVisitor;
using Microsoft.QualityTools.Testing.Fakes;

namespace ECA.Business.Test.Service.Persons
{
    [TestClass]
    public class ParticipantPersonsSevisServiceTest
    {
        private TestEcaContext context;
        private ParticipantPersonsSevisService sevisService;

        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
            sevisService = new ParticipantPersonsSevisService(context, null);
        }
        #region Send To Sevis
        [TestMethod]
        public async Task TestSendToSevis_ReadyToSubmit()
        {
            var now = DateTimeOffset.Now;
            var yesterday = now.AddDays(-1.0);
            var projectId = 1;
            ParticipantPersonSevisCommStatus status1 = null;
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
                status1 = new ParticipantPersonSevisCommStatus
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
                participantPerson.ParticipantPersonSevisCommStatuses.Add(status1);
                participantPerson.ParticipantPersonSevisCommStatuses.Add(status2);
                context.ParticipantPersons.Add(participantPerson);
                context.Participants.Add(participant);
                context.ParticipantPersonSevisCommStatuses.Add(status1);
                context.ParticipantPersonSevisCommStatuses.Add(status2);
                model = new ParticipantsToBeSentToSevis(
                    user: new User(1),
                    projectId: projectId,
                    participantIds: new int[] { status1.ParticipantId },
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
                Assert.AreEqual(model.Audit.User.Id, addedStatus.PrincipalId);
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
        public async Task TestSendToSevis_BatchCancelledBySystem_BatchWasQueuedToSubmit()
        {
            var now = DateTimeOffset.Now;
            var yesterday = now.AddDays(-1.0);
            var projectId = 1;
            ParticipantPersonSevisCommStatus status1 = null;
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
                status1 = new ParticipantPersonSevisCommStatus
                {
                    Id = 1,
                    ParticipantId = participant.ParticipantId,
                    SevisCommStatusId = SevisCommStatus.QueuedToSubmit.Id,
                    AddedOn = yesterday,
                    ParticipantPerson = participantPerson,
                };
                status2 = new ParticipantPersonSevisCommStatus
                {
                    Id = 2,
                    ParticipantId = participant.ParticipantId,
                    SevisCommStatusId = SevisCommStatus.BatchCancelledBySystem.Id,
                    AddedOn = now,
                    ParticipantPerson = participantPerson,
                };
                participantPerson.ParticipantPersonSevisCommStatuses.Add(status1);
                participantPerson.ParticipantPersonSevisCommStatuses.Add(status2);
                context.ParticipantPersons.Add(participantPerson);
                context.Participants.Add(participant);
                context.ParticipantPersonSevisCommStatuses.Add(status1);
                context.ParticipantPersonSevisCommStatuses.Add(status2);
                model = new ParticipantsToBeSentToSevis(
                    user: new User(1),
                    projectId: projectId,
                    participantIds: new int[] { status1.ParticipantId },
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
                Assert.AreEqual(model.Audit.User.Id, addedStatus.PrincipalId);
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
        public async Task TestSendToSevis_BatchCancelledBySystem_BatchWasQueuedToValidate()
        {
            var now = DateTimeOffset.Now;
            var yesterday = now.AddDays(-1.0);
            var projectId = 1;
            ParticipantPersonSevisCommStatus status1 = null;
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
                status1 = new ParticipantPersonSevisCommStatus
                {
                    Id = 1,
                    ParticipantId = participant.ParticipantId,
                    SevisCommStatusId = SevisCommStatus.QueuedToValidate.Id,
                    AddedOn = yesterday,
                    ParticipantPerson = participantPerson,
                };
                status2 = new ParticipantPersonSevisCommStatus
                {
                    Id = 2,
                    ParticipantId = participant.ParticipantId,
                    SevisCommStatusId = SevisCommStatus.BatchCancelledBySystem.Id,
                    AddedOn = now,
                    ParticipantPerson = participantPerson,
                };
                participantPerson.ParticipantPersonSevisCommStatuses.Add(status1);
                participantPerson.ParticipantPersonSevisCommStatuses.Add(status2);
                context.ParticipantPersons.Add(participantPerson);
                context.Participants.Add(participant);
                context.ParticipantPersonSevisCommStatuses.Add(status1);
                context.ParticipantPersonSevisCommStatuses.Add(status2);
                model = new ParticipantsToBeSentToSevis(
                    user: new User(1),
                    projectId: projectId,
                    participantIds: new int[] { status1.ParticipantId },
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
                Assert.AreEqual(SevisCommStatus.QueuedToValidate.Id, addedStatus.SevisCommStatusId);
                Assert.AreEqual(participant.ParticipantId, addedStatus.ParticipantId);
                Assert.AreEqual(model.SevisUsername, addedStatus.SevisUsername);
                Assert.AreEqual(model.SevisOrgId, addedStatus.SevisOrgId);
                Assert.AreEqual(model.Audit.User.Id, addedStatus.PrincipalId);
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
        public async Task TestSendToSevis_ReadyToValidate()
        {
            var now = DateTimeOffset.Now;
            var yesterday = now.AddDays(-1.0);
            var projectId = 1;
            ParticipantPersonSevisCommStatus status1 = null;
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
                status1 = new ParticipantPersonSevisCommStatus
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
                    SevisCommStatusId = SevisCommStatus.ReadyToValidate.Id,
                    AddedOn = now,
                    ParticipantPerson = participantPerson,
                };
                participantPerson.ParticipantPersonSevisCommStatuses.Add(status1);
                participantPerson.ParticipantPersonSevisCommStatuses.Add(status2);
                context.ParticipantPersons.Add(participantPerson);
                context.Participants.Add(participant);
                context.ParticipantPersonSevisCommStatuses.Add(status1);
                context.ParticipantPersonSevisCommStatuses.Add(status2);
                model = new ParticipantsToBeSentToSevis(
                    user: new User(1),
                    projectId: projectId,
                    participantIds: new int[] { status1.ParticipantId },
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
                Assert.AreEqual(SevisCommStatus.QueuedToValidate.Id, addedStatus.SevisCommStatusId);
                Assert.AreEqual(participant.ParticipantId, addedStatus.ParticipantId);
                Assert.AreEqual(model.SevisUsername, addedStatus.SevisUsername);
                Assert.AreEqual(model.SevisOrgId, addedStatus.SevisOrgId);
                Assert.AreEqual(model.Audit.User.Id, addedStatus.PrincipalId);
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
        #endregion

        #region GetSevisCommStatusesByParticipantId
        [TestMethod]
        public async Task TestGetSevisCommStatusesByParticipantId_Paged()
        {
            var userAccount = new UserAccount
            {
                PrincipalId = 100,
                DisplayName = "display name",
                EmailAddress = "email"
            };
            var participant = new Participant
            {
                ParticipantId = 1,
                ProjectId = 100
            };
            var participantPerson = new ParticipantPerson
            {
                ParticipantId = participant.ParticipantId,
                Participant = participant
            };
            participant.ParticipantPerson = participantPerson;
            var sevisCommStatus = new SevisCommStatus
            {
                SevisCommStatusId = 500,
                SevisCommStatusName = "sevis comm status name"
            };
            var status1 = new ParticipantPersonSevisCommStatus
            {
                Id = 1,
                AddedOn = DateTimeOffset.UtcNow.AddDays(-1.0),
                BatchId = "batchId",
                ParticipantId = participant.ParticipantId,
                ParticipantPerson = participantPerson,
                PrincipalId = userAccount.PrincipalId,
                SevisCommStatus = sevisCommStatus,
                SevisCommStatusId = sevisCommStatus.SevisCommStatusId,
                SevisOrgId = "sevis org Id",
                SevisUsername = "sevis username"
            };
            var status2 = new ParticipantPersonSevisCommStatus
            {
                Id = 2,
                AddedOn = DateTimeOffset.UtcNow.AddDays(1.0),
                BatchId = "batchId",
                ParticipantId = participant.ParticipantId,
                ParticipantPerson = participantPerson,
                PrincipalId = userAccount.PrincipalId,
                SevisCommStatus = sevisCommStatus,
                SevisCommStatusId = sevisCommStatus.SevisCommStatusId,
                SevisOrgId = "sevis org Id",
                SevisUsername = "sevis username"
            };
            context.UserAccounts.Add(userAccount);
            context.Participants.Add(participant);
            context.ParticipantPersons.Add(participantPerson);
            context.SevisCommStatuses.Add(sevisCommStatus);
            context.ParticipantPersonSevisCommStatuses.Add(status1);
            context.ParticipantPersonSevisCommStatuses.Add(status2);

            var defaultSorter = new ExpressionSorter<ParticipantPersonSevisCommStatusDTO>(x => x.AddedOn, SortDirection.Descending);
            var queryOperator = new QueryableOperator<ParticipantPersonSevisCommStatusDTO>(0, 1, defaultSorter);

            Action<PagedQueryResults<ParticipantPersonSevisCommStatusDTO>> tester = (results) =>
            {
                Assert.AreEqual(2, results.Total);
                Assert.AreEqual(1, results.Results.Count());

                var firstResult = results.Results.First();
                Assert.AreEqual(status2.Id, firstResult.Id);
            };

            var serviceResults = sevisService.GetSevisCommStatusesByParticipantId(participant.ProjectId, participant.ParticipantId, queryOperator);
            var serviceResultsAsync = await sevisService.GetSevisCommStatusesByParticipantIdAsync(participant.ProjectId, participant.ParticipantId, queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetSevisCommStatusesByParticipantId_Filtered()
        {
            var userAccount = new UserAccount
            {
                PrincipalId = 100,
                DisplayName = "display name",
                EmailAddress = "email"
            };
            var participant = new Participant
            {
                ParticipantId = 1,
                ProjectId = 100
            };
            var participantPerson = new ParticipantPerson
            {
                ParticipantId = participant.ParticipantId,
                Participant = participant
            };
            participant.ParticipantPerson = participantPerson;
            var sevisCommStatus = new SevisCommStatus
            {
                SevisCommStatusId = 500,
                SevisCommStatusName = "sevis comm status name"
            };
            var status = new ParticipantPersonSevisCommStatus
            {
                Id = 1,
                AddedOn = DateTimeOffset.UtcNow,
                BatchId = "batchId",
                ParticipantId = participant.ParticipantId,
                ParticipantPerson = participantPerson,
                PrincipalId = userAccount.PrincipalId,
                SevisCommStatus = sevisCommStatus,
                SevisCommStatusId = sevisCommStatus.SevisCommStatusId,
                SevisOrgId = "sevis org Id",
                SevisUsername = "sevis username"
            };
            context.UserAccounts.Add(userAccount);
            context.Participants.Add(participant);
            context.ParticipantPersons.Add(participantPerson);
            context.SevisCommStatuses.Add(sevisCommStatus);
            context.ParticipantPersonSevisCommStatuses.Add(status);

            var defaultSorter = new ExpressionSorter<ParticipantPersonSevisCommStatusDTO>(x => x.AddedOn, SortDirection.Descending);
            var queryOperator = new QueryableOperator<ParticipantPersonSevisCommStatusDTO>(0, 1, defaultSorter);
            var filter = new ExpressionFilter<ParticipantPersonSevisCommStatusDTO>(x => x.ParticipantId, ComparisonType.Equal, participant.ParticipantId);
            queryOperator.Filters.Add(filter);
            Action<PagedQueryResults<ParticipantPersonSevisCommStatusDTO>> tester = (results) =>
            {
                Assert.AreEqual(1, results.Total);
                Assert.AreEqual(1, results.Results.Count());

                var firstResult = results.Results.First();
                Assert.AreEqual(status.Id, firstResult.Id);
            };

            var serviceResults = sevisService.GetSevisCommStatusesByParticipantId(participant.ProjectId, participant.ParticipantId, queryOperator);
            var serviceResultsAsync = await sevisService.GetSevisCommStatusesByParticipantIdAsync(participant.ProjectId, participant.ParticipantId, queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }
        #endregion

        #region GetBatchInfoByBatchId
        [TestMethod]
        public async Task TestGetBatchInfoByBatchId()
        {
            var batchId = "batchId";
            var userId = 10;
            var participant = new Participant
            {
                ParticipantId = 1,
                ProjectId = 2
            };
            var commStatus = new ParticipantPersonSevisCommStatus
            {
                ParticipantId = participant.ParticipantId,
                BatchId = batchId,
            };
            var batch = new SevisBatchProcessing
            {
                BatchId = batchId
            };
            context.Participants.Add(participant);
            context.ParticipantPersonSevisCommStatuses.Add(commStatus);
            context.SevisBatchProcessings.Add(batch);

            Assert.IsNotNull(sevisService.GetBatchInfoByBatchId(userId, participant.ProjectId, participant.ParticipantId, batchId));
            Assert.IsNotNull(await sevisService.GetBatchInfoByBatchIdAsync(userId, participant.ProjectId, participant.ParticipantId, batchId));
        }

        [TestMethod]
        public async Task TestGetBatchInfoByBatchId_BatchIdDoesNotExist()
        {
            var batchId = "batchId";
            var userId = 10;
            var participant = new Participant
            {
                ParticipantId = 1,
                ProjectId = 2
            };
            var commStatus = new ParticipantPersonSevisCommStatus
            {
                ParticipantId = participant.ParticipantId,
                BatchId = batchId,
            };
            var batch = new SevisBatchProcessing
            {
                BatchId = batchId
            };
            context.Participants.Add(participant);
            context.ParticipantPersonSevisCommStatuses.Add(commStatus);
            context.SevisBatchProcessings.Add(batch);

            Assert.IsNotNull(sevisService.GetBatchInfoByBatchId(userId, participant.ProjectId, participant.ParticipantId, batchId));
            Assert.IsNotNull(await sevisService.GetBatchInfoByBatchIdAsync(userId, participant.ProjectId, participant.ParticipantId, batchId));

            Assert.IsNull(sevisService.GetBatchInfoByBatchId(userId, participant.ProjectId, participant.ParticipantId, batchId + "a"));
            Assert.IsNull(await sevisService.GetBatchInfoByBatchIdAsync(userId, participant.ProjectId, participant.ParticipantId, batchId + "a"));
        }

        [TestMethod]
        public async Task TestGetBatchInfoByBatchId_CommStatusesDoNotExist()
        {
            var batchId = "batchId";
            var userId = 10;
            var participant = new Participant
            {
                ParticipantId = 1,
                ProjectId = 2
            };
            var batch = new SevisBatchProcessing
            {
                BatchId = batchId
            };
            context.Participants.Add(participant);
            context.SevisBatchProcessings.Add(batch);

            Assert.IsNull(sevisService.GetBatchInfoByBatchId(userId, participant.ProjectId, participant.ParticipantId, batchId));
            Assert.IsNull(await sevisService.GetBatchInfoByBatchIdAsync(userId, participant.ProjectId, participant.ParticipantId, batchId));
        }

        [TestMethod]
        public async Task TestGetBatchInfoByBatchId_ParticipantDoesNotBelongToProject()
        {
            var batchId = "batchId";
            var userId = 10;
            var participant = new Participant
            {
                ParticipantId = 1,
                ProjectId = 2
            };
            var batch = new SevisBatchProcessing
            {
                BatchId = batchId
            };
            context.Participants.Add(participant);
            context.SevisBatchProcessings.Add(batch);

            var message = String.Format("The user with id [{0}] attempted to validate a participant with id [{1}] and project id [{2}] but should have been denied access.",
                        userId,
                        participant.ParticipantId,
                        participant.ProjectId + 1);
            Action a = () => sevisService.GetBatchInfoByBatchId(userId, participant.ProjectId + 1, participant.ParticipantId, batchId);
            Func<Task> f = () => sevisService.GetBatchInfoByBatchIdAsync(userId, participant.ProjectId + 1, participant.ParticipantId, batchId);
            a.ShouldThrow<BusinessSecurityException>().WithMessage(message);
            f.ShouldThrow<BusinessSecurityException>().WithMessage(message);
        }

        [TestMethod]
        public async Task TestGetBatchInfoByBatchId_ParticipantDoesNotExist()
        {
            var batchId = "batchId";
            var userId = 10;
            var participantId = 1;
            var projectId = 2;
            var message = String.Format("The model of type [{0}] with id [{1}] was not found.", typeof(Participant).Name, participantId);
            Action a = () => sevisService.GetBatchInfoByBatchId(userId, projectId + 1, participantId, batchId);
            Func<Task> f = () => sevisService.GetBatchInfoByBatchIdAsync(userId, projectId, participantId, batchId);
            a.ShouldThrow<ModelNotFoundException>().WithMessage(message);
            f.ShouldThrow<ModelNotFoundException>().WithMessage(message);
        }
        #endregion

        #region Ready to Validate Participants
        [TestMethod]
        public void TestGetEarliestNeedsValidationInfoParticipantDate()
        {
            using (ShimsContext.Create())
            {
                var now = new DateTime(2016, 1, 1, 1, 0, 0);
                var nowOffset = new DateTimeOffset(now);
                System.Fakes.ShimDateTimeOffset.UtcNowGet = () => nowOffset;

                var expectedDate = nowOffset.AddDays(ParticipantPersonsSevisService.NUMBER_OF_DAYS_BEFORE_START_DATE_A_PARTICIPANT_NEEDS_VALIDATION_INFO);
                Assert.AreEqual(expectedDate, ParticipantPersonsSevisService.GetEarliestNeedsValidationInfoParticipantDate());
            }
        }

        [TestMethod]
        public async Task TestGetReadyToValidateParticipants_Paged()
        {
            var status = new ParticipantStatus
            {
                ParticipantStatusId = ParticipantStatus.Active.Id
            };
            var participant = new Participant
            {
                ParticipantId = 1,
                ProjectId = 10,
                Status = status,
                ParticipantStatusId = status.ParticipantStatusId
            };
            var participantPerson = new ParticipantPerson
            {
                ParticipantId = participant.ParticipantId,
                Participant = participant,
                SevisId = "sevisId",
                StartDate = DateTimeOffset.UtcNow.AddDays(-1.0)
            };
            participant.ParticipantPerson = participantPerson;
            var participantCommStatus = new ParticipantPersonSevisCommStatus
            {
                Id = 1,
                SevisCommStatusId = SevisCommStatus.CreatedByBatch.Id,
                ParticipantId = participant.ParticipantId,
                ParticipantPerson = participantPerson
            };
            participantPerson.ParticipantPersonSevisCommStatuses.Add(participantCommStatus);

            context.Participants.Add(participant);
            context.ParticipantPersons.Add(participantPerson);
            context.ParticipantPersonSevisCommStatuses.Add(participantCommStatus);
            context.ParticipantStatuses.Add(status);

            var start = 0;
            var limit = 1;
            var defaultSorter = new ExpressionSorter<ReadyToValidateParticipantDTO>(x => x.ParticipantId, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<ReadyToValidateParticipantDTO>(start, limit, defaultSorter);
            Action<PagedQueryResults<ReadyToValidateParticipantDTO>> tester = (results) =>
            {
                Assert.AreEqual(1, results.Total);
                Assert.AreEqual(1, results.Results.Count);
                var firstResult = results.Results.First();
                Assert.AreEqual(participant.ParticipantId, firstResult.ParticipantId);
            };
            var serviceResults = sevisService.GetReadyToValidateParticipants(queryOperator);
            var serviceResultsAsync = await sevisService.GetReadyToValidateParticipantsAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetReadyToValidateParticipants_CheckStartDateIsAfterMinimumStartDate()
        {
            var status = new ParticipantStatus
            {
                ParticipantStatusId = ParticipantStatus.Active.Id
            };
            var participant = new Participant
            {
                ParticipantId = 1,
                ProjectId = 10,
                Status = status,
                ParticipantStatusId = status.ParticipantStatusId
            };
            var participantPerson = new ParticipantPerson
            {
                ParticipantId = participant.ParticipantId,
                Participant = participant,
                SevisId = "sevisId",
                StartDate = DateTimeOffset.UtcNow.AddDays(ParticipantPersonsSevisService.NUMBER_OF_DAYS_BEFORE_START_DATE_A_PARTICIPANT_NEEDS_VALIDATION_INFO - 1.0)
            };
            participant.ParticipantPerson = participantPerson;
            var participantCommStatus = new ParticipantPersonSevisCommStatus
            {
                Id = 1,
                SevisCommStatusId = SevisCommStatus.CreatedByBatch.Id,
                ParticipantId = participant.ParticipantId,
                ParticipantPerson = participantPerson
            };
            participantPerson.ParticipantPersonSevisCommStatuses.Add(participantCommStatus);

            context.Participants.Add(participant);
            context.ParticipantPersons.Add(participantPerson);
            context.ParticipantPersonSevisCommStatuses.Add(participantCommStatus);
            context.ParticipantStatuses.Add(status);

            var start = 0;
            var limit = 1;
            var defaultSorter = new ExpressionSorter<ReadyToValidateParticipantDTO>(x => x.ParticipantId, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<ReadyToValidateParticipantDTO>(start, limit, defaultSorter);

            Assert.AreEqual(1, sevisService.GetReadyToValidateParticipants(queryOperator).Total);
            Assert.AreEqual(1, (await sevisService.GetReadyToValidateParticipantsAsync(queryOperator)).Total);

            participantPerson.StartDate = DateTimeOffset.UtcNow.AddDays(ParticipantPersonsSevisService.NUMBER_OF_DAYS_BEFORE_START_DATE_A_PARTICIPANT_NEEDS_VALIDATION_INFO + 1.0);
            Action<PagedQueryResults<ReadyToValidateParticipantDTO>> tester = (results) =>
            {
                Assert.AreEqual(0, results.Total);
                Assert.AreEqual(0, results.Results.Count);
            };
            var serviceResults = sevisService.GetReadyToValidateParticipants(queryOperator);
            var serviceResultsAsync = await sevisService.GetReadyToValidateParticipantsAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestIsParticipantReadyToValidate()
        {
            var status = new ParticipantStatus
            {
                ParticipantStatusId = ParticipantStatus.Active.Id
            };
            var participant = new Participant
            {
                ParticipantId = 1,
                ProjectId = 10,
                Status = status,
                ParticipantStatusId = status.ParticipantStatusId
            };
            var participantPerson = new ParticipantPerson
            {
                ParticipantId = participant.ParticipantId,
                Participant = participant,
                SevisId = "sevisId",
                StartDate = DateTimeOffset.UtcNow.AddDays(-1.0)
            };
            participant.ParticipantPerson = participantPerson;
            var participantCommStatus = new ParticipantPersonSevisCommStatus
            {
                Id = 1,
                SevisCommStatusId = SevisCommStatus.CreatedByBatch.Id,
                ParticipantId = participant.ParticipantId,
                ParticipantPerson = participantPerson
            };
            participantPerson.ParticipantPersonSevisCommStatuses.Add(participantCommStatus);

            context.Participants.Add(participant);
            context.ParticipantPersons.Add(participantPerson);
            context.ParticipantPersonSevisCommStatuses.Add(participantCommStatus);
            context.ParticipantStatuses.Add(status);


            Action<bool> tester = (results) =>
            {
                Assert.IsTrue(results);
            };
            var serviceResults = sevisService.IsParticipantReadyToValidate(participant.ParticipantId);
            var serviceResultsAsync = await sevisService.IsParticipantReadyToValidateAsync(participant.ParticipantId);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestIsParticipantReadyToValidate_HasBeenValidated()
        {
            var status = new ParticipantStatus
            {
                ParticipantStatusId = ParticipantStatus.Active.Id
            };
            var participant = new Participant
            {
                ParticipantId = 1,
                ProjectId = 10,
                Status = status,
                ParticipantStatusId = status.ParticipantStatusId
            };
            var participantPerson = new ParticipantPerson
            {
                ParticipantId = participant.ParticipantId,
                Participant = participant,
                SevisId = "sevisId",
                StartDate = DateTimeOffset.UtcNow.AddDays(-1.0)
            };
            participant.ParticipantPerson = participantPerson;
            var participantCommStatus = new ParticipantPersonSevisCommStatus
            {
                Id = 1,
                SevisCommStatusId = SevisCommStatus.ReadyToValidate.Id,
                ParticipantId = participant.ParticipantId,
                ParticipantPerson = participantPerson
            };
            participantPerson.ParticipantPersonSevisCommStatuses.Add(participantCommStatus);

            context.Participants.Add(participant);
            context.ParticipantPersons.Add(participantPerson);
            context.ParticipantPersonSevisCommStatuses.Add(participantCommStatus);
            context.ParticipantStatuses.Add(status);


            Action<bool> tester = (results) =>
            {
                Assert.IsFalse(results);
            };
            var serviceResults = sevisService.IsParticipantReadyToValidate(participant.ParticipantId);
            var serviceResultsAsync = await sevisService.IsParticipantReadyToValidateAsync(participant.ParticipantId);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestIsParticipantReadyToValidate_DifferentParticipant()
        {
            var status = new ParticipantStatus
            {
                ParticipantStatusId = ParticipantStatus.Active.Id
            };
            var participant = new Participant
            {
                ParticipantId = 1,
                ProjectId = 10,
                Status = status,
                ParticipantStatusId = status.ParticipantStatusId
            };
            var participantPerson = new ParticipantPerson
            {
                ParticipantId = participant.ParticipantId,
                Participant = participant,
                SevisId = "sevisId",
                StartDate = DateTimeOffset.UtcNow.AddDays(-1.0)
            };
            participant.ParticipantPerson = participantPerson;
            var participantCommStatus = new ParticipantPersonSevisCommStatus
            {
                Id = 1,
                SevisCommStatusId = SevisCommStatus.CreatedByBatch.Id,
                ParticipantId = participant.ParticipantId,
                ParticipantPerson = participantPerson
            };
            participantPerson.ParticipantPersonSevisCommStatuses.Add(participantCommStatus);

            context.Participants.Add(participant);
            context.ParticipantPersons.Add(participantPerson);
            context.ParticipantPersonSevisCommStatuses.Add(participantCommStatus);
            context.ParticipantStatuses.Add(status);

            Assert.IsTrue(sevisService.IsParticipantReadyToValidate(participant.ParticipantId));

            Action<bool> tester = (results) =>
            {
                Assert.IsFalse(results);
            };
            var serviceResults = sevisService.IsParticipantReadyToValidate(participant.ParticipantId + 1);
            var serviceResultsAsync = await sevisService.IsParticipantReadyToValidateAsync(participant.ParticipantId + 1);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }
        #endregion

        #region GetDS2019FileName
        [TestMethod]
        public async Task TestGetDS2019FileName()
        {
            var user = new User(1);
            var participant = new Participant
            {
                ParticipantId = 1,
                ProjectId = 2
            };
            var participantPerson = new ParticipantPerson
            {
                ParticipantId = participant.ParticipantId,
                DS2019FileName = "test.pdf",
                Participant = participant
            };
            participant.ParticipantPerson = participantPerson;

            Action<string> tester = (fileName) =>
            {
                Assert.AreEqual(participantPerson.DS2019FileName, fileName);
            };
            context.ParticipantPersons.Add(participantPerson);
            context.Participants.Add(participant);

            var response = sevisService.GetDS2019FileName(user, participant.ProjectId, participant.ParticipantId);
            var responseAsync = await sevisService.GetDS2019FileNameAsync(user, participant.ProjectId, participant.ParticipantId);
            tester(response);
            tester(responseAsync);
        }

        [TestMethod]
        public async Task TestGetDS2019FileName_FileNameIsNull()
        {
            var user = new User(1);
            var participant = new Participant
            {
                ParticipantId = 1,
                ProjectId = 2
            };
            var participantPerson = new ParticipantPerson
            {
                ParticipantId = participant.ParticipantId,
                Participant = participant
            };
            participant.ParticipantPerson = participantPerson;

            Action<string> tester = (fileName) =>
            {
                Assert.IsNull(fileName);
            };
            context.ParticipantPersons.Add(participantPerson);
            context.Participants.Add(participant);

            var response = sevisService.GetDS2019FileName(user, participant.ProjectId, participant.ParticipantId);
            var responseAsync = await sevisService.GetDS2019FileNameAsync(user, participant.ProjectId, participant.ParticipantId);
            tester(response);
            tester(responseAsync);
        }

        [TestMethod]
        public async Task TestGetDS2019FileName_ParticipantPersonIsNull()
        {
            var user = new User(1);
            var participant = new Participant
            {
                ParticipantId = 1,
                ProjectId = 2
            };

            Action<string> tester = (fileName) =>
            {
                Assert.IsNull(fileName);
            };
            context.Participants.Add(participant);

            var response = sevisService.GetDS2019FileName(user, participant.ProjectId, participant.ParticipantId);
            var responseAsync = await sevisService.GetDS2019FileNameAsync(user, participant.ProjectId, participant.ParticipantId);
            tester(response);
            tester(responseAsync);
        }

        [TestMethod]
        public async Task TestGetDS2019FileName_ParticipantDoesNotBelongToProject()
        {
            var user = new User(1);
            var participant = new Participant
            {
                ParticipantId = 1,
                ProjectId = 2
            };
            context.Participants.Add(participant);
            var message = String.Format("The user with id [{0}] attempted to validate a participant with id [{1}] and project id [{2}] but should have been denied access.",
                        user.Id,
                        participant.ParticipantId,
                        participant.ProjectId + 1);
            Action a = () => sevisService.GetDS2019FileName(user, participant.ProjectId + 1, participant.ParticipantId);
            Func<Task> f = () => sevisService.GetDS2019FileNameAsync(user, participant.ProjectId + 1, participant.ParticipantId);

            a.ShouldThrow<BusinessSecurityException>().WithMessage(message);
            f.ShouldThrow<BusinessSecurityException>().WithMessage(message);
        }

        [TestMethod]
        public async Task TestGetDS2019FileName_ParticipantDoesNotExist()
        {
            var user = new User(1);
            var participantId = 1;
            var projectId = 1;
            var message = String.Format("The model of type [{0}] with id [{1}] was not found.", typeof(Participant).Name, participantId);
            Action a = () => sevisService.GetDS2019FileName(user, projectId, participantId);
            Func<Task> f = () => sevisService.GetDS2019FileNameAsync(user, projectId, participantId);

            a.ShouldThrow<ModelNotFoundException>().WithMessage(message);
            f.ShouldThrow<ModelNotFoundException>().WithMessage(message);
        }
        #endregion

        #region Update

        [TestMethod]
        public void TestUpdate_All()
        {
            DateTimeOffset now = DateTime.UtcNow;
            int participantId = 1;
            bool isCancelled = false;
            bool isDS2019Printed = false;
            bool isDS2019SentToTraveler = false;
            bool isSentToSevisViaRTI = false;
            bool isValidatedViaRTI = false;
            DateTimeOffset startDate = now.AddDays(-20);
            DateTimeOffset endDate = now.AddDays(+20);
            string sevisId = "N000234193";
            int userId = 5;
            int projectId = 2;
            int personId = 3;

            var participant = new Participant
            {
                ParticipantId = participantId,
                ProjectId = projectId,
                PersonId = personId,
                ParticipantStatusId = 1,
                ParticipantTypeId = 1,
            };

            var person = new Person
            {
                PersonId = personId,
                FullName = "Jack Diddly"
            };

            context.People.Add(person);
            participant.Person = person;

            var participantPerson = new ParticipantPerson
            {
                ParticipantId = participantId,
                IsCancelled = isCancelled,
                IsDS2019Printed = isDS2019Printed,
                IsDS2019SentToTraveler = isDS2019SentToTraveler,
                IsSentToSevisViaRTI = isSentToSevisViaRTI,
                IsValidatedViaRTI = isValidatedViaRTI,
                StartDate = startDate,
                EndDate = endDate,
                SevisId = sevisId,
            };

            var user = new User(userId);

            participantPerson.Participant = participant;
            context.ParticipantPersons.Add(participantPerson);
            participant.ParticipantPerson = participantPerson;
            context.Participants.Add(participant);

            context.SaveChanges();

            bool isCancelledNew = true;
            bool isDS2019PrintedNew = true;
            bool isDS2019SentToTravelerNew = true;
            bool isSentToSevisViaRTINew = true;
            bool isValidatedViaRTINew = true;
            bool isNeedsUpdateNew = true;
            DateTimeOffset startDateNew = now.AddDays(-10);
            DateTimeOffset endDateNew = now.AddDays(+10);
            string sevisIdNew = "N000234194";

            var updatedParticipantPersonSevis = new UpdatedParticipantPersonSevis(user, participantId, sevisIdNew, isSentToSevisViaRTINew, isValidatedViaRTINew, isCancelledNew, isDS2019PrintedNew, isNeedsUpdateNew, isDS2019SentToTravelerNew, startDateNew, endDateNew);
            sevisService.Update(updatedParticipantPersonSevis);
            context.SaveChanges();

            var participantPersonUpdated = sevisService.GetParticipantPersonsSevisById(projectId, participantId);
            Assert.AreEqual(participantId, participantPersonUpdated.ParticipantId);
            Assert.AreEqual(isCancelledNew, participantPersonUpdated.IsCancelled);
            Assert.AreEqual(isDS2019PrintedNew, participantPersonUpdated.IsDS2019Printed);
            Assert.AreEqual(isDS2019SentToTravelerNew, participantPersonUpdated.IsDS2019SentToTraveler);
            Assert.AreEqual(isSentToSevisViaRTINew, participantPersonUpdated.IsSentToSevisViaRTI);
            Assert.AreEqual(isValidatedViaRTINew, participantPersonUpdated.IsValidatedViaRTI);
            Assert.AreEqual(isNeedsUpdateNew, true);

        }

        [TestMethod]
        public void TestUpdate_DoSManualStatusesUpdate()
        {
            DateTimeOffset now = DateTime.UtcNow;
            int participantId = 1;
            bool isCancelled = false;
            bool isDS2019Printed = false;
            bool isDS2019SentToTraveler = false;
            bool isSentToSevisViaRTI = false;
            bool isValidatedViaRTI = false;
            DateTimeOffset startDate = now.AddDays(-20);
            DateTimeOffset endDate = now.AddDays(+20);
            string sevisId = "N000234193";
            int userId = 5;
            int projectId = 2;
            int personId = 3;

            var participant = new Participant
            {
                ParticipantId = participantId,
                ProjectId = projectId,
                PersonId = personId,
                ParticipantStatusId = 1,
                ParticipantTypeId = 1,
            };

            var person = new Person
            {
                PersonId = personId,
                FullName = "Jack Diddly"
            };

            context.People.Add(person);
            participant.Person = person;

            var participantPerson = new ParticipantPerson
            {
                ParticipantId = participantId,
                IsCancelled = isCancelled,
                IsDS2019Printed = isDS2019Printed,
                IsDS2019SentToTraveler = isDS2019SentToTraveler,
                IsSentToSevisViaRTI = isSentToSevisViaRTI,
                IsValidatedViaRTI = isValidatedViaRTI,
                StartDate = startDate,
                EndDate = endDate,
                SevisId = sevisId,
            };

            var user = new User(userId);

            participantPerson.Participant = participant;
            context.ParticipantPersons.Add(participantPerson);
            participant.ParticipantPerson = participantPerson;
            context.Participants.Add(participant);

            context.SaveChanges();

            bool isCancelledNew = true;
            bool isDS2019PrintedNew = true;
            bool isDS2019SentToTravelerNew = true;
            bool isSentToSevisViaRTINew = true;
            bool isValidatedViaRTINew = true;
            bool isNeedsUpdateNew = true;
            DateTimeOffset startDateNew = now.AddDays(-10);
            DateTimeOffset endDateNew = now.AddDays(+10);
            string sevisIdNew = "N000234194";

            var updatedParticipantPersonSevis = new UpdatedParticipantPersonSevis(user, participantId, sevisIdNew, isSentToSevisViaRTI, isValidatedViaRTI, isCancelledNew, isDS2019Printed, false, isDS2019SentToTraveler, startDateNew, endDateNew);
            sevisService.Update(updatedParticipantPersonSevis);
            context.SaveChanges();

            var participantPersonUpdated = sevisService.GetParticipantPersonsSevisById(projectId, participantId);
            Assert.AreEqual(participantId, participantPersonUpdated.ParticipantId);
            Assert.AreEqual(isCancelledNew, participantPersonUpdated.IsCancelled);
            Assert.IsTrue(context.ParticipantPersonSevisCommStatuses.Count() == 1);
            Assert.IsTrue(context.ParticipantPersonSevisCommStatuses.FirstOrDefault().SevisCommStatusId == SevisCommStatus.Cancelled.Id);

            updatedParticipantPersonSevis = new UpdatedParticipantPersonSevis(user, participantId, sevisIdNew, isSentToSevisViaRTINew, isValidatedViaRTI, isCancelledNew, isDS2019Printed, false, isDS2019SentToTraveler, startDateNew, endDateNew);
            sevisService.Update(updatedParticipantPersonSevis);
            context.SaveChanges();
            Assert.IsTrue(context.ParticipantPersonSevisCommStatuses.Count() == 2);
            Assert.IsTrue(context.ParticipantPersonSevisCommStatuses.FirstOrDefault(x => x.SevisCommStatusId == SevisCommStatus.SentToDhsViaRti.Id).SevisCommStatusId == SevisCommStatus.SentToDhsViaRti.Id);


            updatedParticipantPersonSevis = new UpdatedParticipantPersonSevis(user, participantId, sevisIdNew, isSentToSevisViaRTINew, isValidatedViaRTINew, isCancelledNew, isDS2019Printed, false, isDS2019SentToTraveler, startDateNew, endDateNew);
            sevisService.Update(updatedParticipantPersonSevis);
            context.SaveChanges();
            Assert.IsTrue(context.ParticipantPersonSevisCommStatuses.Count() == 3);
            Assert.IsTrue(context.ParticipantPersonSevisCommStatuses.FirstOrDefault(x => x.SevisCommStatusId == SevisCommStatus.ValidatedViaRti.Id).SevisCommStatusId == SevisCommStatus.ValidatedViaRti.Id);


            updatedParticipantPersonSevis = new UpdatedParticipantPersonSevis(user, participantId, sevisIdNew, isSentToSevisViaRTINew, isValidatedViaRTINew, isCancelledNew, isDS2019PrintedNew, false, isDS2019SentToTraveler, startDateNew, endDateNew);
            sevisService.Update(updatedParticipantPersonSevis);
            context.SaveChanges();
            Assert.IsTrue(context.ParticipantPersonSevisCommStatuses.Count() == 4);
            Assert.IsTrue(context.ParticipantPersonSevisCommStatuses.FirstOrDefault(x => x.SevisCommStatusId == SevisCommStatus.Ds2019Printed.Id).SevisCommStatusId == SevisCommStatus.Ds2019Printed.Id);


            updatedParticipantPersonSevis = new UpdatedParticipantPersonSevis(user, participantId, sevisIdNew, isSentToSevisViaRTINew, isValidatedViaRTINew, isCancelledNew, isDS2019PrintedNew, false, isDS2019SentToTravelerNew, startDateNew, endDateNew);
            sevisService.Update(updatedParticipantPersonSevis);
            context.SaveChanges();
            Assert.IsTrue(context.ParticipantPersonSevisCommStatuses.Count() == 5);
            Assert.IsTrue(context.ParticipantPersonSevisCommStatuses.FirstOrDefault(x => x.SevisCommStatusId == SevisCommStatus.Ds2019SentToTraveler.Id).SevisCommStatusId == SevisCommStatus.Ds2019SentToTraveler.Id);

            #endregion Update

        }
    }
}
