﻿using ECA.Business.Service;
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
        [Required]
        public DateTimeOffset DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets the city of birth
        /// </summary>
        [Required]
        public int CityOfBirth { get; set; }

        /// <summary>
        /// Gets or sets the countries of citizenship
        /// </summary>
        [Required]
        public List<int> CountriesOfCitizenship { get; set; }

        /// <summary>
        /// Creates a new person business object
        /// </summary>
        /// <param name="userId">The user that created the entity</param>
        /// <returns>New person business object</returns>
        public NewPerson ToNewPerson(int userId)
        {
            return new NewPerson(new User(userId), this.ProjectId, this.FirstName, this.LastName, this.Gender, this.DateOfBirth,
                                 this.CityOfBirth, this.CountriesOfCitizenship);
        }
    }
}