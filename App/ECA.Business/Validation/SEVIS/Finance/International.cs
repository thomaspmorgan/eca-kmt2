using ECA.Business.Sevis.Model;
using FluentValidation.Attributes;
using System;
using System.Xml.Serialization;

namespace ECA.Business.Validation.Sevis.Finance
{
    /// <summary>
    /// International organization funding
    /// </summary>
    [Validator(typeof(InternationalValidator))]
    public class International
    {
        /// <summary>
        /// International organization 1
        /// </summary>
        public string Org1 { get; set; }

        /// <summary>
        /// Other International organization 1
        /// </summary>
        public string OtherName1 { get; set; }

        /// <summary>
        /// International organization 1 funding amount
        /// </summary>
        public string Amount1 { get; set; }

        /// <summary>
        /// International organization 2
        /// </summary>
        public string Org2 { get; set; }

        /// <summary>
        /// Other International organization 2
        /// </summary>
        public string OtherName2 { get; set; }

        /// <summary>
        /// International organization 2 funding amount
        /// </summary>
        public string Amount2 { get; set; }

        /// <summary>
        /// Returns a sevis exchange visitor OtherFundsTypeInternational model instance.
        /// </summary>
        /// <returns>A sevis exchange visitor OtherFundsTypeInternational model instance.</returns>
        public OtherFundsTypeInternational GetOtherFundsTypeInternational()
        {
            Func<string, InternationalOrgCodeType> getOrgCode = (code) =>
            {
                return code.GetInternationalOrgCodeType();
            };
            var instance = new OtherFundsTypeInternational
            {
                Amount1 = this.Amount1,
                Amount2 = this.Amount2,
                OtherName1 = this.OtherName1,
                OtherName2 = this.OtherName2,
                Org2Specified = false
            };

            if (!String.IsNullOrWhiteSpace(this.Org1))
            {
                instance.Org1 = getOrgCode(this.Org1);
            }
            if (!String.IsNullOrWhiteSpace(this.Org2))
            {
                instance.Org2 = getOrgCode(this.Org2);
                instance.Org2Specified = true;
            }
            if (!String.IsNullOrWhiteSpace(this.OtherName2))
            {
                instance.Org2Specified = true;
            }
            return instance;
        }

        /// <summary>
        /// Returns a OtherFundsNullableTypeInternational instance from this USGovt funding instance.
        /// </summary>
        /// <returns>A OtherFundsNullableTypeInternational instance from this USGovt funding instance.</returns>
        public OtherFundsNullableTypeInternational GetOtherFundsNullableTypeInternational()
        {
            var instance = new OtherFundsNullableTypeInternational
            {
                Amount1 = this.Amount1,
                Amount2 = this.Amount2,
                OtherName1 = this.OtherName1,
                OtherName2 = this.OtherName2,
                Org1 = this.Org1,
                Org2 = this.Org2,
            };
            return instance;
        }
    }
}