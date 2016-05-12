using ECA.Business.Queries.Models.Sevis;
using ECA.Business.Service;
using ECA.Business.Service.Sevis;
using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace ECA.WebJobs.Sevis.Core
{
    /// <summary>
    /// A ZipArchiveSevisApiResponseHandler is capable of handling a stream response from the sevis api which returns a zip archive.
    /// </summary>
    public class ZipArchiveSevisApiResponseHandler : ISevisApiResponseHandler, IDisposable
    {
        private ISevisBatchProcessingService service;
        private Func<Stream, ZipArchive> zipArchiveDelegate;

        /// <summary>
        /// Creates a new instance with the sevis batch processing service and the zip archive delegate.
        /// </summary>
        /// <param name="service">The service to use when processing a zip archive.</param>
        /// <param name="zipArchiveDelegate">The zip archive delegate, useful when unit testing, otherwise set null and it will be initialized.</param>
        public ZipArchiveSevisApiResponseHandler(ISevisBatchProcessingService service, Func<Stream, ZipArchive> zipArchiveDelegate = null)
        {
            Contract.Requires(service != null, "The sevis batch processing service must not be null.");
            this.service = service;
            if (zipArchiveDelegate != null)
            {
                this.zipArchiveDelegate = zipArchiveDelegate;
            }
            else
            {
                this.zipArchiveDelegate = (s) =>
                {
#if DEBUG
                    //using (var fileStream = File.Open(@"c:\dev\test.zip", FileMode.Create))
                    //{
                    //    s.CopyTo(fileStream);
                    //    s.Seek(0, SeekOrigin.Begin);
                    //}
#endif
                    return new ZipArchive(s);
                };
            }
        }
        #region Handle Upload Response

        /// <summary>
        /// Handles a sevis api upload response.
        /// </summary>
        /// <param name="user">The user performing the processing.</param>
        /// <param name="dto">The dto representing the uploaded batch.</param>
        /// <param name="stream">The response stream.</param>
        public void HandleUploadResponseStream(User user, SevisBatchProcessingDTO dto, Stream stream)
        {
            using (var streamReader = new StreamReader(stream))
            {
                var xml = streamReader.ReadToEnd();
                this.service.ProcessTransactionLog(user, dto.BatchId, xml, new NullDS2019FileProvider());
            }
        }

        /// <summary>
        /// Handles a sevis api upload response.
        /// </summary>
        /// <param name="user">The user performing the processing.</param>
        /// <param name="dto">The dto representing the uploaded batch.</param>
        /// <param name="stream">The response stream.</param>
        public async Task HandleUploadResponseStreamAsync(User user, SevisBatchProcessingDTO dto, Stream stream)
        {
            using (var streamReader = new StreamReader(stream))
            {
                var xml = await streamReader.ReadToEndAsync();
                await this.service.ProcessTransactionLogAsync(user, dto.BatchId, xml, new NullDS2019FileProvider());
            }
        }
        #endregion

        #region Handle Download Response
        /// <summary>
        /// Handles a sevis api response which provides a zip archive as the stream.
        /// </summary>
        /// <param name="user">The user performing the operations.</param>
        /// <param name="dto">The sevis batch processing instance representing the stream response.</param>
        /// <param name="stream">The stream representing the api response.</param>
        public void HandleDownloadResponseStream(User user, SevisBatchProcessingDTO dto, Stream stream)
        {
            using (var sevisBatchFileHandler = new SevisBatchZipArchiveHandler(this.zipArchiveDelegate(stream)))
            {
                var transactionLogXml = sevisBatchFileHandler.GetTransactionLogXml();
                service.ProcessTransactionLog(user, dto.BatchId, transactionLogXml, sevisBatchFileHandler);
            }
        }

        /// <summary>
        /// Handles a sevis api response which provides a zip archive as the stream.
        /// </summary>
        /// <param name="user">The user performing the operations.</param>
        /// <param name="dto">The sevis batch processing instance representing the stream response.</param>
        /// <param name="stream">The stream representing the api response.</param>
        public async Task HandleDownloadResponseStreamAsync(User user, SevisBatchProcessingDTO dto, Stream stream)
        {
            using (var sevisBatchFileHandler = new SevisBatchZipArchiveHandler(this.zipArchiveDelegate(stream)))
            {
                var transactionLogXml = await sevisBatchFileHandler.GetTransactionLogXmlAsync();
                await service.ProcessTransactionLogAsync(user, dto.BatchId, transactionLogXml, sevisBatchFileHandler);
            }
        }
        #endregion

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
                if (this.service is IDisposable)
                {
                    ((IDisposable)this.service).Dispose();
                    this.service = null;
                }
            }
        }
        #endregion
    }
}
