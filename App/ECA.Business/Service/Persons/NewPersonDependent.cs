using ECA.Business.Queries.Models.Persons;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace ECA.Business.Service.Persons
{
    public class NewPersonDependent : IAuditable
    {
        /// <summary>
        /// Class for new person dependent
        /// </summary>
        /// <param name="createdBy">User that created the person</param>
        /// <param name="fullName">The full name object</param>
        /// <param name="dateOfBirth">The date of birth</param>
        /// <param name="gender">The gender</param>
        /// <param name="cityOfBirth">The city of birth</param>
        /// <param name="countryOfBirth">The country of birth</param>
        /// <param name="countriesOfCitizenship">The countries of citizenship</param>
        /// <param name="permanentResidenceCountryCode">The permanent residence country</param>
        /// <param name="birthCountryReason">The birth country reason</param>
        /// <param name="emailAddress">The email address</param>
        /// <param name="personTypeId">The relationship type</param>
        public NewPersonDependent(
            User createdBy,
            FullNameDTO fullName,
            DateTime dateOfBirth,
            int gender,
            int cityOfBirth,
            int countryOfBirth,
            List<Location> countriesOfCitizenship,
            int permanentResidenceCountryCode,
            string birthCountryReason,
            string emailAddress,
            int personTypeId)
        {
            Contract.Requires(createdBy != null, "The created by user must not be null.");
            Contract.Requires(fullName != null, "The full name must not be null.");
            Contract.Requires(dateOfBirth != null, "The date of birth must not be null.");
            Contract.Requires(gender > 0, "The gender must not be null.");
            Contract.Requires(cityOfBirth > 0, "The city of birth must not be null.");
            Contract.Requires(countryOfBirth > 0, "The country of birth must not be null.");
            Contract.Requires(countriesOfCitizenship != null, "The countries of citizenship must not be null.");
            Contract.Requires(permanentResidenceCountryCode > 0, "The permanent residence country must not be null.");
            Contract.Requires(personTypeId > 0, "The person type must not be null.");
            this.FullName = fullName;
            this.DateOfBirth = dateOfBirth;
            this.Gender = gender;
            this.CityOfBirth = cityOfBirth;
            this.CountryOfBirth = countryOfBirth;
            this.CountriesOfCitizenship = countriesOfCitizenship;
            this.PermanentResidenceCountryCode = permanentResidenceCountryCode;
            this.BirthCountryReason = birthCountryReason;
            this.EmailAddress = emailAddress;
            this.PersonTypeId = personTypeId;
            this.Audit = new Create(createdBy);
        }
        
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

        /// <summary>
        /// Gets and sets the audit record
        /// </summary>
        public Audit Audit { get; set; }
    }
}
