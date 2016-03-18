using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Projects
{
    /// <summary>
    /// Business model for default exchange visitor funding
    /// </summary>
    public class UpdatedDefaultExchangeVisitorFunding : IAuditable
    {
        public UpdatedDefaultExchangeVisitorFunding(
            User updater,
            int projectId,
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
            this.ProjectId = projectId;
            this.FundingSponsor = fundingSponsor;
            this.FundingPersonal = fundingPersonal;
            this.FundingVisGovt = fundingVisGovt;
            this.FundingVisBNC = fundingVisBNC;
            this.FundingGovtAgency1 = fundingGovtAgency1;
            this.GovtAgency1Id = govtAgency1Id != 0 ? govtAgency1Id : null;
            this.GovtAgency1OtherName = govtAgency1OtherName;
            this.FundingGovtAgency2 = fundingGovtAgency2;
            this.GovtAgency2Id = govtAgency2Id != 0 ? govtAgency2Id : null;
            this.GovtAgency2OtherName = govtAgency2OtherName;
            this.FundingIntlOrg1 = fundingIntlOrg1;
            this.IntlOrg1Id = intlOrg1Id != 0 ? intlOrg1Id : null;
            this.IntlOrg1OtherName = intlOrg1OtherName;
            this.FundingIntlOrg2 = fundingIntlOrg2;
            this.IntlOrg2Id = intlOrg2Id != 0 ? intlOrg2Id : null;
            this.IntlOrg2OtherName = intlOrg2OtherName;
            this.FundingOther = fundingOther;
            this.OtherName = otherName;
            this.FundingTotal = fundingTotal;
        }

        /// <summary>
        /// Gets or sets the project id
        /// </summary>
        public int ProjectId { get; private set; }

        /// <summary>
        /// Funding coming from the sponsor
        /// </summary>
        public decimal? FundingSponsor { get; private set; }

        /// <summary>
        /// Funding coming from the visitor
        /// </summary>
        public decimal? FundingPersonal { get; private set; }

        /// <summary>
        /// Funding from the visiting participant's government
        /// </summary>
        public decimal? FundingVisGovt { get; private set; }

        /// <summary>
        /// Funding from the visiting participant's BNC
        /// </summary>
        public decimal? FundingVisBNC { get; private set; }

        /// <summary>
        /// Funding from another U.S. government agency
        /// </summary>
        public decimal? FundingGovtAgency1 { get; private set; }

        /// <summary>
        /// Id of another U.S. government agency
        /// </summary>
        public int? GovtAgency1Id { get; private set; }

        /// <summary>
        /// Other Name of another U.S. government agency
        /// </summary>
        public string GovtAgency1OtherName { get; private set; }

        /// <summary>
        /// Funding from another U.S. government agency
        /// </summary>
        public decimal? FundingGovtAgency2 { get; private set; }

        /// <summary>
        /// Id of  another U.S. government agency
        /// </summary>
        public int? GovtAgency2Id { get; private set; }

        /// <summary>
        /// Other Name of another U.S. government agency
        /// </summary>
        public string GovtAgency2OtherName { get; private set; }

        /// <summary>
        /// Funding from another international org
        /// </summary>
        public decimal? FundingIntlOrg1 { get; private set; }

        /// <summary>
        /// id of another International org
        /// </summary>
        public int? IntlOrg1Id { get; private set; }

        /// <summary>
        /// Other Name of another International org
        /// </summary>
        public string IntlOrg1OtherName { get; private set; }

        /// <summary>
        /// Funding from another international org
        /// </summary>
        public decimal? FundingIntlOrg2 { get; private set; }

        /// <summary>
        /// Id of another International org
        /// </summary>
        public int? IntlOrg2Id { get; private set; }

        /// <summary>
        /// Other Name of another International org
        /// </summary>
        public string IntlOrg2OtherName { get; private set; }

        /// <summary>
        /// Funding from other source
        /// </summary>
        public decimal? FundingOther { get; private set; }

        /// <summary>
        /// Name of  other org
        /// </summary>
        public string OtherName { get; private set; }

        /// <summary>
        /// Total funding
        /// </summary>
        public decimal? FundingTotal { get; private set; }

        /// <summary>
        /// Gets and sets the update audit
        /// </summary>
        public Audit Audit { get; private set; }
    }
}
