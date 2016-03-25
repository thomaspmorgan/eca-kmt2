using ECA.Business.Queries.Sevis;
using ECA.Business.Queries.Models.Sevis;
using ECA.Core.Exceptions;
using ECA.Core.Service;
using ECA.Data;
using NLog;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json;
using ECA.Business.Service.Persons;
using ECA.Business.Sevis.Model;
using ECA.Business.Validation.Sevis;
using ECA.Business.Sevis.Model.TransLog;
using System.IO;
using System.Xml.Serialization;
using FluentValidation;
using FluentValidation.Results;

namespace ECA.Business.Service.Sevis
{
    /// <summary>
    /// The SevisBatchProcessingService is capable of handling crud operations on a SevisBatchProcessingService record
    /// and its phone number.
    /// </summary>
    public class SevisBatchProcessingService : DbContextService<EcaContext>, ISevisBatchProcessingService
    {
        /// <summary>
        /// The number of queued to submit participants to page.
        /// </summary>
        private const int QUERY_BATCH_SIZE = 50;

        private ISevisBatchProcessingNotificationService notificationService;
        private IExchangeVisitorService exchangeVisitorService;
        private IExchangeVisitorValidationService exchangeVisitorValidationService;
        private string sevisOrgId;
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly Action<SevisBatchProcessing, object> throwIfSevisBatchProcessingNotFound;
        private readonly Func<List<StagedSevisBatch>, User, StagedSevisBatch> getNewStagedSevisBatch;

        /// <summary>
        /// Creates a new instance and initializes the context..
        /// </summary>
        /// <param name="context">The context to operate against.</param>
        /// <param name="exchangeVisitorService">The exchange visitor service.</param>
        /// <param name="queryBatchSize">The number of ready to submit participants to query in a batch. </param>
        /// <param name="participantBatchSize">The number of participants to process in a sevis batch.</param>
        /// <param name="sevisOrgId">The organization id to send in a sevis batch.</param>
        /// <param name="saveActions">The save actions.</param>
        public SevisBatchProcessingService(
            EcaContext context,
            IExchangeVisitorService exchangeVisitorService,
            ISevisBatchProcessingNotificationService notificationService,
            IExchangeVisitorValidationService exchangeVisitorValidationService,
            string sevisOrgId,
            int maxCreateExchangeVisitorRecordsPerBatch = StagedSevisBatch.MAX_CREATE_EXCHANGE_VISITOR_RECORDS_PER_BATCH_DEFAULT,
            int maxUpdateExchangeVisitorRecordsPerBatch = StagedSevisBatch.MAX_UPDATE_EXCHANGE_VISITOR_RECORD_PER_BATCH_DEFAULT,
            List<ISaveAction> saveActions = null)
            : base(context, saveActions)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(exchangeVisitorService != null, "The exchange visitor service must not be null.");
            Contract.Requires(sevisOrgId != null, "The sevis org id must not be null.");
            Contract.Requires(notificationService != null, "The notification service must not be null.");
            throwIfSevisBatchProcessingNotFound = (sevisBatchProcessing, batchId) =>
            {
                if (sevisBatchProcessing == null)
                {
                    throw new ModelNotFoundException(String.Format("The SEVIS batch processing record with the batch id [{0}] was not found.", batchId));
                }
            };
            getNewStagedSevisBatch = (batches, user) =>
            {
                var stagedSevisBatch = GetNewStagedSevisBatch(user);
                batches.Add(stagedSevisBatch);
                return stagedSevisBatch;
            };
            this.notificationService = notificationService;
            this.exchangeVisitorService = exchangeVisitorService;
            this.exchangeVisitorValidationService = exchangeVisitorValidationService;
            this.sevisOrgId = sevisOrgId;
            this.MaxCreateExchangeVisitorRecordsPerBatch = maxCreateExchangeVisitorRecordsPerBatch;
            this.MaxUpdateExchangeVisitorRecordsPerBatch = maxUpdateExchangeVisitorRecordsPerBatch;

        }

        /// <summary>
        /// Gets the maximum number of records to place in the sevis create exchange visitor array before serialization.
        /// </summary>
        public int MaxCreateExchangeVisitorRecordsPerBatch { get; private set; }

        /// <summary>
        /// Gets the maximum number of records to place in the sevis update exchange visitor array before serialization.
        /// </summary>
        public int MaxUpdateExchangeVisitorRecordsPerBatch { get; private set; }

        #region Get
        /// <summary>
        /// Returns the next batch record to send to sevis.
        /// </summary>
        /// <returns>Gets the next batch record to send to sevis.</returns>
        public SevisBatchProcessingDTO GetNextBatchToUpload()
        {
            return CreateGetNextBatchRecordToUploadQuery().FirstOrDefault();
        }

