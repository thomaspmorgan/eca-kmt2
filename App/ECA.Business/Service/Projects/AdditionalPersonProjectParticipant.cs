using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Projects
{
    /// <summary>
    /// An AdditionalPersonProjectParticipant represents a new project participant that is a person.
    /// </summary>
    public class AdditionalPersonProjectParticipant : AdditionalProjectParticipant
    {
        /// <summary>
        /// Creates a new AdditionalPersonProjectParticipant.
        /// </summary>
        /// <param name="projectOwner">The project owner that is adding the person as a participant.</param>
        /// <param name="projectId">The project id.</param>
        /// <param name="personId">The person id.</param>
        /// <param name="participantTypeId">The participant type id.</param>
        public AdditionalPersonProjectParticipant(User projectOwner, int projectId, int personId, int participantTypeId)
            : base(projectOwner, projectId, participantTypeId)
        {
            Contract.Requires(projectOwner != null, "The project owner must not be null.");
            this.PersonId = personId;
        }

        /// <summary>
        /// Gets the person id.
        /// </summary>
        public int PersonId { get; private set; }

        /// <summary>
        /// Sets the given participant to reference the person as a participant.
        /// </summary>
        /// <param name="participant">The participant to update.</param>
        protected override void UpdateParticipantDetails(Participant participant)
        {
            participant.OrganizationId = null;
            participant.PersonId = this.PersonId;
            participant.ParticipantPerson = new ParticipantPerson
            {
                Participant = participant
            };
            participant.ParticipantExchangeVisitor = new ParticipantExchangeVisitor
            {
                Participant = participant,
                ParticipantPerson = participant.ParticipantPerson
            };
        }
    }
}
