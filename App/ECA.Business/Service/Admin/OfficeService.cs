using ECA.Business.Queries.Admin;
using ECA.Business.Queries.Models.Admin;
using ECA.Business.Queries.Models.Office;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using ECA.Core.Service;
using ECA.Data;
using NLog;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;

namespace ECA.Business.Service.Admin
{
    /// <summary>
    /// An OfficeService is used to perform crud operations on an office given a DbContextService.
    /// </summary>
    public class OfficeService : DbContextService<EcaContext>, ECA.Business.Service.Admin.IOfficeService
    {
        public static char[] OFFICE_HIERARCHY_SPLIT_CHARS = new char[] { '-' };

        /// <summary>
        /// Gets the name of the GetPrograms sproc in the database.
        /// </summary>
        public const string GET_PROGRAMS_SPROC_NAME = "GetPrograms";

        public const string GET_OFFICES_SPROC_NAME = "GetOffices";

        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Creates a new OfficeService with the context and logger implementations.
        /// </summary>
        /// <param name="context">The context to operate against.</param>
        /// <param name="saveActions">The save actions.</param>
        public OfficeService(EcaContext context, List<ISaveAction> saveActions = null)
            : base(context, saveActions)
        {
            Contract.Requires(context != null, "The context must not be null.");
        }

        #region Get

        /// <summary>
        /// Returns the office with the given id.
        /// </summary>
        /// <param name="officeId">The id of the office.</param>
        /// <returns>The office with the given id or null.</returns>
        public OfficeDTO GetOfficeById(int officeId)
        {
            var dto = OfficeQueries.CreateGetOfficeByIdQuery(this.Context, officeId).FirstOrDefault();
            this.logger.Trace("Retrieved office by id [{0}].", officeId);
            return dto;
        }

        /// <summary>
        /// Returns the office with the given id.
        /// </summary>
        /// <param name="officeId">The id of the office.</param>
        /// <returns>The office with the given id or null.</returns>
        public async Task<OfficeDTO> GetOfficeByIdAsync(int officeId)
        {
            var dto = await OfficeQueries.CreateGetOfficeByIdQuery(this.Context, officeId).FirstOrDefaultAsync();
            this.logger.Trace("Retrieved office by id [{0}].", officeId);
            return dto;
        }

        /// <summary>
        /// Returns the first level child offices/branches/divisions of the office with the given id.
        /// </summary>
        /// <param name="officeId">The office id.</param>
        /// <returns>The child offices, branches, and divisions.</returns>
        public List<SimpleOfficeDTO> GetChildOffices(int officeId)
        {
            var childOffices = OfficeQueries.CreateGetChildOfficesByOfficeIdQuery(this.Context, officeId).ToList();
            this.logger.Trace("Retrieved child offices of office with id [{0}].", officeId);
            return childOffices;
        }

        /// <summary>
        /// Returns the first level child offices/branches/divisions of the office with the given id.
        /// </summary>
        /// <param name="officeId">The office id.</param>
        /// <returns>The child offices, branches, and divisions.</returns>
        public async Task<List<SimpleOfficeDTO>> GetChildOfficesAsync(int officeId)
        {
            var childOffices = await OfficeQueries.CreateGetChildOfficesByOfficeIdQuery(this.Context, officeId).ToListAsync();
            this.logger.Trace("Retrieved child offices of office with id [{0}].", officeId);
            return childOffices;
        }

        /// <summary>
        /// Returns the programs for the office with the given id.
        /// </summary>
        /// <param name="officeId">The id the office.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The office programs.</returns>
        public PagedQueryResults<OrganizationProgramDTO> GetPrograms(int officeId, QueryableOperator<OrganizationProgramDTO> queryOperator)
        {
            var results = CreateGetOrganizationProgramsSqlQuery().ToArray();
            var pagedResults = GetPagedQueryResults(officeId, results, queryOperator);
            this.logger.Trace("Retrieved programs with office id [{0}] and query operator [{1}].", officeId, queryOperator);
            return pagedResults;
        }

        /// <summary>
        /// Returns the programs for the office with the given id.
        /// </summary>
        /// <param name="officeId">The id the office.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The office programs.</returns>
        public async Task<PagedQueryResults<OrganizationProgramDTO>> GetProgramsAsync(int officeId, QueryableOperator<OrganizationProgramDTO> queryOperator)
        {
            var results = (await CreateGetOrganizationProgramsSqlQuery().ToArrayAsync());
            var pagedResults = GetPagedQueryResults(officeId, results, queryOperator);
            this.logger.Trace("Retrieved programs with office id [{0}] and query operator [{1}].", officeId, queryOperator);
            return pagedResults;
        }

