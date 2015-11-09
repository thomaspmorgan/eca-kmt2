using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using System.Threading.Tasks;

namespace ECA.Business.Service.Lookup
{
    /// <summary>
    /// An IPositionService is capable of performing crud operations on SEVIS ProgramCategories.
    /// </summary>
    public interface IInternationalOrganizationService
    {
        /// <summary>
        /// Returns paged, filtered and sorted International Organizations in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The ProgramCategories in the system.</returns>
        PagedQueryResults<SimpleSevisLookupDTO> Get(QueryableOperator<SimpleSevisLookupDTO> queryOperator);

        /// <summary>
        /// Returns paged, filtered, and sorted International Organizations s in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The ProgramCategories in the system.</returns>
        Task<PagedQueryResults<SimpleSevisLookupDTO>> GetAsync(QueryableOperator<SimpleSevisLookupDTO> queryOperator);
    }
}
