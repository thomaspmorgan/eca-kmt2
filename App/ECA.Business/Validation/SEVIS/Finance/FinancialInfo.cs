using ECA.Business.Sevis.Model;
using FluentValidation.Attributes;
using Newtonsoft.Json;

namespace ECA.Business.Validation.Sevis.Finance
{
    /// <summary>
    /// Financial support information
    /// </summary>
    [Validator(typeof(FinancialInfoValidator))]
    public class FinancialInfo : IFormPrintable
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
    }
}
