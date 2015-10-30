using ECA.Core.Exceptions;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Persons
{
    /// <summary>
    /// An UpdatedParticipantPerson is used by a business layer client to update a person that is a project participant.
    /// </summary>
    public class UpdatedParticipantPersonSevis : IAuditable
    {
        /// <summary>
        /// A class to update a Participant Persons SEVIS info
        /// </summary>
        /// <param name="updater"></param>
        /// <param name="participantId"></param>
        /// <param name="sevisId"></param>
        /// <param name="fieldOfStudyId"></param>
        /// <param name="positionId"></param>
        /// <param name="programCategoryId"></param>
        /// <param name="isSentToSevisViaRTI"></param>
        /// <param name="isValidatedViaRTI"></param>
        /// <param name="isCancelled"></param>
        /// <param name="isDS2019Printed"></param>
        /// <param name="isNeedsUpdate"></param>
        /// <param name="isDS2019SentToTraveler"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="fundingSponsor"></param>
        /// <param name="fundingPersonal"></param>
        /// <param name="fundingVisGovt"></param>
        /// <param name="fundingVisBNC"></param>
        /// <param name="fundingGovtAgency1"></param>
        /// <param name="fundingGovtAgency2"></param>
        /// <param name="fundingIntlOrg1"></param>
        /// <param name="fundingIntlOrg2"></param>
        /// <param name="fundingOther"></param>
        /// <param name="fundingTotal"></param>
        public UpdatedParticipantPersonSevis(
            User updater, 
            int participantId, 
            string sevisId,
            int? fieldOfStudyId,
            int? positionId,
            int? programCategoryId,
            bool isSentToSevisViaRTI,
            bool isValidatedViaRTI,
            bool isCancelled,
            bool isDS2019Printed,
            bool isNeedsUpdate,
            bool isDS2019SentToTraveler,
            DateTimeOffset? startDate,
            DateTimeOffset? endDate,
            decimal? fundingSponsor,
            decimal? fundingPersonal,
            decimal? fundingVisGovt,
            decimal? fundingVisBNC,
            decimal? fundingGovtAgency1,
            decimal? fundingGovtAgency2,
            decimal? fundingIntlOrg1,
            decimal? fundingIntlOrg2,
            decimal? fundingOther,
            decimal? fundingTotal)
        {
            this.Audit = new Update(updater);
            this.ParticipantId = participantId;
            this.SevisId = sevisId;
            this.FieldOfStudyId = fieldOfStudyId;
            this.ProgramCategoryId = programCategoryId;
            this.PositionId = positionId;
            this.IsSentToSevisViaRTI = isSentToSevisViaRTI;
            this.IsValidatedViaRTI = isValidatedViaRTI;
            this.IsCancelled = isCancelled;
            this.IsDS2019Printed = isDS2019Printed;
            this.IsNeedsUpdate = isNeedsUpdate;
            this.IsDS2019SentToTraveler = isDS2019SentToTraveler;
            this.StartDate = startDate;
            this.EndDate = endDate;
            this.FundingSponsor = fundingSponsor;
            this.FundingPersonal = fundingPersonal;
            this.FundingVisGovt = fundingVisGovt;
            this.FundingVisBNC = fundingVisBNC;
            this.FundingGovtAgency1 = fundingGovtAgency1;
            this.FundingGovtAgency2 = fundingGovtAgency2;
            this.FundingIntlOrg1 = fundingIntlOrg1;
            this.FundingIntlOrg2 = fundingIntlOrg2;
            this.FundingOther = fundingOther;
            this.FundingTotal = fundingTotal;
        }

        /// <summary>
        /// Gets the participant id.
        /// </summary>
        public int ParticipantId { get; private set; }

               /// <summary>
        /// Gets or sets the participant's sevis id
        /// </summary>
        public string SevisId { get; set; }

        /// <summary>
        /// Gets or sets the participantPerson's Field of Study
        /// </summary>
        public int? FieldOfStudyId { get; set; }

        /// <summary>
        /// Gets or sets the participantPersons's Position
        /// </summary>
        public int? PositionId { get; set; }

        /// <summary>
        /// Gets or sets the participantPerson's ProgramCategory
        /// </summary>
        public int? ProgramCategoryId { get; set; }

        /// <summary>
        /// has the participant been sent to Sevis via RTI (manual web interface)
        /// </summary>
        public bool IsSentToSevisViaRTI { get; set; }

        /// <summary>
        /// has the participant been validated via RTI (manual web interface)
        /// </summary>
        public bool IsValidatedViaRTI { get; set; }

        /// <summary>
        /// has the participant been cancelled
        /// </summary>
        public bool IsCancelled { get; set; }

        /// <summary>
        /// has the DS2019 been printed
        /// </summary>
        public bool IsDS2019Printed { get; set; }

        /// <summary>
        /// does the participant need updating in Sevis (previous Sevis data sent has been changed)
        /// </summary>
        public bool IsNeedsUpdate { get; set; }

        /// <summary>
        /// has the DS2019 been sent to the traveler
        /// </summary>
        public bool IsDS2019SentToTraveler { get; set; }

        /// <summary>
        /// the start date of the visit
        /// </summary>
        public DateTimeOffset? StartDate { get; set; }

        /// <summary>
        /// The end date of the visit
        /// </summary>
        public DateTimeOffset? EndDate { get; set; }

        /// <summary>
        /// Funding coming from the sponsor
        /// </summary>
        public decimal? FundingSponsor { get; set; }

        /// <summary>
        /// Funding coming from the visitor
        /// </summary>
        public decimal? FundingPersonal { get; set; }

        /// <summary>
        /// Funding from the visiting participant's government
        /// </summary>
        public decimal? FundingVisGovt { get; set; }

        /// <summary>
        /// Funding from the visiting participant's BNC
        /// </summary>
        public decimal? FundingVisBNC { get; set; }

        /// <summary>
        /// Funding from another U.S. government agency
        /// </summary>
        public decimal? FundingGovtAgency1 { get; set; }

        /// <summary>
        /// Funding from another U.S. government agency
        /// </summary>
        public decimal? FundingGovtAgency2 { get; set; }

        /// <summary>
        /// Funding from another international org
        /// </summary>
        public decimal? FundingIntlOrg1 { get; set; }

        /// <summary>
        /// Funding from another international org
        /// </summary>
        public decimal? FundingIntlOrg2 { get; set; }

        /// <summary>
        /// Funding from other source
        /// </summary>
        public decimal? FundingOther { get; set; }

        /// <summary>
        /// Total funding
        /// </summary>
        public decimal? FundingTotal { get; set; }

        /// <summary>
        /// Gets the update audit.
        /// </summary>
        public Audit Audit { get; private set; }
    }
}
