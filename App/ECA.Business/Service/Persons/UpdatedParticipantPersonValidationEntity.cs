using ECA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Persons
{
    /// <summary>
    /// An UpdatedParticipantPersonValidationEntity is used to validate a business layer client's request to update a participant person.
    /// </summary>
    public class UpdatedParticipantPersonValidationEntity
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="participantType">The participant type.</param>
        public UpdatedParticipantPersonValidationEntity(ParticipantType participantType)
        {
            this.ParticipantType = participantType;
        }

        /// <summary>
        /// Gets the participant type.
        /// </summary>
        public ParticipantType ParticipantType { get; private set; }
    }
}
