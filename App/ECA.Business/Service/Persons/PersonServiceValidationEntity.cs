using ECA.Data;
using System;
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
        public PersonServiceValidationEntity(
            Person person, 
            DateTime? dateOfBirth,
            int genderId, 
            List<Location> countriesOfCitizenship, 
            int? placeOfBirthId, 
            bool? isDateOfBirthUnknown,
            bool? isDateOfBirthEstimated,
            bool? isPlaceOfBirthUnknown)
        {
            this.Person = person;
            this.GenderId = genderId;
            this.CountriesOfCitizenship = countriesOfCitizenship;
            this.PlaceOfBirthId = placeOfBirthId;
            this.IsDateOfBirthEstimated = isDateOfBirthEstimated;
            this.IsPlaceOfBirthUnknown = isPlaceOfBirthUnknown;
            this.IsDateOfBirthUnknown = isDateOfBirthUnknown;
            this.DateOfBirth = dateOfBirth;
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

        /// <summary>
        /// Place of birth
        /// </summary>
        public int? PlaceOfBirthId { get; private set; }
        
        /// <summary>
        /// Gets whether or not the date of birth is estimated.
        /// </summary>
        public bool? IsDateOfBirthEstimated { get; private set; }

        /// <summary>
        /// Gets whether or not the date of birth is unknown.
        /// </summary>
        public bool? IsDateOfBirthUnknown { get; private set; }

        /// <summary>
        /// Gets whether or not the place of birth is unknown.
        /// </summary>
        public bool? IsPlaceOfBirthUnknown { get; private set; }

        /// <summary>
        /// Gets the date of birth.
        /// </summary>
        public DateTime? DateOfBirth { get; private set; }
    }

}
