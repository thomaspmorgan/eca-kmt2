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
        public const int MAX_SEVIS_BATCH_SIZE = 250;

        public const int MAX_QUERY_BATCH_SIZE = 25;

        private IExchangeVisitorService exchangeVisitorService;
        private string sevisOrgId;
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly Action<SevisBatchProcessing, int> throwIfSevisBatchProcessingNotFound;

        /// <summary>
        /// Creates a new instance and initializes the context..
        /// </summary>
        /// <param name="context">The context to operate against.</param>
        /// <param name="exchangeVisitorService">The exchange visitor service.</param>
        /// <param name="queryBatchSize">The number of ready to submit participants to query in a batch. </param>
        /// <param name="sevisBatchSize">The number of exchange visitor records to attach to a sevis batch.</param>
        /// <param name="sevisOrgId">The organization id to send in a sevis batch.</param>
        /// <param name="saveActions">The save actions.</param>
        public SevisBatchProcessingService(
            EcaContext context,
            IExchangeVisitorService exchangeVisitorService,
            string sevisOrgId,
            int queryBatchSize = MAX_QUERY_BATCH_SIZE,
            int sevisBatchSize = MAX_SEVIS_BATCH_SIZE,
            List<ISaveAction> saveActions = null)
            : base(context, saveActions)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(exchangeVisitorService != null, "The exchange visitor service must not be null.");
            Contract.Requires(sevisOrgId != null, "The sevis org id must not be null.");
            Contract.Requires(sevisBatchSize > 0, "The batch size must be larger than 0.");
            Contract.Requires(sevisBatchSize <= MAX_SEVIS_BATCH_SIZE, "The batch size must not be larger than the max.");
            throwIfSevisBatchProcessingNotFound = (sevisBatchProcessing, batchId) =>
            {
                if (sevisBatchProcessing == null)
                {
                    throw new ModelNotFoundException(String.Format("The SEVIS batch processing record with the batch id [{0}] was not found.", batchId));
                }
            };
            this.exchangeVisitorService = exchangeVisitorService;
            this.sevisOrgId = sevisOrgId;
            this.SevisBatchSize = sevisBatchSize;
            this.QueryBatchSize = queryBatchSize;
        }

        /// <summary>
        /// Gets the batch size.
        /// </summary>
        public int SevisBatchSize { get; private set; }

        /// <summary>
        /// Gets the number of participants to process into exchange visitors at a time.
        /// </summary>
        public int QueryBatchSize { get; private set; }

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
        public List<StagedSevisBatch> StageBatches(User user)
        {
            var stagedSevisBatches = new List<StagedSevisBatch>();
            var readyToSubmitCount = SevisBatchProcessingQueries.CreateGetReadyToSubmitParticipantDTOsQuery(this.Context).Count();
            if (readyToSubmitCount > 0)
            {
                stagedSevisBatches.AddRange(GetCreatedParticipantStagedSevisBatches(user));
                stagedSevisBatches.AddRange(GetUpdatedParticipantStagedSevisBatches(user));
                HandleStagedSevisBatches(stagedSevisBatches);
                this.Context.SaveChanges();
            }
            return stagedSevisBatches;
        }

        public async Task<List<StagedSevisBatch>> StageBatchesAsync(User user)
        {
            var stagedSevisBatches = new List<StagedSevisBatch>();
            var readyToSubmitCount = SevisBatchProcessingQueries.CreateGetReadyToSubmitParticipantDTOsQuery(this.Context).Count();
            if (readyToSubmitCount > 0)
            {
                stagedSevisBatches.AddRange(await GetCreatedParticipantStagedSevisBatchesAsync(user));
                stagedSevisBatches.AddRange(await GetUpdatedParticipantStagedSevisBatchesAsync(user));
                HandleStagedSevisBatches(stagedSevisBatches);
                await this.Context.SaveChangesAsync();
            }
            return stagedSevisBatches;
        }

        private void HandleStagedSevisBatches(List<StagedSevisBatch> stagedSevisBatches)
        {
            stagedSevisBatches.ForEach(x =>
            {
                x.SerializeSEVISBatchCreateUpdateEV();
            });
        }

        private List<StagedSevisBatch> GetCreatedParticipantStagedSevisBatches(User user)
        {
            var stagedSevisBatches = new List<StagedSevisBatch>();
            var skip = 0;
            var newSevisParticipantsCount = CreateGetNewReadyToSubmitParticipantsBatchQuery().Count();
            while (newSevisParticipantsCount > 0)
            {
                StagedSevisBatch stagedSevisBatch = null;
                var newParticipants = CreateGetNewReadyToSubmitParticipantsBatchQuery()
                    .Skip(() => skip)
                    .Take(() => this.QueryBatchSize)
                    .ToList();

                foreach (var newSevisParticipant in newParticipants)
                {
                    var exchangeVisitor = this.exchangeVisitorService.GetExchangeVisitor(user, newSevisParticipant.ProjectId, newSevisParticipant.ParticipantId);
                    if (stagedSevisBatch == null || !stagedSevisBatch.CanAccomodate(exchangeVisitor))
                    {
                        stagedSevisBatch = GetNewStagedSevisBatch(
                            user: user,
                            batchId: SequentialGuidGenerator.NewSqlServerSequentialGuid());
                        stagedSevisBatches.Add(stagedSevisBatch);
                    }
                    stagedSevisBatch.AddExchangeVisitor(exchangeVisitor);
                    AddPendingSendToSevisStatus(newSevisParticipant.ParticipantId);
                    skip++;
                    newSevisParticipantsCount--;
                }
            }
            return stagedSevisBatches;
        }

        private List<StagedSevisBatch> GetUpdatedParticipantStagedSevisBatches(User user)
        {
            var stagedSevisBatches = new List<StagedSevisBatch>();
            var skip = 0;
            var updatedSevisParticipantsCount = CreateGetUpdatedReadyToSubmitParticipantsBatchQuery().Count();
            while (updatedSevisParticipantsCount > 0)
            {
                StagedSevisBatch stagedSevisBatch = null;
                var updatedSevisParticipants = CreateGetUpdatedReadyToSubmitParticipantsBatchQuery()
                                    .Skip(() => skip)
                                    .Take(() => this.QueryBatchSize)
                                    .ToList();
                foreach (var updatedSevisParticipant in updatedSevisParticipants)
                {
                    var exchangeVisitor = this.exchangeVisitorService.GetExchangeVisitor(user, updatedSevisParticipant.ProjectId, updatedSevisParticipant.ParticipantId);
                    if (stagedSevisBatch == null || !stagedSevisBatch.CanAccomodate(exchangeVisitor))
                    {
                        stagedSevisBatch = GetNewStagedSevisBatch(
                            user: user,
                            batchId: SequentialGuidGenerator.NewSqlServerSequentialGuid());
                        stagedSevisBatches.Add(stagedSevisBatch);
                    }
                    stagedSevisBatch.AddExchangeVisitor(exchangeVisitor);
                    AddPendingSendToSevisStatus(updatedSevisParticipant.ParticipantId);
                    skip++;
                    updatedSevisParticipantsCount--;
                }
            }
            return stagedSevisBatches;
        }

        private async Task<List<StagedSevisBatch>> GetCreatedParticipantStagedSevisBatchesAsync(User user)
        {
            var stagedSevisBatches = new List<StagedSevisBatch>();
            var skip = 0;
            var newSevisParticipantsCount = await CreateGetNewReadyToSubmitParticipantsBatchQuery().CountAsync();
            while (newSevisParticipantsCount > 0)
            {
                StagedSevisBatch stagedSevisBatch = null;
                var newParticipants = await CreateGetNewReadyToSubmitParticipantsBatchQuery()
                   .Skip(() => skip)
                   .Take(() => this.QueryBatchSize)
                   .ToListAsync();
                foreach (var newSevisParticipant in newParticipants)
                {
                    var exchangeVisitor = await this.exchangeVisitorService.GetExchangeVisitorAsync(user, newSevisParticipant.ProjectId, newSevisParticipant.ParticipantId);
                    if (stagedSevisBatch == null || !stagedSevisBatch.CanAccomodate(exchangeVisitor))
                    {
                        stagedSevisBatch = GetNewStagedSevisBatch(
                            user: user,
                            batchId: SequentialGuidGenerator.NewSqlServerSequentialGuid());
                        stagedSevisBatches.Add(stagedSevisBatch);
                    }
                    stagedSevisBatch.AddExchangeVisitor(exchangeVisitor);
                    AddPendingSendToSevisStatus(newSevisParticipant.ParticipantId);
                    skip++;
                    newSevisParticipantsCount--;
                }
            }
            return stagedSevisBatches;
        }

        private async Task<List<StagedSevisBatch>> GetUpdatedParticipantStagedSevisBatchesAsync(User user)
        {
            var stagedSevisBatches = new List<StagedSevisBatch>();
            var skip = 0;
            var updatedSevisParticipantsCount = await CreateGetUpdatedReadyToSubmitParticipantsBatchQuery().CountAsync();
            while (updatedSevisParticipantsCount > 0)
            {
                StagedSevisBatch stagedSevisBatch = null;
                var updatedSevisParticipants = await CreateGetUpdatedReadyToSubmitParticipantsBatchQuery()
                                    .Skip(() => skip)
                                    .Take(() => this.QueryBatchSize)
                                    .ToListAsync();
                foreach (var updatedSevisParticipant in updatedSevisParticipants)
                {
                    var exchangeVisitor = await this.exchangeVisitorService.GetExchangeVisitorAsync(user, updatedSevisParticipant.ProjectId, updatedSevisParticipant.ParticipantId);
                    if (stagedSevisBatch == null || !stagedSevisBatch.CanAccomodate(exchangeVisitor))
                    {
                        stagedSevisBatch = GetNewStagedSevisBatch(
                            user: user,
                            batchId: SequentialGuidGenerator.NewSqlServerSequentialGuid());
                        stagedSevisBatches.Add(stagedSevisBatch);
                    }
                    stagedSevisBatch.AddExchangeVisitor(exchangeVisitor);
                    AddPendingSendToSevisStatus(updatedSevisParticipant.ParticipantId);
                    skip++;
                    updatedSevisParticipantsCount--;
                }
            }
            return stagedSevisBatches;
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

        private IQueryable<ReadyToSubmitParticipantDTO> CreateGetNewReadyToSubmitParticipantsBatchQuery()
        {
            return SevisBatchProcessingQueries.CreateGetReadyToSubmitParticipantDTOsQuery(this.Context)
                .Where(x => x.SevisId == null || x.SevisId.Length == 0)
                .OrderBy(x => x.ParticipantId);
        }

        private IQueryable<ReadyToSubmitParticipantDTO> CreateGetUpdatedReadyToSubmitParticipantsBatchQuery()
        {
            return SevisBatchProcessingQueries.CreateGetReadyToSubmitParticipantDTOsQuery(this.Context)
                .Where(x => x.SevisId != null && x.SevisId.Length != 0)
                .OrderBy(x => x.ParticipantId);
        }

        private StagedSevisBatch GetNewStagedSevisBatch(User user, Guid batchId)
        {
            var stagedSevisBatch = new StagedSevisBatch(
                maxCreate: this.SevisBatchSize,
                maxUpdate: this.SevisBatchSize,
                batchId: batchId,
                orgId: this.sevisOrgId);
            stagedSevisBatch.SEVISBatchCreateUpdateEV.userID = user.Id.ToString();
            Context.SevisBatchProcessings.Add(stagedSevisBatch.SevisBatchProcessing);
            return stagedSevisBatch;
        }


        #endregion
    }
}
