using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace ECA.Data
{
    /// <summary>
    /// Store for Batch Processing Sevis transactions
    /// </summary>
    public class SevisBatchProcessing
    {
        private const int NAME_LENGTH = 5;

        /// <summary>
        /// The Id of the batch record
        /// </summary>
        [Key]
        public int BatchId { get; set; }

        /// <summary>
        /// Date of SEVIS Batch Submission
        /// </summary>
        [Required]     
        public DateTimeOffset? SubmitDate { get; set; }

        /// <summary>
        /// Date SEVIS Batch was retrieved after processing
        /// </summary>
        public DateTimeOffset? RetrieveDate { get; set; }

        /// <summary>
        /// Storage for SEVIS Submission XML
        /// </summary>
        [Required]
        [Column(TypeName = "xml")]
        public string SendString { get; set; }
        
        /// <summary>
        /// Storage for SEVIS Transaction Log XML
        /// </summary>
        [Column(TypeName = "xml")]
        public string TransactionLogString { get; set; }

        /// <summary>
        /// Property to save/retrieve XML submission string as an XElement
        /// </summary>
        [NotMapped]
        public XElement SendXml
        {
            get { return (SendString != null ? XElement.Parse(SendString) : null); }
            set { SendString = value == null ? null : value.ToString(); }
        }

        /// <summary>
        /// Property to save/retrieve XML transaction log string as an XElement
        /// </summary>
        [NotMapped]
        public XElement TransactionLogXml
        {
            get { return (TransactionLogString != null ? XElement.Parse(TransactionLogString) : null); }
            set { TransactionLogString = value == null ? null : value.ToString(); }
        }

        /// <summary>
        /// Error code for SEVIS Upload (submission)
        /// </summary>
        [MinLength(NAME_LENGTH), MaxLength(NAME_LENGTH)]
        public string UploadDispositionCode { get; set; }

        /// <summary>
        /// Error code for SEVIS processing
        /// </summary>
        [MinLength(NAME_LENGTH), MaxLength(NAME_LENGTH)]
        public string ProcessDispositionCode { get; set; }

        /// <summary>
        /// Error code for SEVIS retrieval (transaction log)
        /// </summary>
        [MinLength(NAME_LENGTH), MaxLength(NAME_LENGTH)]
        public string DownloadDispositionCode { get; set; }
    }
}
