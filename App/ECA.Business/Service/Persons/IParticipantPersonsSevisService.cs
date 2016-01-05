using ECA.Business.Queries.Models.Persons;
using ECA.Core.Query;
using System.Threading.Tasks;
using ECA.Core.DynamicLinq;
using System.Linq;
using ECA.Core.Service;

namespace ECA.Business.Service.Persons
{
    /// <summary>
    /// An IParticipantPersonSevisService is capable of performing crud operations on participants and their SEVIS information.
    /// </summary>
    public interface IParticipantPersonsSevisService: ISaveable 
    {
        /// <summary>
        /// Returns the participantPersonSevises in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The participantPersonSevises.</returns>
        PagedQueryResults<ParticipantPersonSevisDTO> GetParticipantPersonsSevis(QueryableOperator<ParticipantPersonSevisDTO> queryOperator);

        /// <summary>
        /// Returns the participantPersonSevises in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The participantPersonSevises.</returns>
        Task<PagedQueryResults<ParticipantPersonSevisDTO>> GetParticipantPersonsSevisAsync(QueryableOperator<ParticipantPersonSevisDTO> queryOperator);

        /// <summary>
        /// Returns the participantPersonSevises for the project with the given id in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <param name="projectId">The id of the project.</param>
        /// <returns>The participantPersonSevises.</returns>
        PagedQueryResults<ParticipantPersonSevisDTO> GetParticipantPersonsSevisByProjectId(int projectId, QueryableOperator<ParticipantPersonSevisDTO> queryOperator);

        /// <summary>
        /// Returns the participantPersonSevises for the project with the given id in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <param name="projectId">The id of the project.</param>
        /// <returns>The participantPersonSevises.</returns>
        Task<PagedQueryResults<ParticipantPersonSevisDTO>> GetParticipantPersonsSevisByProjectIdAsync(int projectId, QueryableOperator<ParticipantPersonSevisDTO> queryOperator);

        /// <summary>
        /// Returns the participantPersonSevis by id
        /// </summary>
        /// <param name="participantId">The participant id to lookup</param>
        /// <returns>The participantPersonSevis</returns>
        ParticipantPersonSevisDTO GetParticipantPersonsSevisById(int participantId);

        /// <summary>
        /// Returns the participantPersonSevis by id asyncronously
        /// </summary>
        /// <param name="participantId">The participant id to lookup</param>
        /// <returns>The participantPersonSevis</returns>
        Task<ParticipantPersonSevisDTO> GetParticipantPersonsSevisByIdAsync(int participantId);

        /// Sevis Comm Status

        /// <summary>
        /// Returns the participantPersonSevises in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The participantPersonSevises.</returns>
        PagedQueryResults<ParticipantPersonSevisCommStatusDTO> GetParticipantPersonsSevisCommStatuses(QueryableOperator<ParticipantPersonSevisCommStatusDTO> queryOperator);

        /// <summary>
        /// Returns the participantPersonSevises in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The participantPersonSevises.</returns>
        Task<PagedQueryResults<ParticipantPersonSevisCommStatusDTO>> GetParticipantPersonsSevisCommStatusesAsync(QueryableOperator<ParticipantPersonSevisCommStatusDTO> queryOperator);

        /// <summary>
        /// Returns a participantPersonSevisCommStatus
        /// </summary>
        /// <param name="participantId">The participantId to lookup</param>
        /// <returns>The participantPersonSevis</returns>
        PagedQueryResults<ParticipantPersonSevisCommStatusDTO> GetParticipantPersonsSevisCommStatusesById(int participantId, QueryableOperator<ParticipantPersonSevisCommStatusDTO> queryOperator);

        /// <summary>
        /// Returns a participantPersonSevisCommStatus asyncronously
        /// </summary>
        /// <param name="participantId">The participant id to lookup</param>
        /// <returns>The participantPersonSevisCommStatuses</returns>
        Task<PagedQueryResults<ParticipantPersonSevisCommStatusDTO>> GetParticipantPersonsSevisCommStatusesByIdAsync(int participantId, QueryableOperator<ParticipantPersonSevisCommStatusDTO> queryOperator);

        /// <summary>
        /// Returns a participantPersonSevisCommStatus
        /// </summary>
        /// <param name="participantId">The participant id to lookup</param>
        /// <returns>The participantPersonSevisCommStatuses</returns>
        IQueryable<ParticipantPersonSevisCommStatusDTO> GetParticipantPersonsSevisCommStatusesByParticipantIds(int[] participantIds);

        /// <summary>
        /// Updates a participant person SEVIS info with given updated SEVIS information.
        /// </summary>
        /// <param name="updatedParticipantPersonSevis">The updated participant person SEVIS info.</param>
        ParticipantPersonSevisDTO Update(UpdatedParticipantPersonSevis updatedPerson);

        /// <summary>
        /// Updates a participant person SEVIS info with given updated SEVIS information.
        /// </summary>
        /// <param name="updatedParticipantPersonSevis">The updated participant person SEVIS info.</param>
        /// <returns>The task.</returns>
        Task<ParticipantPersonSevisDTO> UpdateAsync(UpdatedParticipantPersonSevis updatedParticipantPersonSevis);

        /// <summary>
        /// Sets sevis communication status for participant ids
        /// </summary>
        /// <param name="participantIds">The participant ids to update communcation status</param>
        /// <returns>List of participant ids that were updated</returns>
        Task<int[]> SendToSevis(int[] participantIds);

        /// <summary>
        /// Update a participant SEVIS pre-validation status
        /// </summary>
        /// <param name="participantId">Participant ID</param>
        /// <param name="errorCount">Count of validation errors</param>
        /// <param name="isValid">Validation status</param>
        void UpdateParticipantPersonSevisCommStatus(int participantId, int errorCount, bool isValid);
    }
}
