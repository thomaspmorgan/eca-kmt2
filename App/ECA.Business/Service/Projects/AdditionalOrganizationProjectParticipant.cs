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
    /// An AdditionalOrganizationProjectParticipant represents a new project participant that is an organization.
    /// </summary>
    public class AdditionalOrganizationProjectParticipant : AdditionalProjectParticipant
    {
        /// <summary>
        /// Creates a new AdditionalOrganizationProjectParticipant.
        /// </summary>
        /// <param name="projectOwner">The project owner that is adding the organization participant.</param>
        /// <param name="projectId">The project id.</param>
        /// <param name="organizationId">The organization id.</param>
        /// <param name="participantTypeId">The participant type id.</param>
        public AdditionalOrganizationProjectParticipant(User projectOwner, int projectId, int organizationId, int participantTypeId)
            : base(projectOwner, projectId, participantTypeId)
        {
            Contract.Requires(projectOwner != null, "The project owner must not be null.");
            this.OrganizationId = organizationId;
        }

        /// <summary>
        /// Gets the organization id.
        /// </summary>
        public int OrganizationId { get; private set; }

        /// <summary>
        /// Sets the given participant to reference the organization as a participant.
        /// </summary>
        /// <param name="participant">The participant to update.</param>
        protected override void UpdateParticipantDetails(Participant participant, VisitorType visitorType, DefaultExchangeVisitorFunding defaultExchangeVisitorFunding)
        {
            participant.OrganizationId = this.OrganizationId;
            participant.PersonId = null;
        }
    }
}
