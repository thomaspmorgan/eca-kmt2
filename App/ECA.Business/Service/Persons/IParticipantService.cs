using System;
namespace ECA.Business.Service.Persons
{
    /// <summary>
    /// An IParticipantService is capable of performing crud operations on participants.
    /// </summary>
    public interface IParticipantService
    {
        /// <summary>
        /// Returns the participants in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The participants.</returns>
        ECA.Core.Query.PagedQueryResults<ECA.Business.Queries.Models.Persons.SimpleParticipantDTO> GetParticipants(ECA.Core.DynamicLinq.QueryableOperator<ECA.Business.Queries.Models.Persons.SimpleParticipantDTO> queryOperator);

        /// <summary>
        /// Returns the participants in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The participants.</returns>
        System.Threading.Tasks.Task<ECA.Core.Query.PagedQueryResults<ECA.Business.Queries.Models.Persons.SimpleParticipantDTO>> GetParticipantsAsync(ECA.Core.DynamicLinq.QueryableOperator<ECA.Business.Queries.Models.Persons.SimpleParticipantDTO> queryOperator);

        /// <summary>
        /// Returns the participants for the project with the given id in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <param name="projectId">The id of the project.</param>
        /// <returns>The participants.</returns>
        ECA.Core.Query.PagedQueryResults<ECA.Business.Queries.Models.Persons.SimpleParticipantDTO> GetParticipantsByProjectId(int projectId, ECA.Core.DynamicLinq.QueryableOperator<ECA.Business.Queries.Models.Persons.SimpleParticipantDTO> queryOperator);

        /// <summary>
        /// Returns the participants for the project with the given id in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <param name="projectId">The id of the project.</param>
        /// <returns>The participants.</returns>
        System.Threading.Tasks.Task<ECA.Core.Query.PagedQueryResults<ECA.Business.Queries.Models.Persons.SimpleParticipantDTO>> GetParticipantsByProjectIdAsync(int projectId, ECA.Core.DynamicLinq.QueryableOperator<ECA.Business.Queries.Models.Persons.SimpleParticipantDTO> queryOperator);
    }
}
