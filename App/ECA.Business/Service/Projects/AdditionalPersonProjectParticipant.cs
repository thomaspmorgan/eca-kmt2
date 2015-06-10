using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Projects
{
    public class AdditionalPersonProjectParticipant : AdditionalProjectParticipant
    {
        public AdditionalPersonProjectParticipant(User projectOwner, int projectId, int personId)
            : base(projectOwner, projectId)
        {
            Contract.Requires(projectOwner != null, "The project owner must not be null.");
            this.PersonId = personId;
            this.ParticipantTypeId = ParticipantType.Individual.Id;
        }

        public int PersonId { get; private set; }

        protected override void UpdateParticipantDetails(Participant participant)
        {
            participant.PersonId = this.PersonId;
        }
    }
}
