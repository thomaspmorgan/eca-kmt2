using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Data;
using ECA.Business.Queries.Persons;
using ECA.Business.Queries.Models.Persons;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Core.DynamicLinq.Filter;

namespace ECA.Business.Test.Queries.Persons
{
    [TestClass]
    public class ParticipantPersonsSevisQueriesTest
    {
        private TestEcaContext context;

        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
        }

        #region CreateGetParticipantPersonsSevisDTOQuery

        [TestMethod]
        public void TestCreateGetParticipantPersonsSevisDTOQuery_CheckProperties()
        {
            var person = new Person
            {
                PersonId = 1,
                FullName = "full name"
            };
            var status = new ParticipantStatus
            {
                ParticipantStatusId = 1,
                Status = "status",
            };
            var participantType = new ParticipantType
            {
                IsPerson = true,
                Name = "part type",
                ParticipantTypeId = 90
            };
            var participant = new Participant
            {
                ParticipantId = 1,
                Status = status,
                ParticipantStatusId = status.ParticipantStatusId,
                ProjectId = 250,
                ParticipantTypeId = participantType.ParticipantTypeId,
                ParticipantType = participantType,
                Person = person,
                PersonId = person.PersonId
            };
            var participantPerson = new ParticipantPerson
            {
                ParticipantId = participant.ParticipantId,
                Participant = participant,
                EndDate = DateTimeOffset.UtcNow.AddDays(10.0),
                IsCancelled = true,
                IsDS2019Printed = true,
                IsDS2019SentToTraveler = true,
                IsSentToSevisViaRTI = true,
                IsValidatedViaRTI = true,
                SevisBatchResult = "sevis batch result",
                SevisId = "sevis id",
                SevisValidationResult = "sevis validation result",
                StartDate = DateTimeOffset.UtcNow.AddDays(-10.0)
            };
            participant.ParticipantPerson = participantPerson;

            context.ParticipantTypes.Add(participantType);
            context.ParticipantStatuses.Add(status);
            context.Participants.Add(participant);
            context.ParticipantPersons.Add(participantPerson);
            context.People.Add(person);

            var results = ParticipantPersonsSevisQueries.CreateGetParticipantPersonsSevisDTOQuery(context).ToList();
            Assert.AreEqual(1, results.Count());
            var firstResult = results.First();
            Assert.AreEqual(status.Status, firstResult.ParticipantStatus);
            Assert.AreEqual(participant.ParticipantId, firstResult.ParticipantId);
            Assert.AreEqual(participantPerson.SevisId, firstResult.SevisId);
            Assert.AreEqual(participant.ProjectId, firstResult.ProjectId);
            Assert.AreEqual(participantType.Name, firstResult.ParticipantType);
            Assert.AreEqual(participantPerson.IsCancelled, firstResult.IsCancelled);
            Assert.AreEqual(participantPerson.IsDS2019Printed, firstResult.IsDS2019Printed);
            Assert.AreEqual(participantPerson.IsDS2019SentToTraveler, firstResult.IsDS2019SentToTraveler);
            Assert.AreEqual(participantPerson.IsSentToSevisViaRTI, firstResult.IsSentToSevisViaRTI);
            Assert.AreEqual(participantPerson.IsValidatedViaRTI, firstResult.IsValidatedViaRTI);
            Assert.AreEqual(participantPerson.SevisBatchResult, firstResult.SevisBatchResult);
            Assert.AreEqual(participantPerson.SevisId, firstResult.SevisId);
            Assert.AreEqual(participantPerson.EndDate, firstResult.EndDate);
            Assert.AreEqual(participantPerson.StartDate, firstResult.StartDate);
            Assert.AreEqual(person.PersonId, firstResult.PersonId);
            Assert.AreEqual(person.FullName, firstResult.FullName);
        }

