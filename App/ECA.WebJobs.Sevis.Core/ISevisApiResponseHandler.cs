using ECA.Business.Queries.Models.Sevis;
using ECA.Business.Service;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.WebJobs.Sevis.Core
{
    /// <summary>
    /// An ISevisApiResponseHandler is capable of processing a response from the sevis api.
    /// </summary>
    [ContractClass(typeof(SevisApiResponseHandlerContract))]
    public interface ISevisApiResponseHandler
    {
        /// <summary>
        /// Handles an upload response from the sevis api.
        /// </summary>
        /// <param name="user">The user performing the processing.</param>
        /// <param name="dto">The dto representing the service api's response.</param>
        /// <param name="stream">The stream representing the response.</param>
        void HandleUploadResponseStream(User user, SevisBatchProcessingDTO dto, Stream stream);

        /// <summary>
        /// Handles an upload response from the sevis api.
        /// </summary>
        /// <param name="user">The user performing the processing.</param>
        /// <param name="dto">The dto representing the service api's response.</param>
        /// <param name="stream">The stream representing the response.</param>
        Task HandleUploadResponseStreamAsync(User user, SevisBatchProcessingDTO dto, Stream stream);

        /// <summary>
        /// Handles a download response from the sevis api.
        /// </summary>
        /// <param name="user">The user performing the processing.</param>
        /// <param name="dto">The dto representing the service api's response.</param>
        /// <param name="stream">The stream representing the response.</param>
        void HandleDownloadResponseStream(User user, SevisBatchProcessingDTO dto, Stream stream);

        /// <summary>
        /// Handles a download response from the sevis api.
        /// </summary>
        /// <param name="user">The user performing the processing.</param>
        /// <param name="dto">The dto representing the service api's response.</param>
        /// <param name="stream">The stream representing the response.</param>
        Task HandleDownloadResponseStreamAsync(User user, SevisBatchProcessingDTO dto, Stream stream);
    }

    /// <summary>
    /// 
    /// </summary>
    [ContractClassFor(typeof(ISevisApiResponseHandler))]
    public abstract class SevisApiResponseHandlerContract : ISevisApiResponseHandler
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="dto"></param>
        /// <param name="stream"></param>
        public void HandleDownloadResponseStream(User user, SevisBatchProcessingDTO dto, Stream stream)
        {
            Contract.Requires(user != null, "The user must not be null.");
            Contract.Requires(dto != null, "The dto must not be null.");
            Contract.Requires(stream != null, "The stream must not be null.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="dto"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public Task HandleDownloadResponseStreamAsync(User user, SevisBatchProcessingDTO dto, Stream stream)
        {
            Contract.Requires(user != null, "The user must not be null.");
            Contract.Requires(dto != null, "The dto must not be null.");
            Contract.Requires(stream != null, "The stream must not be null.");
            return Task.FromResult<Object>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="dto"></param>
        /// <param name="stream"></param>
        public void HandleUploadResponseStream(User user, SevisBatchProcessingDTO dto, Stream stream)
        {
            Contract.Requires(user != null, "The user must not be null.");
            Contract.Requires(dto != null, "The dto must not be null.");
            Contract.Requires(stream != null, "The stream must not be null.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="dto"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public Task HandleUploadResponseStreamAsync(User user, SevisBatchProcessingDTO dto, Stream stream)
        {
            Contract.Requires(user != null, "The user must not be null.");
            Contract.Requires(dto != null, "The dto must not be null.");
            Contract.Requires(stream != null, "The stream must not be null.");
            return Task.FromResult<Object>(null);
        }
    }
}
