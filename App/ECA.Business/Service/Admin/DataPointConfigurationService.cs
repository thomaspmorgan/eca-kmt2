using ECA.Core.Exceptions;
using ECA.Core.Service;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Admin
{
    /// <summary>
    /// Service implementation for data point configurations
    /// </summary>
    public class DataPointConfigurationService : DbContextService<EcaContext>, IDataPointConfigurationService
    {
        /// <summary>
        /// Data point configuration not found error
        /// </summary>
        public const string DATA_POINT_CONFIGURATION_NOT_FOUND_ERROR = "The data point configuration could not be found.";

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="saveActions"></param>
        public DataPointConfigurationService(EcaContext context, List<ISaveAction> saveActions = null) : base(context, saveActions)
        {
            Contract.Requires(context != null, "The context must not be null");
        }

        /// <summary>
        /// Delete a data point configuration
        /// </summary>
        /// <param name="id">The id to delete</param>
        /// <returns></returns>
        public async Task DeleteDataPointConfigurationAsync(int id)
        {
            var dataPointConfiguration = await Context.DataPointConfigurations.FindAsync(id);
            if(dataPointConfiguration != null)
            {
                Context.DataPointConfigurations.Remove(dataPointConfiguration);
            }
            else
            {
                throw new ModelNotFoundException(DATA_POINT_CONFIGURATION_NOT_FOUND_ERROR);
            }
        }
    }
}
