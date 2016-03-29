using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Data;
using ECA.Business.Queries.Sevis;

namespace ECA.Business.Test.Queries.Sevis
{
    [TestClass]
    public class SevisBatchProcessingQueriesTest
    {
        private TestEcaContext context;

        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
        }

        #region SevisBatchProcessingDTOs
        [TestMethod]
        public void TestCreateGetSevisBatchProcessingDTOQuery()
        {
            var model = new SevisBatchProcessing
            {
                BatchId = "batch id",
                DownloadDispositionCode = "download code",
                Id = 1,
                ProcessDispositionCode = "process code",
                RetrieveDate = DateTimeOffset.UtcNow.AddDays(1.0),
                SendString = "send string",
                SubmitDate = DateTimeOffset.UtcNow.AddDays(2.0),
                TransactionLogString = "transaction log",
                UploadDispositionCode = "upload code"
            };
            context.SevisBatchProcessings.Add(model);

            var results = SevisBatchProcessingQueries.CreateGetSevisBatchProcessingDTOQuery(context).ToList();
            Assert.AreEqual(1, results.Count);

            var firstResult = results.First();
            Assert.AreEqual(model.BatchId, firstResult.BatchId);
            Assert.AreEqual(model.DownloadDispositionCode, firstResult.DownloadDispositionCode);
            Assert.AreEqual(model.Id, firstResult.Id);
            Assert.AreEqual(model.ProcessDispositionCode, firstResult.ProcessDispositionCode);
            Assert.AreEqual(model.RetrieveDate, firstResult.RetrieveDate);
            Assert.AreEqual(model.SendString, firstResult.SendString);
            Assert.AreEqual(model.SubmitDate, firstResult.SubmitDate);
            Assert.AreEqual(model.TransactionLogString, firstResult.TransactionLogString);
            Assert.AreEqual(model.UploadDispositionCode, firstResult.UploadDispositionCode);
        }
        #endregion
        
        #region CreateGetSevisBatchProcessingDTOsToUploadQuery
        [TestMethod]
        public void TestCreateGetSevisBatchProcessingDTOsToUploadQuery_DoesNotHaveSubmitDate()
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

            var results = SevisBatchProcessingQueries.CreateGetSevisBatchProcessingDTOQuery(context).ToList();
            Assert.AreEqual(1, results.Count);

