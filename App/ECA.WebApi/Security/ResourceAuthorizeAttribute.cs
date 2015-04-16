﻿using ECA.Core.Logging;
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
            :this(new ModelPermission(property, modelType, permissionName, resourceType))
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
            var webApiUser = GetWebApiUser();
            var logger = LoggerFactory();
            var cacheService = CacheServiceFactory();
            var actionArguments = actionContext.ActionArguments;

            var userCache = await cacheService.GetUserCacheAsync(webApiUser);
            Contract.Assert(userCache != null, "The user cache must not be null.");
            foreach (var permission in Permissions)
            {
                logger.Information("Validating {0} action permission {1} with user's cached permissions.", actionContext.ActionDescriptor.ActionName, permission.ToString());
                var requestedPermission = new ResourcePermission
                {
                    PermissionName = permission.PermissionName,
                    ResourceType = permission.ResourceType,
                    ResourceId = permission.GetResourceId(actionArguments)
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