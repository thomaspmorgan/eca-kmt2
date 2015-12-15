using ECA.Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Admin
{
    /// <summary>
    /// Interface for data point configuration service
    /// </summary>
    public interface IDataPointConfigurationService : ISaveable
    {
        /// <summary>
        /// Delete a data point configuration
        /// </summary>
        /// <param name="id">The id to delete</param>
        /// <returns></returns>
        Task DeleteDataPointConfigurationAsync(int id);
    }
}
