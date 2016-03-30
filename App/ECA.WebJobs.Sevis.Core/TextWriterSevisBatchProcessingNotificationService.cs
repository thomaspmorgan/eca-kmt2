using ECA.Business.Service.Sevis;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ECA.Business.Validation.Sevis;
using ECA.Business.Sevis.Model;

namespace ECA.WebJobs.Sevis.Core
{
    /// <summary>
    /// Writes to the console sevis batch processing notification messages.
    /// </summary>
    public class TextWriterSevisBatchProcessingNotificationService : ISevisBatchProcessingNotificationService
    {
        private Stopwatch stagingStopwatch;

        /// <summary>
        /// Writes to the console the disposition code and batch id of the batch whose details were processed.
        /// </summary>
        /// <param name="batchId">The batch id.</param>
        /// <param name="dispositionCode">The disposition code.</param>
        public void NotifyFinishedProcessingSevisBatchDetails(string batchId, DispositionCode dispositionCode)
        {
            Console.WriteLine("The batch details for batch id [{0}] were processed with the disposition code [{1}] ({2}).", batchId, dispositionCode.Code, dispositionCode.Description);
        }

        /// <summary>
        /// Writes to the console the disposition code and batch id of the batch whose download details were processed.
        /// </summary>
        /// <param name="batchId">The batch id.</param>
        /// <param name="dispositionCode">The disposition code.</param>
        public void NotifyDownloadedBatchProcessed(string batchId, DispositionCode dispositionCode)
        {
            Console.WriteLine("The downloaded batch with id [{0}] was processed with the disposition code [{1}] ({2}).", batchId, dispositionCode.Code, dispositionCode.Description);
        }

        /// <summary>
        /// Writes to the console the invalid exchange visitor that failed validation.
        /// </summary>
        /// <param name="exchangeVisitor">The exchange visitor that failed validation.</param>
        public void NotifyInvalidExchangeVisitor(ExchangeVisitor exchangeVisitor)
        {
            var participantId = exchangeVisitor != null && exchangeVisitor.Person != null ? exchangeVisitor.Person.ParticipantId : -1;
            var personId = exchangeVisitor != null && exchangeVisitor.Person != null ? exchangeVisitor.Person.PersonId : -1;
            Console.WriteLine(String.Format("ExchangeVisitor with participant id [{0}] and person id [{1}] failed validation.", participantId, personId));
        }

        /// <summary>
        /// Writes to the console the number of participants that will be staged to sevis batches.
        /// </summary>
        /// <param name="numberOfParticipants">The number of participants.</param>
        public void NotifyNumberOfParticipantsToStage(int numberOfParticipants)
        {
            stagingStopwatch = Stopwatch.StartNew();
            Console.WriteLine(String.Format("Found [{0}] participants to stage for sevis.", numberOfParticipants));
        }

        /// <summary>
        /// Writes to the console the new batch that was created.
        /// </summary>
        /// <param name="batch">The new batch.</param>
        public void NotifyStagedSevisBatchCreated(StagedSevisBatch batch)
        {
            Console.WriteLine(String.Format("A new sevis batch with id [{0}] was created.", batch.BatchId));
        }

        /// <summary>
        /// Writes to the console the number of batches that were successfully staged.
        /// </summary>
        /// <param name="batches">The number of batches.</param>
        public void NotifyStagedSevisBatchesFinished(List<StagedSevisBatch> batches)
        {
            stagingStopwatch.Stop();
            Console.WriteLine(String.Format("Finished staging [{0}] sevis batches in [{1}].", batches.Count(), stagingStopwatch.Elapsed));
        }

        /// <summary>
        /// Writes to the console the disposition code and batch id of the batch whose upload details were processed.
        /// </summary>
        /// <param name="batchId">The batch id.</param>
        /// <param name="dispositionCode">The disposition code.</param>
        public void NotifyUploadedBatchProcessed(string batchId, DispositionCode dispositionCode)
        {
            Console.WriteLine("The uploaded batch with id [{0}] was processed with the disposition code [{1}] ({2}).", batchId, dispositionCode.Code, dispositionCode.Description);
        }

        /// <summary>
        /// Writes to the console a batch that has begun updating participants and how many success and failure records the batch contains.
        /// </summary>
        /// <param name="batchId">The id of the batch.</param>
        /// <param name="successCount">The number of successful batch create or update records.</param>
        /// <param name="errorCount">The number of failed batch create or updates.</param>
        public void NotifyStartedProcessingSevisBatchDetails(string batchId, int successCount, int errorCount)
        {
            Console.WriteLine("Began processing sevis batch with id [{0}].  There are [{1}] successful batch records, and [{2}] failed batch records.", batchId, successCount, errorCount);
        }

        /// <summary>
        /// Writes to the console a batch that has been deleted.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="batchId"></param>
        public void NotifyDeletedSevisBatchProcessing(int id, string batchId)
        {
            Console.WriteLine(String.Format("Deleted sevis processing batch with id [{0}] and batch id [{1}].", id, batchId));
        }
    }
}
