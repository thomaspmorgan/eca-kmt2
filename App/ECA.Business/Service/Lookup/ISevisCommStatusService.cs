using ECA.Core.DynamicLinq;
using ECA.Core.Query;

namespace ECA.Business.Service.Lookup
{
    /// <summary>
    /// An ISevisCommStatusService is capable of performing crud operations on prominent categories.
    /// </summary>
    public interface ISevisCommStatusService
    {
        /// <summary>
        /// Returns paged, filtered, and SEVIS Communication Statuses in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The SEVIS Communication Statuses  in the system.</returns>
        PagedQueryResults<SevisCommStatusDTO> Get(QueryableOperator<SevisCommStatusDTO> queryOperator);

        /// <summary>
        /// Returns paged, filtered, and sorted SEVIS Communication Statuses in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The SEVIS Communication Statuses in the system.</returns>
        System.Threading.Tasks.Task<PagedQueryResults<SevisCommStatusDTO>> GetAsync(QueryableOperator<SevisCommStatusDTO> queryOperator);
    }
}
