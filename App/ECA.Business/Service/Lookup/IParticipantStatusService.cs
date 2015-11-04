using System;
namespace ECA.Business.Service.Lookup
{
    /// <summary>
    /// An IParticipantTypeService is capable of performing crud operations on participant types.
    /// </summary>
    public interface IParticipantStatusService
    {
        /// <summary>
        /// Returns paged, filtered, and sorted participant statii in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The participant statii in the system.</returns>
        ECA.Core.Query.PagedQueryResults<ECA.Business.Service.Lookup.ParticipantStatusDTO> Get(ECA.Core.DynamicLinq.QueryableOperator<ECA.Business.Service.Lookup.ParticipantStatusDTO> queryOperator);

        /// <summary>
        /// Returns paged, filtered, and sorted participant statii in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The participant statii in the system.</returns>
        System.Threading.Tasks.Task<ECA.Core.Query.PagedQueryResults<ECA.Business.Service.Lookup.ParticipantStatusDTO>> GetAsync(ECA.Core.DynamicLinq.QueryableOperator<ECA.Business.Service.Lookup.ParticipantStatusDTO> queryOperator);
    }
}
