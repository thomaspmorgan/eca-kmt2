using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Queries.Models.Admin
{
    /// <summary>
    /// A EmailAddressDTO is used to represent an email address entity in the ECA System.
    /// </summary>
    public class EmailAddressDTO
    {
        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the social media type id.
        /// </summary>
        public int EmailAddressTypeId { get; set; }

        /// <summary>
        /// Gets or sets the social media type.
        /// </summary>
        public string EmailAddressType { get; set; }

        /// <summary>
        /// Gets or sets the social media value.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the person id.
        /// </summary>
        public int? PersonId { get; set; }

        /// <summary>
        /// Gets or sets the contact id.
        /// </summary>
        public int? ContactId { get; set; }

        /// <summary>
        /// Gets or sets is primary.
        /// </summary>
        public bool? IsPrimary { get; set; }
    }
}
