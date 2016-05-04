using ECA.Business.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace ECA.WebApi.Custom.Storage
{
    /// <summary>
    /// Implementation of file storage handler
    /// </summary>
    public class FileStorageHandler : IFileStorageHandler
    {
        /// <summary>
        /// The file storage service
        /// </summary>
        private IFileStorageService fileStorageService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="fileStorageService">The file storage service</param>
        public FileStorageHandler(IFileStorageService fileStorageService)
        {
            this.fileStorageService = fileStorageService;
        }

        /// <summary>
        /// Get a file asyncronously
        /// </summary>
        /// <param name="fileName">The file name</param>
        /// <param name="container">The container name</param>
        /// <returns>Http response message with file</returns>
        public async Task<HttpResponseMessage> GetFileAsync(string fileName, string container)
        {
            var blob = await fileStorageService.GetBlobAsync(fileName, container);
            HttpResponseMessage message = null;
            if (blob != null)
            {
                message = new HttpResponseMessage(HttpStatusCode.OK);
                Stream blobStream = await blob.OpenReadAsync();
                message.Content = new StreamContent(blobStream);
                message.Content.Headers.ContentLength = blob.Properties.Length;
                message.Content.Headers.ContentType = new MediaTypeHeaderValue(blob.Properties.ContentType);
                message.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = blob.Name
                };
            }
            return message;
        }
    }
}