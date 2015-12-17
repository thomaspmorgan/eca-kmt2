using System;
using ECA.Business.Service.Persons;
using ECA.Business.Service;

namespace ECA.WebApi.Models.Person
{
    /// <summary>
    /// Binding model for editing exchange visitors
    /// </summary>
    public class UpdatedParticipantExchangeVisitorBindingModel
    {
        /// <summary>
        /// Gets or sets the students id.
        /// </summary>
        public int ParticipantId { get; set; }

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
        /// Convert binding model to business model 
        /// </summary>
        /// <param name="user">The user updating the exchange visitor</param>
        /// <returns>Update exchange visitor business model</returns>
        public UpdatedParticipantExchangeVisitor ToUpdatedParticipantExchangeVisitor(User user)
        {
            return new UpdatedParticipantExchangeVisitor(
                updater: user,
                participantId: this.ParticipantId,
                fieldOfStudyId: this.FieldOfStudyId,
                positionId: this.PositionId,
                programCategoryId: this.ProgramCategoryId,
                fundingSponsor: this.FundingSponsor,
                fundingPersonal: this.FundingPersonal,
                fundingVisGovt: this.FundingVisGovt,
                fundingVisBNC: this.FundingVisBNC,
                fundingGovtAgency1: this.FundingGovtAgency1,
                govtAgency1Id: this.GovtAgency1Id,
                govtAgency1OtherName: GovtAgency1OtherName,
                fundingGovtAgency2: this.FundingGovtAgency2,
                govtAgency2Id: this.GovtAgency2Id,
                govtAgency2OtherName: GovtAgency2OtherName,
                fundingIntlOrg1: FundingIntlOrg1,
                intlOrg1Id: IntlOrg1Id,
                intlOrg1OtherName: IntlOrg1OtherName,
                fundingIntlOrg2: FundingIntlOrg2,
                intlOrg2Id: IntlOrg2Id,
                intlOrg2OtherName: IntlOrg2OtherName,
                fundingOther: FundingOther,
                otherName: OtherName,
                fundingTotal: FundingTotal
            );
        }
    }
}