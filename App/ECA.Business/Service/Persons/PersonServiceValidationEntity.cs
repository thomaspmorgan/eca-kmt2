using ECA.Data;
using System.Collections.Generic;

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
        public PersonServiceValidationEntity(Person person, int genderId, List<Location> countriesOfCitizenship, 
                                                int placeOfBirthId, bool isPlaceOfBirthUnknown)
        {
            this.Person = person;
            this.GenderId = genderId;
            this.CountriesOfCitizenship = countriesOfCitizenship;
            this.PlaceOfBirthId = placeOfBirthId;
            this.IsPlaceOfBirthUnknown = isPlaceOfBirthUnknown;
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

        /// <summary>
        /// Place of birth
        /// </summary>
        public int PlaceOfBirthId { get; private set; }

        /// <summary>
        /// Place of birth unknown flag
        /// </summary>
        public bool IsPlaceOfBirthUnknown { get; private set; }
    }

}
