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
        protected override void UpdateParticipantDetails(Participant participant, VisitorType visitorType, DefaultExchangeVisitorFunding defaultExchangeVisitorFunding)
        {
            participant.OrganizationId = null;
            participant.PersonId = this.PersonId;
            participant.ParticipantPerson = new ParticipantPerson
            {
                Participant = participant
            };
            if (visitorType != null && visitorType.VisitorTypeId == VisitorType.ExchangeVisitor.Id)
            {
                participant.ParticipantExchangeVisitor = new ParticipantExchangeVisitor
                {
                    Participant = participant,
                    ParticipantPerson = participant.ParticipantPerson
                };
                if (defaultExchangeVisitorFunding != null)
                {
                    participant.ParticipantExchangeVisitor.FundingSponsor = defaultExchangeVisitorFunding.FundingSponsor;
                    participant.ParticipantExchangeVisitor.FundingPersonal = defaultExchangeVisitorFunding.FundingPersonal;
                    participant.ParticipantExchangeVisitor.FundingVisGovt = defaultExchangeVisitorFunding.FundingVisGovt;
                    participant.ParticipantExchangeVisitor.FundingVisBNC = defaultExchangeVisitorFunding.FundingVisBNC;
                    participant.ParticipantExchangeVisitor.FundingGovtAgency1 = defaultExchangeVisitorFunding.FundingGovtAgency1;
                    participant.ParticipantExchangeVisitor.GovtAgency1Id = defaultExchangeVisitorFunding.GovtAgency1Id;
                    participant.ParticipantExchangeVisitor.GovtAgency1OtherName = defaultExchangeVisitorFunding.GovtAgency1OtherName;
                    participant.ParticipantExchangeVisitor.FundingGovtAgency2 = defaultExchangeVisitorFunding.FundingGovtAgency2;
                    participant.ParticipantExchangeVisitor.GovtAgency2Id = defaultExchangeVisitorFunding.GovtAgency2Id;
                    participant.ParticipantExchangeVisitor.GovtAgency2OtherName = defaultExchangeVisitorFunding.GovtAgency2OtherName;
                    participant.ParticipantExchangeVisitor.FundingIntlOrg1 = defaultExchangeVisitorFunding.FundingIntlOrg1;
                    participant.ParticipantExchangeVisitor.IntlOrg1Id = defaultExchangeVisitorFunding.IntlOrg1Id;
                    participant.ParticipantExchangeVisitor.IntlOrg1OtherName = defaultExchangeVisitorFunding.IntlOrg1OtherName;
                    participant.ParticipantExchangeVisitor.FundingIntlOrg2 = defaultExchangeVisitorFunding.FundingIntlOrg2;
                    participant.ParticipantExchangeVisitor.IntlOrg2Id = defaultExchangeVisitorFunding.IntlOrg2Id;
                    participant.ParticipantExchangeVisitor.IntlOrg2OtherName = defaultExchangeVisitorFunding.IntlOrg2OtherName;
                    participant.ParticipantExchangeVisitor.FundingOther = defaultExchangeVisitorFunding.FundingOther;
                    participant.ParticipantExchangeVisitor.OtherName = defaultExchangeVisitorFunding.OtherName;
                    participant.ParticipantExchangeVisitor.FundingTotal = defaultExchangeVisitorFunding.FundingTotal;

                }
            }
        }
    }
}
