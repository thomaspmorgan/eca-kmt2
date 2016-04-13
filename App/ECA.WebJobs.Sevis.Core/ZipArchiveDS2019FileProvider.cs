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
    public class ZipArchiveDS2019FileProvider : IDS2019FileProvider, IDisposable
    {
        /// <summary>
        /// Creates a new instance with a zip archive.
        /// </summary>
        /// <param name="zipArchive">The zip archive containing ds 2019 files.</param>
        public ZipArchiveDS2019FileProvider(ZipArchive zipArchive)
        {
            Contract.Requires(zipArchive != null, "The zip archive must not be null.");
            this.ZipArchive = zipArchive;
        }

        /// <summary>
        /// Gets the zip archive this file provider is using for ds2019 files.
        /// </summary>
        public ZipArchive ZipArchive { get; private set; }
       
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
            var entry = this.ZipArchive.Entries.Where(x => x.Name.Contains(sevisId)).FirstOrDefault();
            if(entry != null)
            {
                return entry.Open();
            }
            else
        {
            return Task.FromResult(new byte[] { });
        }
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
