using System;
using ECA.Business.Sevis.Model;
using FluentValidation.Attributes;
using Newtonsoft.Json;
using KellermanSoftware.CompareNetObjects;
using System.Collections.Generic;

namespace ECA.Business.Validation.Sevis.Finance
{
    /// <summary>
    /// Financial support information
    /// </summary>
    [Validator(typeof(FinancialInfoValidator))]
    public class FinancialInfo : IFormPrintable, IChangeComparable<FinancialInfo, FinancialInfoChangeDetail>
    {
        [JsonConstructor]
        public FinancialInfo(
            bool printForm,
            bool receivedUSGovtFunds,
            string programSponsorFunds,
            OtherFunds otherFunds)
        {
            this.PrintForm = printForm;
            this.ReceivedUSGovtFunds = receivedUSGovtFunds;
            this.ProgramSponsorFunds = programSponsorFunds;
            this.OtherFunds = otherFunds;
        }

        /// <summary>
        /// Gets or sets print form.
        /// </summary>
        public bool PrintForm { get; private set; }

        /// <summary>
        /// Indicates receipt of US govt funds
        /// </summary>
        public bool ReceivedUSGovtFunds { get; private set; }

        /// <summary>
        /// Program sponsor funds
        /// </summary>
        public string ProgramSponsorFunds { get; private set; }

        /// <summary>
        /// Other financial support
        /// </summary>
        public OtherFunds OtherFunds { get; private set; }

        /// <summary>
        /// Returns the total funding.
        /// </summary>
        /// <returns>The total funding.</returns>
        public decimal GetTotalFunding()
        {
            decimal total = 0.0m;

            decimal programSponsorFunds;
            if (!string.IsNullOrWhiteSpace(ProgramSponsorFunds) && decimal.TryParse(this.ProgramSponsorFunds, out programSponsorFunds))
            {
                total += programSponsorFunds;
            }
            if (this.OtherFunds != null)
            {
                total += this.OtherFunds.GetTotalFunding();
            }
            return total;
        }

        /// <summary>
        /// Returns the new sevis exchange visitor financial info.
        /// </summary>
        /// <returns>The new exchange visitor financial info.</returns>
        public EVPersonTypeFinancialInfo GetEVPersonTypeFinancialInfo()
        {
            var instance = new EVPersonTypeFinancialInfo
            {
                ProgramSponsorFunds = this.ProgramSponsorFunds,
                ReceivedUSGovtFunds = this.ReceivedUSGovtFunds
            };
            if (this.OtherFunds != null)
            {
                instance.OtherFunds = this.OtherFunds.GetOtherFundsType();
            }
            return instance;
        }

        /// <summary>
        /// Returns an updated sevis exchange visitor's financial information.
        /// </summary>
        /// <returns>An updated sevis exchange visitor's financial information.</returns>
        public SEVISEVBatchTypeExchangeVisitorFinancialInfo GetSEVISEVBatchTypeExchangeVisitorFinancialInfo()
        {
            var instance = new SEVISEVBatchTypeExchangeVisitorFinancialInfo
            {
                printForm = this.PrintForm,
                ProgramSponsorFunds = this.ProgramSponsorFunds,
                ReceivedUSGovtFunds = this.ReceivedUSGovtFunds,
                ReceivedUSGovtFundsSpecified = true
            };
            if (this.OtherFunds != null)
            {
                instance.OtherFunds = this.OtherFunds.GetOtherFundsNullableType();
            }
            return instance;
        }

        /// <summary>
        /// Returns a change detail object for changes from this financial info and the given financial info.
        /// </summary>
        /// <param name="otherChangeComparable">The financial info object to compare.</param>
        /// <returns>The change detail.</returns>
        public FinancialInfoChangeDetail GetChangeDetail(FinancialInfo otherChangeComparable)
        {
            var compareConfig = new ComparisonConfig
            {
                CompareChildren = true,
                MembersToIgnore = new List<string> { nameof(this.PrintForm) }
            };
            var compareLogic = new CompareLogic(compareConfig);
            var result = compareLogic.Compare(this, otherChangeComparable);
            return new FinancialInfoChangeDetail(result);
        }
    }
}
