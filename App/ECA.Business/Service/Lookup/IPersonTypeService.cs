using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Lookup
{
    /// <summary>
    /// An IPersonTypeService is capable of performing crud operations on person types.
    /// </summary>
    public interface IPersonTypeService
    {
        /// <summary>
        /// Returns paged, filtered, and sorted person types in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The person types in the system.</returns>
        ECA.Core.Query.PagedQueryResults<ECA.Business.Service.Lookup.PersonTypeDTO> Get(ECA.Core.DynamicLinq.QueryableOperator<ECA.Business.Service.Lookup.PersonTypeDTO> queryOperator);

        /// <summary>
        /// Returns paged, filtered, and sorted person types in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The person types in the system.</returns>
        System.Threading.Tasks.Task<ECA.Core.Query.PagedQueryResults<ECA.Business.Service.Lookup.PersonTypeDTO>> GetAsync(ECA.Core.DynamicLinq.QueryableOperator<ECA.Business.Service.Lookup.PersonTypeDTO> queryOperator);
    }
}
