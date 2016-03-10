using ECA.Business.Sevis.Model;
using ECA.Business.Validation.Model.Shared;
using FluentValidation.Attributes;
using System.Xml.Serialization;

namespace ECA.Business.Validation.Sevis.Finance
{
    /// <summary>
    /// Financial support information
    /// </summary>
    [Validator(typeof(FinancialInfoValidator))]
    public class FinancialInfo : IFormPrintable
    {
        /// <summary>
        /// Gets or sets print form.
        /// </summary>
        public bool PrintForm { get; set; }

        /// <summary>
        /// Indicates receipt of US govt funds
        /// </summary>
        public bool ReceivedUSGovtFunds { get; set; }

        /// <summary>
        /// Program sponsor funds
        /// </summary>
        public string ProgramSponsorFunds { get; set; }

        /// <summary>
        /// Other financial support
        /// </summary>
        public OtherFunds OtherFunds { get; set; }

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
