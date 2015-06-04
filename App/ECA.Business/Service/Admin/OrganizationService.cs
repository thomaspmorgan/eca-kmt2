using ECA.Business.Queries.Admin;
using System.Data.Entity;
using ECA.Business.Queries.Models.Admin;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using ECA.Core.Service;
using ECA.Data;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Admin
{
    /// <summary>
    /// Service implementation for organizations
    /// </summary>
    public class OrganizationService : DbContextService<EcaContext>, IOrganizationService
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">Context to query</param>
        public OrganizationService(EcaContext context)
            : base(context)
        {
            Contract.Requires(context != null, "The context must not be null.");
        }

        #region Get
        /// <summary>
        /// Returns list of organizations asyncronously
        /// </summary>
        /// <param name="queryOperator">The query operator to apply</param>
        /// <returns>List of organizations</returns>
        public Task<PagedQueryResults<SimpleOrganizationDTO>> GetOrganizationsAsync(QueryableOperator<SimpleOrganizationDTO> queryOperator)
        {
            var organizations = OrganizationQueries.CreateGetSimpleOrganizationsDTOQuery(this.Context, queryOperator).ToPagedQueryResultsAsync(queryOperator.Start, queryOperator.Limit);
            this.logger.Trace("Retrieved organizations with query operator [{0}].", queryOperator);
            return organizations;
        }

        /// <summary>
        /// Returns list of organizations 
        /// </summary>
        /// <param name="queryOperator">The query operator to apply</param>
        /// <returns>List of organizations</returns>
        public PagedQueryResults<SimpleOrganizationDTO> GetOrganizations(QueryableOperator<SimpleOrganizationDTO> queryOperator)
        {
            var organizations = OrganizationQueries.CreateGetSimpleOrganizationsDTOQuery(this.Context, queryOperator).ToPagedQueryResults(queryOperator.Start, queryOperator.Limit);
            this.logger.Trace("Retrieved organizations with query operator [{0}].", queryOperator);
            return organizations;
        }

        /// <summary>
        /// Gets the organization with the given id.
        /// </summary>
        /// <param name="organizationId">The organization id.</param>
        /// <returns>The organization.</returns>
        public OrganizationDTO GetOrganizationById(int organizationId)
        {
            var dto = OrganizationQueries.CreateGetOrganizationDTOByOrganizationIdQuery(this.Context, organizationId).FirstOrDefault();
            this.logger.Trace("Retreived organization by id [{0}].", organizationId);
            return dto;
        }

        /// <summary>
        /// Gets the organization with the given id.
        /// </summary>
        /// <param name="organizationId">The organization id.</param>
        /// <returns>The organization.</returns>
        public async Task<OrganizationDTO> GetOrganizationByIdAsync(int organizationId)
        {
            var dto = await OrganizationQueries.CreateGetOrganizationDTOByOrganizationIdQuery(this.Context, organizationId).FirstOrDefaultAsync();
            this.logger.Trace("Retreived organization by id [{0}].", organizationId);
            return dto;
        }

        #endregion
    }
}
