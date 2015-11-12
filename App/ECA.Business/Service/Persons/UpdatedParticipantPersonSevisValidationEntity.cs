
using ECA.Data;

namespace ECA.Business.Service.Persons
{
    /// <summary>
    /// An UpdatedParticipantPersonSevisValidationEntity is used to validate a business layer client's request to update a participant sevis person.
    /// </summary>
    public class UpdatedParticipantPersonSevisValidationEntity
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="updatedParticipantPersonSevis">The participant person sevis.</param>
        public UpdatedParticipantPersonSevisValidationEntity(ParticipantPerson participantPerson, UpdatedParticipantPersonSevis updatedParticipantPersonSevis)
        {
            this.participantPerson = participantPerson;
            this.updatedParticipantPersonSevis = updatedParticipantPersonSevis;
        }

        /// <summary>
        /// Gets the participant person object.
        /// </summary>
        public ParticipantPerson participantPerson { get; private set; }

        /// <summary>
        /// Gets the participant person sevis object.
        /// </summary>
        public UpdatedParticipantPersonSevis updatedParticipantPersonSevis { get; private set; }
    }
}