        private DbRawSqlQuery<OrganizationProgramDTO> CreateGetOrganizationProgramsSqlQuery()
        {
            return this.Context.Database.SqlQuery<OrganizationProgramDTO>(GET_PROGRAMS_SPROC_NAME);
        }

        private PagedQueryResults<OrganizationProgramDTO> GetPagedQueryResults(int officeId, IEnumerable<OrganizationProgramDTO> enumerable, QueryableOperator<OrganizationProgramDTO> queryOperator)
        {
            return GetPagedQueryResults<OrganizationProgramDTO>(enumerable.Where(x => x.Owner_OrganizationId == officeId).ToList(), queryOperator);
        }

        private PagedQueryResults<T> GetPagedQueryResults<T>(IEnumerable<T> enumerable, QueryableOperator<T> queryOperator) where T : class
        {
            var queryable = enumerable.AsQueryable<T>();
            queryable = queryable.Apply(queryOperator);
            return queryable.ToPagedQueryResults<T>(queryOperator.Start, queryOperator.Limit);
        }

        /// <summary>
        /// Get list of offices in heirarchical list
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The child offices, branches, and divisions.</returns>
        public PagedQueryResults<SimpleOfficeDTO> GetOffices(QueryableOperator<SimpleOfficeDTO> queryOperator)
        {
            var results = CreateGetOfficesSqlQuery().ToArray();
            var pagedResults = GetPagedQueryResults(results, queryOperator);
            this.logger.Trace("Retrieved offices with query operator [{0}].", queryOperator);
            return pagedResults;
        }

        /// <summary>
        /// Returns the first level child offices/branches/divisions of the office with the given id.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The child offices, branches, and divisions.</returns>
        public async Task<PagedQueryResults<SimpleOfficeDTO>> GetOfficesAsync(QueryableOperator<SimpleOfficeDTO> queryOperator)
        {
            var results = await CreateGetOfficesSqlQuery().ToArrayAsync();
            var pagedResults = GetPagedQueryResults(results, queryOperator);
            this.logger.Trace("Retrieved offices with query operator [{0}].", queryOperator);
            return pagedResults;
        }

        private DbRawSqlQuery<SimpleOfficeDTO> CreateGetOfficesSqlQuery()
        {
            return this.Context.Database.SqlQuery<SimpleOfficeDTO>(GET_OFFICES_SPROC_NAME);
        }
        #endregion

        #region Settings
        private void CheckRepeatedKeys(IEnumerable<OfficeSettingDTO> settings)
        {
            var repeatedKeys = settings.Select(x => x.Name).GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key).ToList();
            if (repeatedKeys.Count > 0)
            {
                throw new NotSupportedException(String.Format("The office with id [{0}] has duplicated settings with keys [{1}].", settings.First().OfficeId, String.Join(", ", repeatedKeys)));
            }
        }

        /// <summary>
        /// Returns all settings for the office with the given id.
        /// </summary>
        /// <param name="officeId">The office id.</param>
        /// <returns>The settings.</returns>
        public List<OfficeSettingDTO> GetSettings(int officeId)
        {
            var settings = OfficeQueries.CreateGetOfficeSettingDTOByOfficeIdQuery(this.Context, officeId).ToList();
            logger.Info("Retrieved office settings for office with id [{0}].", officeId);
            CheckRepeatedKeys(settings);
            return settings;
        }

        /// <summary>
        /// Returns all settings for the office with the given id.
        /// </summary>
        /// <param name="officeId">The office id.</param>
        /// <returns>The settings.</returns>
        public async Task<List<OfficeSettingDTO>> GetSettingsAsync(int officeId)
        {
            var settings = await OfficeQueries.CreateGetOfficeSettingDTOByOfficeIdQuery(this.Context, officeId).ToListAsync();
            logger.Info("Retrieved office settings for office with id [{0}].", officeId);
            CheckRepeatedKeys(settings);
            return settings;
        }

        private string DoGetValue(int officeId, string name, IEnumerable<OfficeSettingDTO> settings)
        {
            var value = settings.Where(x => x.OfficeId == officeId && x.Name.ToLower().Trim() == name.ToLower().Trim()).FirstOrDefault();

            if (value == null)
            {
                logger.Trace("Office with id [{0}] does NOT have a setting with key [{1}].", officeId, name);
                return null;
            }
            else
            {
                var v = value.Value;
                logger.Trace("Office with id [{0}] has setting with key [{1}] has a string value of [{2}].", officeId, name, v);
                return v;
            }
        }

        /// <summary>
        /// Returns the setting value for the settings with the given name.
        /// </summary>
        /// <param name="officeId">The office to get a setting for.</param>
        /// <param name="name">The name of the setting.</param>
        /// <returns>The value of the setting, or null if it does not exist.</returns>
        public string GetValue(int officeId, string name)
        {
            var settings = GetSettings(officeId);
            return DoGetValue(officeId, name, settings);
        }

