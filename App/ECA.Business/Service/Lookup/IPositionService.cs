using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using System.Threading.Tasks;

namespace ECA.Business.Service.Lookup
{
    /// <summary>
    /// An IPositionService is capable of performing crud operations on SEVIS positions.
    /// </summary>
    public interface IPositionService
    {
        /// <summary>
        /// Returns paged, filtered and sorted Positions in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The Positions in the system.</returns>
        PagedQueryResults<SimpleSevisLookupDTO> Get(QueryableOperator<SimpleSevisLookupDTO> queryOperator);

        /// <summary>
        /// Returns paged, filtered, and sorted Positions in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The Positions in the system.</returns>
        Task<PagedQueryResults<SimpleSevisLookupDTO>> GetAsync(QueryableOperator<SimpleSevisLookupDTO> queryOperator);
    }
}
