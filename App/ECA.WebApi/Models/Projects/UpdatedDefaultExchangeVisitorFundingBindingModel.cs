using ECA.Business.Service;
using ECA.Business.Service.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Models.Projects
{
    /// <summary>
    /// Binding model for updating a default exchange visitor funding record
    /// </summary>
    public class UpdatedDefaultExchangeVisitorFundingBindingModel
    {
        /// <summary>
        /// Gets or sets the project id
        /// </summary>
        public int ProjectId { get; set; }

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
        /// Id of another U.S. government agency
        /// </summary>
        public int? GovtAgency1Id { get; set; }

        /// <summary>
        /// Other Name of another U.S. government agency
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
        /// Other Name of another U.S. government agency
        /// </summary>
        public string GovtAgency2OtherName { get; set; }

        /// <summary>
        /// Funding from another international org
        /// </summary>
        public decimal? FundingIntlOrg1 { get; set; }

        /// <summary>
        /// id of another International org
        /// </summary>
        public int? IntlOrg1Id { get; set; }

        /// <summary>
        /// Other Name of another International org
        /// </summary>
        public string IntlOrg1OtherName { get; set; }

        /// <summary>
        /// Funding from another international org
        /// </summary>
        public decimal? FundingIntlOrg2 { get; set; }

        /// <summary>
        /// Id of another International org
        /// </summary>
        public int? IntlOrg2Id { get; set; }

        /// <summary>
        /// Other Name of another International org
        /// </summary>
        public string IntlOrg2OtherName { get; set; }

        /// <summary>
        /// Funding from other source
        /// </summary>
        public decimal? FundingOther { get; set; }

        /// <summary>
        /// Name of  other org
        /// </summary>
        public string OtherName { get; set; }

        /// <summary>
        /// Total funding
        /// </summary>
        public decimal? FundingTotal { get; set; }

        /// <summary>
        /// Coverts binding model to business model
        /// </summary>
        /// <param name="user">The user updating the funding</param>
        /// <param name="projectId">The project id</param>
        /// <returns>Update default exchange visitor funding business model</returns>
        public UpdatedDefaultExchangeVisitorFunding ToUpdatedDefaultExchangeVisitorFunding(User user, int projectId)
        {
            return new UpdatedDefaultExchangeVisitorFunding(
                updater: user,
                projectId: projectId,
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