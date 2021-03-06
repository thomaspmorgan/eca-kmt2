﻿using ECA.Business.Queries.Sevis;
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
using ECA.Core.Generation;
using Newtonsoft.Json.Serialization;
using ECA.Core.Settings;
using System.Xml;
using System.Data.Entity.Core.Objects;
using ECA.Business.Storage;
using System.Text;
using ECA.Business.Queries.Persons;
using ECA.Business.Queries.Models.Persons;

namespace ECA.Business.Service.Sevis
{
    /// <summary>
    /// The SevisBatchProcessingService is capable of handling crud operations on a SevisBatchProcessingService record
    /// and its phone number.
    /// </summary>
    public class SevisBatchProcessingService : DbContextService<EcaContext>, ISevisBatchProcessingService
    {
        /// <summary>
        /// The comment to insert into the transaction log when physical corrected addresses have been commented out.
        /// </summary>
        public const string PHYSICAL_CORRECTED_ADDRESSES_COMMENTED_MESSAGE = "PHYSICAL CORRECTED ADDRESSES HAVE BEEN COMMENTED OUT OF THIS TRANSACTION LOG.";

        /// <summary>
        /// The name of the physical corrected address element in the transaction log xml.
        /// </summary>
        public const string PHYSICAL_CORRECTED_ADDRESS_ELEMENT_NAME = "PhysicalCorrectedAddress";

        /// <summary>
        /// The DS2019 file content type.
        /// </summary>
        public const string DS2019_CONTENT_TYPE = "application/pdf";

        /// <summary>
        /// A generic message to set when the batch was cancelled for an indeterminate reason.
        /// </summary>
        public const string GENERIC_BATCH_CANCELLED_REASON_MESSAGE = "The batch was cancelled for an unknown reason.  Inspect the batch values to determine errors.";

        /// <summary>
        /// The number of queued to submit participants to page.
        /// </summary>
        public const int QUERY_BATCH_SIZE = 5;

        private IFileStorageService cloudStorageService;
        private ISevisBatchProcessingNotificationService notificationService;
        private IExchangeVisitorService exchangeVisitorService;
        private IExchangeVisitorValidationService exchangeVisitorValidationService;
        private AppSettings appSettings;
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly Action<SevisBatchProcessing, object> throwIfSevisBatchProcessingNotFound;
        private readonly Func<string, string, List<StagedSevisBatch>, StagedSevisBatch> getNewStagedSevisBatch;

        /// <summary>
        /// Creates a new instance and initializes the context..
        /// </summary>
        /// <param name="context">The context to operate against.</param>
        /// <param name="exchangeVisitorService">The exchange visitor service.</param>
        /// <param name="appSettings">The app settings.</param>
        /// <param name="cloudStorageService">The cloud storage service used to save ds2019 files.</param>
        /// <param name="exchangeVisitorValidationService">The exchange visitor validation service.</param>
        /// <param name="maxCreateExchangeVisitorRecordsPerBatch">The maximum number of records to place in a CreateEV sevis batch.</param>
        /// <param name="maxUpdateExchangeVisitorRecordsPerBatch">The maximum number of records to place in the UpdateEV sevis batch.</param>
        /// <param name="notificationService">The notification service.</param>
        /// <param name="saveActions">The save actions.</param>
        public SevisBatchProcessingService(
            EcaContext context,
            IFileStorageService cloudStorageService,
            IExchangeVisitorService exchangeVisitorService,
            ISevisBatchProcessingNotificationService notificationService,
            IExchangeVisitorValidationService exchangeVisitorValidationService,
            AppSettings appSettings,
            int maxCreateExchangeVisitorRecordsPerBatch = StagedSevisBatch.MAX_CREATE_EXCHANGE_VISITOR_RECORDS_PER_BATCH_DEFAULT,
            int maxUpdateExchangeVisitorRecordsPerBatch = StagedSevisBatch.MAX_UPDATE_EXCHANGE_VISITOR_RECORD_PER_BATCH_DEFAULT,
            List<ISaveAction> saveActions = null)
            : base(context, saveActions)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(exchangeVisitorService != null, "The exchange visitor service must not be null.");
            Contract.Requires(appSettings != null, "The app settings must not be null.");
            Contract.Requires(notificationService != null, "The notification service must not be null.");
            Contract.Requires(cloudStorageService != null, "The cloud storage service must not be null.");
            throwIfSevisBatchProcessingNotFound = (sevisBatchProcessing, batchId) =>
            {
                if (sevisBatchProcessing == null)
                {
                    throw new ModelNotFoundException(String.Format("The SEVIS batch processing record with the batch id [{0}] was not found.", batchId));
                }
            };
            getNewStagedSevisBatch = (sevisUsername, sevisOrgId, batches) =>
            {
                var stagedSevisBatch = GetNewStagedSevisBatch(sevisUsername, sevisOrgId);
                batches.Add(stagedSevisBatch);
                return stagedSevisBatch;
            };
            this.appSettings = appSettings;
            this.cloudStorageService = cloudStorageService;
            this.notificationService = notificationService;
            this.exchangeVisitorService = exchangeVisitorService;
            this.exchangeVisitorValidationService = exchangeVisitorValidationService;
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
            var cooldownSeconds = Int32.Parse(appSettings.SevisUploadCooldownInSeconds);
            if (cooldownSeconds < 0)
            {
                cooldownSeconds = -1 * cooldownSeconds;
            }
            var now = DateTimeOffset.UtcNow;
            return SevisBatchProcessingQueries.CreateGetSevisBatchProcessingDTOsToUploadQuery(this.Context)
                .Where(x => !x.LastUploadTry.HasValue || (DbFunctions.DiffSeconds(x.LastUploadTry.Value, now) > cooldownSeconds));
        }

        /// <summary>
        /// Returns the batch record of the next batch to download from the sevis api.
        /// </summary>
        /// <returns>The batch record of the next batch to download from sevis.</returns>
        public SevisBatchProcessingDTO GetNextBatchToDownload()
        {
            return CreateGetNextBatchToDownload().FirstOrDefault();
        }

        /// <summary>
        /// Returns the batch record of the next batch to download from the sevis api.
        /// </summary>
        /// <returns>The batch record of the next batch to download from sevis.</returns>
        public Task<SevisBatchProcessingDTO> GetNextBatchToDownloadAsync()
        {
            return CreateGetNextBatchToDownload().FirstOrDefaultAsync();
        }

        private IQueryable<SevisBatchProcessingDTO> CreateGetNextBatchToDownload()
        {
            var cooldownSeconds = Int32.Parse(appSettings.SevisDownloadCooldownInSeconds);
            if (cooldownSeconds < 0)
            {
                cooldownSeconds = -1 * cooldownSeconds;
            }
            var now = DateTimeOffset.UtcNow;
            return SevisBatchProcessingQueries.CreateGetSevisBatchProcessingDTOsToDownloadQuery(this.Context)
                .Where(x => !x.LastDownloadTry.HasValue || (DbFunctions.DiffSeconds(x.LastDownloadTry.Value, now) > cooldownSeconds))
                .Where(x => x.SubmitDate.HasValue && (DbFunctions.DiffSeconds(x.SubmitDate.Value, now) > cooldownSeconds));
        }
        #endregion

        #region Error Handling

        /// <summary>
        /// Handles a failed batch upload.
        /// </summary>
        /// <param name="batchId">The id of the batch that failed to upload.</param>
        /// <param name="e">The exception that may have been caught.</param>
        public void HandleFailedUploadBatch(int batchId, Exception e)
        {
            var batch = Context.SevisBatchProcessings.Find(batchId);
            throwIfSevisBatchProcessingNotFound(batch, batchId);
            DoHandleFailedUploadBatch(batch, e);
            this.Context.SaveChanges();
        }