        /// <summary>
        /// Returns the next batch record to send to sevis.
        /// </summary>
        /// <returns>Gets the next batch record to send to sevis.</returns>
        public Task<SevisBatchProcessingDTO> GetNextBatchToUploadAsync()
        {
            return CreateGetNextBatchRecordToUploadQuery().FirstOrDefaultAsync();
        }

        private IQueryable<SevisBatchProcessingDTO> CreateGetNextBatchRecordToUploadQuery()
        {
            return SevisBatchProcessingQueries.CreateGetSevisBatchProcessingDTOsToUploadQuery(this.Context).OrderBy(x => x.Id);
        }

        /// <summary>
        /// Returns the batch id of the next batch to download from the sevis api.
        /// </summary>
        /// <returns>The batch id of the next batch to download from sevis.</returns>
        public string GetNextBatchByBatchIdToDownload()
        {
            return CreateGetNextBatchByBatchIdToDownload().FirstOrDefault();
        }

        /// <summary>
        /// Returns the batch id of the next batch to download from the sevis api.
        /// </summary>
        /// <returns>The batch id of the next batch to download from sevis.</returns>
        public Task<string> GetNextBatchByBatchIdToDownloadAsync()
        {
            return CreateGetNextBatchByBatchIdToDownload().FirstOrDefaultAsync();
        }

        private IQueryable<string> CreateGetNextBatchByBatchIdToDownload()
        {
            return SevisBatchProcessingQueries.CreateGetSevisBatchProcessingDTOsToDownloadQuery(this.Context).OrderBy(x => x.Id).Select(x => x.BatchId);
        }

        /// <summary>
        /// Returns the id of the next batch to process or null if there are no other batches to process.
        /// </summary>
        /// <returns>The id of the next batch to process or null if there are no other batches to process.</returns>
        public int? GetNextBatchIdToProcess()
        {
            int? id = CreateGetNextSevisBatchIdToProcessQuery().FirstOrDefault();
            return id;
        }

        /// <summary>
        /// Returns the id of the next batch to process or null if there are no other batches to process.
        /// </summary>
        /// <returns>The id of the next batch to process or null if there are no other batches to process.</returns>
        public async Task<int?> GetNextBatchIdToProcessAsync()
        {
            int? id = await CreateGetNextSevisBatchIdToProcessQuery().FirstOrDefaultAsync();
            return id;
        }
        #endregion

        #region Update

        /// <summary>
        /// Indicates the batch has been successfully uploaded.
        /// </summary>
        /// <param name="id">The id of the batch.</param>
        public void BatchHasBeenSent(int id)
        {
            var batch = this.Context.SevisBatchProcessings.Find(id);
            throwIfSevisBatchProcessingNotFound(batch, id);
            DoBatchHasBeenSent(batch);
        }

        /// <summary>
        /// Indicates the batch has been successfully uploaded.
        /// </summary>
        /// <param name="id">The id of the batch.</param>
        public async Task BatchHasBeenSentAsync(int id)
        {
            var batch = await this.Context.SevisBatchProcessings.FindAsync(id);
            throwIfSevisBatchProcessingNotFound(batch, id);
            DoBatchHasBeenSent(batch);
        }

        private void DoBatchHasBeenSent(SevisBatchProcessing batch)
        {
            batch.SubmitDate = DateTimeOffset.UtcNow;
        }

        /// <summary>
        /// Saves the given transaction log as xml to the appropriate SevisBatchProcessing model.
        /// </summary>
        /// <param name="xml">The transaction log as xml.</param>
        public void BatchHasBeenRetrieved(string xml)
        {
            var transactionLog = DeserializeTransactionLogType(xml);
            var batch = CreateGetSevisBatchProcessingByTransactionLogQuery(transactionLog).FirstOrDefault();
            throwIfSevisBatchProcessingNotFound(batch, transactionLog.BatchHeader.BatchID);
            DoBatchHasBeenRetrieved(xml, transactionLog, batch);
        }

        /// <summary>
        /// Saves the given transaction log as xml to the appropriate SevisBatchProcessing model.
        /// </summary>
        /// <param name="xml">The transaction log as xml.</param>
        public async Task BatchHasBeenRetrievedAsync(string xml)
        {
            var transactionLog = DeserializeTransactionLogType(xml);
            var batch = await CreateGetSevisBatchProcessingByTransactionLogQuery(transactionLog).FirstOrDefaultAsync();
            throwIfSevisBatchProcessingNotFound(batch, transactionLog.BatchHeader.BatchID);
            DoBatchHasBeenRetrieved(xml, transactionLog, batch);
        }

