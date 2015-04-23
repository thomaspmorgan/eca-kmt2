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
        /// The user principal id in cam.
        /// </summary>
        public int PrincipalId { get; set; }

        /// <summary>
        /// The username.
        /// </summary>
        public string UserName { get; set; }
    }
}