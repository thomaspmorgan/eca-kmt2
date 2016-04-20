using ECA.Business.Queries.Models.Persons;
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
        /// <param name="dependentTypeId">The dependent type id</param>
        /// <param name="firstName">The first name</param>
        /// <param name="lastName">The last name</param>
        /// <param name="nameSuffix">The name suffix</param>
        /// <param name="passportName">The passport name</param>
        /// <param name="preferredName">The preferred name</param>
        /// <param name="genderId">The gender</param>
        /// <param name="dateOfBirth">The date of birth</param>
        /// <param name="placeOfBirthId">The city of birth</param>
        /// <param name="placeOfResidenceId">The permanent residence country code</param>
        /// <param name="birthCountryReasonId">The birth country reason</param>
        /// <param name="emailAddress">The email address</param>
        /// <param name="countriesOfCitizenship">The countries of citizenship</param>
        /// <param name="isTravelWithParticipant">If dependent is travelling with a participant</param>
        public NewPersonDependent(
            User createdBy,
            int personId,
            int dependentTypeId,
            string firstName,
            string lastName,
            string nameSuffix,
            string passportName,
            string preferredName,
            int genderId,
            DateTime dateOfBirth,
            int placeOfBirthId,
            int placeOfResidenceId,
            int? birthCountryReasonId,
            string emailAddress,
            List<CitizenCountryDTO> countriesOfCitizenship,
            bool isTravelWithParticipant)
        {
            this.PersonId = personId;
            this.DependentTypeId = dependentTypeId;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.NameSuffix = nameSuffix;
            this.PassportName = passportName;
            this.PreferredName = preferredName;
            this.GenderId = genderId;
            this.DateOfBirth = dateOfBirth;
            this.PlaceOfBirthId = placeOfBirthId;
            this.PlaceOfResidenceId = placeOfResidenceId;
            this.BirthCountryReasonId = birthCountryReasonId;
            this.EmailAddress = emailAddress;
            this.CountriesOfCitizenship = countriesOfCitizenship;
            this.IsTravellingWithParticipant = isTravelWithParticipant;
            this.Audit = new Create(createdBy);
        }
        
        /// <summary>
        /// Gets or sets the person id.
        /// </summary>
        public int PersonId { get; set; }
        
        /// <summary>
        /// Gets or sets the dependent type id.
        /// </summary>
        public int DependentTypeId { get; set; }

        /// <summary>
        /// Gets or sets FirstName.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets last name.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the name suffix.
        /// </summary>
        public string NameSuffix { get; set; }

        /// <summary>
        /// Gets or sets passport name.
        /// </summary>
        public string PassportName { get; set; }

        /// <summary>
        /// Gets or sets preferred name.
        /// </summary>
        public string PreferredName { get; set; }

        /// <summary>
        /// Gets or sets the gender id.
        /// </summary>
        public int GenderId { get; set; }

        /// <summary>
        /// Gets or sets the date of birth.
        /// </summary>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets the place of birth.
        /// </summary>
        public int PlaceOfBirthId { get; set; }

        /// <summary>
        /// Gets or sets the country of residence.
        /// </summary>
        public int PlaceOfResidenceId { get; set; }

        /// <summary>
        /// Gets or sets the birth country reason.
        /// </summary>
        public int? BirthCountryReasonId { get; set; }

        /// <summary>
        /// Gets and sets the countries of citizenship
        /// </summary>
        public List<CitizenCountryDTO> CountriesOfCitizenship { get; set; }

        /// <summary>
        /// Gets and sets the email address
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets depended travelling with participant
        /// </summary>
        public bool IsTravellingWithParticipant { get; set; }
        
        /// <summary>
        /// Gets and sets the audit record
        /// </summary>
        public Audit Audit { get; set; }
    }
}
