using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Security
{
    /// <summary>
    /// An AuthorizationResult is used the ResourceAuthorizeAttribute class and details
    /// the request's authorization result.  This is used mainly in testing and debugging.
    /// </summary>
    public enum AuthorizationResult
    {
        /// <summary>
        /// Authorization passed.
        /// </summary>
        Allowed,

        /// <summary>
        /// Denied authorization.
        /// </summary>
        Denied,

        /// <summary>
        /// The request resource does not exist.
        /// </summary>
        ResourceDoesNotExist,

        /// <summary>
        /// The user exists in AD but does not exist in CAM.
        /// </summary>
        InvalidCamUser
    }
}