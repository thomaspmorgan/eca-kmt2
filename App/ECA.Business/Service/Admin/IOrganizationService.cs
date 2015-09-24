using ECA.Business.Queries.Models.Admin;
using ECA.Business.Service.Lookup;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using ECA.Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Admin
{
    /// <summary>
    /// Interface for organization service
    /// </summary>
    public interface IOrganizationService : ISaveable
    {
        /// <summary>
        /// Returns a list of organizations asyncronously
        /// </summary>
        /// <param name="queryOperator">The query operator to apply</param>
        /// <returns>List of organizations</returns>
        Task<PagedQueryResults<SimpleOrganizationDTO>> GetOrganizationsAsync(QueryableOperator<SimpleOrganizationDTO> queryOperator);

        /// <summary>
        /// Returns a list of organizations
        /// </summary>
        /// <param name="queryOperator">The query operator to apply</param>
        /// <returns>List of organizations</returns>
        PagedQueryResults<SimpleOrganizationDTO> GetOrganizations(QueryableOperator<SimpleOrganizationDTO> queryOperator);

        /// <summary>
        /// Gets the organization with the given id.
        /// </summary>
        /// <param name="organizationId">The organization id.</param>
        /// <returns>The organization.</returns>
        OrganizationDTO GetOrganizationById(int organizationId);

        /// <summary>
        /// Gets the organization with the given id.
        /// </summary>
        /// <param name="organizationId">The organization id.</param>
        /// <returns>The organization.</returns>
        Task<OrganizationDTO> GetOrganizationByIdAsync(int organizationId);

        /// <summary>
        /// Updates an organization.
        /// </summary>
        /// <param name="organization">The updated organization.</param>
        void Update(EcaOrganization organization);

        /// <summary>
        /// Updates an organization.
        /// </summary>
        /// <param name="organization">The updated organization.</param>
        Task UpdateAsync(EcaOrganization organization);

        /// <summary>
        /// Gets the organization roles
        /// </summary>
        /// <param name="queryOperator">The query operator to apply</param>
        /// <returns>List of organization roles</returns>
        Task<PagedQueryResults<SimpleLookupDTO>> GetOrganizationRolesAsync(QueryableOperator<SimpleLookupDTO> queryOperator);
    }
}
