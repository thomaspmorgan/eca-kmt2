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
    /// Interface for gender service
    /// </summary>
    public interface IGenderService
    {
        /// <summary>
        /// Gets a list of genders
        /// </summary>
        /// <param name="queryOperator">The query operator to apply</param>
        /// <returns>A list of genders</returns>
        PagedQueryResults<SimpleLookupDTO> Get(QueryableOperator<SimpleLookupDTO> queryOperator);

        /// <summary>
        /// Gets a list of genders asynchronous
        /// </summary>
        /// <param name="queryOperator">The query operator to apply</param>
        /// <returns>A list of genders</returns>
        Task<PagedQueryResults<SimpleLookupDTO>> GetAsync(QueryableOperator<SimpleLookupDTO> queryOperator);
    }
}
