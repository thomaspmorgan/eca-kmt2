using System;
using System.Collections.Generic;
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
        /// Returns the file bytes for the file with the given file name.
        /// </summary>
        /// <param name="fileName">The filename of the file to retrieve.</param>
        /// <returns>The file bytes.</returns>
        byte[] GetDS2019File(int participantId, string batchId, string sevisId);

        /// <summary>
        /// Returns the file bytes for the file with the given file name.
        /// </summary>
        /// <param name="fileName">The filename of the file to retrieve.</param>
        /// <returns>The file bytes.</returns>
        Task<byte[]> GetDS2019FileAsync(int participantId, string batchId, string sevisId);
    }
}
