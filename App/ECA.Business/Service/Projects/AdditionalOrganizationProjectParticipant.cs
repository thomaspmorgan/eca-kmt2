using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Projects
{
    public class AdditionalOrganizationProjectParticipant : AdditionalProjectParticipant
    {
        public AdditionalOrganizationProjectParticipant(User projectOwner, int projectId, int organizationId)
            : base(projectOwner, projectId)
        {
            Contract.Requires(projectOwner != null, "The project owner must not be null.");
            this.OrganizationId = organizationId;
        }

        public int OrganizationId { get; private set; }

        protected override void UpdateParticipantDetails(Participant participant)
        {
            participant.OrganizationId = this.OrganizationId;
        }
    }
}
