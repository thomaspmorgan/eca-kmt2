using ECA.Business.Queries.Models.Sevis;
using ECA.Data;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml.Linq;

namespace ECA.Business.Queries.Sevis
{
    /// <summary>
    /// Contains queries against an ECA Context for SEVIS Batch processing entities.
    /// </summary>
    public static class SevisBatchProcessingQueries
    {
        /// <summary>
        /// Returns a query to get SEVIS batch processing dtos from the given context.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <returns>The query to retrieve SEVIS batch processing dtos.</returns>
        public static IQueryable<SevisBatchProcessingDTO> CreateGetSevisBatchProcessingDTOQuery(EcaContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");

            var query = from batch in context.SevisBatchProcessings
                        select new SevisBatchProcessingDTO
                        {
                            BatchId = batch.BatchId,
                            SubmitDate = batch.SubmitDate,
                            RetrieveDate = batch.RetrieveDate,
                            SendString = batch.SendString,
                            TransactionLogString = batch.TransactionLogString,
                            UploadDispositionCode = batch.UploadDispositionCode,
                            ProcessDispositionCode = batch.ProcessDispositionCode,
                            DownloadDispositionCode = batch.DownloadDispositionCode
                        };

            return query;
        }
        
        /// <summary>
        /// Returns a query to get the social media dto for the social media entity with the given id.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="batchId">The id of the SEVIS batch processing record.</param>
        /// <returns>The SEVIS batch processing dto with the given id.</returns>
        public static IQueryable<SevisBatchProcessingDTO> CreateGetSevisBatchProcessingDTOByIdQuery(EcaContext context, int batchId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            return CreateGetSevisBatchProcessingDTOQuery(context).Where(x => x.BatchId == batchId);
        }
    }
}
