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
        /// Returns the CntryCodeWithoutType for the given string value.
        /// </summary>
        /// <returns>The CntryCodeWithoutType for the given string value.</returns>
        public static CntryCodeWithoutType GetCountryCodeWithType(this string value)
        {
            Contract.Requires(value != null, "The string value must not be null.");
            return GetCodeType<CntryCodeWithoutType>(value);
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
    }
}
