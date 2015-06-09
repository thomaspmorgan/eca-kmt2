using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Projects
{
    public abstract class AdditionalProjectParticipant
    {
        public AdditionalProjectParticipant(User projectOwner, int projectId)
        {
            Contract.Requires(projectOwner != null, "The project owner must not be null.");
            this.Audit = new Create(projectOwner);
            this.ProjectId = projectId;
        }

        public Audit Audit { get; private set; }

        public int ProjectId { get; private set; }

        public abstract void UpdateParticipant(Participant participant);
    }
}
