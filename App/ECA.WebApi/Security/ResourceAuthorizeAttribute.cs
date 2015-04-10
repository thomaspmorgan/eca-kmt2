using ECA.Core.Logging;
using ECA.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;

namespace ECA.WebApi.Security
{
    /// <summary>
    /// The ResourceAuthorizeAttribute is used to secure controllers or actions with resource permissions given to a user.
    /// 
    /// Typically, an action will require only one permission, in that case use the single action permission controller.
    /// 
    /// If an action requires more than one permission, ActionPermissions can be set by a properly formatted string.
    /// </summary>
    public class ResourceAuthorizeAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// The default argument name for an action whose parameter is named id.
        /// </summary>
        public const string DEFAULT_ID_ARGUMENT_NAME = "id";

        private static readonly string COMPONENT_NAME = typeof(ResourceAuthorizeAttribute).FullName;

        /// <summary>
        /// The factory to create a cache factory.
        /// </summary>
        public static Func<IUserCacheService> CacheServiceFactory { get; set; }

        /// <summary>
        /// The factory to create a logger.
        /// </summary>
        public static Func<ILogger> LoggerFactory { get; set; }

        /// <summary>
        /// A Function to return to the WebApiUserBase.
        /// </summary>
        public static Func<WebApiUserBase> GetWebApiUser { get; set; }

        /// <summary>
        /// Creates a new ResourceAuthorizeAttribute with the action permissions.
        /// </summary>
        /// <param name="actionPermissions">The action permissions.</param>
        public ResourceAuthorizeAttribute(params ActionPermission[] actionPermissions)
        {
            this.ActionPermissions = actionPermissions;
        }

        /// <summary>
        /// Allows to set multiple permissions on an action with a formatted string of permissions.
        /// See the ActionPermission.Parse method for how to format a string of permissions.
        /// </summary>
        /// <param name="actionPermissions">The formatted string containing 1 or more permissions.</param>
        public ResourceAuthorizeAttribute(string actionPermissions)
            : this(ActionPermission.Parse(actionPermissions).ToArray())
        {

        }

        /// <summary>
        /// Creates a new ResourceAuthorizeAttribute with which only one permission is required.
        /// </summary>
        /// <param name="permissionName">The name of the permission.</param>
        /// <param name="resourceType">The resource type.</param>
        /// <param name="argumentName">The name of the action argument.  If none is supplied, it is assumed the argument is named 'id'.</param>
        public ResourceAuthorizeAttribute(string permissionName, string resourceType, string argumentName = DEFAULT_ID_ARGUMENT_NAME)
            : this(new ActionPermission
            {
            ArgumentName = argumentName,
            PermissionName = permissionName,
            ResourceType = resourceType
        })
        {
            Contract.Requires(permissionName != null, "The permission name must not be null.");
            Contract.Requires(resourceType != null, "The resource type must not be null.");
            Contract.Requires(argumentName != null, "The argument name must not be null.");
        }

        /// <summary>
        /// Gets the permissions required of this attribute.
        /// </summary>
        public IEnumerable<ActionPermission> ActionPermissions { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionContext"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task OnActionExecutingAsync(System.Web.Http.Controllers.HttpActionContext actionContext, System.Threading.CancellationToken cancellationToken)
        {
            var webApiUser = GetWebApiUser();
            var logger = LoggerFactory();
            var cacheService = CacheServiceFactory();
            var actionArguments = actionContext.ActionArguments;

            var userCache = await cacheService.GetUserCacheAsync(webApiUser);
            Contract.Assert(userCache != null, "The user cache must not be null.");
            foreach (var actionPermission in ActionPermissions)
            {
                Contract.Assert(actionArguments.ContainsKey(actionPermission.ArgumentName),
                    String.Format("The argument named [{0}] does not exist in the action arguments.", actionPermission.ArgumentName));
                logger.Information("Validating {0} action permission {1} with user's cached permissions.", actionContext.ActionDescriptor.ActionName, actionPermission.ToString());
                var actionArgumentValue = actionArguments[actionPermission.ArgumentName];
                if (actionArgumentValue.GetType() != typeof(int))
                {
                    throw new NotSupportedException(String.Format("The action argument must be an integer."));
                }
                var requestedPermission = new ResourcePermission
                {
                    PermissionName = actionPermission.PermissionName,
                    ResourceType = actionPermission.ResourceType,
                    ResourceId = (int)actionArgumentValue
                };
                if (!webApiUser.HasPermission(requestedPermission, userCache.Permissions))
                {
                    throw new HttpResponseException(HttpStatusCode.Unauthorized);
                }
            }
            base.OnActionExecuting(actionContext);
        }
    }
}