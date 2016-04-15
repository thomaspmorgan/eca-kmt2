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
        /// Gets or sets the dependent id.
        /// </summary>
        public int DependentId { get; set; }

        /// <summary>
        /// Gets or sets the person id.
        /// </summary>
        public int PersonId { get; set; }

        /// <summary>
        /// the SEVIS ID (assigned by SEVIS)
        /// </summary>
        public string SevisId { get; set; }

        /// <summary>
        /// Gets or sets the dependent type id.
        /// </summary>
        public int DependentTypeId { get; set; }

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
        public int PlaceOfBirthId { get; set; }

        /// <summary>
        /// Gets or sets the country of residence.
        /// </summary>
        public int PlaceOfResidenceId { get; set; }

        /// <summary>
        /// Gets or sets the birth country reason.
        /// </summary>
        public int? BirthCountryReasonId { get; set; }

        /// <summary>
        /// Gets and sets the email address
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// Gets and sets the countries of citizenship
        /// </summary>
        public List<PersonDependentCitizenCountry> CountriesOfCitizenship { get; set; }

        /// <summary>
        /// Gets or sets depended travelling with participant
        /// </summary>
        public bool IsTravellingWithParticipant { get; set; }

        /// <summary>
        /// Gets or sets depended was delete in ECA
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Gets or sets depended was delete in SEVIS
        /// </summary>
        public bool IsSevisDeleted { get; set; }

        /// <summary>
        /// Returns a business layer UpdatedPersonDependent instance
        /// </summary>
        /// <param name="user"></param>
        /// <returns>The business layer update entity</returns>
        public UpdatedPersonDependent ToUpdatePersonDependent(User user)
        {
            var model = new UpdatedPersonDependent(
                updater: user,
                dependentId: DependentId,
                personId: PersonId,
                dependentTypeId: DependentTypeId,
                sevisId: SevisId,
                firstName: FirstName,
                lastName: LastName,
                nameSuffix: NameSuffix,
                passportName: PassportName,
                preferredName: PreferredName,
                genderId: GenderId,
                dateOfBirth: DateOfBirth,
                placeOfBirthId: PlaceOfBirthId,
                placeOfResidenceId: PlaceOfResidenceId,
                birthCountryReasonId: BirthCountryReasonId,
                emailAddress: EmailAddress,
                countriesOfCitizenship: CountriesOfCitizenship,
                isTravelWithParticipant: IsTravellingWithParticipant,
                isDeleted: IsDeleted,
                isSevisDeleted: IsSevisDeleted);
            return model;
        }
    }
}