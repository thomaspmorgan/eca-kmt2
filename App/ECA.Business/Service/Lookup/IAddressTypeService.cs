using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Lookup
{
    /// <summary>
    /// An IAddressTypeService is capable of performing crud operations on address types.
    /// </summary>
    public interface IAddressTypeService
    {
        /// <summary>
        /// Returns paged, filtered, and sorted address types in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The address types in the system.</returns>
        ECA.Core.Query.PagedQueryResults<ECA.Business.Service.Lookup.AddressTypeDTO> Get(ECA.Core.DynamicLinq.QueryableOperator<ECA.Business.Service.Lookup.AddressTypeDTO> queryOperator);

        /// <summary>
        /// Returns paged, filtered, and sorted address types in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The address types in the system.</returns>
        System.Threading.Tasks.Task<ECA.Core.Query.PagedQueryResults<ECA.Business.Service.Lookup.AddressTypeDTO>> GetAsync(ECA.Core.DynamicLinq.QueryableOperator<ECA.Business.Service.Lookup.AddressTypeDTO> queryOperator);
    }
}