        [TestMethod]
        public void TestCreateGetParticipantPersonsSevisDTOQuery_CheckUsesLatestSevisCommStatus()
        {
            var person = new Person
            {
                PersonId = 1,
                FullName = "full name"
            };
            var status = new ParticipantStatus
            {
                ParticipantStatusId = 1,
                Status = "status",
            };
            var participantType = new ParticipantType
            {
                IsPerson = true,
                Name = "part type",
                ParticipantTypeId = 90
            };
            var participant = new Participant
            {
                ParticipantId = 1,
                Status = status,
                ParticipantStatusId = status.ParticipantStatusId,
                ProjectId = 250,
                ParticipantTypeId = participantType.ParticipantTypeId,
                ParticipantType = participantType,
                Person = person,
                PersonId = person.PersonId
            };

            var participantPerson = new ParticipantPerson
            {
                ParticipantId = participant.ParticipantId,
                Participant = participant,
                EndDate = DateTimeOffset.UtcNow.AddDays(10.0),
                IsCancelled = true,
                IsDS2019Printed = true,
                IsDS2019SentToTraveler = true,
                IsSentToSevisViaRTI = true,
                IsValidatedViaRTI = true,
                SevisBatchResult = "sevis batch result",
                SevisId = "sevis id",
                SevisValidationResult = "sevis validation result",
                StartDate = DateTimeOffset.UtcNow.AddDays(-10.0)
            };
            participant.ParticipantPerson = participantPerson;

            var readyToSubmitStatus = new SevisCommStatus
            {
                SevisCommStatusId = SevisCommStatus.ReadyToSubmit.Id,
                SevisCommStatusName = SevisCommStatus.ReadyToSubmit.Value
            };
            var pendingSendToSevisStatus = new SevisCommStatus
            {
                SevisCommStatusId = SevisCommStatus.PendingSevisSend.Id,
                SevisCommStatusName = SevisCommStatus.PendingSevisSend.Value
            };
            var yesterdayStatus = new ParticipantPersonSevisCommStatus
            {
                AddedOn = DateTimeOffset.UtcNow.AddDays(-1.0),
                BatchId = "batch id",
                Id = 500,
                ParticipantId = participant.ParticipantId,
                ParticipantPerson = participantPerson,
                SevisCommStatus = readyToSubmitStatus,
                SevisCommStatusId = readyToSubmitStatus.SevisCommStatusId,
            };
            var todayStatus = new ParticipantPersonSevisCommStatus
            {
                AddedOn = DateTimeOffset.UtcNow,
                BatchId = "batch id",
                Id = 501,
                ParticipantId = participant.ParticipantId,
                ParticipantPerson = participantPerson,
                SevisCommStatus = pendingSendToSevisStatus,
                SevisCommStatusId = pendingSendToSevisStatus.SevisCommStatusId,
            };
            participantPerson.ParticipantPersonSevisCommStatuses.Add(todayStatus);
            participantPerson.ParticipantPersonSevisCommStatuses.Add(yesterdayStatus);

            context.People.Add(person);
            context.SevisCommStatuses.Add(readyToSubmitStatus);
            context.SevisCommStatuses.Add(pendingSendToSevisStatus);
            context.ParticipantPersonSevisCommStatuses.Add(todayStatus);
            context.ParticipantPersonSevisCommStatuses.Add(yesterdayStatus);
            context.ParticipantTypes.Add(participantType);
            context.ParticipantStatuses.Add(status);
            context.Participants.Add(participant);
            context.ParticipantPersons.Add(participantPerson);

            var results = ParticipantPersonsSevisQueries.CreateGetParticipantPersonsSevisDTOQuery(context).ToList();
            Assert.AreEqual(1, results.Count());
            var firstResult = results.First();

            Assert.AreEqual(todayStatus.AddedOn, firstResult.LastBatchDate);
            Assert.AreEqual(pendingSendToSevisStatus.SevisCommStatusId, firstResult.SevisStatusId);
            Assert.AreEqual(pendingSendToSevisStatus.SevisCommStatusName, firstResult.SevisStatus);
        }

