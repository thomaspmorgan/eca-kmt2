﻿using ECA.Business.Queries.Models.Persons;
using ECA.Business.Service;
using ECA.Business.Service.Persons;
using ECA.Data;
using System;
using System.Collections.Generic;

namespace ECA.WebApi.Models.Person
{
    /// <summary>
    /// An UpdatedPersonDependentBindingModel is used by a web api client to update a person dependent's details.
    /// </summary>
    public class UpdatedPersonDependentBindingModel
    {
        /// <summary>
        /// Gets and sets the first name
        /// </summary>
        public FullNameDTO FullName { get; set; }

        /// <summary>
        /// Gets and sets the date of birth
        /// </summary>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Gets and sets the gender
        /// </summary>
        public int Gender { get; set; }

        /// <summary>
        /// Gets and sets the city of birth
        /// </summary>
        public int CityOfBirth { get; set; }

        /// <summary>
        /// Gets or sets the city of birth
        /// </summary>
        public int CountryOfBirth { get; set; }

        /// <summary>
        /// Gets and sets the countries of citizenship
        /// </summary>
        public List<Location> CountriesOfCitizenship { get; set; }

        /// <summary>
        /// Gets or sets the premanent residence country code
        /// </summary>
        public int PermanentResidenceCountryCode { get; set; }

        /// <summary>
        /// Gets or sets the birth country reason
        /// </summary>
        public string BirthCountryReason { get; set; }

        /// <summary>
        /// Gets or sets the email address
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the dependent person type
        /// </summary>
        public int PersonTypeId { get; set; }

        /// <summary>
        /// Returns a business layer UpdatedPersonDependent instance
        /// </summary>
        /// <param name="user"></param>
        /// <returns>The business layer update entity</returns>
        public UpdatedPersonDependent ToUpdatedPersonDependent(User user)
        {
            var model = new UpdatedPersonDependent(
                updater: user,
                fullName: FullName,
                dateOfBirth: DateOfBirth,
                gender: Gender,
                cityOfBirth: CityOfBirth,
                countryOfBirth: CountryOfBirth,
                countriesOfCitizenship: CountriesOfCitizenship,
                permanentResidenceCountryCode: PermanentResidenceCountryCode,
                birthCountryReason: BirthCountryReason,
                emailAddress: EmailAddress,
                personTypeId: PersonTypeId);
            return model;
        }

    }
}