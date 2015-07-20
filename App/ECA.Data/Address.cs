using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECA.Data
{
    /// <summary>
    /// An address is a physical address associated with an organization or person
    /// </summary>
    public class Address : IHistorical
    {
        /// <summary>
        /// The max length of the address display name.
        /// </summary>
        public const int ADDRESS_DISPLAY_NAME_MAX_LENGTH = 300;

        public Address()
        {
            this.History = new History();
        }

        public int AddressId { get; set; }
        public int AddressTypeId { get; set; }
        public virtual AddressType AddressType { get; set; }
        public int LocationId { get; set; }
        public virtual Location Location { get; set; }

        [Required]
        [MaxLength(ADDRESS_DISPLAY_NAME_MAX_LENGTH)]
        public string DisplayName { get; set; }

        // relationships
        public virtual Person Person { get; set; }
        public int? PersonId { get; set; }
        public virtual Organization Organization { get; set; }
        public int? OrganizationId { get; set; }

        public History History { get; set; }
    }

}
