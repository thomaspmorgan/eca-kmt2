using ECA.Business.Sevis.Model;
using ECA.Business.Validation.Model.Shared;
using FluentValidation.Attributes;
using System.Xml.Serialization;

namespace ECA.Business.Validation.Sevis.Finance
{
    /// <summary>
    /// Other financial support
    /// </summary>
    [Validator(typeof(OtherFundsValidator))]
    public class OtherFunds
    {
        public OtherFunds()
        {
            USGovt = new USGovt();
            International = new International();
            Other = new Other();
        }

        /// <summary>
        /// US government organization
        /// </summary>
        public USGovt USGovt { get; set; }

        /// <summary>
        /// International organization funding
        /// </summary>
        public International International { get; set; }

        /// <summary>
        /// Funding from exchange visitor government
        /// </summary>
        public string EVGovt { get; set; }

        /// <summary>
        /// Funding from binational commission
        /// </summary>
        public string BinationalCommission { get; set; }

        /// <summary>
        /// Other organization funding
        /// </summary>
        public Other Other { get; set; }

        /// <summary>
        /// Personal funds
        /// </summary>
        public string Personal { get; set; }

        /// <summary>
        /// Returns a new sevis exchange visitor's other funds financial info.
        /// </summary>
        /// <returns>A new sevis exchange visitor's other fund financial info.</returns>
        public OtherFundsTypeUSGovt GetOtherFundsTypeUSGovt()
        {
            return new OtherFundsTypeUSGovt
            {

            };
        }
    }
}