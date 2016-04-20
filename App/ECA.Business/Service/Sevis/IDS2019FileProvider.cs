using ECA.Business.Sevis.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Sevis
{
    /// <summary>
    /// An IDS2019FileProvider is used to retrieve a DS2019 file from a datastore.
    /// </summary>
    public interface IDS2019FileProvider
    {
        /// <summary>
        /// Returns the file stream for the file with the given file name.
        /// </summary>
        /// <param name="requestId">The request id.</param>
        /// <param name="fileName">The filename of the file to retrieve.</param>
        /// <returns>The file stream.</returns>
        Stream GetDS2019FileStream(RequestId requestId, string sevisId);

        /// <summary>
        /// Returns the file stream for the file with the given file name.
        /// </summary>
        /// <param name="fileName">The filename of the file to retrieve.</param>
        /// <returns>The file stream.</returns>
        Task<Stream> GetDS2019FileStreamAsync(RequestId requestId, string sevisId);
    }
}
