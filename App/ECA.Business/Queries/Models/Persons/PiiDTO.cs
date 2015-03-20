using ECA.Business.Service.Lookup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Queries.Models.Persons
{
    public class PiiDTO
    {
        public PiiDTO ()
        {
            this.CountriesOfCitizenship = new List<SimpleLookupDTO>();
            this.HomeAddresses = new List<LocationDTO>();
        }

        public string Gender { get; set; }
        public DateTimeOffset DateOfBirth { get; set; }
        public IEnumerable<SimpleLookupDTO> CountriesOfCitizenship {get; set;}
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NamePrefix { get; set; }
        public string NameSuffix { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string MiddleName { get; set; }
        public string Patronym { get; set; }
        public string Alias { get; set; }
        public IEnumerable<LocationDTO> HomeAddresses { get; set; }
    }
}
