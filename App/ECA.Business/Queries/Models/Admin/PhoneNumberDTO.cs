using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Queries.Models.Admin
{
    /// <summary>
    /// A PhoneNumberDTO is used to represent a phone number entity in the ECA System.
    /// </summary>
    public class PhoneNumberDTO
    {
        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the social media type id.
        /// </summary>
        public int PhoneNumberTypeId { get; set; }

        /// <summary>
        /// Gets or sets the social media type.
        /// </summary>
        public string PhoneNumberType { get; set; }

        /// <summary>
        /// Gets or sets the social media value.
        /// </summary>
        public string Number { get; set; }
    }
}
