using ECA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Persons
{
    /// <summary>
    /// Validation entity for person service
    /// </summary>
    public class PersonServiceValidationEntity
    {

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="person">The person</param>
        /// <param name="firstName">The first name</param>
        /// <param name="lastName">The last name</param>
        /// <param name="genderId">The gender id</param>
        /// <param name="dateOfBirth">The date of birth</param>
        /// <param name="cityOfBirth">The city of birth</param>
        /// <param name="countriesOfCitizenship">The countries of citizenship</param>
        public PersonServiceValidationEntity(Person person, int genderId, 
                                             DateTime dateOfBirth, Location cityOfBirth, List<Location> countriesOfCitizenship)
        {
            this.Person = person;
            this.GenderId = genderId;
            this.DateOfBirth = dateOfBirth;
            this.CityOfBirth = cityOfBirth;
            this.CountriesOfCitizenship = countriesOfCitizenship;
        }

        /// <summary>
        /// Gets or sets person
        /// </summary>
        public Person Person { get; private set; }

        /// <summary>
        /// Gets or sets gender id
        /// </summary>
        public int GenderId { get; private set; }

        /// <summary>
        /// Gets or sets date of birth
        /// </summary>
        public DateTime DateOfBirth { get; private set; }

        /// <summary>
        /// Gets or sets city or birth
        /// </summary>
        public Location CityOfBirth { get; private set; }

        /// <summary>
        /// Gets or sets countries of citizenship
        /// </summary>
        public List<Location> CountriesOfCitizenship { get; private set; }
    }

}
