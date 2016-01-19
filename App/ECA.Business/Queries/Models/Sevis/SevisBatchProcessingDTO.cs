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
        /// Gets or sets the batch Id.
        /// </summary>
        public int BatchId { get; set; }

        /// <summary>
        /// Date of SEVIS Batch Submission
        /// </summary>
        public DateTimeOffset? SubmitDate { get; set; }

        /// <summary>
        /// Date SEVIS Batch was retrieved after processing
        /// </summary>
        public DateTimeOffset? RetrieveDate { get; set; }

        /// <summary>
        /// Property to save/retrieve XML submission string as an XElement
        /// </summary>
        public XElement SendXml { get;  set; }

        /// <summary>
        /// Property to save/retrieve XML transaction log string as an XElement
        /// </summary>
        public XElement TransactionLogXml { get; set; }

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
