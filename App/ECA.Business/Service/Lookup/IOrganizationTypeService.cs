using System;
namespace ECA.Business.Service.Lookup
{
    /// <summary>
    /// An IOrganizationTypeService is capable of performing crud operations on organization types.
    /// </summary>
    public interface IOrganizationTypeService
    {
        /// <summary>
        /// Returns paged, filtered, and sorted organization types in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The organization types in the system.</returns>
        ECA.Core.Query.PagedQueryResults<ECA.Business.Queries.Models.Lookup.OrganizationTypeDTO> Get(ECA.Core.DynamicLinq.QueryableOperator<ECA.Business.Queries.Models.Lookup.OrganizationTypeDTO> queryOperator);

        /// <summary>
        /// Returns paged, filtered, and sorted organization types in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The organization types in the system.</returns>
        System.Threading.Tasks.Task<ECA.Core.Query.PagedQueryResults<ECA.Business.Queries.Models.Lookup.OrganizationTypeDTO>> GetAsync(ECA.Core.DynamicLinq.QueryableOperator<ECA.Business.Queries.Models.Lookup.OrganizationTypeDTO> queryOperator);
    }
}
