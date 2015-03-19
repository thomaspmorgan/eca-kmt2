using ECA.Business.Queries.Admin;
using ECA.Business.Queries.Models.Admin;
using ECA.Business.Queries.Models.Office;
using ECA.Core.DynamicLinq;
using ECA.Core.Logging;
using ECA.Core.Query;
using ECA.Core.Service;
using ECA.Data;
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
        public const string GET_PROGRAMS_SPROC_NAME = "GetPrograms";

        private static readonly string COMPONENT_NAME = typeof(OfficeService).FullName;

        private readonly ILogger logger;

        /// <summary>
        /// Creates a new OfficeService with the context and logger implementations.
        /// </summary>
        /// <param name="context">The context to operate against.</param>
        /// <param name="logger">the logger.</param>
        public OfficeService(EcaContext context, ILogger logger)
            : base(context, logger)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(logger != null, "The logger must not be null.");
            this.logger = logger;
        }

        #region Get

        /// <summary>
        /// Returns the office with the given id.
        /// </summary>
        /// <param name="officeId">The id of the office.</param>
        /// <returns>The office with the given id or null.</returns>
        public OfficeDTO GetOfficeById(int officeId)
        {
            var stopWatch = Stopwatch.StartNew();
            var dto = OfficeQueries.CreateGetOfficeByIdQuery(this.Context, officeId).FirstOrDefault();
            stopWatch.Stop();
            logger.TraceApi(COMPONENT_NAME, stopWatch.Elapsed);
            return dto;
        }

        /// <summary>
        /// Returns the office with the given id.
        /// </summary>
        /// <param name="officeId">The id of the office.</param>
        /// <returns>The office with the given id or null.</returns>
        public async Task<OfficeDTO> GetOfficeByIdAsync(int officeId)
        {
            var stopWatch = Stopwatch.StartNew();
            var dto = await OfficeQueries.CreateGetOfficeByIdQuery(this.Context, officeId).FirstOrDefaultAsync();
            stopWatch.Stop();
            logger.TraceApi(COMPONENT_NAME, stopWatch.Elapsed);
            return dto;
        }

        /// <summary>
        /// Returns the programs for the office with the given id.
        /// </summary>
        /// <param name="officeId">The id the office.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The office programs.</returns>
        public PagedQueryResults<OrganizationProgramDTO> GetPrograms(int officeId, QueryableOperator<OrganizationProgramDTO> queryOperator)
        {
            var stopWatch = Stopwatch.StartNew();
            var results = CreateGetOrganizationProgramsSqlQuery().ToArray();
            stopWatch.Stop();            
            var pagedResults = GetPagedQueryResults(officeId, results, queryOperator);
            logger.TraceApi(COMPONENT_NAME, stopWatch.Elapsed);
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
            var stopWatch = Stopwatch.StartNew();
            var results = (await CreateGetOrganizationProgramsSqlQuery().ToArrayAsync());
            stopWatch.Stop();
            var pagedResults = GetPagedQueryResults(officeId, results, queryOperator);
            logger.TraceApi(COMPONENT_NAME, stopWatch.Elapsed);
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

        public List<SimpleOfficeDTO> GetOffices()
        {
            var stopWatch = Stopwatch.StartNew();
            var dto = OfficeQueries.CreateGetOfficesQuery(this.Context).ToList();
            stopWatch.Stop();
            logger.TraceApi(COMPONENT_NAME, stopWatch.Elapsed);
            return dto;
        }

        #endregion
    }
}
