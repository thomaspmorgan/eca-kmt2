using ECA.Business.Queries.Models.Admin;
using ECA.Business.Service;
using ECA.Business.Service.Lookup;
using System;
using System.Collections.Generic;

namespace ECA.Business.Queries.Models.Persons
{
    /// <summary>
    /// The SimplePersonDependentDTO is used to represent dependents in the ECA system.
    /// </summary>
    public class SimplePersonDependentDTO : IAuditable
    {
        public SimplePersonDependentDTO()
        {
            CountriesOfCitizenship = new List<SimpleLookupDTO>();
        }

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
        /// Gets or sets the dependent type.
        /// </summary>
        public string DependentType { get; set; }

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
        /// Gets or sets the gender.
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// Gets or sets the date of birth.
        /// </summary>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets the place of birth.
        /// </summary>
        public int PlaceOfBirthId { get; set; }

        public LocationDTO PlaceOfBirth { get; set; }

        /// <summary>
        /// Gets or sets the country of residence id.
        /// </summary>
        public int PlaceOfResidenceId { get; set; }

        /// <summary>
        /// Gets or sets the country of residence.
        /// </summary>
        public LocationDTO PlaceOfResidence { get; set; }

        /// <summary>
        /// Gets or sets the birth country reason id.
        /// </summary>
        public int? BirthCountryReasonId { get; set; }

        /// <summary>
        /// Gets or sets the birth country reason.
        /// </summary>
        public string BirthCountryReason { get; set; }

        /// <summary>
        /// Gets and sets the countries of citizenship
        /// </summary>
        public IEnumerable<SimpleLookupDTO> CountriesOfCitizenship { get; set; }

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
        /// Gets and sets the audit record
        /// </summary>
        public Audit Audit { get; set; }        
    }
}
