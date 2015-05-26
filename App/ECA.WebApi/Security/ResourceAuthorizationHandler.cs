using CAM.Business.Model;
using CAM.Business.Service;
using ECA.Core.Query;
using ECA.Core.Service;
using ECA.WebApi.Models.Query;
using ECA.WebApi.Models.Security;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;

namespace ECA.WebApi.Security
{
    /// <summary>
    /// A ResourceAuthorizationHandler is responsible for handling requests to grant, revoke, or delete permissions to and from resources.
    /// </summary>
    public class ResourceAuthorizationHandler : ISaveable, ECA.WebApi.Security.IResourceAuthorizationHandler
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private IUserProvider userProvider;
        private IPrincipalService principalService;
        private IResourceService resourceService;
        private IUserService userService;
        private Action<int, int> throwIfGrantorAndGranteePrincipalIdEqual;

        /// <summary>
        /// Creates a new ResourceAuthorizationHandler.
        /// </summary>
        /// <param name="resourceService">The resource service.</param>
        /// <param name="userProvider">The user provider.</param>
        /// <param name="principalService">The principal service.</param>
        /// <param name="userService">The user service.</param>
        public ResourceAuthorizationHandler(IResourceService resourceService, IUserProvider userProvider, IPrincipalService principalService, IUserService userService)
        {
            Contract.Requires(userProvider != null, "The user provider must not be null.");
            Contract.Requires(principalService != null, "The principal service must not be null.");
            Contract.Requires(resourceService != null, "The resource service must not be null.");
            Contract.Requires(userService != null, "The user service must not be null.");
            this.userProvider = userProvider;
            this.principalService = principalService;
            this.resourceService = resourceService;
            this.userService = userService;
            throwIfGrantorAndGranteePrincipalIdEqual = (granteePrincipalId, grantorPrincipalId) =>
            {
                if (granteePrincipalId == grantorPrincipalId)
                {
                    throw new HttpResponseException(HttpStatusCode.Forbidden);
                }
            };
        }

        /// <summary>
        /// Returns the Id of the current user that is granting a resource permission.
        /// </summary>
        /// <returns>The principal id of the user granting a permission.</returns>
        public int GetGrantorPrincipalId()
        {
            var currentUser = userProvider.GetCurrentUser();
            var businessUser = userProvider.GetBusinessUser(currentUser);
            return businessUser.Id;
        }

        /// <summary>
        /// Clears the grantee user cache from the user provider.  This forces the user's
        /// cached permissions to be refreshed during the next request.
        /// </summary>
        /// <param name="granteePrincipalId">The principal id of the user whose permissions are being modified.</param>
        /// <returns>The task.</returns>
        public async Task ClearUserCacheAsync(int granteePrincipalId)
        {
            var grantee = await this.userService.GetUserByIdAsync(granteePrincipalId);
            if (grantee == null)
            {
                throw new ArgumentException(String.Format("The grantee user could not be found with id [{0}].", granteePrincipalId));
            }
            var adGuid = grantee.AdGuid;
            Contract.Assert(adGuid != Guid.Empty, "The ad guid should be valid.");
            userProvider.Clear(adGuid);
        }

        /// <summary>
        /// Grants a permission to a user given the model.
        /// </summary>
        /// <param name="grantedPermission">The model containing the granted permission.</param>
        /// <returns>The task.</returns>
        public async Task GrantPermissionAsync(IGrantedPermissionBindingModel grantedPermission)
        {
            var grantorUserId = GetGrantorPrincipalId();
            var permission = grantedPermission.ToGrantedPermission(grantorUserId);
            throwIfGrantorAndGranteePrincipalIdEqual(permission.GranteePrincipalId, grantorUserId);
            await principalService.GrantPermissionsAsync(permission);
            await ClearUserCacheAsync(permission.GranteePrincipalId);
        }

