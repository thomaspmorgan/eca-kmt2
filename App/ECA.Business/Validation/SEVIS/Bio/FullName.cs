using ECA.Business.Sevis.Model;
using FluentValidation.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace ECA.Business.Validation.Sevis.Bio
{
    /// <summary>
    /// A FullName is used to represent a sevis exchange visitor full name.
    /// </summary>
    [Validator(typeof(FullNameValidator))]
    public class FullName
    {
        /// <summary>
        /// Creates a new full name instance with the given name parts.
        /// </summary>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="passportName">The passport name.</param>
        /// <param name="preferredName">The preferred name.</param>
        /// <param name="suffix">The suffix.</param>
        public FullName(string firstName, string lastName, string passportName, string preferredName, string suffix)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.PassportName = passportName;
            this.PreferredName = preferredName;
            this.Suffix = suffix;
        }

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
                PreferredName = this.PreferredName,
            };
            if (!String.IsNullOrWhiteSpace(this.Suffix))
            {
                instance.Suffix = this.Suffix.GetNameSuffixCodeType();
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
                LastName = this.LastName,
                PassportName = this.PassportName,
                PreferredName = this.PreferredName,
                Suffix = this.Suffix
            };
        }
    }
}
