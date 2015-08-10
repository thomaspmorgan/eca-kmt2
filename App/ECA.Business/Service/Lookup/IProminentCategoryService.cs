using System;
namespace ECA.Business.Service.Lookup
{
    /// <summary>
    /// An IParticipantTypeService is capable of performing crud operations on prominent categories.
    /// </summary>
    public interface IProminentCategoryService
    {
        /// <summary>
        /// Returns paged, filtered, and sorted prominent categories in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The prominent categories  in the system.</returns>
        ECA.Core.Query.PagedQueryResults<ECA.Business.Service.Lookup.ProminentCategoryDTO> Get(ECA.Core.DynamicLinq.QueryableOperator<ECA.Business.Service.Lookup.ProminentCategoryDTO> queryOperator);

        /// <summary>
        /// Returns paged, filtered, and sorted prominent categories in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The prominent categories in the system.</returns>
        System.Threading.Tasks.Task<ECA.Core.Query.PagedQueryResults<ECA.Business.Service.Lookup.ProminentCategoryDTO>> GetAsync(ECA.Core.DynamicLinq.QueryableOperator<ECA.Business.Service.Lookup.ProminentCategoryDTO> queryOperator);
    }
}
