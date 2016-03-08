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

        private const string successCode = "S0000";
        /// <summary>
        /// Returns a query to get SEVIS batch processing dtos from the given context.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <returns>The query to retrieve SEVIS batch processing dtos.</returns>
        public static IEnumerable<SevisBatchProcessingDTO> CreateGetSevisBatchProcessingDTOQuery(EcaContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            return context.SevisBatchProcessings.Select(x => new SevisBatchProcessingDTO
            {
                BatchId = x.BatchId,
                SubmitDate = x.SubmitDate,
                RetrieveDate = x.RetrieveDate,
                SendString = x.SendString,
                TransactionLogString = x.TransactionLogString,
                UploadDispositionCode = x.UploadDispositionCode,
                ProcessDispositionCode = x.ProcessDispositionCode,
                DownloadDispositionCode = x.DownloadDispositionCode
            }).ToList();
        }

        public static SevisBatchProcessingDTO CreateGetSevisBatchProcessingDTOByIdQuery(EcaContext context, int batchId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            return context.SevisBatchProcessings.Where(x => x.BatchId == batchId)
                .Select(x => new SevisBatchProcessingDTO
                {
                    BatchId = x.BatchId,
                    SubmitDate = x.SubmitDate,
                    RetrieveDate = x.RetrieveDate,
                    SendXml = x.SendXml,
                    TransactionLogXml = x.TransactionLogXml,
                    UploadDispositionCode = x.UploadDispositionCode,
                    ProcessDispositionCode = x.ProcessDispositionCode,
                    DownloadDispositionCode = x.DownloadDispositionCode
                }).FirstOrDefault();
        }

        /// <summary>
        /// Returns a query to get the sevis batch dtos.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <returns>The SEVIS batch processing dtos.</returns>
        public static IEnumerable<SevisBatchProcessingDTO> CreateGetSevisBatchProcessingQuery(EcaContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            return CreateGetSevisBatchProcessingDTOQuery(context);
        }

        /// <summary>
        /// Returns a query to get the sevis batch dtos for upload.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <returns>The SEVIS batch processing dtos.</returns>
        public static IEnumerable<SevisBatchProcessingDTO> CreateGetSevisBatchProcessingDTOsForUpload(EcaContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var forUpload = context.SevisBatchProcessings.Where(x => x.SubmitDate == null && x.SendString != null);

            return forUpload.Select(x => new SevisBatchProcessingDTO
            {
                BatchId = x.BatchId,
                SubmitDate = x.SubmitDate,
                RetrieveDate = x.RetrieveDate,
                SendString = x.SendString,
                TransactionLogString = x.TransactionLogString,
                UploadDispositionCode = x.UploadDispositionCode,
                ProcessDispositionCode = x.ProcessDispositionCode,
                DownloadDispositionCode = x.DownloadDispositionCode
            }).AsEnumerable();
        }

        /// <summary>
        /// Returns a query to get the sevis batch dtos for download.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <returns>The SEVIS batch processing dtos.</returns>
        public static IEnumerable<SevisBatchProcessingDTO> CreateGetSevisBatchProcessingDTOsForDownload(EcaContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var forUpload = context.SevisBatchProcessings.Where(x => x.RetrieveDate != null && x.TransactionLogString != null 
                                                                && x.UploadDispositionCode == successCode);

            return forUpload.Select(x => new SevisBatchProcessingDTO
            {
                BatchId = x.BatchId,
                SubmitDate = x.SubmitDate,
                RetrieveDate = x.RetrieveDate,
                SendString = x.SendString,
                TransactionLogString = x.TransactionLogString,
                UploadDispositionCode = x.UploadDispositionCode,
                ProcessDispositionCode = x.ProcessDispositionCode,
                DownloadDispositionCode = x.DownloadDispositionCode
            }).AsEnumerable();
        }

        /// <summary>
        /// Returns a query to get the sevis batch dtos for processing.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <returns>The SEVIS batch processing dtos.</returns>
        public static IEnumerable<SevisBatchProcessingDTO> CreateGetSevisBatchProcessingDTOsToProcess(EcaContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var forUpload = context.SevisBatchProcessings.Where(x => x.DownloadDispositionCode == successCode &&
                                                                     x.ProcessDispositionCode == null);
            return forUpload.Select(x => new SevisBatchProcessingDTO
            {
                BatchId = x.BatchId,
                SubmitDate = x.SubmitDate,
                RetrieveDate = x.RetrieveDate,
                SendString = x.SendString,
                TransactionLogString = x.TransactionLogString,
                UploadDispositionCode = x.UploadDispositionCode,
                ProcessDispositionCode = x.ProcessDispositionCode,
                DownloadDispositionCode = x.DownloadDispositionCode
            }).AsEnumerable();
        }

        /// <summary>
        /// Returns a query to get the sevis batch dto for the sevis batch entity with the given id.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="batchId">The id of the SEVIS batch processing record.</param>
        /// <returns>The SEVIS batch processing dto with the given id.</returns>
        public static SevisBatchProcessingDTO CreateGetSevisBatchProcessingByIdQuery(EcaContext context, int batchId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            return CreateGetSevisBatchProcessingDTOByIdQuery(context, batchId);
        }
    }
}
