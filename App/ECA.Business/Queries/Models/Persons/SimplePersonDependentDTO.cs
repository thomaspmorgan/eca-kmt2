using ECA.Data;
using System;
using System.Collections.Generic;

namespace ECA.Business.Queries.Models.Persons
{
    /// <summary>
    /// The SimplePersonDependentDTO is used to represent dependents in the ECA system.
    /// </summary>
    public class SimplePersonDependentDTO
    {
        /// <summary>
        /// Gets and sets the first name
        /// </summary>
        public FullNameDTO FullName { get; private set; }

        /// <summary>
        /// Gets and sets the date of birth
        /// </summary>
        public DateTime DateOfBirth { get; private set; }

        /// <summary>
        /// Gets and sets the gender
        /// </summary>
        public int Gender { get; private set; }

        /// <summary>
        /// Gets and sets the city of birth
        /// </summary>
        public int CityOfBirth { get; private set; }

        /// <summary>
        /// Gets or sets the city of birth
        /// </summary>
        public int CountryOfBirth { get; set; }

        /// <summary>
        /// Gets and sets the countries of citizenship
        /// </summary>
        public List<Location> CountriesOfCitizenship { get; private set; }

        /// <summary>
        /// Gets or sets the premanent residence country code
        /// </summary>
        public int PermanentResidenceCountryCode { get; set; }

        /// <summary>
        /// Gets or sets the birth country reason
        /// </summary>
        public string BirthCountryReason { get; set; }

        /// <summary>
        /// Gets or sets the email address
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the dependent person type
        /// </summary>
        public int PersonTypeId { get; set; }
    }
}
