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
        /// <param name="genderId">The gender id</param>
        /// <param name="countriesOfCitizenship">The countries of citizenship</param>
        public PersonServiceValidationEntity(Person person, int genderId, List<Location> countriesOfCitizenship)
        {
            this.Person = person;
            this.GenderId = genderId;
            this.CountriesOfCitizenship = countriesOfCitizenship;
        }

        public PersonServiceValidationEntity(Person person, List<ProminentCategory> prominentCategories)
        {
            this.Person = person;
            this.ProminentCategories = prominentCategories;
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
        /// Gets or sets countries of citizenship
        /// </summary>
        public List<Location> CountriesOfCitizenship { get; private set; }

        public List<ProminentCategory> ProminentCategories { get; private set; }

    }

}