        private void DoBatchHasBeenRetrieved(string xml, TransactionLogType transactionLog, SevisBatchProcessing sevisBatchProcessing)
        {
            sevisBatchProcessing.RetrieveDate = DateTimeOffset.UtcNow;
            sevisBatchProcessing.TransactionLogString = xml;
        }
        #endregion

        #region Process Transaction Log

        public void ProcessTransactionLog(int id)
        {
            var batchToProcess = CreateGetSevisBatchToProcessById(id).FirstOrDefault();
            if (batchToProcess != null)
            {

            }
        }

        public async Task ProcessTransactionLogAsync(int id)
        {
            var batchToProcess = await CreateGetSevisBatchToProcessById(id).FirstOrDefaultAsync();
            if (batchToProcess != null)
            {

            }
        }

        //public void ProcessTransactionLog(string xml)
        //{
        //    var instance = DeserializeTransactionLogType(xml);
        //    ProcessTransactionLog(xml, instance);
        //}

        //public Task ProcessTransactionLogAsync(string xml)
        //{
        //    var instance = DeserializeTransactionLogType(xml);
        //    return ProcessTransactionLogAsync(xml, instance);
        //}

        private TransactionLogType DeserializeTransactionLogType(string xml)
        {
            using (var memoryStream = new MemoryStream())
            using (var stringReader = new StringReader(xml))
            {
                var serializer = new XmlSerializer(typeof(TransactionLogType));
                var transactionLogType = (TransactionLogType)serializer.Deserialize(stringReader);
                return transactionLogType;
            }
        }

        /// <summary>
        /// Update a participant record with sevis batch results
        /// </summary>
        /// <param name="participantID"></param>
        /// <param name="status"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        //private async Task<ParticipantSevisBatchProcessingResultDTO> UpdateParticipant(int participantID, string status, string json)
        //{
        //    var result = new ParticipantSevisBatchProcessingResultDTO();

        //    var participantPersonDTO = await Context.ParticipantPersons.FindAsync(participantID);
        //    participantPersonDTO.SevisBatchResult = json;

        //    result.ParticipantId = participantID;
        //    result.ProjectId = participantPersonDTO.Participant.ProjectId;
        //    if (status == "1")
        //    {
        //        result.SevisCommStatus = SevisCommStatus.BatchRequestSuccessful.Value;
        //    }
        //    else
        //    {
        //        result.SevisCommStatus = SevisCommStatus.BatchRequestUnsuccessful.Value;
        //    }

        //    return result;
        //}

        private IQueryable<SevisBatchProcessing> CreateGetSevisBatchProcessingByTransactionLogQuery(TransactionLogType transactionLog)
        {
            Contract.Requires(transactionLog != null, "The transaction log must not be null.");
            return CreateGetSevisBatchProcessingByBatchIdQuery(transactionLog.BatchHeader.BatchID);
        }

        private IQueryable<SevisBatchProcessing> CreateGetSevisBatchProcessingByBatchIdQuery(string batchId)
        {
            return this.Context.SevisBatchProcessings.Where(x => x.BatchId == batchId);
        }

        private IQueryable<SevisBatchProcessing> CreateGetSevisBatchToProcessQuery()
        {
            return this.Context.SevisBatchProcessings
                .Where(x => x.RetrieveDate.HasValue)
                .Where(x => x.TransactionLogString != null)
                .OrderBy(x => x.BatchId);
        }

        private IQueryable<int?> CreateGetNextSevisBatchIdToProcessQuery()
        {
            return CreateGetSevisBatchToProcessQuery().Select(x => (int?)x.Id);
        }

        private IQueryable<SevisBatchProcessing> CreateGetSevisBatchToProcessById(int id)
        {
            return CreateGetSevisBatchToProcessQuery().Where(x => x.Id == id);
        }

        #endregion

        #region Delete
        /// <summary>
        /// Deletes the SEVIS Batch Processing record with the given id.
        /// </summary>
        /// <param name="batchId">The id of the SEVIS Batch Processing record to delete.</param>
        public void Delete(int batchId)
        {
            var sevisBatchProcessing = Context.SevisBatchProcessings.Find(batchId);
            DoDelete(sevisBatchProcessing);
        }

        /// <summary>
        /// Deletes the SEVIS Batch Processing record with the given id.
        /// </summary>
        /// <param name="phoneNumberId">The id of the SEVIS Batch Processing record to delete.</param>
        public async Task DeleteAsync(int batchId)
        {
            var sevisBatchProcessing = await Context.SevisBatchProcessings.FindAsync(batchId);
            DoDelete(sevisBatchProcessing);
        }

