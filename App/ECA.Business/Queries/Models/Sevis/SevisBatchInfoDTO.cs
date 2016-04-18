using ECA.Business.Sevis.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Queries.Models.Sevis
{
    public class SevisBatchInfoDTO
    {
        public int Id { get; set; }

        public string BatchId { get; set; }

        public int UploadTries { get; set; }

        public int DownloadTries { get; set; }

        public DateTimeOffset? LastUploadTry { get; set; }

        public DateTimeOffset? LastDownloadTry { get; set; }

        public string DownloadDispositionCode { get; set; }

        public string UploadDispositionCode { get; set; }

        public string ProcessDispositionCode { get; set; }

        public bool IsCancelled { get; set; }

        public DateTimeOffset? SubmitDate { get; set; }

        public DateTimeOffset? RetrieveDate { get; set; }

        public string SevisUsername { get; set; }

        public string CancelledReason { get; set; }

        public DateTimeOffset? CancelledOn { get; set; }

        public string UploadDispositionCodeDescription
        {
            get
            {
                return this.UploadDispositionCode == null ? null : DispositionCode.ToDispositionCode(this.UploadDispositionCode).Description;
            }
        }

        public string DownloadDispositionCodeDescription
        {
            get
            {
                return this.DownloadDispositionCode == null ? null : DispositionCode.ToDispositionCode(this.DownloadDispositionCode).Description;
            }
        }

        public string ProcessDispositionCodeDescription
        {
            get
            {
                return this.ProcessDispositionCode == null ? null : DispositionCode.ToDispositionCode(this.ProcessDispositionCode).Description;
            }
        }
    }
}
