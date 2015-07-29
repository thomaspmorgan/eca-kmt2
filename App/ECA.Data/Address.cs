using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Contracts;

namespace ECA.Data
{
    /// <summary>
    /// An address is a physical address associated with an organization or person
    /// </summary>
    public class Address : IHistorical
    {
        /// <summary>
        /// Creates a new Address instance and initializes the history.
        /// </summary>
        public Address()
        {
            this.History = new History();
        }

        /// <summary>
        /// Gets or sets the Address Id.
        /// </summary>
        public int AddressId { get; set; }

        /// <summary>
        /// Gets or sets the Address Type Id.
        /// </summary>
        public int AddressTypeId { get; set; }

        /// <summary>
        /// Gets or sets the Address Type.
        /// </summary>
        public virtual AddressType AddressType { get; set; }

        /// <summary>
        /// Gets or sets the Location Id.
        /// </summary>
        public int LocationId { get; set; }

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        public virtual Location Location { get; set; }

        /// <summary>
        /// Gets or sets the is primary flag.
        /// </summary>
        public bool IsPrimary { get; set; }

        /// <summary>
        /// Gets or sets the Person.
        /// </summary>
        public virtual Person Person { get; set; }

        /// <summary>
        /// Gets or sets the Person id.
        /// </summary>
        public int? PersonId { get; set; }

        /// <summary>
        /// Gets or sets the organization.
        /// </summary>
        public virtual Organization Organization { get; set; }

        /// <summary>
        /// Gets or sets the Organization id.
        /// </summary>
        public int? OrganizationId { get; set; }

        /// <summary>
        /// Gets or sets the history.
        /// </summary>
        public History History { get; set; }
    }

}