        /// <summary>
        /// Grants a permission to a user given the model.
        /// </summary>
        /// <param name="revokedPermission">The model containing the revoked permission.</param>
        /// <returns>The task.</returns>
        public async Task RevokePermissionAsync(IRevokedPermissionBindingModel revokedPermission)
        {
            var grantorUserId = GetGrantorPrincipalId();
            var permission = revokedPermission.ToRevokedPermission(grantorUserId);
            throwIfGrantorAndGranteePrincipalIdEqual(permission.GranteePrincipalId, grantorUserId);
            await principalService.RevokePermissionAsync(permission);
            await ClearUserCacheAsync(permission.GranteePrincipalId);
        }

        /// <summary>
        /// Grants a permission to a user given the model.
        /// </summary>
        /// <param name="deletedPermission">The model containing the deleted permission.</param>
        /// <returns>The task.</returns>
        public async Task DeletePermissionAsync(IDeletedPermissionBindingModel deletedPermission)
        {
            var grantorUserId = GetGrantorPrincipalId();
            var permission = deletedPermission.ToDeletedPermission(grantorUserId);
            throwIfGrantorAndGranteePrincipalIdEqual(permission.GranteePrincipalId, grantorUserId);
            await principalService.DeletePermissionAsync(permission);
            await ClearUserCacheAsync(permission.GranteePrincipalId);
        }

        /// <summary>
        /// Saves the changes made via this handler.
        /// </summary>
        /// <param name="saveActions">The save actions.</param>
        /// <returns>The underlying context results.</returns>
        public int SaveChanges(IList<ISaveAction> saveActions = null)
        {
            var result = principalService.SaveChanges(saveActions);
            logger.Info("Successfully saved changes to principal service.");
            return result;
        }

        /// <summary>
        /// Saves the changes made via this handler.
        /// </summary>
        /// <param name="saveActions">The save actions.</param>
        /// <returns>The underlying context results.</returns>
        public async Task<int> SaveChangesAsync(IList<ISaveAction> saveActions = null)
        {
            var result = await principalService.SaveChangesAsync(saveActions);
            logger.Info("Successfully saved changes to principal service.");
            return result;
        }

        /// <summary>
        /// Handles the given model from a client via the given controller.
        /// </summary>
        /// <param name="model">The permission model.</param>
        /// <param name="controller">The controller that is handling the client request.</param>
        /// <returns>The result the controller should return.</returns>
        public async Task<IHttpActionResult> HandleGrantedPermissionBindingModelAsync(IGrantedPermissionBindingModel model, ApiController controller)
        {
            if (controller.ModelState.IsValid)
            {
                await GrantPermissionAsync(model);
                await SaveChangesAsync();
                return new OkResult(controller);
            }
            else
            {
                return new InvalidModelStateResult(controller.ModelState, controller);
            }
        }

        /// <summary>
        /// Handles the given model from a client via the given controller.
        /// </summary>
        /// <param name="model">The permission model.</param>
        /// <param name="controller">The controller that is handling the client request.</param>
        /// <returns>The result the controller should return.</returns>
        public async Task<IHttpActionResult> HandleRevokedPermissionBindingModelAsync(IRevokedPermissionBindingModel model, ApiController controller)
        {
            if (controller.ModelState.IsValid)
            {
                await RevokePermissionAsync(model);
                await SaveChangesAsync();
                return new OkResult(controller);
            }
            else
            {
                return new InvalidModelStateResult(controller.ModelState, controller);
            }
        }

        /// <summary>
        /// Handles the given model from a client via the given controller.
        /// </summary>
        /// <param name="model">The permission model.</param>
        /// <param name="controller">The controller that is handling the client request.</param>
        /// <returns>The result the controller should return.</returns>
        public async Task<IHttpActionResult> HandleDeletedPermissionBindingModelAsync(IDeletedPermissionBindingModel model, ApiController controller)
        {
            if (controller.ModelState.IsValid)
            {
                await DeletePermissionAsync(model);
                await SaveChangesAsync();
                return new OkResult(controller);
            }
            else
            {
                return new InvalidModelStateResult(controller.ModelState, controller);
            }
        }
    }
}