        [TestMethod]
        public void TestCreateGetParticipantPersonsSevisDTOQuery_NoStatusesHaveBatchId()
        {
            var person = new Person
            {
                PersonId = 1,
                FullName = "full name"
            };
            var status = new ParticipantStatus
            {
                ParticipantStatusId = 1,
                Status = "status",
            };
            var participantType = new ParticipantType
            {
                IsPerson = true,
                Name = "part type",
                ParticipantTypeId = 90
            };
            var participant = new Participant
            {
                ParticipantId = 1,
                Status = status,
                ParticipantStatusId = status.ParticipantStatusId,
                ProjectId = 250,
                ParticipantTypeId = participantType.ParticipantTypeId,
                ParticipantType = participantType,
                PersonId = person.PersonId,
                Person = person
            };

            var participantPerson = new ParticipantPerson
            {
                ParticipantId = participant.ParticipantId,
                Participant = participant,
                EndDate = DateTimeOffset.UtcNow.AddDays(10.0),
                IsCancelled = true,
                IsDS2019Printed = true,
                IsDS2019SentToTraveler = true,
                IsSentToSevisViaRTI = true,
                IsValidatedViaRTI = true,
                SevisBatchResult = "sevis batch result",
                SevisId = "sevis id",
                SevisValidationResult = "sevis validation result",
                StartDate = DateTimeOffset.UtcNow.AddDays(-10.0)
            };
            participant.ParticipantPerson = participantPerson;

            var readyToSubmitStatus = new SevisCommStatus
            {
                SevisCommStatusId = SevisCommStatus.ReadyToSubmit.Id,
                SevisCommStatusName = SevisCommStatus.ReadyToSubmit.Value
            };
            var pendingSendToSevisStatus = new SevisCommStatus
            {
                SevisCommStatusId = SevisCommStatus.PendingSevisSend.Id,
                SevisCommStatusName = SevisCommStatus.PendingSevisSend.Value
            };
            var yesterdayStatus = new ParticipantPersonSevisCommStatus
            {
                AddedOn = DateTimeOffset.UtcNow.AddDays(-1.0),
                Id = 500,
                ParticipantId = participant.ParticipantId,
                ParticipantPerson = participantPerson,
                SevisCommStatus = readyToSubmitStatus,
                SevisCommStatusId = readyToSubmitStatus.SevisCommStatusId,
            };
            var todayStatus = new ParticipantPersonSevisCommStatus
            {
                AddedOn = DateTimeOffset.UtcNow,
                Id = 501,
                ParticipantId = participant.ParticipantId,
                ParticipantPerson = participantPerson,
                SevisCommStatus = pendingSendToSevisStatus,
                SevisCommStatusId = pendingSendToSevisStatus.SevisCommStatusId,
            };
            participantPerson.ParticipantPersonSevisCommStatuses.Add(todayStatus);
            participantPerson.ParticipantPersonSevisCommStatuses.Add(yesterdayStatus);

            context.SevisCommStatuses.Add(readyToSubmitStatus);
            context.SevisCommStatuses.Add(pendingSendToSevisStatus);
            context.ParticipantPersonSevisCommStatuses.Add(todayStatus);
            context.ParticipantPersonSevisCommStatuses.Add(yesterdayStatus);
            context.ParticipantTypes.Add(participantType);
            context.ParticipantStatuses.Add(status);
            context.Participants.Add(participant);
            context.ParticipantPersons.Add(participantPerson);
            context.People.Add(person);

            var results = ParticipantPersonsSevisQueries.CreateGetParticipantPersonsSevisDTOQuery(context).ToList();
            Assert.AreEqual(1, results.Count());
            var firstResult = results.First();

            Assert.IsNull(firstResult.LastBatchDate);
        }

