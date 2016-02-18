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
        /// <summary>
        /// Returns a query to get SEVIS batch processing dtos from the given context.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <returns>The query to retrieve SEVIS batch processing dtos.</returns>
        public static IEnumerable<SevisBatchProcessingDTO> CreateGetSevisBatchProcessingDTOQuery(EcaContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            //StringBuilder sb = new StringBuilder();
            //sb.Append(@"<root><Process><Record sevisID='N0012309439' requestID='1179' userID='50'>");
            //sb.Append(@"<Result><ErrorCode>S1056</ErrorCode><ErrorMessage>Invalid student visa type for this action</ErrorMessage></Result>");
            //sb.Append(@"</Record></Process></root>");
            //string xmlstring = sb.ToString();

            return context.SevisBatchProcessings.Select(x => new SevisBatchProcessingDTO
            {
                BatchId = x.BatchId,
                SubmitDate = x.SubmitDate,
                RetrieveDate = x.RetrieveDate,
                SendXml = x.SendXml,
                TransactionLogXml = x.TransactionLogXml,
                UploadDispositionCode = x.UploadDispositionCode,
                ProcessDispositionCode = x.ProcessDispositionCode,
                DownloadDispositionCode = x.DownloadDispositionCode
            }).ToList();
        }
        
        /// <summary>
        /// Returns a query to get the social media dto for the social media entity with the given id.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="batchId">The id of the SEVIS batch processing record.</param>
        /// <returns>The SEVIS batch processing dto with the given id.</returns>
        public static IEnumerable<SevisBatchProcessingDTO> CreateGetSevisBatchProcessingDTOByIdQuery(EcaContext context, int batchId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            return CreateGetSevisBatchProcessingDTOQuery(context).Where(x => x.BatchId == batchId);
        }
    }
}
