using ECA.Business.Validation.Model.Shared;
using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model
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
    }
}
