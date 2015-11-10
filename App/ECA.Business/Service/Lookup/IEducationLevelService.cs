using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using System.Threading.Tasks;

namespace ECA.Business.Service.Lookup
{
    /// <summary>
    /// An IEducationLevel is capable of performing crud operations on SEVIS EducationLevels.
    /// </summary>
    public interface IEducationLevelService
    {
        /// <summary>
        /// Returns paged, filtered and sorted EducationLevels in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The EducationLevels in the system.</returns>
        PagedQueryResults<SimpleSevisLookupDTO> Get(QueryableOperator<SimpleSevisLookupDTO> queryOperator);

        /// <summary>
        /// Returns paged, filtered, and sorted EducationLevels in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The EducationLevels in the system.</returns>
        Task<PagedQueryResults<SimpleSevisLookupDTO>> GetAsync(QueryableOperator<SimpleSevisLookupDTO> queryOperator);
    }
}
