using System;
using System.Collections.Generic;

namespace ECA.Business.Service.Persons
{
    public class NewPersonDependent : IAuditable
    {
        /// <summary>
        /// Class for new person dependent
        /// </summary>
        /// <param name="createdBy">User that created the person</param>
        /// <param name="personId">The parent person id</param>
        /// <param name="firstName">The first name</param>
        /// <param name="lastName">The last name</param>
        /// <param name="nameSuffix">The name suffix</param>
        /// <param name="passportName">The passport name</param>
        /// <param name="preferredName">The preferred name</param>
        /// <param name="gender">The gender</param>
        /// <param name="dateOfBirth">The date of birth</param>
        /// <param name="cityOfBirth">The city of birth</param>
        /// <param name="emailAddress">The email address</param>
        /// <param name="personTypeId">The person type id</param>
        /// <param name="countriesOfCitizenship">The countries of citizenship</param>
        /// <param name="permanentResidenceCountryCode">The permanent residence country code</param>
        /// <param name="birthCountryReason">The birth country reason</param>
        public NewPersonDependent(
            User createdBy,
            int personId,
            string firstName,
            string lastName,
            string nameSuffix,
            string passportName,
            string preferredName,
            int gender,
            DateTime? dateOfBirth,
            int? cityOfBirth,
            string emailAddress,
            int personTypeId,
            List<int> countriesOfCitizenship,
            int permanentResidenceCountryCode,
            string birthCountryReason)
        {
            this.PersonId = personId;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.NameSuffix = nameSuffix;
            this.PassportName = passportName;
            this.PreferredName = preferredName;
            this.Gender = gender;
            this.DateOfBirth = dateOfBirth;
            this.CityOfBirth = cityOfBirth;
            this.EmailAddress = emailAddress;
            this.PersonTypeId = personTypeId;
            this.CountriesOfCitizenship = countriesOfCitizenship;
            this.PermanentResidenceCountryCode = permanentResidenceCountryCode;
            this.BirthCountryReason = birthCountryReason;
            this.Audit = new Create(createdBy);
        }

        /// <summary>
        /// Gets or sets the person id.
        /// </summary>
        public int PersonId { get; set; }

        /// <summary>
        /// Gets the person type id.
        /// </summary>
        public int PersonTypeId { get; private set; }
        
        /// <summary>
        /// Gets and sets the first name
        /// </summary>
        public string FirstName { get; private set; }

        /// <summary>
        /// Gets and sets the last name
        /// </summary>
        public string LastName { get; private set; }

        /// <summary>
        /// Gets or sets the name suffix.
        /// </summary>
        public string NameSuffix { get; set; }

        /// <summary>
        /// Person passport name.
        /// </summary>
        public string PassportName { get; set; }

        /// <summary>
        /// Person preferred name.
        /// </summary>
        public string PreferredName { get; set; }

        /// <summary>
        /// Gets and sets the gender
        /// </summary>
        public int Gender { get; private set; }

        /// <summary>
        /// Gets and sets the date of birth
        /// </summary>
        public DateTime? DateOfBirth { get; private set; }
        
        /// <summary>
        /// Gets and sets the city of birth
        /// </summary>
        public int? CityOfBirth { get; private set; }

        /// <summary>
        /// Gets or sets the email address
        /// </summary>
        public string EmailAddress { get; set; }

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
        /// Gets and sets the audit record
        /// </summary>
        public Audit Audit { get; private set; }
    }
}
