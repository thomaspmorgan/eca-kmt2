using ECA.Business.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ECA.Business.Service.Persons;

namespace ECA.WebApi.Models.Person
{
    public class PiiBindingModel
    {
        [Required]
        public int PersonId { get; set; }

        [Required]
        public int ParticipantId { get; set; }

        /// <summary>
        /// Gets or sets the first name
        /// </summary>
        [Required]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name
        /// </summary>
        [Required]
        public string LastName { get; set; }

        public string NamePrefix { get; set; }

        public string NameSuffix { get; set; }

        public string GivenName { get; set; }

        public string FamilyName { get; set; }

        public string MiddleName { get; set; }

        public string Patronym { get; set; }

        public string Alias { get; set; }

        /// <summary>
        /// Gets or sets the gender
        /// </summary>
        [Required]
        public int GenderId { get; set; }

        public string Ethnicity { get; set; }

        /// <summary>
        /// Gets or sets the city of birth
        /// </summary>
        [Required]
        public int CityOfBirthId { get; set; }

        /// <summary>
        /// Gets or sets the date of birth
        /// </summary>
        [Required]
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets the countries of citizenship
        /// </summary>
        [Required]
        public List<int> CountriesOfCitizenship { get; set; }

        public List<AddressBindingModel> HomeAddresses { get; set; }

        public string MedicalConditions { get; set; }

        public int MaritalStatusId { get; set; }

        public string SevisId { get; set; }

        public UpdatePii ToUpdatePii(User user)
        {
            return new UpdatePii(
                updatedBy: user,
                personId: this.PersonId,
                participantId: this.ParticipantId,
                firstName: this.FirstName,
                lastName: this.LastName,
                namePrefix: this.NamePrefix,
                nameSuffix: this.NameSuffix,
                givenName: this.GivenName,
                familyName: this.FamilyName,
                middleName: this.MiddleName,
                patronym: this.Patronym,
                alias: this.Alias,
                genderId: this.GenderId,
                ethnicity: this.Ethnicity,
                cityOfBirthId: this.CityOfBirthId,
                dateOfBirth: this.DateOfBirth,
                countriesOfCitizenship: this.CountriesOfCitizenship,
                homeAddresses: ToHomeAddresses(user),
                medicalConditions: this.MedicalConditions,
                maritalStatusId: this.MaritalStatusId,
                sevisId: this.SevisId
                );
        }

        private List<HomeAddress> ToHomeAddresses(User user)
        {
            var homeAddresses = new List<HomeAddress>();
            foreach (AddressBindingModel address in this.HomeAddresses)
            {
                homeAddresses.Add(address.ToHomeAddress(user));
            }
            return homeAddresses;
        }
    }
}