using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ECA.Business.Queries.Models.Admin
{
    /// <summary>
    /// A MembershipDTO is used to represent a membership entity in the ECA System.
    /// </summary>
    public class MembershipDTO
    {
        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the membership name.
        /// </summary>
        public string Name { get; set; }
    }
}
