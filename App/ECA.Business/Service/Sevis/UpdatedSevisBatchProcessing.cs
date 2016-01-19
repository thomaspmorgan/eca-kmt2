using ECA.Core.Exceptions;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ECA.Business.Service.Sevis
{
    /// <summary>
    /// An UpdatedPhoneNumber is used by a business layer client to update a phone number entity.
    /// </summary>
    public class UpdatedSevisBatchProcessing
    {
        /// <summary>
        /// Creates a new UpdatedPhoneNumber and initialized it with the given values.
        /// </summary>
        /// <param name="updator">The updator.</param>
        /// <param name="id">The id of the phone number.</param>
        /// <param name="number">The value.</param>
        /// <param name="phoneNumberTypeId">The phone number type id</param>
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

        /// <summary>
        /// Gets or sets the batch Id.
        /// </summary>
        public int BatchId { get; set; }

        /// <summary>
        /// Date of SEVIS Batch Submission
        /// </summary>
        public DateTimeOffset SubmitDate { get; set; }

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
