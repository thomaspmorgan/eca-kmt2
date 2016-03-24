using ECA.Business.Service;
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
        /// Gets or sets the person id.
        /// </summary>
        public int PersonId { get; set; }

        /// <summary>
        /// Gets or sets the person type id.
        /// </summary>
        public int PersonTypeId { get; set; }

        /// <summary>
        /// Gets or sets FirstName.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets last name.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the name suffix.
        /// </summary>
        public string NameSuffix { get; set; }

        /// <summary>
        /// Gets or sets passport name.
        /// </summary>
        public string PassportName { get; set; }

        /// <summary>
        /// Gets or sets preferred name.
        /// </summary>
        public string PreferredName { get; set; }

        /// <summary>
        /// Gets or sets the gender id.
        /// </summary>
        public int GenderId { get; set; }

        /// <summary>
        /// Gets or sets the date of birth.
        /// </summary>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets the place of birth.
        /// </summary>
        public int PlaceOfBirth_LocationId { get; set; }

        /// <summary>
        /// Gets or sets the country of residence.
        /// </summary>
        public int Residence_LocationId { get; set; }

        /// <summary>
        /// Gets or sets the birth country reason.
        /// </summary>
        public string BirthCountryReason { get; set; }

        /// <summary>
        /// Gets and sets the countries of citizenship
        /// </summary>
        public List<int> CountriesOfCitizenship { get; set; }

        /// <summary>
        /// Gets or sets depended travelling with participant
        /// </summary>
        public bool IsTravellingWithParticipant { get; set; }

        /// <summary>
        /// Creates a new person dependent business object
        /// </summary>
        /// <param name="user">The user that created the entity</param>
        /// <returns>New person dependent business object</returns>
        public NewPersonDependent ToNewDependent(User user)
        {
            return new NewPersonDependent(
                createdBy: user,
                personId: PersonId,
                personTypeId: PersonTypeId,
                firstName: FirstName,
                lastName: LastName,
                nameSuffix: NameSuffix,
                passportName: PassportName,
                preferredName: PreferredName,
                genderId: GenderId,
                dateOfBirth: DateOfBirth,
                locationOfBirthId: PlaceOfBirth_LocationId,
                residenceLocationId: Residence_LocationId,
                birthCountryReason: BirthCountryReason,
                countriesOfCitizenship: CountriesOfCitizenship,
                isTravelWithParticipant: IsTravellingWithParticipant);
        }
    }
}