        /// <summary>
        /// Handles a failed batch upload.
        /// </summary>
        /// <param name="batchId">The id of the batch that failed to upload.</param>
        /// <param name="e">The exception that may have been caught.</param>
        public async Task HandleFailedUploadBatchAsync(int batchId, Exception e)
        {
            var batch = await Context.SevisBatchProcessings.FindAsync(batchId);
            throwIfSevisBatchProcessingNotFound(batch, batchId);
            DoHandleFailedUploadBatch(batch, e);
            await this.Context.SaveChangesAsync();
        }

        private void DoHandleFailedUploadBatch(SevisBatchProcessing batch, Exception e)
        {
            batch.UploadTries++;
            batch.LastUploadTry = DateTimeOffset.UtcNow;
        }

        /// <summary>
        /// Handles a failed batch download.
        /// </summary>
        /// <param name="batchId">The id of the batch that failed to download.</param>
        /// <param name="e">The exception that may have been caught.</param>
        public void HandleFailedDownloadBatch(int batchId, Exception e)
        {
            var batch = Context.SevisBatchProcessings.Find(batchId);
            throwIfSevisBatchProcessingNotFound(batch, batchId);
            DoHandleFailedDownloadBatch(batch, e);
            this.Context.SaveChanges();
        }

        /// <summary>
        /// Handles a failed batch download.
        /// </summary>
        /// <param name="batchId">The id of the batch that failed to download.</param>
        /// <param name="e">The exception that may have been caught.</param>
        public async Task HandleFailedDownloadBatchAsync(int batchId, Exception e)
        {
            var batch = await Context.SevisBatchProcessings.FindAsync(batchId);
            throwIfSevisBatchProcessingNotFound(batch, batchId);
            DoHandleFailedDownloadBatch(batch, e);
            await this.Context.SaveChangesAsync();
        }

        private void DoHandleFailedDownloadBatch(SevisBatchProcessing batch, Exception e)
        {
            batch.DownloadTries++;
            batch.LastDownloadTry = DateTimeOffset.UtcNow;
        }

        /// <summary>
        /// Cancels the given batch for the given reason.
        /// </summary>
        /// <param name="batch">The batch to cancel.</param>
        /// <param name="reason">The reasoning for the cancel.</param>
        public void Cancel(SevisBatchProcessing batch, string reason)
        {
            var participantIds = CreateGetParticipantIdsByBatchId(batch.BatchId).ToList();
            DoCancelBatch(batch, participantIds, reason);
        }

        /// <summary>
        /// Cancels the given batch for the given reason.
        /// </summary>
        /// <param name="batch">The batch to cancel.</param>
        /// <param name="reason">The reasoning for the cancel.</param>
        public async Task CancelAsync(SevisBatchProcessing batch, string reason)
        {
            var participantIds = await CreateGetParticipantIdsByBatchId(batch.BatchId).ToListAsync();
            DoCancelBatch(batch, participantIds, reason);
        }

        private void DoCancelBatch(SevisBatchProcessing batch, List<int> cancelledParticipantsById, string reason)
        {
            Contract.Requires(batch != null, "The batch must not be null.");
            Contract.Requires(reason != null, "The reason must not be null.");
            Contract.Requires(cancelledParticipantsById != null, "The cancelledParticipantsById list must not be null.");
            var now = DateTimeOffset.UtcNow;
            var cancelledBatch = new CancelledSevisBatchProcessing
            {
                BatchId = batch.BatchId,
                CancelledOn = now,
                DownloadDispositionCode = batch.DownloadDispositionCode,
                DownloadTries = batch.DownloadTries,
                LastDownloadTry = batch.LastDownloadTry,
                LastUploadTry = batch.LastUploadTry,
                ProcessDispositionCode = batch.ProcessDispositionCode,
                Reason = reason,
                RetrieveDate = batch.RetrieveDate,
                SendString = batch.SendString,
                SevisOrgId = batch.SevisOrgId,
                SevisUsername = batch.SevisUsername,
                SubmitDate = batch.SubmitDate,
                TransactionLogString = batch.TransactionLogString,
                UploadDispositionCode = batch.UploadDispositionCode,
                UploadTries = batch.UploadTries
            };
            var sevisBatchResultJsonString = GetBatchCancelledBySystemAsSevisBatchResultJsonString(reason);

            foreach (var id in cancelledParticipantsById)
            {
                var attachedParticipant = new ParticipantPerson
                {
                    ParticipantId = id,
                    SevisBatchResult = sevisBatchResultJsonString
                };
                Context.ParticipantPersons.Attach(attachedParticipant);
                Context.GetEntry(attachedParticipant).Property(x => x.SevisBatchResult).IsModified = true;

                var commStatus = new ParticipantPersonSevisCommStatus
                {
                    AddedOn = now,
                    BatchId = batch.BatchId,
                    ParticipantId = id,
                    SevisCommStatusId = SevisCommStatus.BatchCancelledBySystem.Id,
                    SevisOrgId = batch.SevisOrgId,
                    SevisUsername = batch.SevisUsername,
                };
                Context.ParticipantPersonSevisCommStatuses.Add(commStatus);
            }
            Context.CancelledSevisBatchProcessings.Add(cancelledBatch);
            Context.SevisBatchProcessings.Remove(batch);
            notificationService.NotifyCancelledSevisBatch(batch.BatchId, reason);
        }

        /// <summary>
        /// Returns a json string of representing the sevis batch result cancellation.
        /// </summary>
        /// <param name="reason">The reason for the cancellation.</param>
        /// <returns>A Json string of the cancellation reasons.</returns>
        public string GetBatchCancelledBySystemAsSevisBatchResultJsonString(string reason)
        {
            var list = new List<SimpleSevisBatchErrorResult>();
            list.Add(new SimpleSevisBatchErrorResult(SevisCommStatus.BatchCancelledBySystem.Value, reason));
            return JsonConvert.SerializeObject(list, GetSerializerSettings());
        }

        #endregion

        #region Process Transaction Log

        /// <summary>
        /// Processes a given sevis transaction log as an xml string and updates system data appropriately.
        /// </summary>
        /// <param name="user">The user performing the processing.</param>
        /// <param name="xml">The sevis transaction log xml as a string.</param>
        /// <param name="batchId">The string batch id of the transaciton log to process.</param>
        /// <param name="fileProvider">The file provider.</param>
        public void ProcessTransactionLog(User user, string batchId, string xml, IDS2019FileProvider fileProvider)
        {
            var transactionLog = DeserializeTransactionLogType(xml);
            ProcessTransactionLog(user, batchId, transactionLog, fileProvider);
        }

        /// <summary>
        /// Processes a given sevis transaction log as an xml string and updates system data appropriately.
        /// </summary>
        /// <param name="user">The user performing the processing.</param>
        /// <param name="batchId">The string batch id of the transaciton log to process.</param>
        /// <param name="xml">The sevis transaction log xml as a string.</param>
        /// <param name="fileProvider">The file provider.</param>
        public Task ProcessTransactionLogAsync(User user, string batchId, string xml, IDS2019FileProvider fileProvider)
        {
            var transactionLog = DeserializeTransactionLogType(xml);
            return ProcessTransactionLogAsync(user, batchId, transactionLog, fileProvider);
        }

        /// <summary>
        /// Processes the given transaction log and its original xml
        /// </summary>
        /// <param name="user">The user performing the transaction.</param>
        /// <param name="batchId">The string batch id of the transaction log.</param>
        /// <param name="transactionLog">The deserialized transaction log.</param>
        /// <param name="fileProvider">The file provider.</param>
        public void ProcessTransactionLog(User user, string batchId, TransactionLogType transactionLog, IDS2019FileProvider fileProvider)
        {
            Contract.Requires(user != null, "The user must not be null.");
            Contract.Requires(batchId != null, "The batch id must not be null.");
            Contract.Requires(transactionLog != null, "The transaction log must not be null.");
            Contract.Requires(fileProvider != null, "The file provider must not be null.");
            var batch = CreateGetSevisBatchProcessingByBatchIdQuery(batchId).FirstOrDefault();
            throwIfSevisBatchProcessingNotFound(batch, batchId);
            ProcessUpload(transactionLog.BatchDetail.Upload, batch);
            DoProcessTransactionLog(user, transactionLog, batch);
            if (transactionLog.BatchDetail != null && transactionLog.BatchDetail.Process != null)
            {
                ProcessBatchDetailProcess(user, transactionLog.BatchDetail.Process, batch, fileProvider);
            }
            this.Context.SaveChanges();
        }

