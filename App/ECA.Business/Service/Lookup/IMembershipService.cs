using System;
namespace ECA.Business.Service.Lookup
{
    /// <summary>
    /// An IMembershipService is capable of performing crud operations on person memberships.
    /// </summary>
    public interface IMembershipService
    {
        /// <summary>
        /// Returns paged, filtered, and sorted Memberships in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The memberships in the system.</returns>
        ECA.Core.Query.PagedQueryResults<ECA.Business.Service.Lookup.MembershipDTO> Get(ECA.Core.DynamicLinq.QueryableOperator<ECA.Business.Service.Lookup.MembershipDTO> queryOperator);

        /// <summary>
        /// Returns paged, filtered, and sorted memberships in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The memberships in the system.</returns>
        System.Threading.Tasks.Task<ECA.Core.Query.PagedQueryResults<ECA.Business.Service.Lookup.MembershipDTO>> GetAsync(ECA.Core.DynamicLinq.QueryableOperator<ECA.Business.Service.Lookup.MembershipDTO> queryOperator);
    }
}