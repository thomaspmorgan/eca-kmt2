using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Security
{
    /// <summary>
    /// A container class for when an admin with permissions wishes to impersonate another user.
    /// </summary>
    public class ImpersonatedUser : IWebApiUser
    {
        private string impersonatorUsername;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="impersonatorUserId">The id of the user that will be impersonating another user.</param>
        /// <param name="impersonatedUserId">The id of the user to be impersontated.</param>
        /// <param name="impersonatorUserName">The user name of the user who will be impersonating another user.</param>
        public ImpersonatedUser(Guid impersonatorUserId, Guid impersonatedUserId, string impersonatorUserName)
        {
            Contract.Requires(!String.IsNullOrWhiteSpace(impersonatorUserName), "The user must not be null or whitespace or empty.");
            if (impersonatorUserId == Guid.Empty)
            {
                throw new ArgumentException("The id of the impersonator may not be empty.");
            }
            if (impersonatedUserId == Guid.Empty)
            {
                throw new ArgumentException("The id of the impersonated may not be empty.");
            }
            this.ImpersonatedUserId = impersonatedUserId;
            this.ImpersonatorUserId = impersonatorUserId;
            this.impersonatorUsername = impersonatorUserName;
        }

        /// <summary>
        /// Gets the Id of the impersonated user.
        /// </summary>
        public Guid Id
        {
            get
            {
                return ImpersonatedUserId;
            }
        }

        /// <summary>
        /// Gets the impersonated user id.
        /// </summary>
        public Guid ImpersonatedUserId
        {
            get; private set;
        }

        /// <summary>
        /// Gets the id of the user doing the impersonation.
        /// </summary>
        public Guid ImpersonatorUserId
        {
            get;
            private set;
        }

        /// <summary>
        /// Returns the username of the user impersonating another user.
        /// </summary>
        /// <returns>The username of the user impersonating another user.</returns>
        public string GetUsername()
        {
            return impersonatorUsername;
        }
    }
}