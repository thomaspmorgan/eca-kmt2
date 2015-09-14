using ECA.Business.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ECA.Business.Service.Persons;

namespace ECA.WebApi.Models.Person
{
    /// <summary>
    /// Binding model for editing pii
    /// </summary>
    public class PiiBindingModel
    {
        /// <summary>
        /// Gets and sets the person id
        /// </summary>
        [Required]
        public int PersonId { get; set; }

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

        /// <summary>
        /// Gets or sets the name prefix
        /// </summary>
        public string NamePrefix { get; set; }

        /// <summary>
        /// Gets or sets the name suffix
        /// </summary>
        public string NameSuffix { get; set; }

        /// <summary>
        /// Gets or sets the given name
        /// </summary>
        public string GivenName { get; set; }

        /// <summary>
        /// Gets or sets the family name
        /// </summary>
        public string FamilyName { get; set; }

        /// <summary>
        /// Gets or sets the middle name
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
        /// Gets or sets the patronym
        /// </summary>
        public string Patronym { get; set; }

        /// <summary>
        /// Gets or sets the alias
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// Gets or sets the gender
        /// </summary>
        [Required]
        public int GenderId { get; set; }

        /// <summary>
        /// Gets or sets the ethnicity
        /// </summary>
        public string Ethnicity { get; set; }

        /// <summary>
        /// Gets or sets the city of birth
        /// </summary>
        public int? CityOfBirthId { get; set; }

        /// <summary>
        /// Gets or sets the date of birth
        /// </summary>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets the date of birth unknown flag.
        /// </summary>
        public bool? IsDateOfBirthUnknown { get; set; }

        /// <summary>
        /// Gets or sets the countries of citizenship
        /// </summary>
        [Required]
        public List<int> CountriesOfCitizenship { get; set; }

        /// <summary>
        /// Gets or sets the medical conditions
        /// </summary>
        public string MedicalConditions { get; set; }

        /// <summary>
        /// Gets or sets the marital status id
        /// </summary>
        public int? MaritalStatusId { get; set; }

        /// <summary>
        /// Convert binding model to business model 
        /// </summary>
        /// <param name="user">The user updating the pii</param>
        /// <returns>Update pii business model</returns>
        public UpdatePii ToUpdatePii(User user)
        {
            return new UpdatePii(
                updatedBy: user,
                personId: this.PersonId,
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
                isDateOfBirthUnknown: this.IsDateOfBirthUnknown,
                countriesOfCitizenship: this.CountriesOfCitizenship,
                medicalConditions: this.MedicalConditions,
                maritalStatusId: this.MaritalStatusId
                );
        }
    }
}