        /// <summary>
        /// Processes the given transaction log and its original xml
        /// </summary>
        /// <param name="user">The user performing the transaction.</param>
        /// <param name="batchId">The string batch id of the transaction log.</param>
        /// <param name="transactionLog">The deserialized transaction log.</param>
        /// <param name="fileProvider">The file provider.</param>
        public async Task ProcessTransactionLogAsync(User user, string batchId, TransactionLogType transactionLog, IDS2019FileProvider fileProvider)
        {
            Contract.Requires(user != null, "The user must not be null.");
            Contract.Requires(batchId != null, "The batch id must not be null.");
            Contract.Requires(transactionLog != null, "The transaction log must not be null.");
            Contract.Requires(fileProvider != null, "The file provider must not be null.");
            var batch = await CreateGetSevisBatchProcessingByBatchIdQuery(batchId).FirstOrDefaultAsync();
            throwIfSevisBatchProcessingNotFound(batch, batchId);
            await ProcessUploadAsync(transactionLog.BatchDetail.Upload, batch);
            DoProcessTransactionLog(user, transactionLog, batch);
            if (transactionLog.BatchDetail != null && transactionLog.BatchDetail.Process != null)
            {
                await ProcessBatchDetailProcessAsync(user, transactionLog.BatchDetail.Process, batch, fileProvider);
            }
            await this.Context.SaveChangesAsync();
        }

        private void DoProcessTransactionLog(User user, TransactionLogType transactionLog, SevisBatchProcessing batch)
        {
            Contract.Requires(batch != null, "The batch must not be null.");
            ProcessDownload(transactionLog.BatchDetail.Download, batch);
            using (var textWriter = new StringWriter())
            {
                var settings = new XmlWriterSettings();
                settings.OmitXmlDeclaration = true;

                var writer = XmlWriter.Create(textWriter, settings);
                var serializer = new XmlSerializer(typeof(TransactionLogType));
                serializer.Serialize(writer, transactionLog);
                batch.TransactionLogString = textWriter.ToString();
            }
        }

        /// <summary>
        /// Updates the given sevis batch with the upload details.
        /// </summary>
        /// <param name="uploadDetail">The upload details from the transaction log.</param>
        /// <param name="batch">The batch to update.</param>
        public void ProcessUpload(TransactionLogTypeBatchDetailUpload uploadDetail, SevisBatchProcessing batch)
        {
            Contract.Requires(batch != null, "The batch must not be null.");
            if (uploadDetail != null)
            {
                var dispositionCode = uploadDetail.DispositionCode;
                batch.UploadDispositionCode = dispositionCode.Code;
                batch.SubmitDate = uploadDetail.dateTimeStamp.ToUniversalTime();
                if (dispositionCode == DispositionCode.Success)
                {
                    var participantIds = CreateGetParticipantIdsWhoNeedSuccessfulUploadStatus(batch.BatchId).ToList();
                    AddSuccessfulUploadSevisCommStatus(participantIds, batch);
                }
                else
                {
                    if (dispositionCode == DispositionCode.DuplicateBatchId
                        || dispositionCode == DispositionCode.DocumentNameInvalid
                        || dispositionCode == DispositionCode.MalformedXml
                        || dispositionCode == DispositionCode.InvalidXml
                        || dispositionCode == DispositionCode.InvalidOrganizationInformation)
                    {
                        Cancel(batch, dispositionCode.Description);
                    }
                    else
                    {
                        batch.UploadTries++;
                        batch.LastUploadTry = DateTimeOffset.UtcNow;
                    }
                }
                notificationService.NotifyUploadedBatchProcessed(batch.BatchId, dispositionCode);
            }
        }

        /// <summary>
        /// Updates the given sevis batch with the upload details.
        /// </summary>
        /// <param name="uploadDetail">The upload details from the transaction log.</param>
        /// <param name="batch">The batch to update.</param>
        public async Task ProcessUploadAsync(TransactionLogTypeBatchDetailUpload uploadDetail, SevisBatchProcessing batch)
        {
            Contract.Requires(batch != null, "The batch must not be null.");
            if (uploadDetail != null)
            {
                var dispositionCode = uploadDetail.DispositionCode;
                batch.UploadDispositionCode = dispositionCode.Code;
                batch.SubmitDate = uploadDetail.dateTimeStamp.ToUniversalTime();
                if (dispositionCode == DispositionCode.Success)
                {
                    var participantIds = await CreateGetParticipantIdsWhoNeedSuccessfulUploadStatus(batch.BatchId).ToListAsync();
                    AddSuccessfulUploadSevisCommStatus(participantIds, batch);
                }
                else
                {
                    if (dispositionCode == DispositionCode.DuplicateBatchId
                        || dispositionCode == DispositionCode.DocumentNameInvalid
                        || dispositionCode == DispositionCode.MalformedXml
                        || dispositionCode == DispositionCode.InvalidXml
                        || dispositionCode == DispositionCode.InvalidOrganizationInformation)
                    {
                        Cancel(batch, dispositionCode.Description);
                    }
                    else
                    {
                        batch.UploadTries++;
                        batch.LastUploadTry = DateTimeOffset.UtcNow;
                    }
                }
                notificationService.NotifyUploadedBatchProcessed(batch.BatchId, dispositionCode);
            }
        }

        private void AddSuccessfulUploadSevisCommStatus(List<int> participantIds, SevisBatchProcessing batch)
        {
            Contract.Requires(batch != null, "The batch must not be null.");
            foreach (var id in participantIds)
            {
                Context.ParticipantPersonSevisCommStatuses.Add(new ParticipantPersonSevisCommStatus
                {
                    AddedOn = DateTimeOffset.UtcNow,
                    BatchId = batch.BatchId,
                    ParticipantId = id,
                    SevisCommStatusId = SevisCommStatus.SentByBatch.Id,
                });
            }
        }

        /// <summary>
        /// Updates the given sevis batch with the download details.
        /// </summary>
        /// <param name="downloadDetail">The download details from the transction log.</param>
        /// <param name="batch">The batch to update.</param>
        public void ProcessDownload(TransactionLogTypeBatchDetailDownload downloadDetail, SevisBatchProcessing batch)
        {
            Contract.Requires(batch != null, "The batch must not be null.");
            if (downloadDetail != null)
            {
                batch.DownloadDispositionCode = downloadDetail.DispositionCode.Code;
                batch.RetrieveDate = DateTimeOffset.UtcNow;

                if (downloadDetail.DispositionCode != DispositionCode.Success)
                {
                    batch.DownloadTries++;
                    batch.LastDownloadTry = DateTimeOffset.UtcNow;
                }
                notificationService.NotifyDownloadedBatchProcessed(batch.BatchId, downloadDetail.DispositionCode);
            }
        }

