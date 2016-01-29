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
using ECA.Business.Queries.Models.Admin;
using System.Data.Entity;
using ECA.Business.Queries.Admin;
using System.Data.Entity.Infrastructure;
using ECA.Business.Queries.Models.Office;

namespace ECA.Business.Service.Admin
{
    /// <summary>
    /// Service implementation for data point configurations
    /// </summary>
    public class DataPointConfigurationService : DbContextService<EcaContext>, IDataPointConfigurationService
    {
        public static char[] PATH_SPLIT_CHARS = new char[] { '-' };

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

        /// <summary>
        /// Gets the data point configurations
        /// </summary>
        /// <param name="officeId">The office id</param>
        /// <param name="programId">The program id</param>
        /// <returns>The data point configurations</returns>
        public async Task<List<DataPointConfigurationDTO>> GetDataPointConfigurationsAsync(int? officeId, int? programId, int? projectId)
        {
            var dataPointConfigurations = new List<DataPointConfigurationDTO>();

            if(officeId.HasValue)
            {
                dataPointConfigurations = await GetOfficeDataPointConfigurationsAsync(officeId.Value);
            }
            else if (programId.HasValue)
            {
                dataPointConfigurations = await GetProgramDataPointConfigurationsAsync(programId.Value);
            }
            else if (projectId.HasValue)
            {
                dataPointConfigurations = await GetProjectDataPointConfigurationsAsync(projectId.Value);
            }

            return dataPointConfigurations;
        }

        private async Task<List<DataPointConfigurationDTO>> GetOfficeDataPointConfigurationsAsync(int officeId)
        {
            // Get available properties
            var dataPointConfigurations = await Context.DataPointCategoryProperties.Select(x => new DataPointConfigurationDTO
            {
                CategoryPropertyId = x.DataPointCategoryPropertyId,
                OfficeId = officeId,
                CategoryId = x.DataPointCategoryId,
                CategoryName = x.DataPointCategory.DataPointCategoryName,
                PropertyId = x.DataPointPropertyId,
                PropertyName = x.DataPointProperty.DataPointPropertyName,
                IsRequired = false,
                IsInherited = false
            }).ToListAsync();

            // Get parent office data configs
            var parentOfficeIds = await GetParentOfficeIds(officeId);
            var parentDataPointConfigurations = DataPointConfigurationQueries.CreateGetDataPointConfigurations(this.Context)
                .Where(x => parentOfficeIds.Contains(x.OfficeId.Value)).ToList();
            foreach (var config in parentDataPointConfigurations)
            {
                var temp = dataPointConfigurations.Where(x => x.CategoryPropertyId == config.CategoryPropertyId).FirstOrDefault();
                temp.DataPointConfigurationId = config.DataPointConfigurationId;
                temp.IsRequired = true;
                temp.IsInherited = true;
            }

            // Get child office data configs
            var childDataPointConfigurations = DataPointConfigurationQueries.CreateGetDataPointConfigurations(this.Context)
                .Where(x => x.OfficeId == officeId).ToList();
            foreach (var config in childDataPointConfigurations)
            {
                var temp = dataPointConfigurations.Where(x => x.CategoryPropertyId == config.CategoryPropertyId).FirstOrDefault();
                if (temp.DataPointConfigurationId == null)
                {
                    temp.DataPointConfigurationId = config.DataPointConfigurationId;
                    temp.IsRequired = true;
                }
            }

            return dataPointConfigurations;
        }

        public async Task<List<DataPointConfigurationDTO>> GetProgramDataPointConfigurationsAsync(int programId)
        {
            var dataPointConfigurations = await Context.DataPointCategoryProperties.Select(x => new DataPointConfigurationDTO
            {
                CategoryPropertyId = x.DataPointCategoryPropertyId,
                ProgramId = programId,
                CategoryId = x.DataPointCategoryId,
                CategoryName = x.DataPointCategory.DataPointCategoryName,
                PropertyId = x.DataPointPropertyId,
                PropertyName = x.DataPointProperty.DataPointPropertyName,
                IsRequired = false,
                IsInherited = false
            }).ToListAsync();

            var program = Context.Programs.Where(x => x.ProgramId == programId).FirstOrDefault();

            var parentOfficeIds = await GetParentOfficeIds(program.OwnerId);
            parentOfficeIds.Add(program.OwnerId);

            var parentProgramIds = await GetParentProgramIds(program.ProgramId);

            var parentDataPointConfigurations = DataPointConfigurationQueries.CreateGetDataPointConfigurations(this.Context)
                .Where(x => parentOfficeIds.Contains(x.OfficeId.Value) || parentProgramIds.Contains(x.ProgramId.Value)).ToList();
            foreach (var config in parentDataPointConfigurations)
            {
                var temp = dataPointConfigurations.Where(x => x.CategoryPropertyId == config.CategoryPropertyId).FirstOrDefault();
                temp.DataPointConfigurationId = config.DataPointConfigurationId;
                temp.IsRequired = true;
                temp.IsInherited = true;
            }

            var childDataPointConfigurations = DataPointConfigurationQueries.CreateGetDataPointConfigurations(this.Context)
                .Where(x => x.ProgramId == programId).ToList();
            foreach (var config in childDataPointConfigurations)
            {
                var temp = dataPointConfigurations.Where(x => x.CategoryPropertyId == config.CategoryPropertyId).FirstOrDefault();
                if (temp.DataPointConfigurationId == null)
                {
                    temp.DataPointConfigurationId = config.DataPointConfigurationId;
                    temp.IsRequired = true;
                }
            }

            return dataPointConfigurations;
        }

