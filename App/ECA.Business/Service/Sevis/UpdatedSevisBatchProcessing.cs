using System;
using System.Xml.Linq;
using ECA.Business.Queries.Models.Sevis;

namespace ECA.Business.Service.Sevis
{
    /// <summary>
    /// An updated SEVIS Batch Processing record is used by a business layer client to update a sevis batch entity.
    /// </summary>
    public class UpdatedSevisBatchProcessing
    {
        /// <summary>
        /// Creates a new UpdatedSevisBatchProcessing and initialized it with the given values.
        /// </summary>
        public UpdatedSevisBatchProcessing(int batchId, DateTimeOffset submitDate, DateTimeOffset? retrieveDate,  XElement sendXml, XElement transactionLogXml, string uploadDispositionCode, string processDispositionCode, string downloadDispositionCode)
        {
            this.BatchId = batchId;
            this.SubmitDate = submitDate;
            this.RetrieveDate = retrieveDate;
            this.SendXml = sendXml;
            this.TransactionLogXml = transactionLogXml;
            this.UploadDispositionCode = uploadDispositionCode;
            this.ProcessDispositionCode = processDispositionCode;
            this.DownloadDispositionCode = downloadDispositionCode;
        }

        public UpdatedSevisBatchProcessing(SevisBatchProcessingDTO batchProcessing)
        {
            this.BatchId = batchProcessing.BatchId;
            this.SubmitDate = batchProcessing.SubmitDate;
            this.RetrieveDate = batchProcessing.RetrieveDate;
            this.SendXml = batchProcessing.SendXml;
            this.TransactionLogXml = batchProcessing.TransactionLogXml;
            this.UploadDispositionCode = batchProcessing.UploadDispositionCode;
            this.ProcessDispositionCode = batchProcessing.ProcessDispositionCode;
            this.DownloadDispositionCode = batchProcessing.DownloadDispositionCode;
        }

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
        public XElement SendXml { get; set; }

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
