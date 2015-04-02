using ECA.Business.Service.Lookup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            this.CountriesOfCitizenship = new List<SimpleLookupDTO>();
            this.HomeAddresses = new List<LocationDTO>();
        }

        /// <summary>
        /// Gets and sets gender
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// Gets and sets date of birth
        /// </summary>
        public DateTimeOffset DateOfBirth { get; set; }

        /// <summary>
        /// Gets and sets countries of citizenship
        /// </summary>
        public IEnumerable<SimpleLookupDTO> CountriesOfCitizenship {get; set;}

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
        /// Gets and sets ethnicity
        /// </summary>
        public string Ethnicity { get; set; }

        /// <summary>
        /// Gets and sets medical conditions
        /// </summary>
        public string MedicalConditions { get; set; }

        /// <summary>
        /// Gets and sets home addresses
        /// </summary>
        public IEnumerable<LocationDTO> HomeAddresses { get; set; }

        /// <summary>
        /// Gets and sets city of birth
        /// </summary>
        public string CityOfBirth { get; set; }

        /// <summary>
        /// Gets and sets country of birth 
        /// </summary>
        public string CountryOfBirth { get; set; }
    }
}
