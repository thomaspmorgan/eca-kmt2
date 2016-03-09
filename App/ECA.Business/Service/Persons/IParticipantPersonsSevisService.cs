using ECA.Business.Queries.Models.Persons;
using ECA.Core.Service;
using System.Threading.Tasks;

namespace ECA.Business.Service.Persons
{
    /// <summary>
    /// An IParticipantPersonSevisService is capable of performing crud operations on participants and their SEVIS information.
    /// </summary>
    public interface IParticipantPersonsSevisService : ISaveable
    {
        /// <summary>
        /// Returns the participantPersonSevis by id
        /// </summary>
        /// <param name="participantId">The participant id to lookup</param>
        /// <param name="projectId">The project id.</param>
        /// <returns>The participantPersonSevis</returns>
        ParticipantPersonSevisDTO GetParticipantPersonsSevisById(int projectId, int participantId);

        /// <summary>
        /// Returns the participantPersonSevis by id asyncronously
        /// </summary>
        /// <param name="participantId">The participant id to lookup</param>
        /// <param name="projectId">The project id.</param>
        /// <returns>The participantPersonSevis</returns>
        Task<ParticipantPersonSevisDTO> GetParticipantPersonsSevisByIdAsync(int projectId, int participantId);

        /// <summary>
        /// Updates a participant person SEVIS info with given updated SEVIS information.
        /// </summary>
        /// <param name="updatedParticipantPersonSevis">The updated participant person SEVIS info.</param>
        void Update(UpdatedParticipantPersonSevis updatedPerson);

        /// <summary>
        /// Updates a participant person SEVIS info with given updated SEVIS information.
        /// </summary>
        /// <param name="updatedParticipantPersonSevis">The updated participant person SEVIS info.</param>
        /// <returns>The task.</returns>
        Task UpdateAsync(UpdatedParticipantPersonSevis updatedParticipantPersonSevis);

        /// <summary>
        /// Sets sevis communication status for participant ids
        /// </summary>
        /// <param name="participantIds">The participant ids to update communcation status</param>
        /// <param name="projectId">The id of the project the participants belong to.</param>
        /// <returns>List of participant ids that were updated</returns>
        Task<int[]> SendToSevisAsync(int projectId, int[] participantIds);

        /// <summary>
        /// Sets sevis communication status for participant ids
        /// </summary>
        /// <param name="participantIds">The participant ids to update communcation status</param>
        /// <param name="projectId">The id of the project the participants belong to.</param>
        /// <returns>List of participant ids that were updated</returns>
        int[] SendToSevis(int projectId, int[] participantIds);        
    }
}
