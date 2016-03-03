using ECA.Business.Queries.Models.Admin;
using ECA.Business.Service.Lookup;
using System;
using System.Collections.Generic;

namespace ECA.Business.Queries.Models.Persons
{
    /// <summary>
    /// DTO for personally identifiable information
    /// </summary>
    public class PiiDTO
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public PiiDTO ()
        {
            CountriesOfCitizenship = new List<SimpleLookupDTO>();
            Addresses = new List<AddressDTO>();
            Dependants = new List<SimpleLookupDTO>();
        }

        /// <summary>
        /// Gets and sets gender
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// Gets and sets gender id
        /// </summary>
        public int GenderId { get; set; }

        /// <summary>
        /// Gets and sets date of birth
        /// </summary>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Gets and sets date of birth estimated.
        /// </summary>
        public bool? IsDateOfBirthEstimated { get; set; }

        /// <summary>
        /// Gets or sets the date of birth unknown flag.
        /// </summary>
        public bool? IsDateOfBirthUnknown { get; set; }

        /// <summary>
        /// Gets and sets countries of citizenship
        /// </summary>
        public IEnumerable<SimpleLookupDTO> CountriesOfCitizenship {get; set;}

        /// <summary>
        /// Gets and sets dependants
        /// </summary>
        public IEnumerable<SimpleLookupDTO> Dependants { get; set; }

        /// <summary>
        /// Gets and sets first name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets and sets last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets and sets name prefix
        /// </summary>
        public string NamePrefix { get; set; }

        /// <summary>
        /// Gets and sets name suffix
        /// </summary>
        public string NameSuffix { get; set; }

        /// <summary>
        /// Gets and sets given name
        /// </summary>
        public string GivenName { get; set; }

        /// <summary>
        /// Gets and sets family name
        /// </summary>
        public string FamilyName { get; set; }

        /// <summary>
        /// Gets and sets middle name
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
        /// Gets and sets patronym
        /// </summary>
        public string Patronym { get; set; }

        /// <summary>
        /// Gets and sets alias
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// Gets and sets marital status
        /// </summary>
        public string MaritalStatus { get; set; }

        /// <summary>
        /// Gets and sets marital status id
        /// </summary>
        public int? MaritalStatusId { get; set; }

        /// <summary>
        /// Gets and sets ethnicity
        /// </summary>
        public string Ethnicity { get; set; }

        /// <summary>
        /// Gets and sets medical conditions
        /// </summary>
        public string MedicalConditions { get; set; }

        /// <summary>
        /// Gets and sets addresses.
        /// </summary>
        public IEnumerable<AddressDTO> Addresses { get; set; }
        
        /// <summary>
        /// Gets or sets the place of birth unknown flag.
        /// </summary>
        public bool? IsPlaceOfBirthUnknown { get; set; }
        
        /// Gets or sets the city of birth location.
        /// </summary>
        public LocationDTO PlaceOfBirth { get; set; }

        /// <summary>
        /// Gets or sets the participant id.
        /// </summary>
        public int ParticipantId { get; set; }

        /// <summary>
        /// Gets or sets the ProjectId of this participant.
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// Gets or sets the participant's sevis id
        /// </summary>
        public string SevisId { get; set; }

        /// <summary>
        /// Sevis Status (person participants only)
        /// </summary>
        public string SevisStatus { get; set; }
    }
}
