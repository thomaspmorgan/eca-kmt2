using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Data
{
    [Table("CancelledSevisBatchProcessing")]
    public class CancelledSevisBatchProcessing
    {
        /// <summary>
        /// The Id of the batch record
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the Batch Id string.
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
        /// Gets or sets the sevis org id that requested the batch.
        /// </summary>
        public string SevisOrgId { get; set; }

        /// <summary>
        /// Gets or sets the sevis username that requested the batch.
        /// </summary>
        public string SevisUsername { get; set; }

        /// <summary>
        /// Storage for SEVIS Submission XML
        /// </summary>
        [Column("SendXml", TypeName = "xml")]
        public string SendString { get; set; }

        /// <summary>
        /// Storage for SEVIS Transaction Log XML
        /// </summary>
        [Column("TransactionLogXml", TypeName = "xml")]
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

        /// <summary>
        /// Gets or sets the upload tries counter.
        /// </summary>
        public int UploadTries { get; set; }

        /// <summary>
        /// Gets or sets the download tries counter.
        /// </summary>
        public int DownloadTries { get; set; }

        /// <summary>
        /// Gets or sets the date this batch was last uploaded.
        /// </summary>
        public DateTimeOffset? LastUploadTry { get; set; }

        /// <summary>
        /// Gets or sets the date this batch was last downloaded.
        /// </summary>
        public DateTimeOffset? LastDownloadTry { get; set; }


        /// <summary>
        /// Gets or sets the date the batch was cancelled.
        /// </summary>
        public DateTimeOffset CancelledOn { get; set; }

        /// <summary>
        /// Gets or sets the reason for cancellation.
        /// </summary>
        public string Reason { get; set; }
    }
}
