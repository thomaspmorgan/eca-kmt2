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


        private IExchangeVisitorService exchangeVisitorService;
        private string sevisOrgId;
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly Action<SevisBatchProcessing, int> throwIfSevisBatchProcessingNotFound;
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
            string sevisOrgId,
            int maxCreateExchangeVisitorRecordsPerBatch = StagedSevisBatch.MAX_CREATE_EXCHANGE_VISITOR_RECORDS_PER_BATCH_DEFAULT,
            int maxUpdateExchangeVisitorRecordsPerBatch = StagedSevisBatch.MAX_UPDATE_EXCHANGE_VISITOR_RECORD_PER_BATCH_DEFAULT,
            List<ISaveAction> saveActions = null)
            : base(context, saveActions)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(exchangeVisitorService != null, "The exchange visitor service must not be null.");
            Contract.Requires(sevisOrgId != null, "The sevis org id must not be null.");
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
            this.exchangeVisitorService = exchangeVisitorService;
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


        //#region Get
        ///// <summary>
        ///// Retrieves the SevisBatchProcessing dto with the given id.
        ///// </summary>
        ///// <param name="batchId">The id of the SEVIS Batch Processing record.</param>
        ///// <returns>The phone number dto.</returns>
        //public SevisBatchProcessingDTO GetById(int batchId)
        //{
        //    var dto = SevisBatchProcessingQueries.CreateGetSevisBatchProcessingByIdQuery(this.Context, batchId);
        //    logger.Info("Retrieved the Sevis Batch Processing dto with the given id [{0}].", batchId);
        //    return dto;
        //}

        ///// <summary>
        ///// Retrieves the SevisBatchProcessing  dto with the given id.
        ///// </summary>
        ///// <param name="batchId">The id of the SevisBatchProcessing record.</param>
        ///// <returns>The SevisBatchProcessing  dto.</returns>
        //public Task<SevisBatchProcessingDTO> GetByIdAsync(int batchId)
        //{
        //    var dto = Task.FromResult(SevisBatchProcessingQueries.CreateGetSevisBatchProcessingDTOByIdQuery(this.Context, batchId));
        //    logger.Info("Retrieved the Sevis Batch Processing dto with the given id [{0}].", batchId);
        //    return dto;
        //}

        ///// <summary>
        /////  Retrieves the list of SEVIS Batch DTOs that have not been uploaded yet.
        ///// </summary>
        ///// <returns>The list of SevisBatchProcessing dtos.</returns>
        //public IQueryable<SevisBatchProcessingDTO> GetSevisBatchesToUpload()
        //{
        //    var dtos = SevisBatchProcessingQueries.CreateGetSevisBatchProcessingDTOsForUpload(this.Context);
        //    logger.Trace("Retrieved the SEVIS Batch Processing dtos to upload");
        //    return dtos;
        //}

        ///// <summary>
        ///// Retrieves the list of SEVIS Batch DTOs that have not been uploaded yet.
        ///// </summary>
        ///// <returns>The list of SevisBatchProcessing dtos.</returns>
        //public Task<IQueryable<SevisBatchProcessingDTO>> GetSevisBatchesToUploadAsync()
        //{
        //    var dtos = Task.FromResult(SevisBatchProcessingQueries.CreateGetSevisBatchProcessingDTOsForUpload(this.Context));
        //    logger.Trace("Retrieved the SEVIS Batch Processing dtos to upload");
        //    return dtos;
        //}

        ///// <summary>
        /////  Retrieves the list of SEVIS Batch DTOs that have not been downloaded yet.
        ///// </summary>
        ///// <returns>The list of SevisBatchProcessing dtos.</returns>
        //public IQueryable<SevisBatchProcessingDTO> GetSevisBatchesToDownload()
        //{
        //    var dtos = SevisBatchProcessingQueries.CreateGetSevisBatchProcessingDTOsForDownload(this.Context);
        //    logger.Trace("Retrieved the SEVIS Batch Processing dtos to download");
        //    return dtos;
        //}

        ///// <summary>
        ///// Retrieves the list of SEVIS Batch DTOs that have not been downloaded yet.
        ///// </summary>
        ///// <returns>The list of SevisBatchProcessing dtos.</returns>
        //public Task<IQueryable<SevisBatchProcessingDTO>> GetSevisBatchesToDownloadAsync()
        //{
        //    var dtos = Task.FromResult(SevisBatchProcessingQueries.CreateGetSevisBatchProcessingDTOsForDownload(this.Context));
        //    logger.Trace("Retrieved the SEVIS Batch Processing dtos to download");
        //    return dtos;
        //}

        ///// <summary>
        /////  Retrieves the list of SEVIS Batch DTOs that have not been processed yet.
        ///// </summary>
        ///// <returns>The list of SevisBatchProcessing dtos.</returns>
        //public IQueryable<SevisBatchProcessingDTO> GetSevisBatchesToProcess()
        //{
        //    var dtos = SevisBatchProcessingQueries.CreateGetSevisBatchProcessingDTOsToProcess(this.Context);
        //    logger.Trace("Retrieved the SEVIS Batch Processing dtos to download");
        //    return dtos;
        //}

        ///// <summary>
        ///// Retrieves the list of SEVIS Batch DTOs that have not been processed yet.
        ///// </summary>
        ///// <returns>The list of SevisBatchProcessing dtos.</returns>
        //public Task<IQueryable<SevisBatchProcessingDTO>> GetSevisBatchesToProcessAsync()
        //{
        //    var dtos = Task.FromResult(SevisBatchProcessingQueries.CreateGetSevisBatchProcessingDTOsToProcess(this.Context));
        //    logger.Trace("Retrieved the SEVIS Batch Processing dtos to download");
        //    return dtos;
        //}
        //#endregion

        #region Create
        /// <summary>
        /// Creates a new SEVIS Batch Processing record in the ECA system.
        /// </summary>
        /// <returns>The created SEVIS Batch Processing entity.</returns>
        public SevisBatchProcessing Create()
        {
            return DoCreate();
        }

        /// <summary>
        /// Creates a new Sevis Batch Processing record in the ECA system.
        /// </summary>
        /// <returns>The created  phone number entity.</returns>
        public async Task<SevisBatchProcessing> CreateAsync()
        {
            return await DoCreateAsync();
        }

        private SevisBatchProcessing DoCreate()
        {
            var newSevisBatchProcessing = new SevisBatchProcessing();
            SaveChanges();
            return newSevisBatchProcessing;
        }

        private async Task<SevisBatchProcessing> DoCreateAsync()
        {
            var newSevisBatchProcessing = new SevisBatchProcessing();
            await SaveChangesAsync();
            return newSevisBatchProcessing;
        }

        #endregion

        #region Update

        /// <summary>
        /// Process SEVIS batch transaction log
        /// </summary>
        /// <param name="batchId">Batch ID</param>
        //public async Task<IQueryable<ParticipantSevisBatchProcessingResultDTO>> UpdateParticipantPersonSevisBatchStatusAsync(User user, int batchId)
        //{
        //    var batchLog = await GetByIdAsync(batchId);
        //    var xml = batchLog.TransactionLogXml;
        //    var doc = XDocument.Parse(xml.ToString());
        //    List<ParticipantSevisBatchProcessingResultDTO> results = new List<ParticipantSevisBatchProcessingResultDTO>();

        //    foreach (XElement record in doc.Descendants("Record"))
        //    {
        //        var participantID = Convert.ToInt32(record.Attribute("requestID").Value);
        //        var status = record.Descendants("Result").First().Attribute("status").Value;
        //        string json = JsonConvert.SerializeXNode(record);

        //        // update participant person batch result
        //        results.Add(await UpdateParticipant(participantID, status, json));
        //    }

        //    return results.AsQueryable();
        //}

        /// <summary>
        /// Update a participant record with sevis batch results
        /// </summary>
        /// <param name="participantID"></param>
        /// <param name="status"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        private async Task<ParticipantSevisBatchProcessingResultDTO> UpdateParticipant(int participantID, string status, string json)
        {
            var result = new ParticipantSevisBatchProcessingResultDTO();

            var participantPersonDTO = await Context.ParticipantPersons.FindAsync(participantID);
            participantPersonDTO.SevisBatchResult = json;

            result.ParticipantId = participantID;
            result.ProjectId = participantPersonDTO.Participant.ProjectId;
            if (status == "1")
            {
                result.SevisCommStatus = SevisCommStatus.BatchRequestSuccessful.Value;
            }
            else
            {
                result.SevisCommStatus = SevisCommStatus.BatchRequestUnsuccessful.Value;
            }

            return result;
        }

        /// <summary>
        /// Updates the ECA system's SEVIS Batch Processing record with the given updated SEVIS Batch Processing record.
        /// </summary>
        /// <param name="updatedSevisBatchProcessing">The updated SEVIS Batch Processing record.</param>
        public void Update(UpdatedSevisBatchProcessing updatedSevisBatchProcessing)
        {
            var sevisBatchProcessing = Context.SevisBatchProcessings.Find(updatedSevisBatchProcessing.BatchId);
            DoUpdate(updatedSevisBatchProcessing, sevisBatchProcessing);
        }

        /// <summary>
        /// Updates the ECA system's SEVIS Batch Processing record with the given SEVIS Batch Processing record.
        /// </summary>
        /// <param name="updatedSevisBatchProcessing">The SEVIS Batch Processing record.</param>
        public async Task UpdateAsync(UpdatedSevisBatchProcessing updatedSevisBatchProcessing)
        {
            var sevisBatchProcessing = await Context.SevisBatchProcessings.FindAsync(updatedSevisBatchProcessing.BatchId);
            DoUpdate(updatedSevisBatchProcessing, sevisBatchProcessing);
        }

        private void DoUpdate(UpdatedSevisBatchProcessing updatedSevisBatchProcessing, SevisBatchProcessing modelToUpdate)
        {
            Contract.Requires(updatedSevisBatchProcessing != null, "The updatedSevisBatchProcessing must not be null.");
            throwIfSevisBatchProcessingNotFound(modelToUpdate, updatedSevisBatchProcessing.BatchId);
            modelToUpdate.SubmitDate = updatedSevisBatchProcessing.SubmitDate;
            modelToUpdate.RetrieveDate = updatedSevisBatchProcessing.RetrieveDate;
            modelToUpdate.SendXml = updatedSevisBatchProcessing.SendXml;
            modelToUpdate.TransactionLogXml = updatedSevisBatchProcessing.TransactionLogXml;
            modelToUpdate.DownloadDispositionCode = updatedSevisBatchProcessing.DownloadDispositionCode;
            modelToUpdate.ProcessDispositionCode = updatedSevisBatchProcessing.ProcessDispositionCode;
            modelToUpdate.UploadDispositionCode = updatedSevisBatchProcessing.UploadDispositionCode;
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
                    var accomodatingSevisBatch = GetAccomodatingStagedSevisBatch(stagedSevisBatches, exchangeVisitor);
                    if (accomodatingSevisBatch != null)
                    {
                        stagedSevisBatch = accomodatingSevisBatch;
                    }
                    else
                    {
                        PersistStagedSevisBatches(stagedSevisBatches);
                        stagedSevisBatch = getNewStagedSevisBatch(stagedSevisBatches, user);
                    }
                    stagedSevisBatch.AddExchangeVisitor(exchangeVisitor);
                    AddPendingSendToSevisStatus(participant.ParticipantId);
                    skip++;
                    newSevisParticipantsCount--;
                }
            }
            PersistStagedSevisBatches(stagedSevisBatches);
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
                    var accomodatingSevisBatch = GetAccomodatingStagedSevisBatch(stagedSevisBatches, exchangeVisitor);
                    if (accomodatingSevisBatch != null)
                    {
                        stagedSevisBatch = accomodatingSevisBatch;
                    }
                    else
                    {
                        await PersistStagedSevisBatchesAsync(stagedSevisBatches);
                        stagedSevisBatch = getNewStagedSevisBatch(stagedSevisBatches, user);
                    }
                    stagedSevisBatch.AddExchangeVisitor(exchangeVisitor);
                    AddPendingSendToSevisStatus(participant.ParticipantId);
                    skip++;
                    newSevisParticipantsCount--;
                }
            }
            await PersistStagedSevisBatchesAsync(stagedSevisBatches);
            return stagedSevisBatches;
        }

        private void PersistStagedSevisBatches(List<StagedSevisBatch> stagedSevisBatches)
        {
            foreach(var stagedSevisBatch in stagedSevisBatches)
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
            foreach(var batch in batches)
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
