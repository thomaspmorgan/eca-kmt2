using ECA.Business.Service.Sevis;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.WebJobs.Sevis.Core
{
    /// <summary>
    /// A ZipArchiveDS2019FileProvider is used to retrieve ds 2019 forms from the sevis zip archive that is downloaded from the sevis api.
    /// </summary>
    public class SevisBatchZipArchiveHandler : IDS2019FileProvider, IDisposable
    {
        /// <summary>
        /// The default name of the sevis transaction log file.
        /// </summary>
        public const string TRANSACTION_LOG_FILE_NAME = "sevis_transaction_log.xml";

        public SevisBatchZipArchiveHandler(Stream zipArchiveAsStream, string transactionLogFileName = TRANSACTION_LOG_FILE_NAME)
            : this(new ZipArchive(zipArchiveAsStream), transactionLogFileName)
        {
            Contract.Requires(zipArchiveAsStream != null, "The zip archive as a stream must not be null.");
            Contract.Requires(transactionLogFileName != null, "The transaction log file name must not be null.");
        }

        /// <summary>
        /// Creates a new instance with a zip archive.
        /// </summary>
        /// <param name="zipArchive">The zip archive containing ds 2019 files.</param>
        /// <param name="transactionLogFileName">The transaction log file name in the zip archive.</param>
        public SevisBatchZipArchiveHandler(ZipArchive zipArchive, string transactionLogFileName = TRANSACTION_LOG_FILE_NAME)
        {
            Contract.Requires(transactionLogFileName != null, "The transaction log file name must not be null.");
            Contract.Requires(zipArchive != null, "The zip archive must not be null.");
            this.ZipArchive = zipArchive;
            this.TransactionLogFileName = transactionLogFileName;
        }

        /// <summary>
        /// Gets the zip archive this file provider is using for ds2019 files.
        /// </summary>
        public ZipArchive ZipArchive { get; private set; }

        /// <summary>
        /// Gets the transaction log file name.
        /// </summary>
        public string TransactionLogFileName { get; private set; }

        /// <summary>
        /// Returns the transaction log xml as a string from the zip archive.
        /// </summary>
        /// <returns>The transaction log xml as a string, or null, if it was not found.</returns>
        public string GetTransactionLogXml()
        {
            var transactionLogEntry = GetEntryByName(this.TransactionLogFileName);
            if(transactionLogEntry != null)
            {
                using (var stringReader = new StreamReader(transactionLogEntry.Open()))
                {
                    return stringReader.ReadToEnd();
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Returns the transaction log xml as a string from the zip archive.
        /// </summary>
        /// <returns>The transaction log xml as a string, or null, if it was not found.</returns>
        public async Task<string> GetTransactionLogXmlAsync()
        {
            var transactionLogEntry = GetEntryByName(this.TransactionLogFileName);
            if (transactionLogEntry != null)
            {
                using (var stringReader = new StreamReader(transactionLogEntry.Open()))
                {
                    return await stringReader.ReadToEndAsync();
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Returns the ds2019 stream from the provided zip archive.
        /// </summary>
        /// <param name="participantId">The participant id.</param>
        /// <param name="batchId">The batch id.</param>
        /// <param name="sevisId">The sevis id.</param>
        /// <returns></returns>
        public Stream GetDS2019FileStream(int participantId, string batchId, string sevisId)
        {
            return GetStream(sevisId);
        }

        /// <summary>
        /// Returns the ds2019 stream from the provided zip archive.
        /// </summary>
        /// <param name="participantId">The participant id.</param>
        /// <param name="batchId">The batch id.</param>
        /// <param name="sevisId">The sevis id.</param>
        public Task<Stream> GetDS2019FileStreamAsync(int participantId, string batchId, string sevisId)
        {
            return Task.FromResult<Stream>(GetDS2019FileStream(participantId, batchId, sevisId));
        }

        /// <summary>
        /// Returns the ds2019 stream from the zip archive by sevis id.
        /// </summary>
        /// <param name="sevisId">The sevis id to locate the ds2019 with.</param>
        /// <returns>The ds2019 stream, or null if it does not exist.</returns>
        public Stream GetStream(string sevisId)
        {
            Contract.Requires(this.ZipArchive != null, "The zip archive must not be null.");
            var entry = GetEntryByName(sevisId);
            if(entry != null)
            {
                return entry.Open();
            }
            else
            {
                return null;
            }
        }

        private ZipArchiveEntry GetEntryByName(string name)
        {
            return this.ZipArchive.Entries.Where(x => x.Name.Contains(name)).FirstOrDefault();
        }

        #region Dispose
        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.ZipArchive != null)
                {
                    this.ZipArchive.Dispose();
                    this.ZipArchive = null;
                }
            }
        }
        #endregion
    }
}
