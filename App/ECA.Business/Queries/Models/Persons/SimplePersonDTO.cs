using System;

namespace ECA.Business.Queries.Models.Persons
{
    /// <summary>
    /// The SimplePersonDTO is used to represent people in the ECA system.
    /// </summary>
    public class SimplePersonDTO
    {
        /// <summary>
        /// Gets or sets the FullName.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets the person id.
        /// </summary>
        public int PersonId { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the name prefix.
        /// </summary>
        public string NamePrefix { get; set; }

        /// <summary>
        /// Gets or sets the name suffix.
        /// </summary>
        public string NameSuffix { get; set; }

        /// <summary>
        /// Gets or sets the given name.
        /// </summary>
        public string GivenName { get; set; }

        /// <summary>
        /// Gets or sets the family name.
        /// </summary>
        public string FamilyName { get; set; }

        /// <summary>
        /// Gets or sets the middle name.
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
        /// Gets or sets the patronym.
        /// </summary>
        public string Patronym { get; set; }

        /// <summary>
        /// Gets or sets the alias.
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// Gets or sets the date of birth.
        /// </summary>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets the date of birth unknown flag.
        /// </summary>
        public bool? IsDateOfBirthUnknown { get; set; }

        /// <summary>
        /// Gets or sets the gender.
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// Gets or sets the gender id
        /// </summary>
        public int GenderId { get; set; }

        /// <summary>
        /// Gets or sets the current status
        /// </summary>
        public string CurrentStatus { get; set; }

        /// <summary>
        /// Gets or sets the country of birth
        /// </summary>
        public string CountryOfBirth { get; set; }

        /// <summary>
        /// Gets or sets the place of birth unknown flag.
        /// </summary>
        public bool? IsPlaceOfBirthUnknown { get; set; }

        /// <summary>
        /// Gets or sets the city of birth
        /// </summary>
        public string CityOfBirth { get; set; }

        /// <summary>
        /// Gets or sets the division of birth
        /// </summary>
        public string DivisionOfBirth { get; set; }

        /// <summary>
        /// Gets or sets the city of birth id
        /// </summary>
        public int? CityOfBirthId { get; set; }

    }
}
