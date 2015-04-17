using CAM.Business.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Security
{
    /// <summary>
    /// An Anonymous user is a default user when Azure has not authenticated the curren tuser.
    /// </summary>
    public class AnonymousUser : WebApiUserBase
    {
        /// <summary>
        /// The username of an anonymous user.
        /// </summary>
        public const string ANONYMOUS_USER_NAME = "Anonymous";

        /// <summary>
        /// Initializes a new User and sets the user id to the empty guid.
        /// </summary>
        public AnonymousUser()
        {
            this.Id = Guid.Empty;
        }

        /// <summary>
        /// Returns false.  An anonymous user never has permission.
        /// </summary>
        /// <param name="requestedPermission">The requested permission.</param>
        /// <param name="allUserPermissions">The user's current permissions.</param>
        /// <returns>Returns false.</returns>
        public override bool HasPermission(IPermission requestedPermission, IEnumerable<IPermission> allUserPermissions)
        {
            return false;
        }

        /// <summary>
        /// Returns the anonymous username.
        /// </summary>
        /// <returns>The anonymous username.</returns>
        public override string GetUsername()
        {
            return ANONYMOUS_USER_NAME;
        }
    }
}