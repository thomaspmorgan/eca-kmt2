﻿using ECA.Business.Sevis.Model;
using FluentValidation.Attributes;
using Newtonsoft.Json;
using System;

namespace ECA.Business.Validation.Sevis.Finance
{
    /// <summary>
    /// International organization funding is used to represent an exchange visitor's funding from international organizations.
    /// </summary>
    [Validator(typeof(InternationalValidator))]
    public class International : OrganizationFunding
    {
        /// <summary>
        /// Creates a new instance with the given funding details.
        /// </summary>
        /// <param name="org1">The first organization providing funding.  This value is a code.</param>
        /// <param name="otherName1">The name of the first organization if the first organization is 'OTHER'.</param>
        /// <param name="amount1">The first organization funding amount in whole dollars.</param>
        /// <param name="org2">The second organization providing funding.  This value is a code.</param>
        /// <param name="otherName2">The name of the second organization if the second organization is 'OTHER'.</param>
        /// <param name="amount2">The second organization's funding amount in whole dollars.</param>
        [JsonConstructor]
        public International(
            string org1,
            string otherName1,
            string amount1,
            string org2,
            string otherName2,
            string amount2)
            : base(
                 org1: org1,
                 otherName1: otherName1,
                 amount1: amount1,
                 org2: org2,
                 otherName2: otherName2,
                 amount2: amount2)
        {

        }

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