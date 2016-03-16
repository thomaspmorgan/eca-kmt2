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
    public class FinancialInfo
    {
        public FinancialInfo()
        {
            OtherFunds = new OtherFunds();
        }
        
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
            return new EVPersonTypeFinancialInfo
            {

            };
        }

        /// <summary>
        /// Returns an updated sevis exchange visitor's financial information.
        /// </summary>
        /// <returns>An updated sevis exchange visitor's financial information.</returns>
        public SEVISEVBatchTypeExchangeVisitorFinancialInfo GetSEVISEVBatchTypeExchangeVisitorFinancialInfo()
        {
            return new SEVISEVBatchTypeExchangeVisitorFinancialInfo
            {

            };
        }
    }
}
