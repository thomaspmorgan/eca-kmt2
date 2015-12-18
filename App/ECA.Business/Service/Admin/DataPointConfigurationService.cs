using ECA.Business.Exceptions;
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
        /// Model has less than or more than one resource error
        /// </summary>
        public const string MODEL_HAS_LESS_THAN_OR_MORE_THAN_ONE_RESOURCE_ID_ERROR = "The model must contain exactly one resource id.";

        /// <summary>
        /// Bookmark already exists error
        /// </summary>
        public const string DATA_POINT_CONFIGURATION_ALREADY_EXISTS_ERROR = "The data point configuration cannot be created, it already exists.";

        /// <summary>
        /// Resource does not exist error
        /// </summary>
        public const string RESOURCE_DOES_NOT_EXIST_ERROR = "The specified resource id does not exist.";

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

        /// <summary>
        /// Create a data point configuration
        /// </summary>
        /// <param name="newDataPointConfiguration">Data point configuration to create</param>
        /// <returns>Data point configuration created</returns>
        public async Task<DataPointConfiguration> CreateDataPointConfigurationAsync(NewDataPointConfiguration newDataPointConfiguration)
        {
            var modelHasOneResouceId = ModelHasOneResourceId(newDataPointConfiguration);
            if(!modelHasOneResouceId)
            {
                throw new EcaBusinessException(MODEL_HAS_LESS_THAN_OR_MORE_THAN_ONE_RESOURCE_ID_ERROR);
            }
            var dataPointConfigurationAlreadyExists = DataPointConfigurationAlreadyExists(newDataPointConfiguration);
            if(dataPointConfigurationAlreadyExists)
            {
                throw new EcaBusinessException(DATA_POINT_CONFIGURATION_ALREADY_EXISTS_ERROR);
            }
            var resourceExists = await ResourceExists(newDataPointConfiguration);
            if(!resourceExists)
            {
                throw new EcaBusinessException(RESOURCE_DOES_NOT_EXIST_ERROR);
            }
            var dataPointConfiguration = DoCreate(newDataPointConfiguration);
            return dataPointConfiguration;
        }

        private bool ModelHasOneResourceId(NewDataPointConfiguration newDataPointConfiguration)
        {
            var hasOneId = false;

            if ((newDataPointConfiguration.OfficeId != null && newDataPointConfiguration.ProgramId == null && newDataPointConfiguration.ProjectId == null) ||
                (newDataPointConfiguration.OfficeId == null && newDataPointConfiguration.ProgramId != null && newDataPointConfiguration.ProjectId == null) ||
                (newDataPointConfiguration.OfficeId == null && newDataPointConfiguration.ProgramId == null && newDataPointConfiguration.ProjectId != null))
            {
                hasOneId = true;
            }

            return hasOneId;
        }

        private bool DataPointConfigurationAlreadyExists(NewDataPointConfiguration newDataPointConfiguration)
        {
            var dataPointConfiguration = Context.DataPointConfigurations.Where(x => x.OfficeId == newDataPointConfiguration.OfficeId &&
                                                                      x.ProgramId == newDataPointConfiguration.ProgramId &&
                                                                      x.ProjectId == newDataPointConfiguration.ProjectId &&
                                                                      x.DataPointCategoryPropertyId == newDataPointConfiguration.DataPointCategoryPropertyId
                                                                      ).FirstOrDefault();
            return dataPointConfiguration != null;
        }

        private async Task<bool> ResourceExists(NewDataPointConfiguration newDataPointConfiguration)
        {

            Object resource = null;

            if (newDataPointConfiguration.OfficeId != null)
            {
                resource = await Context.Organizations.FindAsync(newDataPointConfiguration.OfficeId);
        }
            else if (newDataPointConfiguration.ProgramId != null)
            {
                resource = await Context.Programs.FindAsync(newDataPointConfiguration.ProgramId);
            }
            else if (newDataPointConfiguration.ProjectId != null)
            {
                resource = await Context.Projects.FindAsync(newDataPointConfiguration.ProjectId);
            }

            return resource != null;

        }

        private DataPointConfiguration DoCreate(NewDataPointConfiguration newDataPointConfiguration)
        {
            var dataPointConfiguration = new DataPointConfiguration
            {
                OfficeId = newDataPointConfiguration.OfficeId,
                ProgramId = newDataPointConfiguration.ProgramId,
                ProjectId = newDataPointConfiguration.ProjectId,
                DataPointCategoryPropertyId = newDataPointConfiguration.DataPointCategoryPropertyId
            };
            this.Context.DataPointConfigurations.Add(dataPointConfiguration);
            return dataPointConfiguration;
        }

    }
}
