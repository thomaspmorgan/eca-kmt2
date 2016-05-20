using ECA.Business.Service;
using ECA.Business.Service.Persons;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ECA.WebApi.Models.Person
{
    /// <summary>
    /// Binding model for person
    /// </summary>
    public class PersonBindingModel
    {
        /// <summary>
        /// Gets or sets the project id
        /// </summary>
        [Required]
        public int ProjectId { get; set; }

        /// <summary>
        /// Gets or set the participant type id
        /// </summary>
        [Required]
        public int ParticipantTypeId { get; set; }

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
        /// Gets or sets the gender
        /// </summary>
        [Required]
        public int Gender { get; set; }

        /// <summary>
        /// Gets or sets the date of birth
        /// </summary>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Denotes if date of birth is unknown
        /// </summary>
        public bool? IsDateOfBirthUnknown { get; set; }

        /// <summary>
        /// Gets or sets the city of birth
        /// </summary>
        public int? CityOfBirth { get; set; }

        /// <summary>
        /// Gets or sets the date of birth is estimated flag.
        /// </summary>
        public bool? IsDateOfBirthEstimated { get; set; }

        /// <summary>
        /// Gets or sets the is place of birth unknown flag.
        /// </summary>
        public bool? IsPlaceOfBirthUnknown { get; set; }

        /// <summary>
        /// Gets or sets the countries of citizenship
        /// </summary>
        public List<int> CountriesOfCitizenship { get; set; }
        
        /// <summary>
        /// Creates a new person business object
        /// </summary>
        /// <param name="user">The user that created the entity</param>
        /// <returns>New person business object</returns>
        public NewPerson ToNewPerson(User user)
        {
            return new NewPerson(
                createdBy: user,
                projectId: this.ProjectId,
                participantTypeId: this.ParticipantTypeId,
                firstName: this.FirstName,
                lastName: this.LastName,
                gender: this.Gender,
                dateOfBirth: this.DateOfBirth,
                isPlaceOfBirthUnknown: this.IsPlaceOfBirthUnknown,
                isDateOfBirthUnknown: this.IsDateOfBirthUnknown,
                isDateOfBirthEstimated: this.IsDateOfBirthEstimated,
                cityOfBirth: this.CityOfBirth,
                countriesOfCitizenship: this.CountriesOfCitizenship);
        }
    }
}