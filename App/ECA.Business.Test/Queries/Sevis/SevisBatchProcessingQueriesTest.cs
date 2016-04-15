using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Data;
using ECA.Business.Queries.Sevis;
using ECA.Business.Sevis.Model;

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
                UploadDispositionCode = "upload code",
                SevisUsername = "user",
                SevisOrgId = "org",
                UploadTries = 1,
                DownloadTries = 2,
                LastUploadTry = DateTimeOffset.UtcNow.AddDays(-10.0),
                LastDownloadTry = DateTimeOffset.UtcNow.AddDays(-5.0)
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
            Assert.AreEqual(model.SevisUsername, firstResult.SevisUsername);
            Assert.AreEqual(model.SevisOrgId, firstResult.SevisOrgId);
            Assert.AreEqual(model.UploadTries, firstResult.UploadTries);
            Assert.AreEqual(model.DownloadTries, firstResult.DownloadTries);
            Assert.AreEqual(model.LastUploadTry, firstResult.LastUploadTry);
            Assert.AreEqual(model.LastDownloadTry, firstResult.LastDownloadTry);
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
                SevisCommStatusId = SevisCommStatus.QueuedToSubmit.Id,
                SevisOrgId = "org",
                SevisUsername = "user"
            };
            participantPerson.ParticipantPersonSevisCommStatuses.Add(status);

            context.Projects.Add(project);
            context.ParticipantPersons.Add(participantPerson);
            context.Participants.Add(participant);
            context.ParticipantPersonSevisCommStatuses.Add(status);

            var results = SevisBatchProcessingQueries.CreateGetQueuedToSubmitParticipantDTOsQuery(context).ToList();
            Assert.AreEqual(1, results.Count);

            var firstResult = results.First();
            Assert.AreEqual(status.SevisOrgId, firstResult.SevisOrgId);
            Assert.AreEqual(status.SevisUsername, firstResult.SevisUsername);
            Assert.AreEqual(1, firstResult.Participants.Count());
            var firstParticipant = firstResult.Participants.First();
            Assert.AreEqual(participant.ParticipantId, firstParticipant.ParticipantId);
            Assert.AreEqual(project.ProjectId, firstParticipant.ProjectId);
            Assert.AreEqual(participantPerson.SevisId, firstParticipant.SevisId);
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

        #region CreateGetProcessedSevisBatchIdsForDeletionQuery
        [TestMethod]
        public void TestCreateGetProcessedSevisBatchIdsForDeletionQuery_AllCodesSuccess()
        {
            var cutOffDate = DateTime.UtcNow;
            var batch = new SevisBatchProcessing
            {
                Id = 1,
                RetrieveDate = cutOffDate.AddDays(-1.0),
                DownloadDispositionCode = DispositionCode.Success.Code,
                UploadDispositionCode = DispositionCode.Success.Code,
                ProcessDispositionCode = DispositionCode.Success.Code
            };
            context.SevisBatchProcessings.Add(batch);
            var results = SevisBatchProcessingQueries.CreateGetProcessedSevisBatchIdsForDeletionQuery(context, cutOffDate);
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(batch.Id, results.First());
        }

        [TestMethod]
        public void TestCreateGetProcessedSevisBatchIdsForDeletionQuery_HasSuccessfulUploadAndDownload_HasBusinessValidationProcessCode()
        {
            var cutOffDate = DateTime.UtcNow;
            var batch = new SevisBatchProcessing
            {
                Id = 1,
                RetrieveDate = cutOffDate.AddDays(-1.0),
                DownloadDispositionCode = DispositionCode.Success.Code,
                UploadDispositionCode = DispositionCode.Success.Code,
                ProcessDispositionCode = DispositionCode.BusinessRuleViolations.Code
            };
            context.SevisBatchProcessings.Add(batch);
            var results = SevisBatchProcessingQueries.CreateGetProcessedSevisBatchIdsForDeletionQuery(context, cutOffDate);
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(batch.Id, results.First());
        }

        [TestMethod]
        public void TestCreateGetProcessedSevisBatchIdsForDeletionQuery_DoesNotHaveSuccessfulProcessDispositionCode()
        {
            var cutOffDate = DateTime.UtcNow;
            var batch = new SevisBatchProcessing
            {
                Id = 1,
                RetrieveDate = cutOffDate.AddDays(-1.0),
                DownloadDispositionCode = DispositionCode.Success.Code,
                UploadDispositionCode = DispositionCode.Success.Code,
                ProcessDispositionCode = DispositionCode.BatchNeverSubmitted.Code
            };
            context.SevisBatchProcessings.Add(batch);
            var results = SevisBatchProcessingQueries.CreateGetProcessedSevisBatchIdsForDeletionQuery(context, cutOffDate);
            Assert.AreEqual(0, results.Count());
        }

        [TestMethod]
        public void TestCreateGetProcessedSevisBatchIdsForDeletionQuery_DoesNotHaveSuccessfulUploadDispositionCode()
        {
            var cutOffDate = DateTime.UtcNow;
            var batch = new SevisBatchProcessing
            {
                Id = 1,
                RetrieveDate = cutOffDate.AddDays(-1.0),
                DownloadDispositionCode = DispositionCode.Success.Code,
                UploadDispositionCode = DispositionCode.BatchNeverSubmitted.Code,
                ProcessDispositionCode = DispositionCode.Success.Code
            };
            context.SevisBatchProcessings.Add(batch);
            var results = SevisBatchProcessingQueries.CreateGetProcessedSevisBatchIdsForDeletionQuery(context, cutOffDate);
            Assert.AreEqual(0, results.Count());
        }

        [TestMethod]
        public void TestCreateGetProcessedSevisBatchIdsForDeletionQuery_DoesNotHaveSuccessfulDownloadDispositionCode()
        {
            var cutOffDate = DateTime.UtcNow;
            var batch = new SevisBatchProcessing
            {
                Id = 1,
                RetrieveDate = cutOffDate.AddDays(-1.0),
                DownloadDispositionCode = DispositionCode.BatchNeverSubmitted.Code,
                UploadDispositionCode = DispositionCode.Success.Code,
                ProcessDispositionCode = DispositionCode.Success.Code
            };
            context.SevisBatchProcessings.Add(batch);
            var results = SevisBatchProcessingQueries.CreateGetProcessedSevisBatchIdsForDeletionQuery(context, cutOffDate);
            Assert.AreEqual(0, results.Count());
        }

        [TestMethod]
        public void TestCreateGetProcessedSevisBatchIdsForDeletionQuery_ProcessDispositionCodeIsNull()
        {
            var cutOffDate = DateTime.UtcNow;
            var batch = new SevisBatchProcessing
            {
                Id = 1,
                RetrieveDate = cutOffDate.AddDays(-1.0),
                DownloadDispositionCode = DispositionCode.Success.Code,
                UploadDispositionCode = DispositionCode.Success.Code,
                ProcessDispositionCode = null
            };
            context.SevisBatchProcessings.Add(batch);
            var results = SevisBatchProcessingQueries.CreateGetProcessedSevisBatchIdsForDeletionQuery(context, cutOffDate);
            Assert.AreEqual(0, results.Count());
        }

        [TestMethod]
        public void TestCreateGetProcessedSevisBatchIdsForDeletionQuery_UploadDispositionCodeIsNull()
        {
            var cutOffDate = DateTime.UtcNow;
            var batch = new SevisBatchProcessing
            {
                Id = 1,
                RetrieveDate = cutOffDate.AddDays(-1.0),
                DownloadDispositionCode = DispositionCode.Success.Code,
                UploadDispositionCode = null,
                ProcessDispositionCode = DispositionCode.Success.Code
            };
            context.SevisBatchProcessings.Add(batch);
            var results = SevisBatchProcessingQueries.CreateGetProcessedSevisBatchIdsForDeletionQuery(context, cutOffDate);
            Assert.AreEqual(0, results.Count());
        }

        [TestMethod]
        public void TestCreateGetProcessedSevisBatchIdsForDeletionQuery_DownloadDispositionCodeIsNull()
        {
            var cutOffDate = DateTime.UtcNow;
            var batch = new SevisBatchProcessing
            {
                Id = 1,
                RetrieveDate = cutOffDate.AddDays(-1.0),
                DownloadDispositionCode = null,
                UploadDispositionCode = DispositionCode.Success.Code,
                ProcessDispositionCode = DispositionCode.Success.Code
            };
            context.SevisBatchProcessings.Add(batch);
            var results = SevisBatchProcessingQueries.CreateGetProcessedSevisBatchIdsForDeletionQuery(context, cutOffDate);
            Assert.AreEqual(0, results.Count());
        }

        [TestMethod]
        public void TestCreateGetProcessedSevisBatchIdsForDeletionQuery_SevisBatchIsAfterCutoffDate()
        {
            var cutOffDate = DateTime.UtcNow;
            var batch = new SevisBatchProcessing
            {
                Id = 1,
                RetrieveDate = cutOffDate.AddDays(1.0),
                DownloadDispositionCode = DispositionCode.Success.Code,
                UploadDispositionCode = DispositionCode.Success.Code,
                ProcessDispositionCode = DispositionCode.Success.Code
            };
            context.SevisBatchProcessings.Add(batch);
            var results = SevisBatchProcessingQueries.CreateGetProcessedSevisBatchIdsForDeletionQuery(context, cutOffDate);
            Assert.AreEqual(0, results.Count());
        }
        #endregion
    }
}
