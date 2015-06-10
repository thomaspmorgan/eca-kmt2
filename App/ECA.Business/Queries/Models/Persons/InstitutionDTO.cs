using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Queries.Models.Persons
{
    public class InstitutionDTO
    {
        public string Name { get; set; }
        public IEnumerable<LocationDTO> Addresses { get; set; }
    }
}