        /// <summary>
        /// Updates the participants and dependents with the given process details from the transaction log.
        /// </summary>
        /// <param name="user">The user processing the transaction log.</param>
        /// <param name="process">The process details from the transaction log.</param>
        /// <param name="fileProvider">The file provider.</param>
        /// <param name="batch">The batch to update.</param>
        public void ProcessBatchDetailProcess(User user, TransactionLogTypeBatchDetailProcess process, SevisBatchProcessing batch, IDS2019FileProvider fileProvider)
        {
            Contract.Requires(batch != null, "The batch must not be null.");
            Contract.Requires(batch.SendString != null, "The batch send string must not be null.");
            if (process != null)
            {
                DoNotifyStartedProcessingBatchDetailProcessed(batch, process);
                var dispositionCode = DispositionCode.ToDispositionCode(process.resultCode);
                batch.ProcessDispositionCode = dispositionCode.Code;
                var sevisBatchCreateUpdateEV = DeserializeSEVISBatchCreateUpdateEV(batch.SendString);
                foreach (var groupedProcessRecord in process.GetGroupedProcessRecords())
                {
                    int? participantId = null;
                    if (groupedProcessRecord.IsParticipant)
                    {
                        var participant = CreateGetParticipantAndDependentsQuery(groupedProcessRecord.ObjectId).FirstOrDefault();
                        var participantPerson = Context.ParticipantPersons.Find(groupedProcessRecord.ObjectId);
                        var dependents = participant.Person.Family.ToList();
                        participantId = participant.ParticipantId;
                        UpdateParticipant(user, participantPerson, dependents, sevisBatchCreateUpdateEV, groupedProcessRecord, batch);
                        UploadDS2019(groupedProcessRecord, participantPerson, fileProvider);
                    }
                    else if (groupedProcessRecord.IsPersonDependent)
                    {
                        var dependent = Context.PersonDependents.Find(groupedProcessRecord.ObjectId);
                        var participantDTO = CreateGetSimplePersonDTOsByParticipantIdQuery(dependent.PersonId).FirstOrDefault();
                        Contract.Assert(participantDTO.ParticipantId.HasValue, "The participant dto should have a participant id.");
                        var participant = CreateGetParticipantAndDependentsQuery(participantDTO.ParticipantId.Value).FirstOrDefault();
                        var participantPerson = participant.ParticipantPerson;
                        var dependents = participant.Person.Family.ToList();
                        participantId = participant.ParticipantId;
                        UpdateParticipant(user, participantPerson, dependents, sevisBatchCreateUpdateEV, groupedProcessRecord, batch);
                        UploadDS2019(groupedProcessRecord, dependent, fileProvider);
                    }
                    Contract.Assert(participantId.HasValue, "The participant id must have a value.");
                    if (participantId.HasValue && dispositionCode == DispositionCode.Success)
                    {
                        var history = Context.ExchangeVisitorHistories.Find(participantId.Value);
                        PromotePendingModelToLastSuccessfulModel(history);
                    }
                }
                notificationService.NotifyFinishedProcessingSevisBatchDetails(batch.BatchId, process.DispositionCode);
            }
        }

        /// <summary>
        /// Updates the participants and dependents with the given process details from the transaction log.
        /// </summary>
        /// <param name="user">The user processing the transaction log.</param>
        /// <param name="process">The process details from the transaction log.</param>
        /// <param name="fileProvider">The file provider.</param>
        /// <param name="batch">The batch to update.</param>
        public async Task ProcessBatchDetailProcessAsync(User user, TransactionLogTypeBatchDetailProcess process, SevisBatchProcessing batch, IDS2019FileProvider fileProvider)
        {
            Contract.Requires(batch != null, "The batch must not be null.");
            Contract.Requires(batch.SendString != null, "The batch send string must not be null.");
            if (process != null)
            {
                DoNotifyStartedProcessingBatchDetailProcessed(batch, process);
                var dispositionCode = DispositionCode.ToDispositionCode(process.resultCode);
                batch.ProcessDispositionCode = dispositionCode.Code;
                var sevisBatchCreateUpdateEV = DeserializeSEVISBatchCreateUpdateEV(batch.SendString);
                foreach (var groupedProcessRecord in process.GetGroupedProcessRecords())
                {
                    int? participantId = null;
                    if (groupedProcessRecord.IsParticipant)
                    {
                        var participant = await CreateGetParticipantAndDependentsQuery(groupedProcessRecord.ObjectId).FirstOrDefaultAsync();
                        var participantPerson = participant.ParticipantPerson;
                        var dependents = participant.Person.Family.ToList();
                        participantId = participant.ParticipantId;
                        UpdateParticipant(user, participantPerson, dependents, sevisBatchCreateUpdateEV, groupedProcessRecord, batch);
                        await UploadDS2019Async(groupedProcessRecord, participantPerson, fileProvider);
                    }
                    else if (groupedProcessRecord.IsPersonDependent)
                    {
                        var dependent = await Context.PersonDependents.FindAsync(groupedProcessRecord.ObjectId);
                        var participantDTO = await CreateGetSimplePersonDTOsByParticipantIdQuery(dependent.PersonId).FirstOrDefaultAsync();
                        Contract.Assert(participantDTO.ParticipantId.HasValue, "The participant dto should have a participant id.");
                        var participant = await CreateGetParticipantAndDependentsQuery(participantDTO.ParticipantId.Value).FirstOrDefaultAsync();
                        var participantPerson = participant.ParticipantPerson;
                        var dependents = participant.Person.Family.ToList();
                        participantId = participant.ParticipantId;
                        UpdateParticipant(user, participantPerson, dependents, sevisBatchCreateUpdateEV, groupedProcessRecord, batch);
                        await UploadDS2019Async(groupedProcessRecord, dependent, fileProvider);
                    }
                    Contract.Assert(participantId.HasValue, "The participant id must have a value.");
                    if (participantId.HasValue && dispositionCode == DispositionCode.Success)
                    {
                        var history = await Context.ExchangeVisitorHistories.FindAsync(participantId.Value);
                        PromotePendingModelToLastSuccessfulModel(history);
                    }
                }
                notificationService.NotifyFinishedProcessingSevisBatchDetails(batch.BatchId, process.DispositionCode);
            }
        }

        private void DoNotifyStartedProcessingBatchDetailProcessed(SevisBatchProcessing batch, TransactionLogTypeBatchDetailProcess process)
        {
            Contract.Requires(batch != null, "The batch must not be null.");
            if (process != null && process.RecordCount != null && batch != null)
            {
                notificationService.NotifyStartedProcessingSevisBatchDetails(batch.BatchId, Int32.Parse(process.RecordCount.Success), Int32.Parse(process.RecordCount.Failure));
            }
        }

        /// <summary>
        /// Creates a json string from the given result type.
        /// </summary>
        /// <param name="resultType">The result type.</param>
        /// <returns>The json string.</returns>
        public string GetSevisBatchResultTypeAsJson(ResultType resultType)
        {
            Contract.Requires(resultType != null, "The result type must not be null.");
            return GetSevisBatchResultTypeAsJson(new List<ResultType> { resultType });
        }

        /// <summary>
        /// Creates a json string from the given result types.
        /// </summary>
        /// <param name="resultType">The result types.</param>
        /// <returns>The json string.</returns>
        public string GetSevisBatchResultTypeAsJson(IEnumerable<ResultType> resultTypes)
        {
            Contract.Requires(resultTypes != null, "The result types must not be null.");
            var list = new List<SimpleSevisBatchErrorResult>();
            foreach (var resultType in resultTypes)
            {
                if (!resultType.status)
                {
                    list.Add(new SimpleSevisBatchErrorResult(resultType));
                }
            }
            return JsonConvert.SerializeObject(list, GetSerializerSettings());
        }

        /// <summary>
        /// Returns the serializer settings for the json serializer.
        /// </summary>
        /// <returns>The settings for the json serializer.</returns>
        private static JsonSerializerSettings GetSerializerSettings()
        {
            return new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
        }

