using ECA.Business.Sevis.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Sevis
{
    /// <summary>
    /// An IDS2019FileProvider is used to retrieve a DS2019 file from a datastore.
    /// </summary>
    [ContractClass(typeof(DS2019FileProviderContract))]
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

    /// <summary>
    /// 
    /// </summary>
    [ContractClassFor(typeof(IDS2019FileProvider))]
    public abstract class DS2019FileProviderContract : IDS2019FileProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestId"></param>
        /// <param name="sevisId"></param>
        /// <returns></returns>
        public Stream GetDS2019FileStream(RequestId requestId, string sevisId)
        {
            Contract.Requires(requestId != null, "The request id must not be null.");
            Contract.Requires(sevisId != null, "The sevis id must not be null.");
            return null;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestId"></param>
        /// <param name="sevisId"></param>
        /// <returns></returns>
        public Task<Stream> GetDS2019FileStreamAsync(RequestId requestId, string sevisId)
        {
            Contract.Requires(requestId != null, "The request id must not be null.");
            Contract.Requires(sevisId != null, "The sevis id must not be null.");
            return Task.FromResult<Stream>(null);
        }
    }
}