        [TestMethod]
        public void TestCreateGetParticipantPersonsSevisDTOByIdQuery()
        {
            var person = new Person
            {
                PersonId = 1,
                FullName = "full name"
            };
            var status = new ParticipantStatus
            {
                ParticipantStatusId = 1,
                Status = "status",
            };
            var participantType = new ParticipantType
            {
                IsPerson = true,
                Name = "part type",
                ParticipantTypeId = 90
            };
            var participant = new Participant
            {
                ParticipantId = 1,
                Status = status,
                ParticipantStatusId = status.ParticipantStatusId,
                ProjectId = 250,
                ParticipantTypeId = participantType.ParticipantTypeId,
                ParticipantType = participantType,
                Person = person,
                PersonId = person.PersonId
            };

            var participantPerson = new ParticipantPerson
            {
                ParticipantId = participant.ParticipantId,
                Participant = participant,
                EndDate = DateTimeOffset.UtcNow.AddDays(10.0),
                IsCancelled = true,
                IsDS2019Printed = true,
                IsDS2019SentToTraveler = true,
                IsSentToSevisViaRTI = true,
                IsValidatedViaRTI = true,
                SevisBatchResult = "sevis batch result",
                SevisId = "sevis id",
                SevisValidationResult = "sevis validation result",
                StartDate = DateTimeOffset.UtcNow.AddDays(-10.0)
            };
            participant.ParticipantPerson = participantPerson;

            context.People.Add(person);
            context.ParticipantTypes.Add(participantType);
            context.ParticipantStatuses.Add(status);
            context.Participants.Add(participant);
            context.ParticipantPersons.Add(participantPerson);

            var results = ParticipantPersonsSevisQueries.CreateGetParticipantPersonsSevisDTOByIdQuery(context, participant.ProjectId, participant.ParticipantId).ToList();
            Assert.AreEqual(1, results.Count());
        }

        [TestMethod]
        public void TestCreateGetParticipantPersonsSevisDTOByIdQuery_ParticipantDoesNotBelongToProject()
        {
            var person = new Person
            {
                PersonId = 1,
                FullName = "full name"
            };
            var status = new ParticipantStatus
            {
                ParticipantStatusId = 1,
                Status = "status",
            };
            var participantType = new ParticipantType
            {
                IsPerson = true,
                Name = "part type",
                ParticipantTypeId = 90
            };
            var participant = new Participant
            {
                ParticipantId = 1,
                Status = status,
                ParticipantStatusId = status.ParticipantStatusId,
                ProjectId = 250,
                ParticipantTypeId = participantType.ParticipantTypeId,
                ParticipantType = participantType,
                Person = person,
                PersonId = person.PersonId
            };

            var participantPerson = new ParticipantPerson
            {
                ParticipantId = participant.ParticipantId,
                Participant = participant,
                EndDate = DateTimeOffset.UtcNow.AddDays(10.0),
                IsCancelled = true,
                IsDS2019Printed = true,
                IsDS2019SentToTraveler = true,
                IsSentToSevisViaRTI = true,
                IsValidatedViaRTI = true,
                SevisBatchResult = "sevis batch result",
                SevisId = "sevis id",
                SevisValidationResult = "sevis validation result",
                StartDate = DateTimeOffset.UtcNow.AddDays(-10.0)
            };
            participant.ParticipantPerson = participantPerson;

            context.People.Add(person);
            context.ParticipantTypes.Add(participantType);
            context.ParticipantStatuses.Add(status);
            context.Participants.Add(participant);
            context.ParticipantPersons.Add(participantPerson);

            var results = ParticipantPersonsSevisQueries.CreateGetParticipantPersonsSevisDTOByIdQuery(context, participant.ProjectId, participant.ParticipantId).ToList();
            Assert.AreEqual(1, results.Count());

            results = ParticipantPersonsSevisQueries.CreateGetParticipantPersonsSevisDTOByIdQuery(context, participant.ProjectId + 1, participant.ParticipantId).ToList();
            Assert.AreEqual(0, results.Count());
        }

