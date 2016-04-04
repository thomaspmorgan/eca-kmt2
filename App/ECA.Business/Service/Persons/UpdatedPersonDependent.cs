using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace ECA.Business.Service.Persons
{
    public class UpdatedPersonDependent : IAuditable
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="createdBy">User that created the person</param>
        /// <param name="dependentId">The person dependent id</param>
        /// <param name="personId">The parent person id</param>
        /// <param name="dependentTypeId">The dependent type id</param>
        /// <param name="sevisId">The person sevis id</param>
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
        /// <param name="countriesOfCitizenship">The countries of citizenship</param>
        public UpdatedPersonDependent(
            User updater,
            int dependentId,
            int personId,
            int dependentTypeId,
            string sevisId,
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
            List<int> countriesOfCitizenship,
            bool isTravelWithParticipant,
            bool isDeleted,
            bool isSevisDeleted)
        {
            Contract.Requires(updater != null, "The created by user must not be null.");
            this.DependentId = dependentId;
            this.PersonId = personId;
            this.DependentTypeId = dependentTypeId;
            this.SevisId = sevisId;
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
            this.CountriesOfCitizenship = countriesOfCitizenship;
            this.IsTravellingWithParticipant = isTravelWithParticipant;
            this.IsDeleted = isDeleted;
            this.IsSevisDeleted = isSevisDeleted;
            this.Audit = new Create(updater);
        }

        /// <summary>
        /// Gets or sets the dependent id.
        /// </summary>
        public int DependentId { get; set; }

        /// <summary>
        /// Gets or sets the person id.
        /// </summary>
        public int PersonId { get; set; }

        /// <summary>
        /// the SEVIS ID (assigned by SEVIS)
        /// </summary>
        public string SevisId { get; set; }

        /// <summary>
        /// Gets or sets the person type id.
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
        public List<int> CountriesOfCitizenship { get; set; }

        /// <summary>
        /// Gets or sets depended travelling with participant
        /// </summary>
        public bool IsTravellingWithParticipant { get; set; }

        /// <summary>
        /// Gets or sets depended was delete in ECA
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Gets or sets depended was delete in SEVIS
        /// </summary>
        public bool IsSevisDeleted { get; set; }

        /// <summary>
        /// Gets and sets the audit record
        /// </summary>
        public Audit Audit { get; set; }
    }
}
