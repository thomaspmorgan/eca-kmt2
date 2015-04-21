using ECA.Business.Queries.Admin;
using ECA.Business.Queries.Models.Admin;
using ECA.Business.Queries.Models.Office;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using ECA.Core.Service;
using ECA.Data;
using NLog;
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
        /// <summary>
        /// Gets the name of the GetPrograms sproc in the database.
        /// </summary>
        private const string GET_PROGRAMS_SPROC_NAME = "GetPrograms";

        private const string GET_OFFICES_SPROC_NAME = "GetOffices";

        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Creates a new OfficeService with the context and logger implementations.
        /// </summary>
        /// <param name="context">The context to operate against.</param>
        public OfficeService(EcaContext context)
            : base(context)
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
    }
}