        [TestMethod]
        public void TestCreateGetParticipantPersonsSevisDTOByIdQuery_ParticipantDoesNotExist()
        {
            var person = new Person
            {
                PersonId = 1,
                FullName = "full name"
            };
            var status = new ParticipantStatus
            {
                ParticipantStatusId = 1,
                Status = "status",
            };
            var participantType = new ParticipantType
            {
                IsPerson = true,
                Name = "part type",
                ParticipantTypeId = 90
            };
            var participant = new Participant
            {
                ParticipantId = 1,
                Status = status,
                ParticipantStatusId = status.ParticipantStatusId,
                ProjectId = 250,
                ParticipantTypeId = participantType.ParticipantTypeId,
                ParticipantType = participantType,
                PersonId = person.PersonId,
                Person = person
            };

            var participantPerson = new ParticipantPerson
            {
                ParticipantId = participant.ParticipantId,
                Participant = participant,
                EndDate = DateTimeOffset.UtcNow.AddDays(10.0),
                IsCancelled = true,
                IsDS2019Printed = true,
                IsDS2019SentToTraveler = true,
                IsSentToSevisViaRTI = true,
                IsValidatedViaRTI = true,
                SevisBatchResult = "sevis batch result",
                SevisId = "sevis id",
                SevisValidationResult = "sevis validation result",
                StartDate = DateTimeOffset.UtcNow.AddDays(-10.0)
            };
            participant.ParticipantPerson = participantPerson;

            context.People.Add(person);
            context.ParticipantTypes.Add(participantType);
            context.ParticipantStatuses.Add(status);
            context.Participants.Add(participant);
            context.ParticipantPersons.Add(participantPerson);

            var results = ParticipantPersonsSevisQueries.CreateGetParticipantPersonsSevisDTOByIdQuery(context, participant.ProjectId, participant.ParticipantId).ToList();
            Assert.AreEqual(1, results.Count());

            results = ParticipantPersonsSevisQueries.CreateGetParticipantPersonsSevisDTOByIdQuery(context, participant.ProjectId, participant.ParticipantId + 1).ToList();
            Assert.AreEqual(0, results.Count());
        }

        [TestMethod]
        public void TestCreateGetParticipantPersonsSevisDTOByIdQuery_ParticipantIsNotAPerson()
        {
            var status = new ParticipantStatus
            {
                ParticipantStatusId = 1,
                Status = "status",
            };
            var participantType = new ParticipantType
            {
                IsPerson = true,
                Name = "part type",
                ParticipantTypeId = 90
            };
            var participant = new Participant
            {
                ParticipantId = 1,
                Status = status,
                ParticipantStatusId = status.ParticipantStatusId,
                ProjectId = 250,
                ParticipantTypeId = participantType.ParticipantTypeId,
                ParticipantType = participantType,
            };
            context.ParticipantTypes.Add(participantType);
            context.ParticipantStatuses.Add(status);
            context.Participants.Add(participant);

            var results = ParticipantPersonsSevisQueries.CreateGetParticipantPersonsSevisDTOByIdQuery(context, participant.ProjectId, participant.ParticipantId).ToList();
            Assert.AreEqual(0, results.Count());
        }

        #endregion

        #region CreateGetParticipantPersonSevisCommStatusesQuery
        [TestMethod]
        public void TestCreateGetParticipantPersonSevisCommStatusesQuery_CheckProperties()
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

