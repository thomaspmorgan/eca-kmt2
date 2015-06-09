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
        public AdditionalPersonProjectParticipant(User projectOwner, int projectId, int personId) : base(projectOwner, projectId)
        {
            Contract.Requires(projectOwner != null, "The project owner must not be null.");
            this.PersonId = personId;
        }

        public int PersonId { get; private set; }

        public override void UpdateParticipant(Data.Participant participant)
        {
            //participant.PersonId = 
        }
    }
}