        /// <summary>
        /// Adds a sevis comm status to the participant based on the sevis transaction log result type.
        /// </summary>
        /// <param name="resultType">The transaction log result.</param>
        /// <param name="participantPerson">The participant to add the status to.</param>
        /// <param name="batch">The batch that the update belongs to.</param>
        /// <param name="requestId">The request id.</param>
        /// <returns>The new sevis comm status added to the participant.</returns>
        public ParticipantPersonSevisCommStatus AddResultTypeSevisCommStatus(RequestId requestId, ResultType resultType, ParticipantPerson participantPerson, SevisBatchProcessing batch)
        {
            Contract.Requires(resultType != null, "The result type must not be null.");
            Contract.Requires(participantPerson != null, "The participant person must not be null.");
            int commStatusId = 0;
            if (resultType.status)
            {
                if (!String.IsNullOrWhiteSpace(participantPerson.SevisId))
                {
                    if (requestId.RequestIdType == RequestIdType.Validate)
                    {
                        commStatusId = SevisCommStatus.ValidatedByBatch.Id;
                    }
                    else
                    {
                        commStatusId = SevisCommStatus.UpdatedByBatch.Id;
                    }
                }
                else
                {
                    commStatusId = SevisCommStatus.CreatedByBatch.Id;
                }
            }
            else
            {
                if (requestId.RequestIdType == RequestIdType.Validate)
                {
                    commStatusId = SevisCommStatus.NeedsValidationInfo.Id;
                }
                else
                {
                    commStatusId = SevisCommStatus.InformationRequired.Id;
                }
            }
            var sevisCommStatus = new ParticipantPersonSevisCommStatus
            {
                AddedOn = DateTimeOffset.UtcNow,
                ParticipantId = participantPerson.ParticipantId,
                ParticipantPerson = participantPerson,
                SevisCommStatusId = commStatusId,
                BatchId = batch.BatchId,
            };
            participantPerson.ParticipantPersonSevisCommStatuses.Add(sevisCommStatus);
            Context.ParticipantPersonSevisCommStatuses.Add(sevisCommStatus);
            return sevisCommStatus;
        }

        private IQueryable<SevisBatchProcessing> CreateGetSevisBatchProcessingByBatchIdQuery(string batchId)
        {
            return Context.SevisBatchProcessings.Where(x => x.BatchId == batchId);
        }


        private IQueryable<Participant> CreateGetParticipantAndDependentsQuery(int participantId)
        {
            var query = Context.Participants
                .Include(x => x.Person)
                .Include(x => x.Person.Family)
                .Include(x => x.ParticipantPerson)
                .Where(x => x.ParticipantId == participantId);
            return query;
        }

        private IQueryable<int> CreateGetParticipantIdsByBatchId(string batchId)
        {
            return SevisBatchProcessingQueries.CreateGetParticipantPersonsByBatchId(this.Context, batchId).Select(x => x.ParticipantId);
        }

        private IQueryable<SimplePersonDTO> CreateGetSimplePersonDTOsByParticipantIdQuery(int personId)
        {
            return PersonQueries.CreateGetSimplePersonDTOsQuery(this.Context).Where(x => x.PersonId == personId);
        }

        private IQueryable<int> CreateGetParticipantIdsWhoNeedSuccessfulUploadStatus(string batchId)
        {
            var query = from participantPerson in Context.ParticipantPersons

                        let statuses = participantPerson.ParticipantPersonSevisCommStatuses
                        let hasStatusesRelatedToBatch = statuses.Where(x => x.BatchId == batchId).Count() > 0
                        let hasSentByBatchStatus = statuses.Where(x => x.BatchId == batchId && x.SevisCommStatusId == SevisCommStatus.SentByBatch.Id).Count() > 0
                        where hasStatusesRelatedToBatch && !hasSentByBatchStatus
                        select participantPerson.ParticipantId;
            return query.Distinct();
        }

        #endregion

        #region Update
        /// <summary>
        /// Updates a participant and dependent information.
        /// </summary>
        /// <param name="user">The user performing the updates.</param>
        /// <param name="participantPerson">The participant to be updated.</param>
        /// <param name="dependents">The participant's family/dependents.</param>
        /// <param name="createUpdateEVBatch">The create update batch.</param>
        /// <param name="groupedDetailProcessBatch">The grouped process records.</param>
        /// <param name="batch">The sevis batch.</param>
        public void UpdateParticipant(
            User user,
            ParticipantPerson participantPerson,
            List<PersonDependent> dependents,
            SEVISBatchCreateUpdateEV createUpdateEVBatch,
            GroupedTransactionLogTypeBatchDetailProcess groupedDetailProcessBatch,
            SevisBatchProcessing batch)
        {
            Contract.Requires(user != null, "The user must not be null.");
            Contract.Requires(participantPerson != null, "The participant person must not be null.");
            Contract.Requires(groupedDetailProcessBatch.ObjectId == participantPerson.ParticipantId, "The grouped process records must be for the given participant.");
            Contract.Requires(groupedDetailProcessBatch.IsParticipant, "The process records must be for a participant.");
            var success = groupedDetailProcessBatch.AllRecordsSuccessful();
            var resultTypes = groupedDetailProcessBatch.Records.Select(x => x.Result).ToList();
            var sevisBatchResult = GetSevisBatchResultTypeAsJson(resultTypes);
            participantPerson.SevisBatchResult = sevisBatchResult;
            var update = new Update(user);
            update.SetHistory(participantPerson);
            var requestIds = groupedDetailProcessBatch.Records.Select(x => x.GetRequestId()).ToList();
            int sevisCommStatusId = GetSevisCommStatusByRequestIds(success, requestIds);
            Contract.Assert(sevisCommStatusId != 0, "The sevis comm status must be set.");
            if (success)
            {   
                if (groupedDetailProcessBatch.IsParticipant)
                {
                    foreach (var record in groupedDetailProcessBatch.Records)
                    {
                        if (!String.IsNullOrWhiteSpace(record.sevisID))
                        {
                            participantPerson.SevisId = record.sevisID;
                        }
                        if (record.Dependent != null)
                        {
                            foreach (var dependentRecord in record.Dependent)
                            {
                                var participantSevisKey = new ParticipantSevisKey(dependentRecord);
                                var dependentToUpdate = dependents.Where(x => x.DependentId == participantSevisKey.PersonId).FirstOrDefault();
                                DoUpdateDependent(dependentToUpdate, update, dependentRecord.dependentSevisID, false);
                            }
                        }
                    }
                }
                else if (groupedDetailProcessBatch.IsPersonDependent)
                {
                    foreach (var record in groupedDetailProcessBatch.Records)
                    {
                        var requestId = record.GetRequestId();
                        Contract.Assert(requestId.IsPersonDependentId, "The request id should be for a dependent.");
                        var dependentToUpdate = dependents.Where(x => x.DependentId == requestId.Id).FirstOrDefault();
                        DoUpdateDependent(dependentToUpdate, update, record.sevisID, createUpdateEVBatch.ContainsDeletedParticipantDependent(record.sevisID));
                    }
                }
            }
            var participantCommStatus = new ParticipantPersonSevisCommStatus
            {
                AddedOn = DateTimeOffset.UtcNow,
                BatchId = batch.BatchId,
                ParticipantId = participantPerson.ParticipantId,
                ParticipantPerson = participantPerson,
                SevisCommStatusId = sevisCommStatusId
            };
            participantPerson.ParticipantPersonSevisCommStatuses.Add(participantCommStatus);
            Context.ParticipantPersonSevisCommStatuses.Add(participantCommStatus);
        }

