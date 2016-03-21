using ECA.Business.Validation.Sevis.Bio;
using System;

namespace ECA.Business.Queries.Models.Persons
{
    /// <summary>
    /// A FullNameDTO is used to represent a person's full name when dealing with Sevis Exchange Visitors.
    /// </summary>
    public class FullNameDTO
    {
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
        /// Gets or sets the middle name.
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
        /// Returns the sevis full name instance.
        /// </summary>
        /// <returns>The sevis full name instance.</returns>
        public FullName GetFullName()
        {
            string firstName = this.FirstName;
            if (!String.IsNullOrWhiteSpace(this.FirstName) && !String.IsNullOrWhiteSpace(this.MiddleName))
            {
                firstName = this.FirstName + " " + this.MiddleName;
            }

            return new FullName(
                firstName: firstName != null
                    ? firstName.Length > FullNameValidator.FIRST_NAME_MAX_LENGTH
                        ? firstName.Substring(0, FullNameValidator.FIRST_NAME_MAX_LENGTH)
                        : firstName
                    : null,
                lastName: this.LastName != null
                    ? this.LastName.Length > FullNameValidator.LAST_NAME_MAX_LENGTH
                        ? this.LastName.Substring(0, FullNameValidator.LAST_NAME_MAX_LENGTH)
                        : this.LastName
                    : null,

                preferredName: this.PreferredName != null
                    ? this.PreferredName.Length > FullNameValidator.PREFERRED_NAME_MAX_LENGTH
                        ? this.PreferredName.Substring(0, FullNameValidator.PREFERRED_NAME_MAX_LENGTH)
                        : this.PreferredName
                    : null,
                passportName: this.PassportName != null
                    ? this.PassportName.Length > FullNameValidator.PASSPORT_NAME_MAX_LENGTH
                        ? this.PassportName.Substring(0, FullNameValidator.PASSPORT_NAME_MAX_LENGTH)
                        : this.PassportName
                    : null,
                suffix: this.Suffix);
        }
    }
}