        /// <summary>
        /// Returns the setting value for the settings with the given name.
        /// </summary>
        /// <param name="officeId">The office to get a setting for.</param>
        /// <param name="name">The name of the setting.</param>
        /// <returns>The value of the setting, or null if it does not exist.</returns>
        public async Task<string> GetValueAsync(int officeId, string name)
        {
            var settings = await GetSettingsAsync(officeId);
            return DoGetValue(officeId, name, settings);
        }

        /// <summary>
        /// Returns the string value or the default value given the settings for an office and a key.
        /// </summary>
        /// <param name="name">The name of the setting to get a value for.</param>
        /// <param name="settings">The office settings.</param>
        /// <param name="defaultValue">The default value to return if the setting does not exist.</param>
        /// <returns>The string value for the setting with the given name, or the default value if it does not exist.</returns>
        public string GetStringValue(string name, List<OfficeSettingDTO> settings, string defaultValue)
        {
            Contract.Requires(name != null, "The name must not be null.");
            Contract.Requires(settings != null, "The settings must not be null.");
            var setting = settings.Where(x => x.Name.ToLower().Trim() == name.Trim().ToLower()).FirstOrDefault();
            if (setting != null)
            {
                logger.Info("Returning office setting value [{0}] for office setting [{1}] with office id [{2}].", setting.Value, name, setting.OfficeId);
                return setting.Value;
            }
            else
            {
                logger.Info("Returning default value [{0}] for office setting [{1}].", defaultValue, name);
                return defaultValue;
            }
        }

        /// <summary>
        /// Returns the boolean value or the default value given the settings for an office and a key.
        /// </summary>
        /// <param name="name">The name of the setting to get a value for.</param>
        /// <param name="settings">The office settings.</param>
        /// <param name="defaultValue">The default value to return if the setting does not exist.</param>
        /// <returns>The boolean value for the setting with the given name, or the default value if it does not exist.</returns>
        public bool GetStringValueAsBool(string name, List<OfficeSettingDTO> settings, bool defaultValue)
        {
            Contract.Requires(name != null, "The name must not be null.");
            Contract.Requires(settings != null, "The settings must not be null.");
            var setting = settings.Where(x => x.Name.ToLower().Trim() == name.Trim().ToLower()).FirstOrDefault();
            if (setting != null)
            {
                bool b;
                if (Boolean.TryParse(setting.Value, out b))
                {
                    logger.Info("Returning boolean value [{0}] for office setting [{1}] with office id [{2}].", b, setting.Name, setting.OfficeId);
                    return b;
                }
                else
                {
                    logger.Error("Unable to parse boolean value from string [{0}] for office setting with id [{1}].", setting.Value, setting.OfficeId);
                    return defaultValue;
                }
            }
            else
            {
                logger.Info("Returning default value [{0}] for office setting [{1}].", defaultValue, name);
                return defaultValue;
            }
        }

        /// <summary>
        /// Returns the int value or the default value given the settings for an office and a key.
        /// </summary>
        /// <param name="name">The name of the setting to get a value for.</param>
        /// <param name="settings">The office settings.</param>
        /// <param name="defaultValue">The default value to return if the setting does not exist.</param>
        /// <returns>The boolean value for the setting with the given name, or the default value if it does not exist.</returns>
        public int GetStringValueAsInt(string name, List<OfficeSettingDTO> settings, int defaultValue)
        {
            Contract.Requires(name != null, "The name must not be null.");
            Contract.Requires(settings != null, "The settings must not be null.");
            var setting = settings.Where(x => x.Name.ToLower().Trim() == name.Trim().ToLower()).FirstOrDefault();
            if (setting != null)
            {
                int i;
                if (Int32.TryParse(setting.Value, out i))
                {
                    logger.Info("Returning int value [{0}] for office setting [{1}] with office id [{2}].", i, setting.Name, setting.OfficeId);
                    return i;
                }
                else
                {
                    logger.Error("Unable to parse int value from string [{0}] for office setting with id [{1}].", setting.Value, setting.OfficeId);
                    return defaultValue;
                }
            }
            else
            {
                logger.Info("Returning default value [{0}] for office setting [{1}].", defaultValue, name);
                return defaultValue;
            }
        }

        /// <summary>
        /// Returns a boolean value indicating whether the given settings has a setting with the given name.
        /// </summary>
        /// <param name="name">The name i.e. key of the setting.</param>
        /// <param name="settings">The office settings.</param>
        /// <returns>True, if the settings contains the setting with the given name.</returns>
        public bool HasSetting(string name, List<OfficeSettingDTO> settings)
        {
            Contract.Requires(name != null, "The name must not be null.");
            Contract.Requires(settings != null, "The settings must not be null.");
            return settings.Where(x => x.Name.ToLower().Trim() == name.Trim().ToLower()).FirstOrDefault() != null;
        }

