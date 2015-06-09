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
            this.ParticipantStatusId = ParticipantStatus.Active.Id;
        }

        public Audit Audit { get; private set; }

        public int ProjectId { get; private set; }

        public int ParticipantStatusId { get; private set; }

        public int ParticipantTypeId { get; protected set; }

        public void UpdateParticipant(Participant participant)
        {
            Contract.Requires(participant != null, "The participant must not be null.");
            //participant.ProjectId = this.ProjectId;
            participant.ParticipantStatusId = this.ParticipantStatusId;
            UpdateParticipantDetails(participant);
        }

        protected abstract void UpdateParticipantDetails(Participant participant);
    }
}
