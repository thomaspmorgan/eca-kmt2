using ECA.Core.Exceptions;
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
    /// An AdditionalProjectPariticipant is used to add a new participant to a project.
    /// </summary>
    [ContractClass(typeof(AdditionalProjectParticipantContract))]
    public abstract class AdditionalProjectParticipant
    {
        /// <summary>
        /// Creates a new AdditionalProjectParticipant with the project owner that is adding the participant
        /// and the project id.
        /// </summary>
        /// <param name="projectOwner">The user adding the participant.</param>
        /// <param name="projectId">The project id.</param>
        /// <param name="participantTypeId">The participant type id.</param>
        public AdditionalProjectParticipant(User projectOwner, int projectId, int participantTypeId)
        {
            Contract.Requires(projectOwner != null, "The project owner must not be null.");
            this.Audit = new Create(projectOwner);
            this.ProjectId = projectId;
            this.ParticipantStatusId = ParticipantStatus.Active.Id;
            var participantType = ParticipantType.GetStaticLookup(participantTypeId);
            if (participantType == null)
            {
                throw new UnknownStaticLookupException(String.Format("The participant type with id [{0}] is not recognized.", participantTypeId));
            }
            this.ParticipantTypeId = participantTypeId;
        }

        /// <summary>
        /// Gets the Audit.
        /// </summary>
        public Audit Audit { get; private set; }

        /// <summary>
        /// Gets the project id.
        /// </summary>
        public int ProjectId { get; private set; }

        /// <summary>
        /// Gets the participant status id.
        /// </summary>
        public int ParticipantStatusId { get; private set; }

        /// <summary>
        /// Gets or sets the participant type id.
        /// </summary>
        public int ParticipantTypeId { get; private set; }

        /// <summary>
        /// Updates the given participant with the AdditionalProjectParticipant details.
        /// </summary>
        /// <param name="participant">The participant that is being created.</param>
        /// <param name="participantType">The participant type.</param>
        public void UpdateParticipant(Participant participant, ParticipantType participantType, VisitorType visitorType, DefaultExchangeVisitorFunding defaultExchangeVisitorFunding)
        {
            Contract.Requires(participant != null, "The participant must not be null.");
            Contract.Requires(participantType != null, "The participant type must not be null.");
            participant.ProjectId = this.ProjectId;
            participant.ParticipantStatusId = this.ParticipantStatusId;
            participant.ParticipantTypeId = participantType.ParticipantTypeId;
            UpdateParticipantDetails(participant, visitorType, defaultExchangeVisitorFunding);
        }

        /// <summary>
        /// Performs participant type specific logic.
        /// </summary>
        /// <param name="participant">The participant being created.</param>
        protected abstract void UpdateParticipantDetails(Participant participant, VisitorType visitorType, DefaultExchangeVisitorFunding defaultExchangeVisitorFunding);
    }

    /// <summary>
    /// AdditionalProjectParticipantContract
    /// </summary>
    [ContractClassFor(typeof(AdditionalProjectParticipant))]
    public abstract class AdditionalProjectParticipantContract : AdditionalProjectParticipant
    {
        public AdditionalProjectParticipantContract(User projectOwner, int projectId, int participantTypeId)
            : base(projectOwner, projectId, participantTypeId)
        {
            Contract.Requires(projectOwner != null, "The project owner must not be null.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="participant"></param>
        protected override void UpdateParticipantDetails(Participant participant, VisitorType visitorType, DefaultExchangeVisitorFunding defaultExchangeVisitorFunding)
        {
            Contract.Requires(participant != null, "The participant must not be null.");
        }
    }
}
