using ECA.Business.Queries.Models.Persons;
using ECA.Business.Service;
using ECA.Business.Service.Lookup;
using ECA.Business.Service.Persons;
using System;
using System.Collections.Generic;

namespace ECA.WebApi.Models.Person
{
    /// <summary>
    /// Binding model for person dependent
    /// </summary>
    public class DependentBindingModel
    {
        /// <summary>
        /// Gets or sets the first name
        /// </summary>
        public FullNameDTO FullName { get; set; }
        
        /// <summary>
        /// Gets or sets the gender
        /// </summary>
        public int Gender { get; set; }

        /// <summary>
        /// Gets or sets the city of birth
        /// </summary>
        public int CityOfBirth { get; set; }

        /// <summary>
        /// Gets or sets the date of birth
        /// </summary>
        public DateTime DateOfBirth { get; set; }
        
        /// <summary>
        /// Gets or sets the city of birth
        /// </summary>
        public int CountryOfBirth { get; set; }

        /// <summary>
        /// Gets or sets the countries of citizenship
        /// </summary>
        public List<SimpleLookupDTO> CountriesOfCitizenship { get; set; }

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
        /// Gets or sets the dependent relationship id.
        /// </summary>
        public int PersonTypeId { get; set; }

        /// <summary>
        /// Creates a new person dependent business object
        /// </summary>
        /// <param name="user">The user that created the entity</param>
        /// <returns>New person dependent business object</returns>
        public NewPersonDependent ToNewDependent(User user)
        {
            return new NewPersonDependent(
                createdBy: user,
                fullName: this.FullName,
                dateOfBirth: this.DateOfBirth,
                gender: this.Gender,
                cityOfBirth: this.CityOfBirth,
                countryOfBirth: this.CountryOfBirth,                                
                countriesOfCitizenship: this.CountriesOfCitizenship,
                permanentResidenceCountryCode: this.PermanentResidenceCountryCode,
                birthCountryReason: this.BirthCountryReason,
                emailAddress: this.EmailAddress,
                personTypeId: this.PersonTypeId);
        }
    }
}