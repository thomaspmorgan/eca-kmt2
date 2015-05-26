using System;
namespace ECA.Business.Service.Persons
{
    /// <summary>
    /// An IParticipantService is capable of performing crud operations on participants.
    /// </summary>
    public interface IParticipantPersonService
    {
        /// <summary>
        /// Returns the participantPersons in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The participantPersons.</returns>
        ECA.Core.Query.PagedQueryResults<ECA.Business.Queries.Models.Persons.SimpleParticipantPersonDTO> GetParticipantPersons(ECA.Core.DynamicLinq.QueryableOperator<ECA.Business.Queries.Models.Persons.SimpleParticipantPersonDTO> queryOperator);

        /// <summary>
        /// Returns the participantPersons in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The participantPersons.</returns>
        System.Threading.Tasks.Task<ECA.Core.Query.PagedQueryResults<ECA.Business.Queries.Models.Persons.SimpleParticipantPersonDTO>> GetParticipantPersonsAsync(ECA.Core.DynamicLinq.QueryableOperator<ECA.Business.Queries.Models.Persons.SimpleParticipantPersonDTO> queryOperator);

        /// <summary>
        /// Returns the participantPersons for the project with the given id in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <param name="projectId">The id of the project.</param>
        /// <returns>The participantPersons.</returns>
        ECA.Core.Query.PagedQueryResults<ECA.Business.Queries.Models.Persons.SimpleParticipantPersonDTO> GetParticipantPersonsByProjectId(int projectId, ECA.Core.DynamicLinq.QueryableOperator<ECA.Business.Queries.Models.Persons.SimpleParticipantPersonDTO> queryOperator);

        /// <summary>
        /// Returns the participantPersons for the project with the given id in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <param name="projectId">The id of the project.</param>
        /// <returns>The participantPersons.</returns>
        System.Threading.Tasks.Task<ECA.Core.Query.PagedQueryResults<ECA.Business.Queries.Models.Persons.SimpleParticipantPersonDTO>> GetParticipantPersonsByProjectIdAsync(int projectId, ECA.Core.DynamicLinq.QueryableOperator<ECA.Business.Queries.Models.Persons.SimpleParticipantPersonDTO> queryOperator);

        /// <summary>
        /// Returns the participantPerson by id
        /// </summary>
        /// <param name="participantId">The participant id to lookup</param>
        /// <returns>The participantPerson</returns>
        ECA.Business.Queries.Models.Persons.SimpleParticipantPersonDTO GetParticipantPersonById(int participantId);

        /// <summary>
        /// Returns the participantPerson by id asyncronously
        /// </summary>
        /// <param name="participantId">The participant id to lookup</param>
        /// <returns>The participantPerson</returns>
        System.Threading.Tasks.Task<ECA.Business.Queries.Models.Persons.SimpleParticipantPersonDTO> GetParticipantPersonByIdAsync(int participantId);
    }
}
