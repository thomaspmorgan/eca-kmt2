using ECA.Business.Sevis.Model;
using FluentValidation.Attributes;
using System;

namespace ECA.Business.Validation.Sevis.Finance
{
    /// <summary>
    /// US government organization funding
    /// </summary>
    [Validator(typeof(USGovtValidator))]
    public class USGovt
    {
        public USGovt(
            string org1,
            string otherName1,
            string amount1,
            string org2,
            string otherName2,
            string amount2)
        {
            this.Org1 = org1;
            this.OtherName1 = otherName1;
            this.Amount1 = amount1;
            this.Org2 = org2;
            this.OtherName2 = otherName2;
            this.Amount2 = amount2;
        }
        /// <summary>
        /// US government organization 1
        /// </summary>
        public string Org1 { get; private set; }

        /// <summary>
        /// Other US government organization 1
        /// </summary>
        public string OtherName1 { get; private set; }

        /// <summary>
        /// US government organization 1 funding amount
        /// </summary>
        public string Amount1 { get; private set; }

        /// <summary>
        /// US government organization 2
        /// </summary>
        public string Org2 { get; private set; }

        /// <summary>
        /// Other US government organization 2
        /// </summary>
        public string OtherName2 { get; private set; }

        /// <summary>
        /// US government organization 2 funding amount
        /// </summary>
        public string Amount2 { get; private set; }

        /// <summary>
        /// Returns a new sevis exchange visitor us government funding.
        /// </summary>
        /// <returns></returns>
        public OtherFundsTypeUSGovt GetOtherFundsTypeUSGovt()
        {
            Func<string, GovAgencyCodeType> getGovAgencyCode = (code) =>
            {
                return code.GetGovAgencyCodeType();
            };
            var instance = new OtherFundsTypeUSGovt
            {
                Amount1 = this.Amount1,
                Amount2 = this.Amount2,
                OtherName1 = this.OtherName1,
                OtherName2 = this.OtherName2,
                Org2Specified = false
            };

            if (!String.IsNullOrWhiteSpace(this.Org1))
            {
                instance.Org1 = getGovAgencyCode(this.Org1);
            }
            if (!String.IsNullOrWhiteSpace(this.Org2))
            {
                instance.Org2 = getGovAgencyCode(this.Org2);
                instance.Org2Specified = true;
            }
            if (!String.IsNullOrWhiteSpace(this.OtherName2))
            {
                instance.Org2Specified = true;
            }
            return instance;
        }

        /// <summary>
        /// Returns a OtherFundsNullableTypeUSGovt instance from this USGovt funding instance.
        /// </summary>
        /// <returns>A OtherFundsNullableTypeUSGovt instance from this USGovt funding instance.</returns>
        public OtherFundsNullableTypeUSGovt GetOtherFundsNullableTypeUSGovt()
        {
            var instance = new OtherFundsNullableTypeUSGovt
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