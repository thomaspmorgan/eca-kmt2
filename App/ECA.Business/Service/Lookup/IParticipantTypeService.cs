using System;
namespace ECA.Business.Service.Lookup
{
    /// <summary>
    /// An IParticipantTypeService is capable of performing crud operations on participant types.
    /// </summary>
    public interface IParticipantTypeService
    {
        /// <summary>
        /// Returns paged, filtered, and sorted participant types in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The participant types in the system.</returns>
        ECA.Core.Query.PagedQueryResults<ECA.Business.Service.Lookup.ParticipantTypeDTO> Get(ECA.Core.DynamicLinq.QueryableOperator<ECA.Business.Service.Lookup.ParticipantTypeDTO> queryOperator);

        /// <summary>
        /// Returns paged, filtered, and sorted participant types in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The participant types in the system.</returns>
        System.Threading.Tasks.Task<ECA.Core.Query.PagedQueryResults<ECA.Business.Service.Lookup.ParticipantTypeDTO>> GetAsync(ECA.Core.DynamicLinq.QueryableOperator<ECA.Business.Service.Lookup.ParticipantTypeDTO> queryOperator);
    }
}
