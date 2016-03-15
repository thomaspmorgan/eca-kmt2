using ECA.Business.Validation.Sevis.Finance;
using System;

namespace ECA.Business.Queries.Models.Persons.ExchangeVisitor
{
    public class ExchangeVisitorFundingDTO
    {
        /// <summary>
        /// Gets or sets organization 1.
        /// </summary>
        public string Org1 { get; set; }

        /// <summary>
        /// Gets or sets the other name for org 1.
        /// </summary>
        public string OtherName1 { get; set; }

        /// <summary>
        /// Gets or sets organization 1 funding amount.
        /// </summary>
        public string Amount1 { get; set; }

        /// <summary>
        /// Gets or sets organization 2.
        /// </summary>
        public string Org2 { get; set; }

        /// <summary>
        /// Gets or sets the other name for org 2.
        /// </summary>
        public string OtherName2 { get; set; }

        /// <summary>
        /// Gets or sets organization 2 funding amount.
        /// </summary>
        public string Amount2 { get; set; }

        /// <summary>
        /// Returns true if this dto no funding levels or agencies set.
        /// </summary>
        /// <returns>True, if this dto has no funding and agencies set.</returns>
        public bool IsEmpty()
        {
            return String.IsNullOrWhiteSpace(this.Org1)
                && String.IsNullOrWhiteSpace(this.Org2)
                && (String.IsNullOrWhiteSpace(this.Amount1) || this.Amount1 == "0")
                && (String.IsNullOrWhiteSpace(this.Amount2) || this.Amount2 == "0")
                && String.IsNullOrWhiteSpace(this.OtherName1)
                && String.IsNullOrWhiteSpace(this.OtherName2);
        }

        /// <summary>
        /// Returns the international funding sevis model.
        /// </summary>
        /// <returns>The international funding sevis model.</returns>
        public International GetInternational()
        {
            return new International(
                org1: this.Org1,
                otherName1: this.OtherName1,
                amount1: this.Amount1,
                org2: this.Org2,
                otherName2: this.OtherName2,
                amount2: this.Amount2);
        }

        /// <summary>
        /// Returns the us government funding sevis model.
        /// </summary>
        /// <returns>The US government funding sevis model.</returns>
        public USGovt GetUSGovt()
        {
            return new USGovt(
                org1: this.Org1,
                otherName1: this.OtherName1,
                amount1: this.Amount1,
                org2: this.Org2,
                otherName2: this.OtherName2,
                amount2: this.Amount2);
        }
    }
}