        private void DoDelete(SevisBatchProcessing sevisBatchProcessingToDelete)
        {
            if (sevisBatchProcessingToDelete != null)
            {
                Context.SevisBatchProcessings.Remove(sevisBatchProcessingToDelete);
            }
        }

        #endregion

        #region Staging
        /// <summary>
        /// Stages all queued to submit sevis participants into sevis batches that can then be sent to sevis for processing.
        /// </summary>
        /// <param name="user">The user performing the staging.</param>
        /// <returns>The list of staged sevis batches.</returns>
        public List<StagedSevisBatch> StageBatches(User user)
        {
            var stagedSevisBatches = new List<StagedSevisBatch>();
            var skip = 0;
            var newSevisParticipantsCount = SevisBatchProcessingQueries.CreateGetQueuedToSubmitParticipantDTOsQuery(this.Context).Count();
            notificationService.NotifyNumberOfParticipantsToStage(newSevisParticipantsCount);
            while (newSevisParticipantsCount > 0)
            {
                StagedSevisBatch stagedSevisBatch = null;
                var participants = SevisBatchProcessingQueries.CreateGetQueuedToSubmitParticipantDTOsQuery(this.Context)
                    .Skip(() => skip)
                    .Take(() => QUERY_BATCH_SIZE)
                    .ToList();

                foreach (var participant in participants)
                {
                    var exchangeVisitor = this.exchangeVisitorService.GetExchangeVisitor(user, participant.ProjectId, participant.ParticipantId);
                    var results = exchangeVisitor.Validate(this.exchangeVisitorValidationService.GetValidator());
                    if (results.IsValid)
                    {
                        var accomodatingSevisBatch = GetAccomodatingStagedSevisBatch(stagedSevisBatches, exchangeVisitor);
                        if (accomodatingSevisBatch != null)
                        {
                            stagedSevisBatch = accomodatingSevisBatch;
                        }
                        else
                        {
                            PersistStagedSevisBatches(stagedSevisBatches);
                            stagedSevisBatch = getNewStagedSevisBatch(stagedSevisBatches, user);
                            this.notificationService.NotifyStagedSevisBatchCreated(stagedSevisBatch);
                        }
                        stagedSevisBatch.AddExchangeVisitor(exchangeVisitor);
                        AddPendingSendToSevisStatus(participant.ParticipantId);
                    }
                    else
                    {
                        HandleInvalidExchangeVisitor(user, participant.ProjectId, participant.ParticipantId);
                        notificationService.NotifyInvalidExchangeVisitor(exchangeVisitor);
                    }
                    skip++;
                    newSevisParticipantsCount--;
                }
            }
            PersistStagedSevisBatches(stagedSevisBatches);
            this.notificationService.NotifyStagedSevisBatchesFinished(stagedSevisBatches);
            return stagedSevisBatches;
        }

        /// <summary>
        /// Stages all queued to submit sevis participants into sevis batches that can then be sent to sevis for processing.
        /// </summary>
        /// <param name="user">The user performing the staging.</param>
        /// <returns>The list of staged sevis batches.</returns>
        public async Task<List<StagedSevisBatch>> StageBatchesAsync(User user)
        {
            var stagedSevisBatches = new List<StagedSevisBatch>();
            var skip = 0;
            var newSevisParticipantsCount = await SevisBatchProcessingQueries.CreateGetQueuedToSubmitParticipantDTOsQuery(this.Context).CountAsync();
            notificationService.NotifyNumberOfParticipantsToStage(newSevisParticipantsCount);
            while (newSevisParticipantsCount > 0)
            {
                StagedSevisBatch stagedSevisBatch = null;
                var participants = await SevisBatchProcessingQueries.CreateGetQueuedToSubmitParticipantDTOsQuery(this.Context)
                    .Skip(() => skip)
                    .Take(() => QUERY_BATCH_SIZE)
                    .ToListAsync();

                foreach (var participant in participants)
                {
                    var exchangeVisitor = await this.exchangeVisitorService.GetExchangeVisitorAsync(user, participant.ProjectId, participant.ParticipantId);
                    var results = exchangeVisitor.Validate(this.exchangeVisitorValidationService.GetValidator());
                    if (results.IsValid)
                    {
                        var accomodatingSevisBatch = GetAccomodatingStagedSevisBatch(stagedSevisBatches, exchangeVisitor);
                        if (accomodatingSevisBatch != null)
                        {
                            stagedSevisBatch = accomodatingSevisBatch;
                        }
                        else
                        {
                            await PersistStagedSevisBatchesAsync(stagedSevisBatches);
                            stagedSevisBatch = getNewStagedSevisBatch(stagedSevisBatches, user);
                            this.notificationService.NotifyStagedSevisBatchCreated(stagedSevisBatch);
                        }
                        stagedSevisBatch.AddExchangeVisitor(exchangeVisitor);
                        AddPendingSendToSevisStatus(participant.ParticipantId);
                    }
                    else
                    {
                        await HandleInvalidExchangeVisitorAsync(user, participant.ProjectId, participant.ParticipantId);
                        notificationService.NotifyInvalidExchangeVisitor(exchangeVisitor);
                    }
                    skip++;
                    newSevisParticipantsCount--;
                }
            }
            await PersistStagedSevisBatchesAsync(stagedSevisBatches);
            this.notificationService.NotifyStagedSevisBatchesFinished(stagedSevisBatches);
            return stagedSevisBatches;
        }

