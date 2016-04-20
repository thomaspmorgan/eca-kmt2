using ECA.Business.Service.Sevis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ECA.Business.Sevis.Model;

namespace ECA.WebJobs.Sevis.Core
{
    /// <summary>
    /// The NullDS2019FileProvider always returns null streams because when this file provider is used, no files are expected to be found ever.
    /// </summary>
    public class NullDS2019FileProvider : IDS2019FileProvider
    {
        /// <summary>
        /// Returns a null stream.
        /// </summary>
        /// <param name="requestId"></param>
        /// <param name="sevisId"></param>
        /// <returns></returns>
        public Stream GetDS2019FileStream(RequestId requestId, string sevisId)
        {
            return null;
        }

        /// <summary>
        /// Returns a null stream.
        /// </summary>
        /// <param name="requestId"></param>
        /// <param name="sevisId"></param>
        /// <returns></returns>
        public Task<Stream> GetDS2019FileStreamAsync(RequestId requestId, string sevisId)
        {
            return Task.FromResult<Stream>(null);
        }
    }
}