            var firstResult = results.First();
            Assert.AreEqual(model.BatchId, firstResult.BatchId);
        }

        [TestMethod]
        public void TestCreateGetSevisBatchProcessingDTOsToUploadQuery_HasSubmitDate()
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

            var results = SevisBatchProcessingQueries.CreateGetSevisBatchProcessingDTOQuery(context).ToList();
            Assert.AreEqual(1, results.Count);
        }
        #endregion

        #region CreateGetQueuedToSubmitParticipantDTOsQuery
        [TestMethod]
        public void CreateGetQueuedToSubmitParticipantDTOsQuery_CheckProperties()
        {
            var project = new Project
            {
                ProjectId = 1
            };
            var participant = new Participant
            {
                ParticipantId = 10,
                ProjectId = project.ProjectId,
                Project = project
            };
            var participantPerson = new ParticipantPerson
            {
                ParticipantId = participant.ParticipantId,
                Participant = participant,
                SevisId = "sevis id"
            };
            participant.ParticipantPerson = participantPerson;

            var status = new ParticipantPersonSevisCommStatus
            {
                Id = 1,
                AddedOn = DateTimeOffset.Now,
                ParticipantId = participant.ParticipantId,
                ParticipantPerson = participantPerson,
                SevisCommStatusId = SevisCommStatus.QueuedToSubmit.Id
            };
            participantPerson.ParticipantPersonSevisCommStatuses.Add(status);

            context.Projects.Add(project);
            context.ParticipantPersons.Add(participantPerson);
            context.Participants.Add(participant);
            context.ParticipantPersonSevisCommStatuses.Add(status);

            var results = SevisBatchProcessingQueries.CreateGetQueuedToSubmitParticipantDTOsQuery(context).ToList();
            Assert.AreEqual(1, results.Count);

            var firstResult = results.First();
            Assert.AreEqual(project.ProjectId, firstResult.ProjectId);
            Assert.AreEqual(participant.ParticipantId, firstResult.ParticipantId);
            Assert.AreEqual(participantPerson.SevisId, firstResult.SevisId);
        }

        [TestMethod]
        public void CreateGetQueuedToSubmitParticipantDTOsQuery_CheckLatestSevisCommStatus()
        {
            var project = new Project
            {
                ProjectId = 1
            };
            var participant = new Participant
            {
                ParticipantId = 10,
                ProjectId = project.ProjectId,
                Project = project
            };
            var participantPerson = new ParticipantPerson
            {
                ParticipantId = participant.ParticipantId,
                Participant = participant,
                SevisId = "sevis id"
            };
            participant.ParticipantPerson = participantPerson;

            var status1 = new ParticipantPersonSevisCommStatus
            {
                Id = 1,
                AddedOn = DateTimeOffset.Now.AddDays(-1.0),
                ParticipantId = participant.ParticipantId,
                ParticipantPerson = participantPerson,
                SevisCommStatusId = SevisCommStatus.ReadyToSubmit.Id
            };
            var status2 = new ParticipantPersonSevisCommStatus
            {
                Id = 2,
                AddedOn = DateTimeOffset.Now.AddDays(1.0),
                ParticipantId = participant.ParticipantId,
                ParticipantPerson = participantPerson,
                SevisCommStatusId = SevisCommStatus.QueuedToSubmit.Id
            };
            participantPerson.ParticipantPersonSevisCommStatuses.Add(status1);
            participantPerson.ParticipantPersonSevisCommStatuses.Add(status2);

            context.Projects.Add(project);
            context.ParticipantPersons.Add(participantPerson);
            context.Participants.Add(participant);
            context.ParticipantPersonSevisCommStatuses.Add(status1);

            var results = SevisBatchProcessingQueries.CreateGetQueuedToSubmitParticipantDTOsQuery(context).ToList();
            Assert.AreEqual(1, results.Count);
        }

        [TestMethod]
        public void CreateGetQueuedToSubmitParticipantDTOsQuery_IsNotQueuedToSubmit()
        {
            var project = new Project
            {
                ProjectId = 1
            };
            var participant = new Participant
            {
                ParticipantId = 10,
                ProjectId = project.ProjectId,
                Project = project
            };
            var participantPerson = new ParticipantPerson
            {
                ParticipantId = participant.ParticipantId,
                Participant = participant,
                SevisId = "sevis id"
            };
            participant.ParticipantPerson = participantPerson;

            var status = new ParticipantPersonSevisCommStatus
            {
                Id = 1,
                AddedOn = DateTimeOffset.Now,
                ParticipantId = participant.ParticipantId,
                ParticipantPerson = participantPerson,
                SevisCommStatusId = SevisCommStatus.ReadyToSubmit.Id
            };
            participantPerson.ParticipantPersonSevisCommStatuses.Add(status);

            context.Projects.Add(project);
            context.ParticipantPersons.Add(participantPerson);
            context.Participants.Add(participant);
            context.ParticipantPersonSevisCommStatuses.Add(status);

            var results = SevisBatchProcessingQueries.CreateGetQueuedToSubmitParticipantDTOsQuery(context).ToList();
            Assert.AreEqual(0, results.Count);
        }
        #endregion

        #region CreateGetParticipantPersonsByBatchId
        [TestMethod]
        public void TestCreateGetParticipantPersonsByBatchId()
        {
            var batchId = "batchId";
            var participantPerson = new ParticipantPerson
            {
                ParticipantId = 1
            };
            var readyToSubmit = new ParticipantPersonSevisCommStatus
            {
                Id = 1,
                SevisCommStatusId = SevisCommStatus.ReadyToSubmit.Id,
                ParticipantPerson = participantPerson,
                ParticipantId = participantPerson.ParticipantId
            };
            var queuedToSubmit = new ParticipantPersonSevisCommStatus
            {
                Id = 2,
                SevisCommStatusId = SevisCommStatus.QueuedToSubmit.Id,
                ParticipantPerson = participantPerson,
                ParticipantId = participantPerson.ParticipantId
            };
            var pendingSevisSend = new ParticipantPersonSevisCommStatus
            {
                Id = 3,
                SevisCommStatusId = SevisCommStatus.PendingSevisSend.Id,
                ParticipantPerson = participantPerson,
                ParticipantId = participantPerson.ParticipantId,
                BatchId = batchId
            };
            participantPerson.ParticipantPersonSevisCommStatuses.Add(readyToSubmit);
            participantPerson.ParticipantPersonSevisCommStatuses.Add(queuedToSubmit);
            participantPerson.ParticipantPersonSevisCommStatuses.Add(pendingSevisSend);

            context.ParticipantPersons.Add(participantPerson);
            context.ParticipantPersonSevisCommStatuses.Add(readyToSubmit);
            context.ParticipantPersonSevisCommStatuses.Add(queuedToSubmit);
            context.ParticipantPersonSevisCommStatuses.Add(pendingSevisSend);
            var results = SevisBatchProcessingQueries.CreateGetParticipantPersonsByBatchId(context, batchId).ToList();
            Assert.AreEqual(1, results.Count);
            Assert.IsTrue(Object.ReferenceEquals(participantPerson, results.First()));
        }

        [TestMethod]
        public void TestCreateGetParticipantPersonsByBatchId_EnsureDistinct()
        {
            var batchId = "batchId";
            var participantPerson = new ParticipantPerson
            {
                ParticipantId = 1
            };
            var readyToSubmit = new ParticipantPersonSevisCommStatus
            {
                Id = 1,
                SevisCommStatusId = SevisCommStatus.ReadyToSubmit.Id,
                ParticipantPerson = participantPerson,
                ParticipantId = participantPerson.ParticipantId
            };
            var queuedToSubmit = new ParticipantPersonSevisCommStatus
            {
                Id = 2,
                SevisCommStatusId = SevisCommStatus.QueuedToSubmit.Id,
                ParticipantPerson = participantPerson,
                ParticipantId = participantPerson.ParticipantId
            };
            var pendingSevisSend = new ParticipantPersonSevisCommStatus
            {
                Id = 3,
                SevisCommStatusId = SevisCommStatus.PendingSevisSend.Id,
                ParticipantPerson = participantPerson,
                ParticipantId = participantPerson.ParticipantId,
                BatchId = batchId
            };
            var otherBatchCommStatus = new ParticipantPersonSevisCommStatus
            {
                Id = 3,
                SevisCommStatusId = SevisCommStatus.SentToDhs.Id,
                ParticipantPerson = participantPerson,
                ParticipantId = participantPerson.ParticipantId,
                BatchId = batchId
            };
            participantPerson.ParticipantPersonSevisCommStatuses.Add(readyToSubmit);
            participantPerson.ParticipantPersonSevisCommStatuses.Add(queuedToSubmit);
            participantPerson.ParticipantPersonSevisCommStatuses.Add(pendingSevisSend);
            participantPerson.ParticipantPersonSevisCommStatuses.Add(otherBatchCommStatus);

            context.ParticipantPersons.Add(participantPerson);
            context.ParticipantPersonSevisCommStatuses.Add(readyToSubmit);
            context.ParticipantPersonSevisCommStatuses.Add(queuedToSubmit);
            context.ParticipantPersonSevisCommStatuses.Add(pendingSevisSend);
            context.ParticipantPersonSevisCommStatuses.Add(otherBatchCommStatus);
            var results = SevisBatchProcessingQueries.CreateGetParticipantPersonsByBatchId(context, batchId).ToList();
            Assert.AreEqual(1, results.Count);
            Assert.IsTrue(Object.ReferenceEquals(participantPerson, results.First()));
        }

        #endregion
    }
}
