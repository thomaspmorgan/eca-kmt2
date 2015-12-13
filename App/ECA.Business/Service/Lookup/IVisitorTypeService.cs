using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using System.Threading.Tasks;

namespace ECA.Business.Service.Lookup
{
    /// <summary>
    /// An IVisitorTypeService is capable of performing crud operations on visitor types.
    /// </summary>
    public interface IVisitorTypeService
    {
        /// <summary>
        /// Returns paged, filtered, and sorted visitor types in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The visitor types in the system.</returns>
        PagedQueryResults<SimpleLookupDTO> Get(QueryableOperator<SimpleLookupDTO> queryOperator);

        /// <summary>
        /// Returns paged, filtered, and sorted organization types in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The organization types in the system.</returns>
       Task<PagedQueryResults<SimpleLookupDTO>> GetAsync(QueryableOperator<SimpleLookupDTO> queryOperator);
    }
}
