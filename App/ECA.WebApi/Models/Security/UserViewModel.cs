using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Models.Security
{
    /// <summary>
    /// A simple class to hold information about the current user.
    /// </summary>
    public class UserViewModel
    {
        /// <summary>
        /// The Azure AD user id.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// The username.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Whether or not the user is registered with the ECA system.
        /// </summary>
        public bool IsRegistered { get; set; }

        /// <summary>
        /// The user's display name in the system.
        /// </summary>
        public string DisplayName { get; set; }
    }
}