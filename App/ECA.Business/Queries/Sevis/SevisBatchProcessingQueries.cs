using ECA.Business.Queries.Models.Sevis;
using ECA.Business.Sevis.Model;
using ECA.Data;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace ECA.Business.Queries.Sevis
{
    /// <summary>
    /// Contains queries against an ECA Context for SEVIS Batch processing entities.
    /// </summary>
    public static class SevisBatchProcessingQueries
    {
        #region SevisBatchProcessingDTOs
        /// <summary>
        /// Returns a query to get SEVIS batch processing dtos from the given context.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <returns>The query to retrieve SEVIS batch processing dtos.</returns>
        public static IQueryable<SevisBatchProcessingDTO> CreateGetSevisBatchProcessingDTOQuery(EcaContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            return context.SevisBatchProcessings.Select(x => new SevisBatchProcessingDTO
            {
                Id = x.Id,
                BatchId = x.BatchId,
                SubmitDate = x.SubmitDate,
                RetrieveDate = x.RetrieveDate,
                SendString = x.SendString,
                TransactionLogString = x.TransactionLogString,
                UploadDispositionCode = x.UploadDispositionCode,
                ProcessDispositionCode = x.ProcessDispositionCode,
                DownloadDispositionCode = x.DownloadDispositionCode
            });
        }

        /// <summary>
        /// Returns a query to retrieve sevis batch processing dtos that are ready to be submitted to sevis.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <returns>The query to retrieve sevis batch processing dtos that are ready to be submitted to sevis.</returns>
        public static IQueryable<SevisBatchProcessingDTO> CreateGetSevisBatchProcessingDTOsToUploadQuery(EcaContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var generalUploadDownloadFailureCode = DispositionCode.GeneralUploadDownloadFailure.Code;
            var batchNeverSubmittedFailureCode = DispositionCode.BatchNeverSubmitted.Code;
            var query = from dto in CreateGetSevisBatchProcessingDTOQuery(context)
                        where !dto.SubmitDate.HasValue 
                        || dto.UploadDispositionCode == generalUploadDownloadFailureCode
                        || dto.DownloadDispositionCode == batchNeverSubmittedFailureCode
                        orderby dto.Id
                        select dto;
            return query;
        }

        /// <summary>
        /// Returns a query to retrieve sevis batch processing dtos that are ready to be submitted to sevis.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <returns>The query to retrieve sevis batch processing dtos that are ready to be submitted to sevis.</returns>
        public static IQueryable<SevisBatchProcessingDTO> CreateGetSevisBatchProcessingDTOsToDownloadQuery(EcaContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var generalUploadDownloadFailureCode = DispositionCode.GeneralUploadDownloadFailure.Code;
            var batchNotYetProcessedCode = DispositionCode.BatchNotYetProcessed.Code;
            var query = from dto in CreateGetSevisBatchProcessingDTOQuery(context)
                        where !dto.RetrieveDate.HasValue
                        || dto.DownloadDispositionCode == generalUploadDownloadFailureCode
                        || dto.DownloadDispositionCode == batchNotYetProcessedCode
                        orderby dto.Id
                        select dto;
            return query;
        }

        #endregion

        /// <summary>
        /// Returns a query to retrieve participants that are included in a batch with the given batch id.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="batchId">The sevis batch id.</param>
        /// <returns>The ParticipantPersons that are part of a sevis batch with the given id.</returns>
        public static IQueryable<ParticipantPerson> CreateGetParticipantPersonsByBatchId(EcaContext context, string batchId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var query = from commStatus in context.ParticipantPersonSevisCommStatuses
                        where commStatus.BatchId == batchId
                        select commStatus.ParticipantPerson;
            return query.Distinct();
        } 

        /// <summary>
        /// Returns a query to get participants that are queued to submit.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <returns>The query to get participants that are queued to submit.</returns>
        public static IQueryable<ReadyToSubmitParticipantDTO> CreateGetQueuedToSubmitParticipantDTOsQuery(EcaContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var statusId = SevisCommStatus.QueuedToSubmit.Id;

            var query = from participantPerson in context.ParticipantPersons
                        let participant = participantPerson.Participant
                        let latestStatus = participantPerson.ParticipantPersonSevisCommStatuses
                                        .OrderByDescending(x => x.AddedOn)
                                        .FirstOrDefault()

                        where latestStatus.SevisCommStatusId == statusId
                        select new ReadyToSubmitParticipantDTO
                        {
                            ParticipantId = participantPerson.ParticipantId,
                            ProjectId = participant.ProjectId,
                            SevisId = participantPerson.SevisId
                        };
            return query.OrderBy(x => x.ParticipantId);
        }
    }
}
