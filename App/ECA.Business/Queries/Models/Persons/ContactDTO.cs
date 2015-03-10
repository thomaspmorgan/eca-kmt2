using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Queries.Models.Persons
{
    /// <summary>
    /// A ContactDTO is to represent a simplified contact within the eca system.
    /// </summary>
    public class ContactDTO
    {
        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the FullName.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        public string Position { get; set; }
    }
}
