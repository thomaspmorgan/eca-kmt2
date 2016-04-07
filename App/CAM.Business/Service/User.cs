using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CAM.Data;

namespace CAM.Business.Service
{
    /// <summary>
    /// The User is a container for holding information about a user when interacting with the business model.  It also holds information
    /// from the Azure AD service.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Gets or sets the principal id.
        /// </summary>
        public int PrincipalId { get; set; }

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Get or sets the account status text.
        /// </summary>
        public String AccountStatus { get; set; }
        
        /// <summary>
        /// Gets or sets the account status id.
        /// </summary>
        public int AccountStatusId { get; set; }

        /// <summary>
        /// Gets or sets the last accessed date.
        /// </summary>
        public DateTimeOffset? LastAccessed { get; set; }

        /// <summary>
        /// Gets or sets the suspended date.
        /// </summary>
        public DateTimeOffset? SuspendedDate { get; set; }

        /// <summary>
        /// Gets or sets the restored date.
        /// </summary>
        public DateTimeOffset? RestoredDate { get; set; }

        /// <summary>
        /// Gets or sets the revoked date.
        /// </summary>
        public DateTimeOffset? RevokedDate { get; set; }

        /// <summary>
        /// Gets or sets the expired date.
        /// </summary>
        public DateTimeOffset? ExpiredDate { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the ad(Azure) guid.
        /// </summary>
        public Guid AdGuid { get; set; }

        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the sevis user name.
        /// </summary>
        public string SevisUsername { get; set; }
    }
}