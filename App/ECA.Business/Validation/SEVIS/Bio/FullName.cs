using ECA.Business.Sevis.Model;
using ECA.Business.Validation.Model.Shared;
using FluentValidation.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace ECA.Business.Validation.Sevis.Bio
{
    [Validator(typeof(FullNameValidator))]
    public class FullName
    {
        public FullName()
        { }

        /// <summary>
        /// Person last name.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Person first name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Person passport name.
        /// </summary>
        public string PassportName { get; set; }

        /// <summary>
        /// Person preferred name.
        /// </summary>
        public string PreferredName { get; set; }

        /// <summary>
        /// Person name suffix.
        /// </summary>
        public string Suffix { get; set; }

        /// <summary>
        /// Returns a sevis name type instance.
        /// </summary>
        /// <returns>A sevis name type instance.</returns>
        public NameType GetNameType()
        {
            var instance = new NameType
            {
                FirstName = this.FirstName,
                LastName = this.LastName,
                PassportName = this.PassportName,
                PreferredName = this.PreferredName,};
            if (!String.IsNullOrWhiteSpace(this.Suffix))
            {
                var suffix = this.Suffix.Replace(".", String.Empty);
                instance.Suffix = (NameSuffixCodeType)Enum.Parse(typeof(NameSuffixCodeType), suffix);
                instance.SuffixSpecified = true;
            }
            else
            {
                instance.SuffixSpecified = false;
            }
            return instance;
        }

        /// <summary>
        /// Returns a sevis NameNullableType instance.
        /// </summary>
        /// <returns>Returns a sevis NameNullableType instance.</returns>
        public NameNullableType GetNameNullableType()
        {
            return new NameNullableType
            {
                FirstName = this.FirstName,
                LastName= this.LastName,
                PassportName = this.PassportName,
                PreferredName = this.PreferredName,
                Suffix = this.Suffix
            };
        }
    }
}
