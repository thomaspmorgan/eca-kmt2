using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Persons
{
    public class UpdatePii : IAuditable
    {
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

        public int PersonId { get; private set; }

        public int ParticipantId { get; private set; }

        public string FirstName { get; private set; }

        /// <summary>
        /// Gets or sets the last name
        /// </summary>
        public string LastName { get; private set; }

        public string NamePrefix { get; private set; }

        public string NameSuffix { get; private set; }

        public string GivenName { get; private set; }

        public string FamilyName { get; private set; }

        public string MiddleName { get; private set; }

        public string Patronym { get; private set; }

        public string Alias { get; private set; }

        /// <summary>
        /// Gets or sets the gender
        /// </summary>
        public int GenderId { get; private set; }

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

        public List<HomeAddress> HomeAddresses { get; private set; }

        public string MedicalConditions { get; private set; }

        public int MaritalStatusId { get; private set; }

        public string SevisId { get; private set; }

        public Audit Audit { get; private set; }
    }
}
