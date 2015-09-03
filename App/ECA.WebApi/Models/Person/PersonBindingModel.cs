using ECA.Business.Service;
using ECA.Business.Service.Persons;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

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
        /// Gets or sets the city of birth
        /// </summary>
        public int? CityOfBirth { get; set; }

        /// <summary>
        /// Gets or sets the countries of citizenship
        /// </summary>
        public List<int> CountriesOfCitizenship { get; set; }

        /// <summary>
        /// Creates a new person business object
        /// </summary>
        /// <param name="userId">The user that created the entity</param>
        /// <returns>New person business object</returns>
        public NewPerson ToNewPerson(User user)
        {
            return new NewPerson(user, this.ProjectId, this.ParticipantTypeId, this.FirstName, this.LastName, this.Gender, this.DateOfBirth,
                                 this.CityOfBirth, this.CountriesOfCitizenship);
        }
    }
}