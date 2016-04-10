
namespace ECA.Business.Service.Lookup
{
    public interface IBirthCountryReasonService
    {
        /// <summary>
        /// Returns paged, filtered, and sorted birth country reasons in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The birth country reasons in the system.</returns>
        ECA.Core.Query.PagedQueryResults<ECA.Business.Service.Lookup.BirthCountryReasonDTO> Get(ECA.Core.DynamicLinq.QueryableOperator<ECA.Business.Service.Lookup.BirthCountryReasonDTO> queryOperator);

        /// <summary>
        /// Returns paged, filtered, and sorted birth country reasons in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The birth country reasons in the system.</returns>
        System.Threading.Tasks.Task<ECA.Core.Query.PagedQueryResults<ECA.Business.Service.Lookup.BirthCountryReasonDTO>> GetAsync(ECA.Core.DynamicLinq.QueryableOperator<ECA.Business.Service.Lookup.BirthCountryReasonDTO> queryOperator);
    }
}
