﻿using System;
using ECA.Business.Service.Persons;
using ECA.Business.Service;

namespace ECA.WebApi.Models.Person
{
    /// <summary>
    /// Binding model for editing membership
    /// </summary>
    public class UpdatedParticipantPersonSevisBindingModel
    {
        /// <summary>
        /// Gets the participant id.
        /// </summary>
        public int ParticipantId { get;  set; }

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
        /// <param name="user">The user updating the membership</param>
        /// <returns>Update membership business model</returns>
        public UpdatedParticipantPersonSevis ToUpdatedParticipantPersonSevis(User user)
        {
            return new UpdatedParticipantPersonSevis(
                updater: user,
                participantId: this.ParticipantId,
                sevisId: this.SevisId,
                fieldOfStudyId: this.FieldOfStudyId,
                positionId: this.PositionId,
                programCategoryId: this.ProgramCategoryId,
                isSentToSevisViaRTI: this.IsSentToSevisViaRTI,
                isValidatedViaRTI: this.IsValidatedViaRTI,
                isCancelled: this.IsCancelled,
                isDS2019Printed: this.IsDS2019Printed,
                isNeedsUpdate: this.IsNeedsUpdate,
                isDS2019SentToTraveler: this.IsDS2019SentToTraveler,
                startDate: this.StartDate,
                endDate: this.EndDate,
                fundingSponsor: this.FundingSponsor,
                fundingPersonal: this.FundingPersonal,
                fundingVisGovt: this.FundingVisGovt,
                fundingVisBNC: this.FundingVisBNC,
                fundingGovtAgency1: this.FundingGovtAgency1,
                govtAgency1Id: this.GovtAgency1Id,
                govtAgency1OtherName: this.GovtAgency1OtherName,
                fundingGovtAgency2: this.FundingGovtAgency2,
                govtAgency2Id: this.GovtAgency2Id,
                govtAgency2OtherName: this.GovtAgency2OtherName,
                fundingIntlOrg1: this.FundingIntlOrg1,
                intlOrg1Id: this.IntlOrg1Id,
                intlOrg1OtherName: this.IntlOrg1OtherName,
                fundingIntlOrg2: this.FundingIntlOrg2,
                intlOrg2Id: this.IntlOrg2Id,
                intlOrg2OtherName: this.IntlOrg2OtherName,
                fundingOther: this.FundingOther,
                otherName: this.OtherName,
                fundingTotal: this.FundingTotal
            );
        }
    }
}