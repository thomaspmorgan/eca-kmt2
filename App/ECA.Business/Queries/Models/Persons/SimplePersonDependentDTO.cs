using ECA.Business.Service;
using ECA.Data;
using System;
using System.Collections.Generic;

namespace ECA.Business.Queries.Models.Persons
{
    /// <summary>
    /// The SimplePersonDependentDTO is used to represent dependents in the ECA system.
    /// </summary>
    public class SimplePersonDependentDTO : IHistorical, IAuditable
    {
        public SimplePersonDependentDTO()
        {
            CountriesOfCitizenship = new List<Location>();
            History = new History();
        }

        /// <summary>
        /// Gets or sets the person id.
        /// </summary>
        public int PersonId { get; set; }

        /// <summary>
        /// Gets and sets the first name
        /// </summary>
        public FullNameDTO FullName { get; set; }

        /// <summary>
        /// Gets and sets the date of birth
        /// </summary>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Gets and sets the gender
        /// </summary>
        public int Gender { get; set; }

        /// <summary>
        /// Gets and sets the city of birth
        /// </summary>
        public int CityOfBirth { get; set; }

        /// <summary>
        /// Gets or sets the city of birth
        /// </summary>
        public int CountryOfBirth { get; set; }

        /// <summary>
        /// Gets and sets the countries of citizenship
        /// </summary>
        public List<Location> CountriesOfCitizenship { get; set; }

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

        /// <summary>
        /// Gets and sets the audit record
        /// </summary>
        public Audit Audit { get; set; }

        public History History { get; set; }
    }
}
