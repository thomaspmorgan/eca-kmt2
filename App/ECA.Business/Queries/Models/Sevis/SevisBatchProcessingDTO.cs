using ECA.Business.Sevis.Model;
using System;

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
        /// Gets or sets the sevis username.
        /// </summary>
        public string SevisUsername { get; set; }

        /// <summary>
        /// Gets or sets the sevis org id.
        /// </summary>
        public string SevisOrgId { get; set; }

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

        /// <summary>
        /// Gets or sets the upload tries counter.
        /// </summary>
        public int UploadTries { get; set; }

        /// <summary>
        /// Gets or sets the date after which an upload can be tried again.
        /// </summary>
        public DateTimeOffset? UploadCooldown { get; set; }

        /// <summary>
        /// Returns the upload disposition code value as a Disposition Code.
        /// </summary>
        /// <returns>The update Dispoition code.</returns>
        public DispositionCode GetUploadDispositionCodeAsCode()
        {
            return DispositionCode.ToDispositionCode(this.UploadDispositionCode);
        }

        /// <summary>
        /// Returns the download disposition code value as a Disposition Code.
        /// </summary>
        /// <returns>The download Dispoition code.</returns>
        public DispositionCode GetDownloadDispositionCodeAsCode()
        {
            return DispositionCode.ToDispositionCode(this.DownloadDispositionCode);
        }

        /// <summary>
        /// Returns the process disposition code value as a Disposition Code.
        /// </summary>
        /// <returns>The process Dispoition code.</returns>
        public DispositionCode GetProcessDispositionCodeAsCode()
        {
            return DispositionCode.ToDispositionCode(this.ProcessDispositionCode);
        }
    }
}
