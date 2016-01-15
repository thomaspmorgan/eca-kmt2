using FluentValidation.Attributes;
using System.Xml.Serialization;

namespace ECA.Business.Validation.Model.Shared
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
        [XmlElement(IsNullable = true)]
        public string EVGovt { get; set; }

        /// <summary>
        /// Funding from binational commission
        /// </summary>
        [XmlElement(IsNullable = true)]
        public string BinationalCommission { get; set; }

        /// <summary>
        /// Other organization funding
        /// </summary>
        public Other Other { get; set; }

        /// <summary>
        /// Personal funds
        /// </summary>
        [XmlElement(IsNullable = true)]
        public string Personal { get; set; }
    }
}