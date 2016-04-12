using CAM.Business.Model;
using System.Linq;
using CAM.Business.Service;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

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
        /// <param name="camUser">The cam business layer user.</param>
        /// <param name="isValidCamUser">True, if the user is valid according to CAM.</param>
        /// <param name="permissions">The permissions of the user.</param>
        public UserCache(IWebApiUser user, User camUser, bool isValidCamUser, IEnumerable<IPermission> permissions = null)
        {
            Contract.Requires(user != null, "The user must not be null.");
            Contract.Requires(camUser != null, "The cam user must not be null.");
            this.UserId = user.Id;
            this.Permissions = permissions ?? new List<IPermission>();
            this.DateCached = DateTime.UtcNow;
            this.UserName = user.GetUsername();
            this.CamPrincipalId = camUser.PrincipalId;
            this.IsValidCamUser = isValidCamUser;
            this.SevisUserAccounts = camUser.SevisUserAccounts.OrderBy(x => x.Username).ToList();
        }

        /// <summary>
        /// Gets the Common Access Module principal id i.e. the Principal Id.
        /// </summary>
        public int CamPrincipalId { get; private set; }

        /// <summary>
        /// Gets the user.
        /// </summary>
        public Guid UserId { get; private set; }

        /// <summary>
        /// Gets the user's permissions.
        /// </summary>
        public IEnumerable<IPermission> Permissions { get; private set; }

        /// <summary>
        /// Gets or sets the date this item was cached.
        /// </summary>
        public DateTime DateCached { get; private set; }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string UserName { get; private set; }

        /// <summary>
        /// Gets whether or not the user is valid according to CAM.
        /// </summary>
        public bool IsValidCamUser { get; private set; }

        /// <summary>
        /// Gets the sevis user accounts for the user.
        /// </summary>
        public IEnumerable<SevisUserAccount> SevisUserAccounts { get; private set; }
    }
}