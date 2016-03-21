using ECA.Business.Sevis.Model;
using ECA.Business.Validation.Sevis.Exceptions;
using System;
using System.Diagnostics.Contracts;

namespace ECA.Business.Validation
{
    public static class SevisEnumHelper
    {
        private static T GetCodeType<T>(string value)
        {
            if (!Enum.IsDefined(typeof(T), value))
            {
                throw new CodeTypeConversionException(value, typeof(T));
            }
            return (T)Enum.Parse(typeof(T), value);
        }

        /// <summary>
        /// Returns the BirthCntryCodeType for the given string value.
        /// </summary>
        /// <returns>The BirthCntryCodeType for the given string value.</returns>
        public static BirthCntryCodeType GetBirthCntryCodeType(this string value)
        {
            Contract.Requires(value != null, "The string value must not be null.");
            return GetCodeType<BirthCntryCodeType>(value);
        }

        /// <summary>
        /// Returns the EVGenderCodeType for the given string value.
        /// </summary>
        /// <returns>The EVGenderCodeType for the given string value.</returns>
        public static EVGenderCodeType GetEVGenderCodeType(this string value)
        {
            Contract.Requires(value != null, "The string value must not be null.");
            return GetCodeType<EVGenderCodeType>(value);
        }

        /// <summary>
        /// Returns the GenderCodeType for the given string value.
        /// </summary>
        /// <returns>The GenderCodeType for the given string value.</returns>
        public static GenderCodeType GetGenderCodeType(this string value)
        {
            Contract.Requires(value != null, "The string value must not be null.");
            return GetCodeType<GenderCodeType>(value);
        }

        /// <summary>
        /// Returns the EVCategoryCodeType for the given string value.
        /// </summary>
        /// <returns>The EVCategoryCodeType for the given string value.</returns>
        public static EVCategoryCodeType GetEVCategoryCodeType(this string value)
        {
            Contract.Requires(value != null, "The string value must not be null.");
            if (!Enum.IsDefined(typeof(EVCategoryCodeType), value))
            {
                value = "Item" + value;
            }

            return GetCodeType<EVCategoryCodeType>(value);
        }

        /// <summary>
        /// Returns the CntryCodeWithoutType for the given string value.
        /// </summary>
        /// <returns>The CntryCodeWithoutType for the given string value.</returns>
        public static CntryCodeWithoutType GetCountryCodeWithType(this string value)
        {
            Contract.Requires(value != null, "The string value must not be null.");
            return GetCodeType<CntryCodeWithoutType>(value);
        }

        /// <summary>
        /// Returns the StateCodeType for the given string value.
        /// </summary>
        /// <returns>The StateCodeType for the given string value.</returns>
        public static StateCodeType GetStateCodeType(this string value)
        {
            Contract.Requires(value != null, "The string value must not be null.");
            return GetCodeType<StateCodeType>(value);
        }


        /// <summary>
        /// Returns the ProgSubjectCodeType for the given string value.
        /// </summary>
        /// <returns>The ProgSubjectCodeType for the given string value.</returns>
        public static ProgSubjectCodeType GetProgSubjectCodeType(this string value)
        {
            Contract.Requires(value != null, "The string value must not be null.");
            value = value.Replace(".", String.Empty);
            if (!Enum.IsDefined(typeof(ProgSubjectCodeType), value))
            {
                value = "Item" + value;
            }
            return GetCodeType<ProgSubjectCodeType>(value);
        }

        /// <summary>
        /// Returns the EVOccupationCategoryCodeType for the given string value.
        /// </summary>
        /// <returns>The EVOccupationCategoryCodeType for the given string value.</returns>
        public static EVOccupationCategoryCodeType GetEVOccupationCategoryCodeType(this string value)
        {
            Contract.Requires(value != null, "The string value must not be null.");
            if (!Enum.IsDefined(typeof(EVOccupationCategoryCodeType), value))
            {
                value = "Item" + value;
            }
            return GetCodeType<EVOccupationCategoryCodeType>(value);
        }

        /// <summary>
        /// Returns the GovAgencyCodeType for the given string value.
        /// </summary>
        /// <returns>The GovAgencyCodeType for the given string value.</returns>
        public static GovAgencyCodeType GetGovAgencyCodeType(this string value)
        {
            Contract.Requires(value != null, "The string value must not be null.");
            return GetCodeType<GovAgencyCodeType>(value);
        }

        /// <summary>
        /// Returns the InternationalOrgCodeType for the given string value.
        /// </summary>
        /// <returns>The InternationalOrgCodeType for the given string value.</returns>
        public static InternationalOrgCodeType GetInternationalOrgCodeType(this string value)
        {
            Contract.Requires(value != null, "The string value must not be null.");
            return GetCodeType<InternationalOrgCodeType>(value);
        }

        /// <summary>
        /// Returns the NameSuffixCodeType for the given string value.
        /// </summary>
        /// <returns>The NameSuffixCodeType for the given string value.</returns>
        public static NameSuffixCodeType GetNameSuffixCodeType(this string value)
        {
            Contract.Requires(value != null, "The string value must not be null.");
            return GetCodeType<NameSuffixCodeType>(value.Replace(".", String.Empty));
        }

        /// <summary>
        /// Returns the DependentCodeType for the given string value.
        /// </summary>
        /// <returns>The DependentCodeType for the given string value.</returns>
        public static DependentCodeType GetDependentCodeType(this string value)
        {
            Contract.Requires(value != null, "The string value must not be null.");
            if(!Enum.IsDefined(typeof(DependentCodeType), value))
            {
                value = "Item" + value;
            }
            return GetCodeType<DependentCodeType>(value);
        }


        /// <summary>
        /// Returns the USAddrDoctorTypeExplanationCode for the given string value.
        /// </summary>
        /// <returns>The USAddrDoctorTypeExplanationCode for the given string value.</returns>
        public static USAddrDoctorTypeExplanationCode GetUSAddrDoctorTypeExplanationCode(this string value)
        {
            Contract.Requires(value != null, "The string value must not be null.");
            return GetCodeType<USAddrDoctorTypeExplanationCode>(value);
        }

    }
}
