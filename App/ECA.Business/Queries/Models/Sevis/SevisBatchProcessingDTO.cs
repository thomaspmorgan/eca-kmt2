using System;
using System.Xml.Linq;

namespace ECA.Business.Queries.Models.Sevis
{
    /// <summary>
    /// A SevisBatchProcessingDTO is used to represent a SEVIS Batch Processing entity in the ECA System.
    /// </summary>
    public class SevisBatchProcessingDTO
    {
        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the batch id.
        /// </summary>
        public string BatchId { get; set; }

        /// <summary>
        /// Date of SEVIS Batch Submission
        /// </summary>
        public DateTimeOffset? SubmitDate { get; set; }

        /// <summary>
        /// Date SEVIS Batch was retrieved after processing
        /// </summary>
        public DateTimeOffset? RetrieveDate { get; set; }

        /// <summary>
        /// Storage for SEVIS Submission XML
        /// </summary>
        public string SendString { get; set; }
        
        /// <summary>
        /// Storage for SEVIS Transaction Log XML
        /// </summary>
        public string TransactionLogString { get; set; }        
        
        /// <summary>
        /// Error code for SEVIS Upload (submission)
        /// </summary>
        public string UploadDispositionCode { get; set; }

        /// <summary>
        /// Error code for SEVIS processing
        /// </summary>
        public string ProcessDispositionCode { get; set; }

        /// <summary>
        /// Error code for SEVIS retrieval (transaction log)
        /// </summary>
        public string DownloadDispositionCode { get; set; }
    }
}