        private void HandleInvalidExchangeVisitor(User user, int projectId, int participantId)
        {
            this.exchangeVisitorValidationService.RunParticipantSevisValidation(user, projectId, participantId);
            this.exchangeVisitorValidationService.SaveChanges();
        }

        private async Task HandleInvalidExchangeVisitorAsync(User user, int projectId, int participantId)
        {
            await this.exchangeVisitorValidationService.RunParticipantSevisValidationAsync(user, projectId, participantId);
            await this.exchangeVisitorValidationService.SaveChangesAsync();
        }

        private void PersistStagedSevisBatches(List<StagedSevisBatch> stagedSevisBatches)
        {
            foreach (var stagedSevisBatch in stagedSevisBatches)
            {
                if (!stagedSevisBatch.IsSaved)
                {
                    stagedSevisBatch.SerializeSEVISBatchCreateUpdateEV();
                    this.Context.SaveChanges();
                    stagedSevisBatch.IsSaved = true;
                }
            }
        }

        private async Task PersistStagedSevisBatchesAsync(List<StagedSevisBatch> stagedSevisBatches)
        {
            foreach (var stagedSevisBatch in stagedSevisBatches)
            {
                if (!stagedSevisBatch.IsSaved)
                {
                    stagedSevisBatch.SerializeSEVISBatchCreateUpdateEV();
                    await this.Context.SaveChangesAsync();
                    stagedSevisBatch.IsSaved = true;
                }
            }
        }

        /// <summary>
        /// Returns the first staged sevis batch that can accomodate the exchange visitor, or nulll, if none of the
        /// batches can accomodate the visitor.
        /// </summary>
        /// <param name="batches">The staged sevis batches.</param>
        /// <param name="visitor">The exchange visitor.</param>
        /// <returns>The first staged sevis batch that can accomodate the visitor, or null if none can accomodate.</returns>
        public StagedSevisBatch GetAccomodatingStagedSevisBatch(List<StagedSevisBatch> batches, ExchangeVisitor visitor)
        {
            Contract.Requires(batches != null, "The batches must not be null.");
            Contract.Requires(visitor != null, "The exchange visitor must not be null.");
            foreach (var batch in batches)
            {
                if (batch.CanAccomodate(visitor) && !batch.IsSaved)
                {
                    return batch;
                }
            }
            return null;
        }

        private ParticipantPersonSevisCommStatus AddPendingSendToSevisStatus(int participantId)
        {
            var sevisCommStatus = new ParticipantPersonSevisCommStatus
            {
                ParticipantId = participantId,
                AddedOn = DateTimeOffset.UtcNow,
                SevisCommStatusId = SevisCommStatus.PendingSevisSend.Id,
            };
            Context.ParticipantPersonSevisCommStatuses.Add(sevisCommStatus);
            return sevisCommStatus;
        }


        private StagedSevisBatch GetNewStagedSevisBatch(User user)
        {
            var stagedSevisBatch = new StagedSevisBatch(
                user: user,
                batchId: SequentialGuidGenerator.NewSqlServerSequentialGuid(),
                orgId: this.sevisOrgId,
                maxCreateExchangeVisitorRecordsPerBatch: this.MaxCreateExchangeVisitorRecordsPerBatch,
                maxUpdateExchangeVisitorRecordPerBatch: this.MaxUpdateExchangeVisitorRecordsPerBatch);
            Context.SevisBatchProcessings.Add(stagedSevisBatch.SevisBatchProcessing);
            return stagedSevisBatch;
        }
        #endregion
    }
}
