using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.Caching;
using System.Web;

namespace ECA.WebApi.Security
{
    /// <summary>
    /// A UserCache is class to hold user's details that should be cached in the web api.
    /// </summary>
    public class UserCache
    {
        /// <summary>
        /// Creates a new user cache.  If the permissions are not provided, the Permissions will be initialized with an empty list.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="permissions">The permissions of the user.</param>
        public UserCache(IWebApiUser user, IEnumerable<ResourcePermission> permissions = null)
        {
            Contract.Requires(user != null, "The user must not be null.");
            this.User = user;
            this.Permissions = permissions ?? new List<ResourcePermission>();
        }

        /// <summary>
        /// Gets the user.
        /// </summary>
        public IWebApiUser User { get; private set; }

        /// <summary>
        /// Gets the user's permissions.
        /// </summary>
        public IEnumerable<ResourcePermission> Permissions { get; private set; }

    }
}