        private OfficeSettings DoGetOfficeSettings(List<OfficeSettingDTO> settings)
        {
            var officeSettings = new OfficeSettings();
            officeSettings.ObjectiveLabel = GetStringValue(OfficeSetting.OBJECTIVE_SETTING_KEY, settings, OfficeSettings.OBJECTIVE_DEFAULT_LABEL);
            officeSettings.CategoryLabel = GetStringValue(OfficeSetting.CATEGORY_SETTING_KEY, settings, OfficeSettings.CATEGORY_DEFAULT_LABEL);
            officeSettings.FocusLabel = GetStringValue(OfficeSetting.FOCUS_SETTING_KEY, settings, OfficeSettings.FOCUS_DEFAULT_LABEL);
            officeSettings.JustificationLabel = GetStringValue(OfficeSetting.JUSTIFICATION_SETTING_KEY, settings, OfficeSettings.JUSTIFICATION_DEFAULT_LABEL);

            officeSettings.IsObjectiveRequired = HasSetting(OfficeSetting.OBJECTIVE_SETTING_KEY, settings) || HasSetting(OfficeSetting.JUSTIFICATION_SETTING_KEY, settings);
            officeSettings.IsCategoryRequired = HasSetting(OfficeSetting.CATEGORY_SETTING_KEY, settings) || HasSetting(OfficeSetting.FOCUS_SETTING_KEY, settings);

            if (officeSettings.IsCategoryRequired)
            {
                officeSettings.MaximumRequiredFoci = GetStringValueAsInt(OfficeSetting.MAX_FOCUS_KEY, settings, 1);
                officeSettings.MinimumRequiredFoci = GetStringValueAsInt(OfficeSetting.MIN_FOCUS_KEY, settings, 1);
            }
            else
            {
                officeSettings.MaximumRequiredFoci = -1;
                officeSettings.MinimumRequiredFoci = -1;
            }

            return officeSettings;
        }

        /// <summary>
        /// Returns a business entity containing settings for an office with the given id.
        /// </summary>
        /// <param name="officeId">The id of the office.</param>
        /// <returns>The settings of the office.</returns>
        public OfficeSettings GetOfficeSettings(int officeId)
        {
            logger.Info("Retrieving office settings for office with id [{0}].", officeId);
            var settingDtos = GetSettings(officeId);
            return DoGetOfficeSettings(settingDtos);
        }

        /// <summary>
        /// Returns a business entity containing settings for an office with the given id.
        /// </summary>
        /// <param name="officeId">The id of the office.</param>
        /// <returns>The settings of the office.</returns>
        public async Task<OfficeSettings> GetOfficeSettingsAsync(int officeId)
        {
            logger.Info("Retrieving office settings for office with id [{0}].", officeId);
            var settingDtos = await GetSettingsAsync(officeId);
            return DoGetOfficeSettings(settingDtos);
        }

        /// <summary>
        /// Returns all data point configurations for the specified office
        /// </summary>
        /// <param name="officeId">The office id</param>
        /// <returns>List of data point configurations</returns>
        public async Task<List<DataPointConfigurationDTO>> GetOfficeDataPointConfigurationsAsync(int officeId)
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
            var parentDataPointConfigurations = OfficeQueries.CreateGetOfficeDataPointConfigurations(this.Context)
                .Where(x => parentOfficeIds.Contains(x.OfficeId.Value)).ToList();
            foreach(var config in parentDataPointConfigurations)
            {
                var temp = dataPointConfigurations.Where(x => x.CategoryPropertyId == config.CategoryPropertyId).FirstOrDefault();
                temp.DataPointConfigurationId = config.DataPointConfigurationId;
                temp.IsRequired = true;
                temp.IsInherited = true;
            }

            // Get child office data configs
            var childDataPointConfigurations = OfficeQueries.CreateGetOfficeDataPointConfigurations(this.Context)
                .Where(x => x.OfficeId == officeId).ToList();
            foreach(var config in childDataPointConfigurations)
            {
                var temp = dataPointConfigurations.Where(x => x.CategoryPropertyId == config.CategoryPropertyId).FirstOrDefault();
                if(temp.DataPointConfigurationId == null)
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
                    } else
                    {
                        path += OFFICE_HIERARCHY_SPLIT_CHARS[0] + paths[i];
                    }
                    var parentOffice = (await CreateGetOfficesSqlQuery().ToArrayAsync()).Where(x => x.Path == path).FirstOrDefault();
                    Contract.Assert(parentOffice != null, String.Format("An office with the path [{0}] should exist.", path));
                    parentOfficeIds.Add(parentOffice.OrganizationId);
                }
            }

            return parentOfficeIds;
        }
        #endregion
    }
}
