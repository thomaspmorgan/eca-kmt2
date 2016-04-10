using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Data;
using ECA.Business.Queries.Persons;

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

        [TestMethod]
        public void TestCreateGetParticipantPersonsSevisDTOQuery_CheckProperties()
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
                ParticipantType = participantType
            };

            var participantPerson = new ParticipantPerson
            {
                ParticipantId = participant.ParticipantId,
                Participant = participant,
                EndDate = DateTimeOffset.UtcNow.AddDays(10.0),
                IsCancelled = true,
                IsDS2019Printed = true,
                IsDS2019SentToTraveler = true,
                IsNeedsUpdate = true,
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
            Assert.AreEqual(participantPerson.IsNeedsUpdate, firstResult.IsNeedsUpdate);
            Assert.AreEqual(participantPerson.IsSentToSevisViaRTI, firstResult.IsSentToSevisViaRTI);
            Assert.AreEqual(participantPerson.IsValidatedViaRTI, firstResult.IsValidatedViaRTI);
            Assert.AreEqual(participantPerson.SevisBatchResult, firstResult.SevisBatchResult);
            Assert.AreEqual(participantPerson.SevisId, firstResult.SevisId);
            Assert.AreEqual(participantPerson.EndDate, firstResult.EndDate);
            Assert.AreEqual(participantPerson.StartDate, firstResult.StartDate);

            Assert.AreEqual(0, firstResult.SevisCommStatuses.Count());
            Assert.AreEqual(ParticipantPersonsSevisQueries.NONE_SEVIS_COMM_STATUS_NAME, firstResult.SevisStatus);
            Assert.AreEqual(ParticipantPersonsSevisQueries.NONE_SEVIS_COMM_STATUS_ID, firstResult.SevisStatusId);
            Assert.IsNull(firstResult.LastBatchDate);
        }

        [TestMethod]
        public void TestCreateGetParticipantPersonsSevisDTOQuery_HasOneCommStatus()
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
                ParticipantType = participantType
            };

            var participantPerson = new ParticipantPerson
            {
                ParticipantId = participant.ParticipantId,
                Participant = participant,
                EndDate = DateTimeOffset.UtcNow.AddDays(10.0),
                IsCancelled = true,
                IsDS2019Printed = true,
                IsDS2019SentToTraveler = true,
                IsNeedsUpdate = true,
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
            var readyToSubmitParticipantSevisCommStatus = new ParticipantPersonSevisCommStatus
            {
                AddedOn = DateTimeOffset.UtcNow,
                BatchId = "batch id",
                Id = 500,
                ParticipantId = participant.ParticipantId,
                ParticipantPerson = participantPerson,
                SevisCommStatus = readyToSubmitStatus,
                SevisCommStatusId = readyToSubmitStatus.SevisCommStatusId,
                SevisOrgId = "sevis org id",
                SevisUsername = "sevis username"
            };
            participantPerson.ParticipantPersonSevisCommStatuses.Add(readyToSubmitParticipantSevisCommStatus);

            context.SevisCommStatuses.Add(readyToSubmitStatus);
            context.ParticipantPersonSevisCommStatuses.Add(readyToSubmitParticipantSevisCommStatus);
            context.ParticipantTypes.Add(participantType);
            context.ParticipantStatuses.Add(status);
            context.Participants.Add(participant);
            context.ParticipantPersons.Add(participantPerson);

            var results = ParticipantPersonsSevisQueries.CreateGetParticipantPersonsSevisDTOQuery(context).ToList();
            Assert.AreEqual(1, results.Count());
            var firstResult = results.First();

            Assert.AreEqual(readyToSubmitParticipantSevisCommStatus.AddedOn, firstResult.LastBatchDate);
            Assert.AreEqual(readyToSubmitStatus.SevisCommStatusId, firstResult.SevisStatusId);
            Assert.AreEqual(readyToSubmitStatus.SevisCommStatusName, firstResult.SevisStatus);

            Assert.AreEqual(1, firstResult.SevisCommStatuses.Count());
            var dto = firstResult.SevisCommStatuses.First();
            Assert.AreEqual(readyToSubmitParticipantSevisCommStatus.AddedOn, dto.AddedOn);
            Assert.AreEqual(readyToSubmitParticipantSevisCommStatus.BatchId, dto.BatchId);
            Assert.AreEqual(readyToSubmitParticipantSevisCommStatus.Id, dto.Id);
            Assert.AreEqual(readyToSubmitParticipantSevisCommStatus.ParticipantId, dto.ParticipantId);
            Assert.AreEqual(readyToSubmitParticipantSevisCommStatus.SevisCommStatus.SevisCommStatusName, dto.SevisCommStatusName);
            Assert.AreEqual(readyToSubmitParticipantSevisCommStatus.SevisOrgId, dto.SevisOrgId);
            Assert.AreEqual(readyToSubmitParticipantSevisCommStatus.SevisUsername, dto.SevisUsername);
        }

        [TestMethod]
        public void TestCreateGetParticipantPersonsSevisDTOQuery_CheckUsesLatestSevisCommStatus()
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
                ParticipantType = participantType
            };

            var participantPerson = new ParticipantPerson
            {
                ParticipantId = participant.ParticipantId,
                Participant = participant,
                EndDate = DateTimeOffset.UtcNow.AddDays(10.0),
                IsCancelled = true,
                IsDS2019Printed = true,
                IsDS2019SentToTraveler = true,
                IsNeedsUpdate = true,
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
                ParticipantType = participantType
            };

            var participantPerson = new ParticipantPerson
            {
                ParticipantId = participant.ParticipantId,
                Participant = participant,
                EndDate = DateTimeOffset.UtcNow.AddDays(10.0),
                IsCancelled = true,
                IsDS2019Printed = true,
                IsDS2019SentToTraveler = true,
                IsNeedsUpdate = true,
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

            var results = ParticipantPersonsSevisQueries.CreateGetParticipantPersonsSevisDTOQuery(context).ToList();
            Assert.AreEqual(1, results.Count());
            var firstResult = results.First();

            Assert.IsNull(firstResult.LastBatchDate);
        }

        [TestMethod]
        public void TestCreateGetParticipantPersonsSevisDTOByIdQuery()
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
                ParticipantType = participantType
            };

            var participantPerson = new ParticipantPerson
            {
                ParticipantId = participant.ParticipantId,
                Participant = participant,
                EndDate = DateTimeOffset.UtcNow.AddDays(10.0),
                IsCancelled = true,
                IsDS2019Printed = true,
                IsDS2019SentToTraveler = true,
                IsNeedsUpdate = true,
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

            var results = ParticipantPersonsSevisQueries.CreateGetParticipantPersonsSevisDTOByIdQuery(context, participant.ProjectId, participant.ParticipantId).ToList();
            Assert.AreEqual(1, results.Count());
        }

        [TestMethod]
        public void TestCreateGetParticipantPersonsSevisDTOByIdQuery_ParticipantDoesNotBelongToProject()
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
                ParticipantType = participantType
            };

            var participantPerson = new ParticipantPerson
            {
                ParticipantId = participant.ParticipantId,
                Participant = participant,
                EndDate = DateTimeOffset.UtcNow.AddDays(10.0),
                IsCancelled = true,
                IsDS2019Printed = true,
                IsDS2019SentToTraveler = true,
                IsNeedsUpdate = true,
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

            var results = ParticipantPersonsSevisQueries.CreateGetParticipantPersonsSevisDTOByIdQuery(context, participant.ProjectId, participant.ParticipantId).ToList();
            Assert.AreEqual(1, results.Count());

            results = ParticipantPersonsSevisQueries.CreateGetParticipantPersonsSevisDTOByIdQuery(context, participant.ProjectId + 1, participant.ParticipantId).ToList();
            Assert.AreEqual(0, results.Count());
        }

        [TestMethod]
        public void TestCreateGetParticipantPersonsSevisDTOByIdQuery_ParticipantDoesNotExist()
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
                ParticipantType = participantType
            };

            var participantPerson = new ParticipantPerson
            {
                ParticipantId = participant.ParticipantId,
                Participant = participant,
                EndDate = DateTimeOffset.UtcNow.AddDays(10.0),
                IsCancelled = true,
                IsDS2019Printed = true,
                IsDS2019SentToTraveler = true,
                IsNeedsUpdate = true,
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

            var results = ParticipantPersonsSevisQueries.CreateGetParticipantPersonsSevisDTOByIdQuery(context, participant.ProjectId, participant.ParticipantId).ToList();
            Assert.AreEqual(1, results.Count());

            results = ParticipantPersonsSevisQueries.CreateGetParticipantPersonsSevisDTOByIdQuery(context, participant.ProjectId, participant.ParticipantId + 1).ToList();
            Assert.AreEqual(0, results.Count());
        }
    }
}
