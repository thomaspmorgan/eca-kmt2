using ECA.Business.Queries.Models.Admin;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
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
    public interface IOrganizationService
    {
        /// <summary>
        /// Returns a list of organizations asyncronously
        /// </summary>
        /// <param name="queryOperator">The query operator to apply</param>
        /// <returns>List of organizations</returns>
        Task<PagedQueryResults<SimpleOrganizationDTO>> GetOrganizationsAsync(QueryableOperator<SimpleOrganizationDTO> queryOperator);
    }
}
