using CAM.Business.Service;
using ECA.WebApi.Models;
using Newtonsoft.Json;
using NLog;
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
        /// A Function to return to the WebApiUserBase.
        /// </summary>
        public static Func<IUserProvider> UserProviderFactory { get; set; }

        /// <summary>
        /// A Function to return a permission store.
        /// </summary>
        public static Func<IPermissionStore<IPermission>> PermissionLookupFactory { get; set; }

        private IPermissionStore<IPermission> permissionStore;
        private AuthorizationResult authorizationResult;
        private bool isAuthorizationResultSet;
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Creates a new ResourceAuthorizeAttribute with the action permissions.
        /// </summary>
        /// <param name="permission">The action permission.</param>
        internal ResourceAuthorizeAttribute(PermissionBase permission)
        {
            Contract.Requires(UserProviderFactory != null, "The user provider factory must not be null.");
            Contract.Requires(PermissionLookupFactory != null, "The permission store factory must not be null.");
            this.Permission = permission;
            this.permissionStore = PermissionLookupFactory();
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
                ForeignResourceId = resourceId,
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
        public PermissionBase Permission { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionContext"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task OnActionExecutingAsync(System.Web.Http.Controllers.HttpActionContext actionContext, System.Threading.CancellationToken cancellationToken)
        {
            using (var userProvider = UserProviderFactory())
            {
                var currentUser = userProvider.GetCurrentUser();
                var actionArguments = actionContext.ActionArguments;
                var actionName = actionContext.ActionDescriptor.ActionName;
                var controllerName = actionContext.ControllerContext.ControllerDescriptor.ControllerName;
                if (!(await userProvider.IsUserValidAsync(currentUser)))
                {
                    this.logger.Info("User [{0}] denied authorization to resource because user is not valid in CAM.", currentUser.GetUsername());
                    SetAuthorizationResult(AuthorizationResult.InvalidCamUser);
                    throw new HttpResponseException(HttpStatusCode.Unauthorized);
                }

                var userPermissions = (await userProvider.GetPermissionsAsync(currentUser)).ToList();
                var principalId = await userProvider.GetPrincipalIdAsync(currentUser);
                Contract.Assert(userPermissions != null, "The user permissions must not be null.");
                var permissionName = this.Permission.PermissionName;
                var resourceTypeName = this.Permission.ResourceType;
                var foreignResourceId = this.Permission.GetResourceId(actionArguments);

                var requestedPermissionId = this.permissionStore.GetPermissionIdByName(permissionName);
                if (requestedPermissionId == 0)
                {
                    throw new NotSupportedException(String.Format("The requested permission [{0}] does not exist in CAM.", permissionName));
                }
                var resourceTypeId = this.permissionStore.GetResourceTypeId(resourceTypeName);
                if (!resourceTypeId.HasValue)
                {
                    throw new NotSupportedException(String.Format("The resource type name [{0}] does not have a matching resource id in CAM.", resourceTypeName));
                }

                var resourceId = this.permissionStore.GetResourceIdByForeignResourceId(foreignResourceId, resourceTypeId.Value);
                if (!resourceId.HasValue || resourceId.Value == 0)
                {
                    this.logger.Warn("User [{0}] granted access to resource of type [{1}] with foreign key of [{2}] because the object is NOT in the CAM resources.",
                        currentUser.GetUsername(),
                        resourceTypeName,
                        foreignResourceId);
                    SetAuthorizationResult(AuthorizationResult.ResourceDoesNotExist);
                }
                else
                {
                    var hasPermission = currentUser.HasPermission(GetRequestedPermission(requestedPermissionId, resourceId.Value, principalId), userPermissions);
                    if (!hasPermission)
                    {
                        this.logger.Info("User [{0}] denied access to resource [{1}] with foreign key of [{2}] because the user does not have the [{3}] permission.",
                            currentUser.GetUsername(),
                            resourceTypeName,
                            foreignResourceId,
                            this.Permission);
                        SetAuthorizationResult(AuthorizationResult.Denied);
                        throw new HttpResponseException(HttpStatusCode.Unauthorized);
                    }
                }
                this.logger.Info("User [{0}] granted access to resource with id [{1}] with foreign key [{2}] of type [{3}] on web api action [{4}].[{5}].",
                    currentUser.GetUsername(),
                    resourceId,
                    foreignResourceId,
                    resourceTypeName,
                    controllerName,
                    actionName);
                SetAuthorizationResult(AuthorizationResult.Allowed);
            }
            base.OnActionExecuting(actionContext);
        }

        private void SetAuthorizationResult(AuthorizationResult result)
        {
            if (!this.isAuthorizationResultSet)
            {
                this.authorizationResult = result;
                this.isAuthorizationResultSet = true;
            }
        }

        public AuthorizationResult GetAuthorizationResult()
        {
            return this.authorizationResult;
        }

        private Permission GetRequestedPermission(int requestedPermissionId, int resourceId, int principalId)
        {
            var requestedPermission = new Permission
            {
                PermissionId = requestedPermissionId,
                ResourceId = resourceId,
                PrincipalId = principalId,
                IsAllowed = true
            };
            return requestedPermission;
        }
    }
}