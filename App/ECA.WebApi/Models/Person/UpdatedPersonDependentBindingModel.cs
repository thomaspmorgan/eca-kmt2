using ECA.Business.Service;
using ECA.Business.Service.Persons;
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
        /// Gets or sets the person id.
        /// </summary>
        public int PersonId { get; set; }

        /// <summary>
        /// Gets the person type id.
        /// </summary>
        public int PersonTypeId { get; private set; }

        /// <summary>
        /// Gets and sets the first name
        /// </summary>
        public string FirstName { get; private set; }

        /// <summary>
        /// Gets and sets the last name
        /// </summary>
        public string LastName { get; private set; }

        /// <summary>
        /// Gets or sets the name suffix.
        /// </summary>
        public string NameSuffix { get; set; }

        /// <summary>
        /// Person passport name.
        /// </summary>
        public string PassportName { get; set; }

        /// <summary>
        /// Person preferred name.
        /// </summary>
        public string PreferredName { get; set; }

        /// <summary>
        /// Gets and sets the gender
        /// </summary>
        public int Gender { get; private set; }

        /// <summary>
        /// Gets and sets the date of birth
        /// </summary>
        public DateTime? DateOfBirth { get; private set; }

        /// <summary>
        /// Gets and sets the city of birth
        /// </summary>
        public int? CityOfBirth { get; private set; }

        /// <summary>
        /// Gets and sets the countries of citizenship
        /// </summary>
        public List<int> CountriesOfCitizenship { get; private set; }

        /// <summary>
        /// Gets or sets the premanent residence country code
        /// </summary>
        public int PermanentResidenceCountryCode { get; set; }

        /// <summary>
        /// Gets or sets the birth country reason
        /// </summary>
        public string BirthCountryReason { get; set; }

        /// <summary>
        /// Returns a business layer UpdatedPersonDependent instance
        /// </summary>
        /// <param name="user"></param>
        /// <returns>The business layer update entity</returns>
        public UpdatedPersonDependent ToUpdatePersonDependent(User user)
        {
            var model = new UpdatedPersonDependent(
                updater: user,
                personId: PersonId,
                firstName: FirstName,
                lastName: LastName,
                nameSuffix: NameSuffix,
                passportName: PassportName,
                preferredName: PreferredName,
                gender: Gender,
                dateOfBirth: DateOfBirth,
                cityOfBirth: CityOfBirth,
                personTypeId: PersonTypeId,
                countriesOfCitizenship: CountriesOfCitizenship,
                permanentResidenceCountryCode: PermanentResidenceCountryCode,
                birthCountryReason: BirthCountryReason);
            return model;
        }
    }
}