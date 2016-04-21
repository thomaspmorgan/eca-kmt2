using ECA.Business.Sevis.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Queries.Models.Sevis
{
    /// <summary>
    /// A SevisBatchInfoDTO is used to provide details on the current state of a sevis batch.
    /// </summary>
    public class SevisBatchInfoDTO
    {
        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the batch Id.
        /// </summary>
        public string BatchId { get; set; }

        /// <summary>
        /// Gets or sets the upload tries counter.
        /// </summary>
        public int UploadTries { get; set; }

        /// <summary>
        /// Gets or sets the download tries counter.
        /// </summary>
        public int DownloadTries { get; set; }

        /// <summary>
        /// Gets or sets the last upload try date.
        /// </summary>
        public DateTimeOffset? LastUploadTry { get; set; }

        /// <summary>
        /// Gets or sets the last download try.
        /// </summary>
        public DateTimeOffset? LastDownloadTry { get; set; }

        /// <summary>
        /// Gets or sets the download disposition code.
        /// </summary>
        public string DownloadDispositionCode { get; set; }

        /// <summary>
        /// Gets or sets the upload disposition code.
        /// </summary>
        public string UploadDispositionCode { get; set; }

        /// <summary>
        /// Gets or sets the process disposition code.
        /// </summary>
        public string ProcessDispositionCode { get; set; }

        /// <summary>
        /// Gets or sets is cancelled.
        /// </summary>
        public bool IsCancelled { get; set; }

        /// <summary>
        /// Gets or sets the submit date.
        /// </summary>
        public DateTimeOffset? SubmitDate { get; set; }

        /// <summary>
        /// Gets or sets the retrieve date.
        /// </summary>
        public DateTimeOffset? RetrieveDate { get; set; }
        
        /// <summary>
        /// Gets or sets the cancelled reason.
        /// </summary>
        public string CancelledReason { get; set; }

        /// <summary>
        /// Gets or sets the cancelled on date.
        /// </summary>
        public DateTimeOffset? CancelledOn { get; set; }

        /// <summary>
        /// Gets the upload disposition code description.
        /// </summary>
        public string UploadDispositionCodeDescription
        {
            get
            {
                return this.UploadDispositionCode == null ? null : DispositionCode.ToDispositionCode(this.UploadDispositionCode).Description;
            }
        }

        /// <summary>
        /// Gets or sets the download disposition code description.
        /// </summary>
        public string DownloadDispositionCodeDescription
        {
            get
            {
                return this.DownloadDispositionCode == null ? null : DispositionCode.ToDispositionCode(this.DownloadDispositionCode).Description;
            }
        }

        /// <summary>
        /// Gets or sets the process disposition code description.
        /// </summary>
        public string ProcessDispositionCodeDescription
        {
            get
            {
                return this.ProcessDispositionCode == null ? null : DispositionCode.ToDispositionCode(this.ProcessDispositionCode).Description;
            }
        }
    }
}
