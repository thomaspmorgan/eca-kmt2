using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace ECA.WebApi.Custom.Storage
{
    /// <summary>
    /// Interface for file storage handler
    /// </summary>
    public interface IFileStorageHandler
    {
        /// <summary>
        /// Gets file asyncronously
        /// </summary>
        /// <param name="fileName">The file name</param>
        /// <param name="container">The container</param>
        /// <returns>Http response message with file</returns>
        Task<HttpResponseMessage> GetFileAsync(string fileName, string container);
    }
}
