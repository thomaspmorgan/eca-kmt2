using ECA.Business.Queries.Models.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Queries.Models.Persons
{
    public class InstitutionDTO
    {

        /// <summary>
        /// Creates a new instance and initializes the addresses.
        /// </summary>
        public InstitutionDTO()
        {
            this.Addresses = new List<AddressDTO>();
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the addresses.
        /// </summary>
        public IEnumerable<AddressDTO> Addresses { get; set; }
    }
}