        private async Task<List<DataPointConfigurationDTO>> GetProjectDataPointConfigurationsAsync(int projectId)
        {
            var dataPointConfigurations = await Context.DataPointCategoryProperties.Select(x => new DataPointConfigurationDTO
            {
                CategoryPropertyId = x.DataPointCategoryPropertyId,
                ProjectId = projectId,
                CategoryId = x.DataPointCategoryId,
                CategoryName = x.DataPointCategory.DataPointCategoryName,
                PropertyId = x.DataPointPropertyId,
                PropertyName = x.DataPointProperty.DataPointPropertyName,
                IsRequired = false,
                IsInherited = false
            }).ToListAsync();

            var project = Context.Projects.Where(x => x.ProjectId == projectId).FirstOrDefault();

            var parentOfficeIds = await GetParentOfficeIds(project.ParentProgram.OwnerId);
            parentOfficeIds.Add(project.ParentProgram.OwnerId);

            var parentProgramIds = await GetParentProgramIds(project.ParentProgram.ProgramId);
            parentProgramIds.Add(project.ParentProgram.ProgramId);

            var parentDataPointConfigurations = DataPointConfigurationQueries.CreateGetDataPointConfigurations(this.Context)
                .Where(x => parentOfficeIds.Contains(x.OfficeId.Value) || parentProgramIds.Contains(x.ProgramId.Value)).ToList();
            foreach (var config in parentDataPointConfigurations)
            {
                var temp = dataPointConfigurations.Where(x => x.CategoryPropertyId == config.CategoryPropertyId).FirstOrDefault();
                temp.DataPointConfigurationId = config.DataPointConfigurationId;
                temp.IsRequired = true;
                temp.IsInherited = true;
            }

            var childDataPointConfigurations = DataPointConfigurationQueries.CreateGetDataPointConfigurations(this.Context)
                .Where(x => x.ProjectId == projectId).ToList();
            foreach (var config in childDataPointConfigurations)
            {
                var temp = dataPointConfigurations.Where(x => x.CategoryPropertyId == config.CategoryPropertyId).FirstOrDefault();
                if (temp.DataPointConfigurationId == null)
                {
                    temp.DataPointConfigurationId = config.DataPointConfigurationId;
                    temp.IsRequired = true;
                }
            }

            return dataPointConfigurations;
        }

        public async Task<List<int>> GetParentOfficeIds(int officeId)
        {
            var office = (await CreateGetOfficesSqlQuery().ToArrayAsync()).Where(x => x.OrganizationId == officeId).FirstOrDefault();

            var parentOfficeIds = new List<int>();

            if (office != null)
            {
                var paths = office.Path.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                paths = paths.Take(paths.Length - 1).ToArray();
                var path = "";

                for (var i = 0; i < paths.Length; i++)
                {
                    if (i == 0)
                    {
                        path = paths[i];
                    }
                    else
                    {
                        path += PATH_SPLIT_CHARS[0] + paths[i];
                    }
                    var parentOffice = (await CreateGetOfficesSqlQuery().ToArrayAsync()).Where(x => x.Path == path).FirstOrDefault();
                    Contract.Assert(parentOffice != null, String.Format("An office with the path [{0}] should exist.", path));
                    parentOfficeIds.Add(parentOffice.OrganizationId);
                }
            }

            return parentOfficeIds;
        }

        public async Task<List<int>> GetParentProgramIds(int programId)
        {
            var program = (await CreateGetProgramsSqlQuery().ToArrayAsync()).Where(x => x.ProgramId == programId).FirstOrDefault();

            var parentProgramIds = new List<int>();

            if (program != null)
            {
                var paths = program.Path.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                paths = paths.Take(paths.Length - 1).ToArray();
                var path = "";

                for (var i = 0; i < paths.Length; i++)
                {
                    if (i == 0)
                    {
                        path = paths[i];
                    }
                    else
                    {
                        path += PATH_SPLIT_CHARS[0] + paths[i];
                    }
                    var parentProgram = (await CreateGetProgramsSqlQuery().ToArrayAsync()).Where(x => x.Path == path).FirstOrDefault();
                    Contract.Assert(parentProgram != null, String.Format("A program with the path [{0}] should exist.", path));
                    parentProgramIds.Add(parentProgram.ProgramId);
                }
            }

            return parentProgramIds;
        }

        private DbRawSqlQuery<SimpleOfficeDTO> CreateGetOfficesSqlQuery()
        {
            return this.Context.Database.SqlQuery<SimpleOfficeDTO>(OfficeService.GET_OFFICES_SPROC_NAME);
        }

        private DbRawSqlQuery<OrganizationProgramDTO> CreateGetProgramsSqlQuery()
        {
            return this.Context.Database.SqlQuery<OrganizationProgramDTO>(OfficeService.GET_PROGRAMS_SPROC_NAME);
        }
    }
}
