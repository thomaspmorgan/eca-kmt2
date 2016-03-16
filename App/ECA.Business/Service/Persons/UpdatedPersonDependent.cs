using ECA.Business.Queries.Models.Persons;
using System;
using System.Collections.Generic;

namespace ECA.Business.Service.Persons
{
    public class UpdatedPersonDependent : IAuditable
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="updater">The user performing the update</param>
        /// <param name="personId">The person id</param>
        /// <param name="fullName">The person full name</param>
        /// <param name="dateOfBirth">The person date of birth</param>
        /// <param name="genderId">The person gender</param>
        /// <param name="cityOfBirth">The person city of birth</param>
        /// <param name="countryOfBirth">The person country of birth</param>
        /// <param name="countriesOfCitizenship">The person countries of citizenship</param>
        /// <param name="permanentResidenceCountryCode">The person permanent residence country</param>
        /// <param name="birthCountryReason">The person birth country reason</param>
        /// <param name="emailAddress">The person email address</param>
        /// <param name="personTypeId">The person type</param>
        public UpdatedPersonDependent(
            User updater,
            int personId,
            FullNameDTO fullName,
            DateTime dateOfBirth,
            int genderId,
            int cityOfBirth,
            int countryOfBirth,
            List<int> countriesOfCitizenship,
            int permanentResidenceCountryCode,
            string birthCountryReason,
            string emailAddress,
            int personTypeId)
        {
            this.Audit = new Update(updater);
            this.PersonId = personId;
            this.FullName = fullName;
            this.DateOfBirth = dateOfBirth;
            this.GenderId = genderId;
            this.CityOfBirth = cityOfBirth;
            this.CountryOfBirth = countryOfBirth;
            this.CountriesOfCitizenship = countriesOfCitizenship;
            this.PermanentResidenceCountryCode = permanentResidenceCountryCode;
            this.BirthCountryReason = birthCountryReason;
            this.EmailAddress = emailAddress;
            this.PersonTypeId = personTypeId;
        }

        /// <summary>
        /// Gets or sets the person id.
        /// </summary>
        public int PersonId { get; set; }
        
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
        public int GenderId { get; private set; }

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
        public List<int> CountriesOfCitizenship { get; private set; }

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
