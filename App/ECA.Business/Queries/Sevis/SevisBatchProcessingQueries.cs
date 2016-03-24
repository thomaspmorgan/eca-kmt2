using ECA.Business.Queries.Models.Sevis;
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
        private const string successCode = "S0000";
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
            return CreateGetSevisBatchProcessingDTOQuery(context).Where(x => !x.SubmitDate.HasValue);
        }

        /// <summary>
        /// Returns a query to retrieve sevis batch processing dtos that are ready to be submitted to sevis.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <returns>The query to retrieve sevis batch processing dtos that are ready to be submitted to sevis.</returns>
        public static IQueryable<SevisBatchProcessingDTO> CreateGetSevisBatchProcessingDTOsToDownloadQuery(EcaContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            return CreateGetSevisBatchProcessingDTOQuery(context).Where(x => x.SubmitDate.HasValue);
        }

        #endregion

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
