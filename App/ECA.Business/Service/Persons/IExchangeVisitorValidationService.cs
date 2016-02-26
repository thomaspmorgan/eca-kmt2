using System.Threading.Tasks;
using ECA.Data;
using ECA.Core.Service;

namespace ECA.Business.Service.Persons
{
    /// <summary>
    /// The IExchangeVisitorValidationService is used to execute sevis validation on a participant and update the sevis comm status.
    /// </summary>
    public interface IExchangeVisitorValidationService : ISaveable
    {
        /// <summary>
        /// Performs sevis validation and returns the sevis comm status.
        /// </summary>
        /// <param name="user">The user requesting the validation.</param>
        /// <param name="projectId">The project id of the participant.</param>
        /// <param name="participantId">The id of the participant to run validation for.</param>
        /// <returns>The new sevis comm status of the participant.</returns>
        ParticipantPersonSevisCommStatus RunParticipantSevisValidation(User user, int projectId, int participantId);

        /// <summary>
        /// Performs sevis validation and returns the sevis comm status.
        /// </summary>
        /// <param name="user">The user requesting the validation.</param>
        /// <param name="projectId">The project id of the participant.</param>
        /// <param name="participantId">The id of the participant to run validation for.</param>
        /// <returns>The new sevis comm status of the participant.</returns>
        Task<ParticipantPersonSevisCommStatus> RunParticipantSevisValidationAsync(User user, int projectId, int participantId);
    }
}