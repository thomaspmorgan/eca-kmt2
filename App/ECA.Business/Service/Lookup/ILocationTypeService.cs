using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Lookup
{
    /// <summary>
    /// An ILocationTypeService is capable of performing crud operations on location types.
    /// </summary>
    public interface ILocationTypeService
    {
        /// <summary>
        /// Returns paged, filtered, and sorted location types in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The location types in the system.</returns>
        ECA.Core.Query.PagedQueryResults<ECA.Business.Service.Lookup.SimpleLookupDTO> Get(ECA.Core.DynamicLinq.QueryableOperator<ECA.Business.Service.Lookup.SimpleLookupDTO> queryOperator);

        /// <summary>
        /// Returns paged, filtered, and sorted location types in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The location types in the system.</returns>
        System.Threading.Tasks.Task<ECA.Core.Query.PagedQueryResults<ECA.Business.Service.Lookup.SimpleLookupDTO>> GetAsync(ECA.Core.DynamicLinq.QueryableOperator<ECA.Business.Service.Lookup.SimpleLookupDTO> queryOperator);
    }
}
