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
    public interface IParticipantPersonSevisService: ISaveable 
    {
        /// <summary>
        /// Returns the participantPersonSevises in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The participantPersonSevises.</returns>
        PagedQueryResults<ParticipantPersonSevisDTO> GetParticipantPersonSevises(QueryableOperator<ParticipantPersonSevisDTO> queryOperator);

        /// <summary>
        /// Returns the participantPersonSevises in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The participantPersonSevises.</returns>
        Task<PagedQueryResults<ParticipantPersonSevisDTO>> GetParticipantPersonSevisesAsync(QueryableOperator<ParticipantPersonSevisDTO> queryOperator);

        /// <summary>
        /// Returns the participantPersonSevises for the project with the given id in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <param name="projectId">The id of the project.</param>
        /// <returns>The participantPersonSevises.</returns>
        PagedQueryResults<ParticipantPersonSevisDTO> GetParticipantPersonSevisesByProjectId(int projectId, QueryableOperator<ParticipantPersonSevisDTO> queryOperator);

        /// <summary>
        /// Returns the participantPersonSevises for the project with the given id in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <param name="projectId">The id of the project.</param>
        /// <returns>The participantPersonSevises.</returns>
        Task<PagedQueryResults<ParticipantPersonSevisDTO>> GetParticipantPersonSevisesByProjectIdAsync(int projectId, QueryableOperator<ParticipantPersonSevisDTO> queryOperator);

        /// <summary>
        /// Returns the participantPersonSevis by id
        /// </summary>
        /// <param name="participantId">The participant id to lookup</param>
        /// <returns>The participantPersonSevis</returns>
        ParticipantPersonSevisDTO GetParticipantPersonSevisById(int participantId);

        /// <summary>
        /// Returns the participantPersonSevis by id asyncronously
        /// </summary>
        /// <param name="participantId">The participant id to lookup</param>
        /// <returns>The participantPersonSevis</returns>
        Task<ParticipantPersonSevisDTO> GetParticipantPersonSevisByIdAsync(int participantId);

        /// Sevis Comm Status

        /// <summary>
        /// Returns the participantPersonSevises in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The participantPersonSevises.</returns>
        PagedQueryResults<ParticipantPersonSevisCommStatusDTO> GetParticipantPersonSevisCommStatuses(QueryableOperator<ParticipantPersonSevisCommStatusDTO> queryOperator);

        /// <summary>
        /// Returns the participantPersonSevises in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The participantPersonSevises.</returns>
        Task<PagedQueryResults<ParticipantPersonSevisCommStatusDTO>> GetParticipantPersonSevisCommStatusesAsync(QueryableOperator<ParticipantPersonSevisCommStatusDTO> queryOperator);

        /// <summary>
        /// Returns a participantPersonSevisCommStatus
        /// </summary>
        /// <param name="participantId">The participantId to lookup</param>
        /// <returns>The participantPersonSevis</returns>
        PagedQueryResults<ParticipantPersonSevisCommStatusDTO> GetParticipantPersonSevisCommStatusesById(int participantId, QueryableOperator<ParticipantPersonSevisCommStatusDTO> queryOperator);

        /// <summary>
        /// Returns a participantPersonSevisCommStatus asyncronously
        /// </summary>
        /// <param name="participantId">The participant id to lookup</param>
        /// <returns>The participantPersonSevisCommStatuses</returns>
        Task<PagedQueryResults<ParticipantPersonSevisCommStatusDTO>> GetParticipantPersonSevisCommStatusesByIdAsync(int participantId, QueryableOperator<ParticipantPersonSevisCommStatusDTO> queryOperator);


        IQueryable<ParticipantPersonSevisCommStatusDTO> GetParticipantPersonSevisCommStatusesByParticipantIds(int[] participantIds);

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

    }
}