        private int GetSevisCommStatusByRequestIds(bool isSuccess, IEnumerable<RequestId> requestIds)
        {
            if (isSuccess)
            {
                if (requestIds.Where(x => x.RequestIdType == RequestIdType.Validate).Count() > 0)
                {
                    return SevisCommStatus.ValidatedByBatch.Id;
                }
                //remember we could be creating a new dependent on an existing exchange visitor - so we'll have a create and an update if we dont check
                //the EV sevis id
                else if (requestIds.Where(x => x.RequestActionType == RequestActionType.Create && x.IsParticipantId).Count() > 0)
                {
                    return SevisCommStatus.CreatedByBatch.Id;
                }
                else if (requestIds.Where(x => x.RequestActionType == RequestActionType.Create && x.IsPersonDependentId).Count() > 0)
                {
                    return SevisCommStatus.UpdatedByBatch.Id;
                }
                else if (requestIds.Where(x => x.RequestActionType == RequestActionType.Update).Count() > 0)
                {
                    return SevisCommStatus.UpdatedByBatch.Id;
                }
                else
                {
                    throw new NotSupportedException("The sevis comm status id could not be determined by the request ids.");
                }
            }
            else
            {
                if (requestIds.Where(x => x.RequestIdType == RequestIdType.Validate).Count() > 0)
                {
                    return SevisCommStatus.NeedsValidationInfo.Id;
                }
                else
                {
                    return SevisCommStatus.InformationRequired.Id;
                }
            }
        }

        private void DoUpdateDependent(PersonDependent dependentToUpdate, Update update, string sevisId, bool isSevisDeleted)
        {
            dependentToUpdate.SevisId = sevisId;
            dependentToUpdate.IsSevisDeleted = isSevisDeleted;
            update.SetHistory(dependentToUpdate);
        }

        #endregion

        #region DS2019 Uploads
        private async Task UploadDS2019Async(GroupedTransactionLogTypeBatchDetailProcess groupedProcessRecord, IDS2019Fileable fileable, IDS2019FileProvider fileProvider)
        {
            Contract.Requires(groupedProcessRecord != null, "The groupedProcessRecord must not be null.");
            Contract.Requires(fileProvider != null, "The file provider must not be null.");
            Contract.Requires(fileable != null, "The fileable must not be null.");
            if (groupedProcessRecord.AllRecordsSuccessful())
            {
                Func<RequestId, string, Task> doUpload = async (rId, sevId) =>
                {
                    var stream = await fileProvider.GetDS2019FileStreamAsync(rId, sevId);
                    if (stream != null)
                    {
                        using (stream)
                        {
                            var url = await SaveDS2019FormAsync(fileable, stream);
                            fileable.DS2019FileName = url;
                        }
                    }
                };
                foreach (var record in groupedProcessRecord.Records)
                {
                    var requestId = record.GetRequestId();
                    await doUpload(requestId, record.sevisID);
                    if (record.Dependent != null)
                    {
                        foreach (var dependentRecord in record.Dependent)
                        {
                            await doUpload(requestId, dependentRecord.dependentSevisID);
                        }
                    }
                }
            }
        }

        private void UploadDS2019(GroupedTransactionLogTypeBatchDetailProcess groupedProcessRecord, IDS2019Fileable fileable, IDS2019FileProvider fileProvider)
        {
            Contract.Requires(groupedProcessRecord != null, "The groupedProcessRecord must not be null.");
            Contract.Requires(fileProvider != null, "The file provider must not be null.");
            Contract.Requires(fileable != null, "The fileable must not be null.");
            if (groupedProcessRecord.AllRecordsSuccessful())
            {
                Action<RequestId, string> doUpload = (rId, sevId) =>
                {
                    var stream = fileProvider.GetDS2019FileStream(rId, sevId);
                    if (stream != null)
                    {
                        using (stream)
                        {
                            var url = SaveDS2019Form(fileable, stream);
                            fileable.DS2019FileName = url;
                        }
                    }
                };
                foreach (var record in groupedProcessRecord.Records)
                {
                    var requestId = record.GetRequestId();
                    doUpload(requestId, record.sevisID);
                    if (record.Dependent != null)
                    {
                        foreach (var dependentRecord in record.Dependent)
                        {
                            doUpload(requestId, dependentRecord.dependentSevisID);
                        }
                    }

                }
            }
        }
        #endregion

        #region XML Serialization
        /// <summary>
        /// Deserializes the given xml into a TransactionLogType instance.  It strips PhysicalCorrectedAddress from the transaction logs.
        /// </summary>
        /// <param name="xml">The xml to deserialize.</param>
        /// <returns>The transaction log.</returns>
        public TransactionLogType DeserializeTransactionLogType(string xml)
        {
            Contract.Requires(xml != null, "The xml must not be null.");
            var sb = new StringBuilder();
            var root = XElement.Parse(xml);
            var physicalCorrectedAddresses = root.Descendants(PHYSICAL_CORRECTED_ADDRESS_ELEMENT_NAME).ToList();
            if (physicalCorrectedAddresses.Count > 0)
            {
                root.AddFirst(new XComment(PHYSICAL_CORRECTED_ADDRESSES_COMMENTED_MESSAGE));
            }
            foreach (var physicalCorrectedAddress in physicalCorrectedAddresses)
            {
                physicalCorrectedAddress.ReplaceWith(new XComment(physicalCorrectedAddress.ToString()));
            }

            XmlWriterSettings xws = new XmlWriterSettings();
            xws.OmitXmlDeclaration = true;
            xws.Indent = true;
            using (var writer = XmlWriter.Create(sb, xws))
            {
                root.WriteTo(writer);
            }

            using (var memoryStream = new MemoryStream())
            using (var stringReader = new StringReader(sb.ToString()))
            {
                var serializer = new XmlSerializer(typeof(TransactionLogType));
                var transactionLogType = (TransactionLogType)serializer.Deserialize(stringReader);
                return transactionLogType;
            }
        }

        private SEVISBatchCreateUpdateEV DeserializeSEVISBatchCreateUpdateEV(string xml)
        {
            Contract.Requires(xml != null, "The xml must not be null.");
            using (var memoryStream = new MemoryStream())
            using (var stringReader = new StringReader(xml))
            {
                var serializer = new XmlSerializer(typeof(SEVISBatchCreateUpdateEV));
                var createUpdateBatch = (SEVISBatchCreateUpdateEV)serializer.Deserialize(stringReader);
                return createUpdateBatch;
            }
        }

        #endregion

        #region History

        /// <summary>
        /// Promotes the given history's pending model to the last successful model.
        /// </summary>
        /// <param name="history">The history to update.</param>
        public void PromotePendingModelToLastSuccessfulModel(ExchangeVisitorHistory history)
        {
            history.LastSuccessfulModel = history.PendingModel;
            history.PendingModel = null;
            history.RevisedOn = DateTimeOffset.UtcNow;
        }

        /// <summary>
        /// Adds or updates an ExchangeVisitorHistory for the given exchange visitor.
        /// </summary>
        /// <param name="exchangeVisitor">The exchange visitor.</param>
        /// <returns>The added or updated exchange visitor history.</returns>
        public ExchangeVisitorHistory AddOrUpdateStagedExchangeVisitorHistory(ExchangeVisitor exchangeVisitor)
        {
            var existingHistory = Context.ExchangeVisitorHistories.Find(exchangeVisitor.Person.ParticipantId);
            var historyToReturn = DoAddOrUpdateStagedExchangeVisitorHistory(exchangeVisitor, existingHistory);
            this.Context.SaveChanges();
            return historyToReturn;
        }

        /// <summary>
        /// Adds or updates an ExchangeVisitorHistory for the given exchange visitor.
        /// </summary>
        /// <param name="exchangeVisitor">The exchange visitor.</param>
        /// <returns>The added or updated exchange visitor history.</returns>
        public async Task<ExchangeVisitorHistory> AddOrUpdateStagedExchangeVisitorHistoryAsync(ExchangeVisitor exchangeVisitor)
        {
            var existingHistory = await Context.ExchangeVisitorHistories.FindAsync(exchangeVisitor.Person.ParticipantId);
            var historyToReturn = DoAddOrUpdateStagedExchangeVisitorHistory(exchangeVisitor, existingHistory);
            await this.Context.SaveChangesAsync();
            return historyToReturn;
        }

