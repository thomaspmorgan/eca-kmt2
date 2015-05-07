using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Persons
{
    /// <summary>
    /// Business model to update pii
    /// </summary>
    public class UpdatePii : IAuditable
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="updatedBy">The user updating the pii</param>
        /// <param name="personId">The person id</param>
        /// <param name="participantId">The participant id</param>
        /// <param name="firstName">The first name</param>
        /// <param name="lastName">The last name</param>
        /// <param name="namePrefix">The name prefix</param>
        /// <param name="nameSuffix">The name suffix</param>
        /// <param name="givenName">The given name</param>
        /// <param name="familyName">The family name</param>
        /// <param name="middleName">The middle name</param>
        /// <param name="patronym">The patronym name</param>
        /// <param name="alias">The alias</param>
        /// <param name="genderId">The gender id</param>
        /// <param name="ethnicity">The ethnicity</param>
        /// <param name="cityOfBirthId">The city of birth id</param>
        /// <param name="dateOfBirth">The date of birth</param>
        /// <param name="countriesOfCitizenship">The coutries of citizenship</param>
        /// <param name="homeAddresses">The home addresses</param>
        /// <param name="medicalConditions">The medical conditions</param>
        /// <param name="maritalStatusId">The marital status id</param>
        /// <param name="sevisId">The sevis id</param>
        public UpdatePii(
            User updatedBy,
            int personId,
            int participantId,
            string firstName,
            string lastName,
            string namePrefix,
            string nameSuffix,
            string givenName,
            string familyName,
            string middleName,
            string patronym,
            string alias,
            int genderId,
            string ethnicity,
            int cityOfBirthId,
            DateTime dateOfBirth,
            List<int> countriesOfCitizenship,
            List<HomeAddress> homeAddresses,
            string medicalConditions,
            int maritalStatusId,
            string sevisId
            )
        {
            Contract.Requires(updatedBy != null, "The updated by user must not be null.");
            this.PersonId = personId;
            this.ParticipantId = participantId;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.NamePrefix = namePrefix;
            this.NameSuffix = nameSuffix;
            this.GivenName = givenName;
            this.FamilyName = familyName;
            this.MiddleName = middleName;
            this.Patronym = patronym;
            this.Alias = alias;
            this.GenderId = genderId;
            this.Ethnicity = ethnicity;
            this.CityOfBirthId = cityOfBirthId;
            this.DateOfBirth = dateOfBirth;
            this.CountriesOfCitizenship = countriesOfCitizenship;
            this.HomeAddresses = homeAddresses;
            this.MedicalConditions = medicalConditions;
            this.MaritalStatusId = maritalStatusId;
            this.SevisId = sevisId;
            this.Audit = new Create(updatedBy);
        }

        /// <summary>
        /// Gets or sets the person id
        /// </summary>
        public int PersonId { get; private set; }

        /// <summary>
        /// Gets or sets the participant id
        /// </summary>
        public int ParticipantId { get; private set; }

        /// <summary>
        /// Gets or sets the first name
        /// </summary>
        public string FirstName { get; private set; }

        /// <summary>
        /// Gets or sets the last name
        /// </summary>
        public string LastName { get; private set; }

        /// <summary>
        /// Gets or sets the name prefix
        /// </summary>
        public string NamePrefix { get; private set; }

        /// <summary>
        /// Gets or sets the name suffix
        /// </summary>
        public string NameSuffix { get; private set; }

        /// <summary>
        /// Gets or sets the given name
        /// </summary>
        public string GivenName { get; private set; }

        /// <summary>
        /// Gets or sets the family name
        /// </summary>
        public string FamilyName { get; private set; }

        /// <summary>
        /// Gets or sets the middle name
        /// </summary>
        public string MiddleName { get; private set; }

        /// <summary>
        /// Gets or sets the patronym
        /// </summary>
        public string Patronym { get; private set; }

        /// <summary>
        /// Gets or sets the alias
        /// </summary>
        public string Alias { get; private set; }

        /// <summary>
        /// Gets or sets the gender
        /// </summary>
        public int GenderId { get; private set; }

        /// <summary>
        /// Gets or sets the ethnicity
        /// </summary>
        public string Ethnicity { get; private set; }

        /// <summary>
        /// Gets or sets the city of birth
        /// </summary>
        public int CityOfBirthId { get; private set; }

        /// <summary>
        /// Gets or sets the date of birth
        /// </summary>
        public DateTime DateOfBirth { get; private set; }

        /// <summary>
        /// Gets or sets the countries of citizenship
        /// </summary>
        public List<int> CountriesOfCitizenship { get; private set; }

        /// <summary>
        /// Gets or sets the home addresses
        /// </summary>
        public List<HomeAddress> HomeAddresses { get; private set; }

        /// <summary>
        /// Gets or sets the medical conditions
        /// </summary>
        public string MedicalConditions { get; private set; }

        /// <summary>
        /// Gets or sets the marital status id
        /// </summary>
        public int MaritalStatusId { get; private set; }

        /// <summary>
        /// Gets or sets the sevis id
        /// </summary>
        public string SevisId { get; private set; }

        /// <summary>
        /// Gets or sets the audit record
        /// </summary>
        public Audit Audit { get; private set; }
    }
}
