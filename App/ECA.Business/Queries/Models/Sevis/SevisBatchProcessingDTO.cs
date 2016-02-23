using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        /// Storage for SEVIS Submission XML
        /// </summary>
        public string SendString { get; set; }

        /// <summary>
        /// Property to save/retrieve XML submission string as an XElement
        /// </summary>
        public XElement SendXml
        {
            get { return (SendString != null ? XElement.Parse(SendString) : null); }
            set { SendString = value != null ? value.ToString() : "<root></root>"; }
        }

        /// <summary>
        /// Storage for SEVIS Transaction Log XML
        /// </summary>
        public string TransactionLogString { get; set; }

        /// <summary>
        /// Property to save/retrieve XML transaction log string as an XElement
        /// </summary>
        public XElement TransactionLogXml
        {
            get { return (TransactionLogString != null ? XElement.Parse(TransactionLogString) : null); }
            set { TransactionLogString = value != null ? value.ToString() : "<root></root>"; }
        }
        
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
