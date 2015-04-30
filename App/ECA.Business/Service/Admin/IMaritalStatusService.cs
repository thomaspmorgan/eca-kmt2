using ECA.Business.Service.Lookup;
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
    /// Interface for marital status service
    /// </summary>
    public interface IMaritalStatusService
    {
        /// <summary>
        /// Gets a list of marital statuses
        /// </summary>
        /// <param name="queryOperator">The query operator to apply</param>
        /// <returns>A list of marital statuses</returns>
        PagedQueryResults<SimpleLookupDTO> Get(QueryableOperator<SimpleLookupDTO> queryOperator);

        /// <summary>
        /// Gets a list of marital statuses asyncronously
        /// </summary>
        /// <param name="queryOperator">The query operator to apply</param>
        /// <returns>A list of marital statuses</returns>
        Task<PagedQueryResults<SimpleLookupDTO>> GetAsync(QueryableOperator<SimpleLookupDTO> queryableOperator);
    }
}
