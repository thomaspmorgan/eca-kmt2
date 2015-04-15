using CAM.Business.Service;
using ECA.Core.Logging;
using ECA.WebApi.Models;
using Newtonsoft.Json;
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
        /// The factory to create a logger.
        /// </summary>
        public static Func<ILogger> LoggerFactory { get; set; }

        /// <summary>
        /// A Function to return to the WebApiUserBase.
        /// </summary>
        public static Func<IUserProvider> UserProviderFactory { get; set; }

        /// <summary>
        /// A Function to return a permission store.
        /// </summary>
        public static Func<IPermissionStore<IPermission>> PermissionLookupFactory { get; set; }

        /// <summary>
        /// Creates a new ResourceAuthorizeAttribute with the action permissions.
        /// </summary>
        /// <param name="permissions">The action permissions.</param>
        internal ResourceAuthorizeAttribute(params PermissionBase[] permissions)
        {
            this.Permissions = permissions;
        }

        /// <summary>
        /// Allows to set multiple permissions on an action with a formatted string of permissions.
        /// See the ActionPermission.Parse method for how to format a string of permissions.
        /// </summary>
        /// <param name="actionPermissions">The formatted string containing 1 or more permissions.</param>
        public ResourceAuthorizeAttribute(string actionPermissions)
            : this(PermissionBase.Parse(actionPermissions).ToArray())
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
        /// Creates a new ResourceAuthorizeAttribute where only the permission name and resource type need to be defined, i.e. the resource id
        /// is not coming from the client.  The resource Id in this case never changes, such as the application resource id.
        /// </summary>
        /// <param name="permissionName">The name of the permission.</param>
        /// <param name="resourceType">The resource type.</param>
        /// <param name="resourceId">The resource id.</param>
        public ResourceAuthorizeAttribute(string permissionName, string resourceType, int resourceId)
            : this(new StaticPermission
            {
                ResourceId = resourceId,
                PermissionName = permissionName,
                ResourceType = resourceType
            })
        {
            Contract.Requires(permissionName != null, "The permission name must not be null.");
            Contract.Requires(resourceType != null, "The resource type must not be null.");
        }

        /// <summary>
        /// Creates a new ResourceAuthorizeAttribute where the action binds to a model and resource id's are
        /// within the model.  The properties may be within subproperties.  If the web api has more than one action argument, then 
        /// the name of the variable is required in the property path.  Otherwise the name of the binding model is assumed and the property path
        /// does not have to have the name of the one action argument.
        /// </summary>
        /// <param name="property">The child property of the model the resource id is bound to.  This property may be a child property of another property 
        /// and seperated by a [.] e.g. modelVariable.MyOtherModel.Id where modelVariable is the name of the method variable, MyOtherModel is a
        /// class property of the modelVariableType, and Id is a property of the MyOtherModel type.</param>
        /// <param name="modelType">The model type that is bound in the action method.</param>
        /// <param name="permissionName">The name of the permission.</param>
        /// <param name="resourceType">The resource type.</param>
        public ResourceAuthorizeAttribute(string permissionName, string resourceType, Type modelType, string property)
            : this(new ModelPermission(property, modelType, permissionName, resourceType))
        {
            Contract.Requires(permissionName != null, "The permission name must not be null.");
            Contract.Requires(resourceType != null, "The resource type must not be null.");
            Contract.Requires(modelType != null, "The model type must not be null.");
            Contract.Requires(property != null, "The property must not be null.");
        }

        /// <summary>
        /// Gets the permissions required of this attribute.
        /// </summary>
        public IEnumerable<PermissionBase> Permissions { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionContext"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task OnActionExecutingAsync(System.Web.Http.Controllers.HttpActionContext actionContext, System.Threading.CancellationToken cancellationToken)
        {
            var permissionStore = PermissionLookupFactory();
            var webApiUserProvider = UserProviderFactory();
            var currentUser = webApiUserProvider.GetCurrentUser();
            var logger = LoggerFactory();
            var actionArguments = actionContext.ActionArguments;

            var actionName = actionContext.ActionDescriptor.ActionName;
            var controllerName = actionContext.ControllerContext.ControllerDescriptor.ControllerName;

            logger.Information("User [{0}] requesting authorization to resource on action [{1}].[{2}].",
                currentUser.GetUsername(),
                controllerName,
                actionName);
            var isUserValid = await webApiUserProvider.IsUserValidAsync(currentUser);
            if (!isUserValid)
            {
                logger.Information("User [{0}] denied authorization to resource because user is not valid in CAM.");
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }

            var userPermissions = (await webApiUserProvider.GetPermissionsAsync(currentUser)).ToList();
            var principalId = await webApiUserProvider.GetPrincipalIdAsync(currentUser);
            Contract.Assert(userPermissions != null, "The user permissions must not be null.");
            foreach (var permission in Permissions)
            {
                var permissionName = permission.PermissionName;
                var resourceTypeName = permission.ResourceType;
                var foreignResourceId = permission.GetResourceId(actionArguments);

                var requestedPermissionId = permissionStore.GetPermissionIdByName(permissionName);
                if (requestedPermissionId == 0)
                {
                    throw new NotSupportedException(String.Format("The requested permission [{0}] does not exist in CAM.", permissionName));
                }
                var resourceTypeId = permissionStore.GetResourceTypeId(resourceTypeName);
                if (!resourceTypeId.HasValue)
                {
                    throw new NotSupportedException(String.Format("The resource type name [{0}] does not have a matching resource id in CAM.", resourceTypeName));
                }

                var resourceId = permissionStore.GetResourceIdByForeignResourceId(foreignResourceId, resourceTypeId.Value);
                if (!resourceId.HasValue)
                {
                    logger.Warning("User [{0}] granted access to resource of type [{1}] with foreign key of [{2}] because the object is in the CAM resources.",
                        currentUser.GetUsername(),
                        resourceTypeName,
                        foreignResourceId);
                }
                else
                {
                    var requestedPermission = new Permission
                    {
                        PermissionId = requestedPermissionId,
                        ResourceId = resourceId.Value,
                        PrincipalId = principalId,
                        IsAllowed = true
                    };
                    var hasPermission = currentUser.HasPermission(requestedPermission, userPermissions);
                    if (!hasPermission)
                    {
                        logger.Information("User [{0}] denied access to resource [{1}] with foreign key of [{2}] because the user do not have the [{3}] permission.",
                            currentUser.GetUsername(),
                            resourceTypeName,
                            foreignResourceId,
                            permission);
                        throw new HttpResponseException(HttpStatusCode.Unauthorized);
                    }
                }
            }
            logger.Information("User [{0}] granted access to resource on action [{1}].[{2}].",
                currentUser.GetUsername(),
                controllerName,
                actionName);
            base.OnActionExecuting(actionContext);
        }
    }
}