            var results = ParticipantPersonsSevisQueries.CreateGetParticipantPersonSevisCommStatusesQuery(context);
            Assert.AreEqual(1, results.Count());
            var firstResult = results.First();
            Assert.AreEqual(status.Id, firstResult.Id);
            Assert.AreEqual(participant.ParticipantId, firstResult.ParticipantId);
            Assert.AreEqual(participant.ProjectId, firstResult.ProjectId);
            Assert.AreEqual(sevisCommStatus.SevisCommStatusId, firstResult.SevisCommStatusId);
            Assert.AreEqual(sevisCommStatus.SevisCommStatusName, firstResult.SevisCommStatusName);
            Assert.AreEqual(status.AddedOn, firstResult.AddedOn);
            Assert.AreEqual(status.BatchId, firstResult.BatchId);
            Assert.AreEqual(status.SevisOrgId, firstResult.SevisOrgId);
            Assert.AreEqual(userAccount.EmailAddress, firstResult.EmailAddress);
            Assert.AreEqual(userAccount.DisplayName, firstResult.DisplayName);
            Assert.AreEqual(userAccount.PrincipalId, firstResult.PrincipalId);
        }

        [TestMethod]
        public void TestCreateGetParticipantPersonSevisCommStatusesQuery_DoesNotHaveUserAccount()
        {
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
                PrincipalId = null,
                SevisCommStatus = sevisCommStatus,
                SevisCommStatusId = sevisCommStatus.SevisCommStatusId,
                SevisOrgId = "sevis org Id",
                SevisUsername = "sevis username"
            };
            context.Participants.Add(participant);
            context.ParticipantPersons.Add(participantPerson);
            context.SevisCommStatuses.Add(sevisCommStatus);
            context.ParticipantPersonSevisCommStatuses.Add(status);

            var results = ParticipantPersonsSevisQueries.CreateGetParticipantPersonSevisCommStatusesQuery(context);
            Assert.AreEqual(1, results.Count());
            var firstResult = results.First();
            Assert.IsNull(firstResult.EmailAddress);
            Assert.IsNull(firstResult.DisplayName);
            Assert.IsNull(firstResult.PrincipalId);
        }

        [TestMethod]
        public void TestCreateGetParticipantPersonSevisCommStatusesByParticipantIdQuery()
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
            var results = ParticipantPersonsSevisQueries.CreateGetParticipantPersonSevisCommStatusesByParticipantIdQuery(context, participant.ProjectId, participant.ParticipantId, queryOperator);
            Assert.AreEqual(1, results.Count());
            var firstResult = results.First();
            Assert.AreEqual(status.Id, firstResult.Id);
        }

        [TestMethod]
        public void TestCreateGetParticipantPersonSevisCommStatusesByParticipantIdQuery_ParticipantDoesNotExist()
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
            var results = ParticipantPersonsSevisQueries.CreateGetParticipantPersonSevisCommStatusesByParticipantIdQuery(context, participant.ProjectId, participant.ParticipantId, queryOperator);
            Assert.AreEqual(1, results.Count());
            var firstResult = results.First();
            Assert.AreEqual(status.Id, firstResult.Id);

            results = ParticipantPersonsSevisQueries.CreateGetParticipantPersonSevisCommStatusesByParticipantIdQuery(context, participant.ProjectId, participant.ParticipantId + 1, queryOperator);
            Assert.AreEqual(0, results.Count());
        }

        [TestMethod]
        public void TestCreateGetParticipantPersonSevisCommStatusesByParticipantIdQuery_ProjectDoesNotExist()
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
            var results = ParticipantPersonsSevisQueries.CreateGetParticipantPersonSevisCommStatusesByParticipantIdQuery(context, participant.ProjectId, participant.ParticipantId, queryOperator);
            Assert.AreEqual(1, results.Count());
            var firstResult = results.First();
            Assert.AreEqual(status.Id, firstResult.Id);

            results = ParticipantPersonsSevisQueries.CreateGetParticipantPersonSevisCommStatusesByParticipantIdQuery(context, participant.ProjectId + 1, participant.ParticipantId, queryOperator);
            Assert.AreEqual(0, results.Count());
        }

        [TestMethod]
        public void TestCreateGetParticipantPersonSevisCommStatusesByParticipantIdQuery_Filtered()
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
            var filter = new ExpressionFilter<ParticipantPersonSevisCommStatusDTO>(x => x.BatchId, ComparisonType.Equal, status.BatchId);
            var queryOperator = new QueryableOperator<ParticipantPersonSevisCommStatusDTO>(0, 1, defaultSorter);
            queryOperator.Filters.Add(filter);

            var results = ParticipantPersonsSevisQueries.CreateGetParticipantPersonSevisCommStatusesByParticipantIdQuery(context, participant.ProjectId, participant.ParticipantId, queryOperator);
            Assert.AreEqual(1, results.Count());
            var firstResult = results.First();
            Assert.AreEqual(status.Id, firstResult.Id);
        }
        #endregion
    }
}
