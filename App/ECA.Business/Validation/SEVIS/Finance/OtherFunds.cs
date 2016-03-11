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
        public OtherFunds(string evGovt, string binationalCommission, string personal, USGovt usGovt, International international, Other other)
        {
            this.EVGovt = evGovt;
            this.BinationalCommission = binationalCommission;
            this.Personal = personal;
            this.USGovt = usGovt;
            this.International = international;
            this.Other = other;
        }

        /// <summary>
        /// US government organization
        /// </summary>
        public USGovt USGovt { get; private set; }

        /// <summary>
        /// International organization funding
        /// </summary>
        public International International { get; private set; }

        /// <summary>
        /// Funding from exchange visitor government
        /// </summary>
        public string EVGovt { get; private set; }

        /// <summary>
        /// Funding from binational commission
        /// </summary>
        public string BinationalCommission { get; private set; }

        /// <summary>
        /// Other organization funding
        /// </summary>
        public Other Other { get; private set; }

        /// <summary>
        /// Personal funds
        /// </summary>
        public string Personal { get; private set; }

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