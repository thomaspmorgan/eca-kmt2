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
        /// Returns the sevis exchange visitor OtherFundsType instance.
        /// </summary>
        /// <returns>Returns the sevis exchange visitor OtherFundsType instance.</returns>
        public OtherFundsType GetOtherFundsType()
        {
            var instance = new OtherFundsType
            {
                BinationalCommission = this.BinationalCommission,
                Personal = this.Personal,
                EVGovt = this.EVGovt,
            };
            if (this.USGovt != null)
            {
                instance.USGovt = this.USGovt.GetOtherFundsTypeUSGovt();
            }
            if (this.International != null)
            {
                instance.International = this.International.GetOtherFundsTypeInternational();
            }
            if(this.Other != null)
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
                EVGovt = this.EVGovt,
            };
            if (this.USGovt != null)
            {
                instance.USGovt = this.USGovt.GetOtherFundsNullableTypeUSGovt();
            }
            if (this.International != null)
            {
                instance.International = this.International.GetOtherFundsNullableTypeInternational();
            }
            if (this.Other != null)
            {
                instance.Other = this.Other.GetOtherFundsNullableTypeInternational();
            }
            return instance;
        }
        
    }
}