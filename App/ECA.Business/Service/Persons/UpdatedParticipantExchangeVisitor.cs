using ECA.Data;
using System;

namespace ECA.Business.Service.Persons
{
    /// <summary>
    /// An UpdatedParticipantExchangeVisitor is used by a business layer client to update a person that is a project participant and an exchange visitor.
    /// </summary>
    public class UpdatedParticipantExchangeVisitor : IAuditable
    {
        /// <summary>
        /// A class to update a Participant Persons SEVIS info
        /// </summary>
        /// <param name="updater"></param>
        /// <param name="participantId"></param>
        /// <param name="fieldOfStudyId"></param>
        /// <param name="positionId"></param>
        /// <param name="programCategoryId"></param>
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
        public UpdatedParticipantExchangeVisitor(
            User updater, 
            int participantId, 
            int? fieldOfStudyId,
            int? positionId,
            int? programCategoryId,
            decimal? fundingSponsor,
            decimal? fundingPersonal,
            decimal? fundingVisGovt,
            decimal? fundingVisBNC,
            decimal? fundingGovtAgency1,
            int? govtAgency1Id,
            string govtAgency1OtherName,
            decimal? fundingGovtAgency2,
            int? govtAgency2Id,
            string govtAgency2OtherName,
            decimal? fundingIntlOrg1,
            int? intlOrg1Id,
            string intlOrg1OtherName,
            decimal? fundingIntlOrg2,
            int? intlOrg2Id,
            string intlOrg2OtherName,
            decimal? fundingOther,
            string otherName,
            decimal? fundingTotal)
        {
            this.Audit = new Update(updater);
            this.ParticipantId = participantId;
            this.FieldOfStudyId = fieldOfStudyId;
            this.ProgramCategoryId = programCategoryId;
            this.PositionId = positionId;
            this.FundingSponsor = fundingSponsor;
            this.FundingPersonal = fundingPersonal;
            this.FundingVisGovt = fundingVisGovt;
            this.FundingVisBNC = fundingVisBNC;
            this.FundingGovtAgency1 = fundingGovtAgency1;
            this.GovtAgency1Id = govtAgency1Id != 0 ? govtAgency1Id : null;
            this.GovtAgency1OtherName = govtAgency1OtherName;
            this.FundingGovtAgency2 = fundingGovtAgency2;
            this.GovtAgency2Id = govtAgency2Id !=0 ? govtAgency2Id: null;
            this.GovtAgency2OtherName = govtAgency2OtherName;
            this.FundingIntlOrg1 = fundingIntlOrg1;
            this.IntlOrg1Id = intlOrg1Id != 0 ? intlOrg1Id: null;
            this.IntlOrg1OtherName = intlOrg1OtherName;
            this.FundingIntlOrg2 = fundingIntlOrg2;
            this.IntlOrg2Id = intlOrg2Id !=0 ? intlOrg2Id: null;
            this.IntlOrg2OtherName = intlOrg2OtherName;
            this.FundingOther = fundingOther;
            this.OtherName = otherName;
            this.FundingTotal = fundingTotal;
        }

        /// <summary>
        /// Gets the participant id.
        /// </summary>
        public int ParticipantId { get; private set; }

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
        /// Id of  another U.S. government agency
        /// </summary>
        public int? GovtAgency1Id { get; set; }

        /// <summary>
        /// Name of another U.S. government agency
        /// </summary>
        public string GovtAgency1OtherName { get; set; }

        /// <summary>
        /// Funding from another U.S. government agency
        /// </summary>
        public decimal? FundingGovtAgency2 { get; set; }

        /// <summary>
        /// Id of  another U.S. government agency
        /// </summary>
        public int? GovtAgency2Id { get; set; }

        /// <summary>
        /// Name of another U.S. government agency
        /// </summary>
        public string GovtAgency2OtherName { get; set; }

        /// <summary>
        /// Funding from another international org
        /// </summary>
        public decimal? FundingIntlOrg1 { get; set; }

        /// <summary>
        /// Id of  another U.S. government agency
        /// </summary>
        public int? IntlOrg1Id { get; set; }

        /// <summary>
        /// Name of another U.S. government agency
        /// </summary>
        public string IntlOrg1OtherName { get; set; }

        /// <summary>
        /// Funding from another international org
        /// </summary>
        public decimal? FundingIntlOrg2 { get; set; }

        /// <summary>
        /// Id of  another U.S. government agency
        /// </summary>
        public int? IntlOrg2Id { get; set; }

        /// <summary>
        /// Name of another U.S. government agency
        /// </summary>
        public string IntlOrg2OtherName { get; set; }

        /// <summary>
        /// Funding from other source
        /// </summary>
        public decimal? FundingOther { get; set; }

        /// <summary>
        /// Name of another U.S. government agency
        /// </summary>
        public string OtherName { get; set; }

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
