using ECA.Business.Queries.Models.Admin;
using ECA.Core.Service;
using ECA.Data;
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

        /// <summary>
        /// Creates a data point configuration
        /// </summary>
        /// <param name="newDataPointConfiguration">The data point to create</param>
        /// <returns>Data point configuration</returns>
        Task<DataPointConfiguration> CreateDataPointConfigurationAsync(NewDataPointConfiguration newDataPointConfiguration);

        /// <summary>
        /// Gets the data point configurations
        /// </summary>
        /// <param name="officeId">The office id</param>
        /// <param name="programId">The program id</param>
        /// <returns>List of data point configurations</returns>
        Task<List<DataPointConfigurationDTO>> GetDataPointConfigurationsAsync(int? officeId, int? programId);
    }
}
