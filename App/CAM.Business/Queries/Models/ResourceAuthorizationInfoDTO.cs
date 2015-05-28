using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.Business.Queries.Models
{
    /// <summary>
    /// A simple object used to contain information about resource authorizations on a cam resource.
    /// </summary>
    public class ResourceAuthorizationInfoDTO
    {
        /// <summary>
        /// Gets or sets the number of principals who have at least one permission on the resource.
        /// </summary>
        public int AllowedPrincipalsCount { get; set; }

        /// <summary>
        /// Gets or sets the date the permissions were last modified.
        /// </summary>
        public DateTimeOffset LastRevisedOn { get; set; }
    }
}
