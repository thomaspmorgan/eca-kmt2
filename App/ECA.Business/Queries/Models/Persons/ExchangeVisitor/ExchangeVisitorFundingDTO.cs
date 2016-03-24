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
        public decimal? Amount1 { get; set; }

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
        public decimal? Amount2 { get; set; }

        /// <summary>
        /// Returns true if this dto no funding levels or agencies set.
        /// </summary>
        /// <returns>True, if this dto has no funding and agencies set.</returns>
        public bool IsEmpty()
        {
            return String.IsNullOrWhiteSpace(this.Org1)
                && String.IsNullOrWhiteSpace(this.Org2)
                && (!this.Amount1.HasValue || this.Amount1 == 0)
                && (!this.Amount2.HasValue || this.Amount2 == 0)
                && String.IsNullOrWhiteSpace(this.OtherName1)
                && String.IsNullOrWhiteSpace(this.OtherName2);
        }

        /// <summary>
        /// Returns the given decimal amount as a string representing whole dollars.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <returns>The amount in whole dollars.</returns>
        public string GetAmountAsWholeDollarString(decimal? amount)
        {
            if (amount.HasValue)
            {
                return ((int)amount.Value).ToString();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Returns the international funding sevis model.
        /// </summary>
        /// <returns>The international funding sevis model.</returns>
        public InternationalFunding GetInternational()
        {
            return new InternationalFunding(
                org1: this.Org1,
                otherName1: this.OtherName1,
                amount1: GetAmountAsWholeDollarString(this.Amount1),
                org2: this.Org2,
                otherName2: this.OtherName2,
                amount2: GetAmountAsWholeDollarString(this.Amount2));
        }

        /// <summary>
        /// Returns the us government funding sevis model.
        /// </summary>
        /// <returns>The US government funding sevis model.</returns>
        public USGovernmentFunding GetUSGovt()
        {
            return new USGovernmentFunding(
                org1: this.Org1,
                otherName1: this.OtherName1,
                amount1: GetAmountAsWholeDollarString(this.Amount1),
                org2: this.Org2,
                otherName2: this.OtherName2,
                amount2: GetAmountAsWholeDollarString(this.Amount2));
        }
    }
}
