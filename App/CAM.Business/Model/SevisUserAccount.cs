using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.Business.Model
{
    /// <summary>
    /// The sevis user account is used to hold information about the sevis batch api credentials.
    /// </summary>
    public class SevisUserAccount
    {
        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the org id.
        /// </summary>
        public string OrgId { get; set; }
    }
}
