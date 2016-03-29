using ECA.Business.Service.Sevis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.WebJobs.Sevis.Core
{
    /// <summary>
    /// A ZipArchiveDS2019FileProvider is used to retrieve ds 2019 forms from the sevis zip archive that is downloaded from the sevis api.
    /// </summary>
    public class ZipArchiveDS2019FileProvider : IDS2019FileProvider
    {
        /// <summary>
        /// Gets the file contents from the zip archive.
        /// </summary>
        /// <param name="participantId">The participant id.</param>
        /// <param name="batchId">The batch id.</param>
        /// <param name="sevisId">The sevis id.</param>
        /// <returns>The file contents.</returns>
        public byte[] GetDS2019File(int participantId, string batchId, string sevisId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the file contents from the zip archive.
        /// </summary>
        /// <param name="participantId">The participant id.</param>
        /// <param name="batchId">The batch id.</param>
        /// <param name="sevisId">The sevis id.</param>
        /// <returns>The file contents.</returns>
        public Task<byte[]> GetDS2019FileAsync(int participantId, string batchId, string sevisId)
        {
            throw new NotImplementedException();
        }
    }
}
