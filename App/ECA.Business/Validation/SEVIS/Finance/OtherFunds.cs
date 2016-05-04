using ECA.Business.Sevis.Model;
using FluentValidation.Attributes;
using Newtonsoft.Json;
using System;

namespace ECA.Business.Validation.Sevis.Finance
{
    /// <summary>
    /// Other financial support
    /// </summary>
    [Validator(typeof(OtherFundsValidator))]
    public class OtherFunds
    {
        /// <summary>
        /// Creates a new OtherFunds instance with the given values.
        /// </summary>
        /// <param name="exchangeVisitorGovernment">The exchange visitor government funding value.</param>
        /// <param name="binationalCommission">the binational commission funding value.</param>
        /// <param name="personal">The person funding value.</param>
        /// <param name="usGovernmentFunding">The USGovernment funding value.</param>
        /// <param name="internationalFunding">The International funding value.</param>
        /// <param name="other">The other funding value.</param>
        [JsonConstructor]
        public OtherFunds(string exchangeVisitorGovernment, string binationalCommission, string personal, USGovernmentFunding usGovernmentFunding, InternationalFunding internationalFunding, Other other)
        {
            this.ExchangeVisitorGovernment = exchangeVisitorGovernment;
            this.BinationalCommission = binationalCommission;
            this.Personal = personal;
            this.USGovernmentFunding = usGovernmentFunding;
            this.InternationalFunding = internationalFunding;
            this.Other = other;
        }

        /// <summary>
        /// Gets or sets the US government organization.
        /// </summary>
        public USGovernmentFunding USGovernmentFunding { get; private set; }

        /// <summary>
        /// Gets or sets the International organization funding.
        /// </summary>
        public InternationalFunding InternationalFunding { get; private set; }

        /// <summary>
        /// Gets or sets the funding from exchange visitor government.
        /// </summary>
        public string ExchangeVisitorGovernment { get; private set; }

        /// <summary>
        /// Gets or sets the funding from binational commission.
        /// </summary>
        public string BinationalCommission { get; private set; }

        /// <summary>
        /// Gets or sets the other organization funding.
        /// </summary>
        public Other Other { get; private set; }

        /// <summary>
        /// Gets or sets the exchange visitors personal funding.
        /// </summary>
        public string Personal { get; private set; }

        /// <summary>
        /// Returns the total funding.
        /// </summary>
        /// <returns>The total funding.</returns>
        public decimal GetTotalFunding()
        {
            var total = 0.0m;
            decimal evGovernmentFunding;
            decimal binationalCommisionFunding;
            decimal personalFunding;

            if (!String.IsNullOrWhiteSpace(this.ExchangeVisitorGovernment) && decimal.TryParse(this.ExchangeVisitorGovernment, out evGovernmentFunding))
            {
                total += evGovernmentFunding;
            }
            if (!String.IsNullOrWhiteSpace(this.BinationalCommission) && decimal.TryParse(this.BinationalCommission, out binationalCommisionFunding))
            {
                total += binationalCommisionFunding;
            }
            if (!String.IsNullOrWhiteSpace(this.Personal) && decimal.TryParse(this.Personal, out personalFunding))
            {
                total += personalFunding;
            }
            if (this.USGovernmentFunding != null)
            {
                total += this.USGovernmentFunding.GetTotalFunding();
            }
            if (this.InternationalFunding != null)
            {
                total += this.InternationalFunding.GetTotalFunding();
            }
            if (this.Other != null)
            {
                total += this.Other.GetTotalFunding();
            }
            return total;
        }

        /// <summary>
        /// Returns the sevis exchange visitor OtherFundsType instance.
        /// </summary>
        /// <returns>Returns the sevis exchange visitor OtherFundsType instance.</returns>
        public OtherFundsType GetOtherFundsType()
        {
            var instance = new OtherFundsType
            {
                BinationalCommission = this.BinationalCommission,
                Personal = this.Personal,
                EVGovt = this.ExchangeVisitorGovernment,
            };
            if (this.USGovernmentFunding != null)
            {
                instance.USGovt = this.USGovernmentFunding.GetOtherFundsTypeUSGovt();
            }
            if (this.InternationalFunding != null)
            {
                instance.International = this.InternationalFunding.GetOtherFundsTypeInternational();
            }
            if (this.Other != null)
            {
                instance.Other = this.Other.GetOtherFundsTypeOther();
            }
            return instance;
        }

        /// <summary>
        /// Returns the sevis exchange visitor OtherFundsNullableType instance.
        /// </summary>
        /// <returns>Returns the sevis exchange visitor OtherFundsNullableType instance.</returns>
        public OtherFundsNullableType GetOtherFundsNullableType()
        {
            var instance = new OtherFundsNullableType
            {
                BinationalCommission = this.BinationalCommission,
                Personal = this.Personal,
                EVGovt = this.ExchangeVisitorGovernment,
            };
            if (this.USGovernmentFunding != null)
            {
                instance.USGovt = this.USGovernmentFunding.GetOtherFundsNullableTypeUSGovt();
            }
            if (this.InternationalFunding != null)
            {
                instance.International = this.InternationalFunding.GetOtherFundsNullableTypeInternational();
            }
            if (this.Other != null)
            {
                instance.Other = this.Other.GetOtherFundsNullableTypeInternational();
            }
            return instance;
        }

    }
}