        private ExchangeVisitorHistory DoAddOrUpdateStagedExchangeVisitorHistory(ExchangeVisitor exchangeVisitor, ExchangeVisitorHistory existingHistory)
        {
            if (existingHistory == null)
            {
                var newHistory = new ExchangeVisitorHistory
                {
                    ParticipantId = exchangeVisitor.Person.ParticipantId,
                    PendingModel = exchangeVisitor.ToJson(),
                    RevisedOn = DateTimeOffset.UtcNow
                };
                Context.ExchangeVisitorHistories.Add(newHistory);
                return newHistory;
            }
            else
            {
                existingHistory.PendingModel = exchangeVisitor.ToJson();
                existingHistory.RevisedOn = DateTimeOffset.UtcNow;
                return existingHistory;
            }
        }

        #endregion

        #region Staging
        /// <summary>
        /// Stages all queued to submit sevis participants into sevis batches that can then be sent to sevis for processing.
        /// </summary>
        /// <param name="user">The user performing the staging.</param>
        /// <returns>The list of staged sevis batches.</returns>
        public List<StagedSevisBatch> StageBatches()
        {
            var stagedSevisBatches = new List<StagedSevisBatch>();
            var skip = 0;
            var queuedToSubmitParticipantGroupsCount = SevisBatchProcessingQueries.CreateGetSevisGroupedParticipantsQuery(this.Context).Count();
            var totalParticipantsToStage = SevisBatchProcessingQueries.CreateGetSevisGroupedParticipantsQuery(this.Context).SelectMany(x => x.Participants).Count();
            notificationService.NotifyNumberOfParticipantsToStage(totalParticipantsToStage);
            while (queuedToSubmitParticipantGroupsCount > 0)
            {
                StagedSevisBatch stagedSevisBatch = null;
                var groupedParticipants = SevisBatchProcessingQueries.CreateGetSevisGroupedParticipantsQuery(this.Context)
                    .Skip(() => skip)
                    .Take(() => QUERY_BATCH_SIZE)
                    .ToList();

                foreach (var groupedParticipant in groupedParticipants)
                {
                    foreach (var participant in groupedParticipant.Participants)
                    {
                        var exchangeVisitor = this.exchangeVisitorService.GetExchangeVisitor(participant.ProjectId, participant.ParticipantId);
                        var results = exchangeVisitor.Validate(this.exchangeVisitorValidationService.GetValidator());
                        if (results.IsValid)
                        {
                            ExchangeVisitor previouslySubmittedExchangeVisitor = null;
                            if (!String.IsNullOrWhiteSpace(exchangeVisitor.SevisId))
                            {
                                var exchangeVisitorHistory = Context.ExchangeVisitorHistories.Find(exchangeVisitor.Person.ParticipantId);
                                previouslySubmittedExchangeVisitor = ExchangeVisitor.GetExchangeVisitor(exchangeVisitorHistory.LastSuccessfulModel);
                            }
                            var accomodatingSevisBatch = GetAccomodatingStagedSevisBatch(stagedSevisBatches, participant, exchangeVisitor, previouslySubmittedExchangeVisitor, groupedParticipant.SevisUsername, groupedParticipant.SevisOrgId);
                            if (accomodatingSevisBatch != null)
                            {
                                stagedSevisBatch = accomodatingSevisBatch;
                            }
                            else
                            {
                                PersistStagedSevisBatches(stagedSevisBatches);
                                stagedSevisBatch = getNewStagedSevisBatch(groupedParticipant.SevisUsername, groupedParticipant.SevisOrgId, stagedSevisBatches);
                                this.notificationService.NotifyStagedSevisBatchCreated(stagedSevisBatch);
                            }
                            stagedSevisBatch.AddExchangeVisitor(participant, exchangeVisitor, previouslySubmittedExchangeVisitor);
                            AddPendingSendToSevisStatus(participant.ParticipantId, stagedSevisBatch.BatchId, groupedParticipant.SevisUsername, groupedParticipant.SevisOrgId);
                            AddOrUpdateStagedExchangeVisitorHistory(exchangeVisitor);
                        }
                        else
                        {
                            HandleInvalidExchangeVisitor(participant.ProjectId, participant.ParticipantId);
                            notificationService.NotifyInvalidExchangeVisitor(exchangeVisitor);
                        }
                    }
                    skip++;
                    queuedToSubmitParticipantGroupsCount--;
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
        public async Task<List<StagedSevisBatch>> StageBatchesAsync()
        {
            var stagedSevisBatches = new List<StagedSevisBatch>();
            var skip = 0;
            var queuedToSubmitParticipantGroupsCount = await SevisBatchProcessingQueries.CreateGetSevisGroupedParticipantsQuery(this.Context).CountAsync();
            var totalParticipantsToStage = await SevisBatchProcessingQueries.CreateGetSevisGroupedParticipantsQuery(this.Context).SelectMany(x => x.Participants).CountAsync();
            notificationService.NotifyNumberOfParticipantsToStage(totalParticipantsToStage);
            while (queuedToSubmitParticipantGroupsCount > 0)
            {
                StagedSevisBatch stagedSevisBatch = null;
                var groupedParticipants = await SevisBatchProcessingQueries.CreateGetSevisGroupedParticipantsQuery(this.Context)
                    .Skip(() => skip)
                    .Take(() => QUERY_BATCH_SIZE)
                    .ToListAsync();
                foreach (var groupedParticipant in groupedParticipants)
                {
                    foreach (var participant in groupedParticipant.Participants)
                    {
                        var exchangeVisitor = await this.exchangeVisitorService.GetExchangeVisitorAsync(participant.ProjectId, participant.ParticipantId);
                        var results = exchangeVisitor.Validate(this.exchangeVisitorValidationService.GetValidator());
                        if (results.IsValid)
                        {
                            ExchangeVisitor previouslySubmittedExchangeVisitor = null;
                            if (!String.IsNullOrWhiteSpace(exchangeVisitor.SevisId))
                            {
                                var exchangeVisitorHistory = await Context.ExchangeVisitorHistories.FindAsync(exchangeVisitor.Person.ParticipantId);
                                previouslySubmittedExchangeVisitor = ExchangeVisitor.GetExchangeVisitor(exchangeVisitorHistory.LastSuccessfulModel);
                            }
                            var accomodatingSevisBatch = GetAccomodatingStagedSevisBatch(stagedSevisBatches, participant, exchangeVisitor, previouslySubmittedExchangeVisitor, groupedParticipant.SevisUsername, groupedParticipant.SevisOrgId);
                            if (accomodatingSevisBatch != null)
                            {
                                stagedSevisBatch = accomodatingSevisBatch;
                            }
                            else
                            {
                                await PersistStagedSevisBatchesAsync(stagedSevisBatches);
                                stagedSevisBatch = getNewStagedSevisBatch(groupedParticipant.SevisUsername, groupedParticipant.SevisOrgId, stagedSevisBatches);
                                this.notificationService.NotifyStagedSevisBatchCreated(stagedSevisBatch);
                            }
                            stagedSevisBatch.AddExchangeVisitor(participant, exchangeVisitor, previouslySubmittedExchangeVisitor);
                            AddPendingSendToSevisStatus(participant.ParticipantId, stagedSevisBatch.BatchId, groupedParticipant.SevisUsername, groupedParticipant.SevisOrgId);
                            await AddOrUpdateStagedExchangeVisitorHistoryAsync(exchangeVisitor);
                        }
                        else
                        {
                            await HandleInvalidExchangeVisitorAsync(participant.ProjectId, participant.ParticipantId);
                            notificationService.NotifyInvalidExchangeVisitor(exchangeVisitor);
                        }
                        skip++;
                        queuedToSubmitParticipantGroupsCount--;
                    }
                }
            }
            await PersistStagedSevisBatchesAsync(stagedSevisBatches);
            this.notificationService.NotifyStagedSevisBatchesFinished(stagedSevisBatches);
            return stagedSevisBatches;
        }

        private void HandleInvalidExchangeVisitor(int projectId, int participantId)
        {
            this.exchangeVisitorValidationService.RunParticipantSevisValidation(projectId, participantId);
            this.exchangeVisitorValidationService.SaveChanges();
        }

        private async Task HandleInvalidExchangeVisitorAsync(int projectId, int participantId)
        {
            await this.exchangeVisitorValidationService.RunParticipantSevisValidationAsync(projectId, participantId);
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
        /// <param name="sevisOrgId">The sevis org id.</param>
        /// <param name="sevisUsername">The sevis username.</param>
        /// <param name="visitor">The exchange visitor.</param>
        /// <param name="participant">The grouped participant representing the exchange visitor.</param>
        /// <param name="previouslySubmittedExchangeVisitor">The previously sent exchange visitor.</param>
        /// <returns>The first staged sevis batch that can accomodate the visitor, or null if none can accomodate.</returns>
        public StagedSevisBatch GetAccomodatingStagedSevisBatch(
            List<StagedSevisBatch> batches,
            SevisGroupedParticipantDTO participant,
            ExchangeVisitor visitor,
            ExchangeVisitor previouslySubmittedExchangeVisitor,
            string sevisUsername,
            string sevisOrgId)
        {
            Contract.Requires(batches != null, "The batches must not be null.");
            Contract.Requires(visitor != null, "The exchange visitor must not be null.");
            Contract.Requires(sevisOrgId != null, "The sevis org id must not be null.");
            Contract.Requires(participant != null, "The participant must not be null.");
            Contract.Requires(participant.ParticipantId == visitor.Person.ParticipantId, "The participant must belong to the exchange visitor.");
            foreach (var batch in batches)
            {
                if (!batch.IsSaved && batch.CanAccomodate(participant, visitor, sevisUsername, sevisOrgId, previouslySubmittedExchangeVisitor))
                {
                    return batch;
                }
            }
            return null;
        }

        private ParticipantPersonSevisCommStatus AddPendingSendToSevisStatus(int participantId, BatchId batchId, string sevisUsername, string sevisOrgId)
        {
            var sevisCommStatus = new ParticipantPersonSevisCommStatus
            {
                ParticipantId = participantId,
                AddedOn = DateTimeOffset.UtcNow,
                SevisCommStatusId = SevisCommStatus.PendingSevisSend.Id,
                BatchId = batchId.ToString(),
                SevisUsername = sevisUsername,
                SevisOrgId = sevisOrgId
            };
            Context.ParticipantPersonSevisCommStatuses.Add(sevisCommStatus);
            return sevisCommStatus;
        }


        private StagedSevisBatch GetNewStagedSevisBatch(string sevisUsername, string sevisOrgId)
        {
            var stagedSevisBatch = new StagedSevisBatch(
                sevisUsername: sevisUsername,
                batchId: BatchId.NewBatchId(),
                sevisOrgId: sevisOrgId,
                maxCreateExchangeVisitorRecordsPerBatch: this.MaxCreateExchangeVisitorRecordsPerBatch,
                maxUpdateExchangeVisitorRecordPerBatch: this.MaxUpdateExchangeVisitorRecordsPerBatch);
            Context.SevisBatchProcessings.Add(stagedSevisBatch.SevisBatchProcessing);
            return stagedSevisBatch;
        }
        #endregion

        #region DS2019
        /// <summary>
        /// Saves the DS2019 form given by sevis.
        /// </summary>
        /// <param name="participantId">The participant id.</param>
        /// <param name="sevisId">The sevis id.</param>
        /// <param name="stream">The file stream.</param>
        /// <returns>The name of the saved file.</returns>
        public string SaveDS2019Form(IDS2019Fileable fileable, Stream stream)
        {
            Contract.Requires(fileable != null, "The fileable must not be null.");
            Contract.Requires(stream != null, "The stream must not be null.");
            var fileName = fileable.GetDS2019FileName();
            var container = appSettings.DS2019FileStorageContainer;
            this.cloudStorageService.UploadBlob(stream, DS2019_CONTENT_TYPE, fileName, container);
            return fileName;
        }

        /// <summary>
        /// Saves the DS2019 form given by sevis.
        /// </summary>
        /// <param name="participantId">The participant id.</param>
        /// <param name="sevisId">The sevis id.</param>
        /// <param name="stream">The file stream.</param>
        /// /// <returns>The name of the saved file.</returns>
        public async Task<string> SaveDS2019FormAsync(IDS2019Fileable fileable, Stream stream)
        {
            Contract.Requires(fileable != null, "The fileable must not be null.");
            Contract.Requires(stream != null, "The stream must not be null.");
            var fileName = fileable.GetDS2019FileName();
            var container = appSettings.DS2019FileStorageContainer;
            await cloudStorageService.UploadBlobAsync(stream, DS2019_CONTENT_TYPE, fileName, container);
            return fileName;
        }

        #endregion

        #region Delete
        /// <summary>
        /// Deletes all processed batches from the context whose retrieval date is older than the number of days to keep.
        /// </summary>
        /// <returns>The task.</returns>
        public void DeleteProcessedBatches()
        {
            var count = CreateGetProcessedSevisBatchIdsForDeletionQuery().Count();
            while (count > 0)
            {
                var ids = CreateGetProcessedSevisBatchIdsForDeletionQuery().Take(() => QUERY_BATCH_SIZE).ToList();
                foreach (var id in ids)
                {
                    var batch = Context.SevisBatchProcessings.Find(id);
                    var batchId = batch.BatchId;
                    Context.SevisBatchProcessings.Remove(batch);
                    this.notificationService.NotifyDeletedSevisBatchProcessing(id, batchId);
                }
                Context.SaveChanges();
                count = CreateGetProcessedSevisBatchIdsForDeletionQuery().Count();
            }
        }

        /// <summary>
        /// Deletes all processed batches from the context whose retrieval date is older than the number of days to keep.
        /// </summary>
        /// <returns>The task.</returns>
        public async Task DeleteProcessedBatchesAsync()
        {
            var count = await CreateGetProcessedSevisBatchIdsForDeletionQuery().CountAsync();
            while (count > 0)
            {
                var ids = await CreateGetProcessedSevisBatchIdsForDeletionQuery().Take(() => QUERY_BATCH_SIZE).ToListAsync();
                foreach (var id in ids)
                {
                    var batch = await Context.SevisBatchProcessings.FindAsync(id);
                    var batchId = batch.BatchId;
                    Context.SevisBatchProcessings.Remove(batch);
                    this.notificationService.NotifyDeletedSevisBatchProcessing(id, batchId);
                }
                await Context.SaveChangesAsync();
                count = await CreateGetProcessedSevisBatchIdsForDeletionQuery().CountAsync();
            }
        }


        private IQueryable<int> CreateGetProcessedSevisBatchIdsForDeletionQuery()
        {
            var days = Double.Parse(appSettings.NumberOfDaysToKeepProcessedSevisBatchRecords);
            days = days > 0 ? -1.0 * days : days;
            var cutOffDate = DateTimeOffset.UtcNow.AddDays(days);
            return SevisBatchProcessingQueries.CreateGetProcessedSevisBatchIdsForDeletionQuery(this.Context, cutOffDate).OrderBy(x => x);
        }
        #endregion

        #region Dispose
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                if (this.cloudStorageService is IDisposable)
                {
                    ((IDisposable)this.cloudStorageService).Dispose();
                    this.cloudStorageService = null;
                }
                if (this.exchangeVisitorService is IDisposable)
                {
                    ((IDisposable)this.exchangeVisitorService).Dispose();
                    this.exchangeVisitorService = null;
                }
                if (this.exchangeVisitorValidationService is IDisposable)
                {
                    ((IDisposable)this.exchangeVisitorValidationService).Dispose();
                    this.exchangeVisitorValidationService = null;
                }
                if (this.notificationService is IDisposable)
                {
                    ((IDisposable)this.notificationService).Dispose();
                    this.notificationService = null;
                }
            }
        }
        #endregion
    }
}
