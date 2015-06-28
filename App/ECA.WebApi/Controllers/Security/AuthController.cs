using System.Linq;
using ECA.WebApi.Models.Security;
using ECA.WebApi.Security;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using CAM.Business.Service;
using System;
using System.Net;

namespace ECA.WebApi.Controllers.Security
{
    /// <summary>
    /// The AuthController provide user authentication and authorization details.
    /// </summary>
    public class AuthController : ApiController
    {
        private IUserProvider provider;
        private IUserService userService;
        private IResourceService resourceService;
        private IPermissionService permissionService;

        /// <summary>
        /// The AuthController provides user authentication and authorization details.
        /// </summary>
        /// <param name="provider">The user provider.</param>
        /// <param name="userService">The user service.</param>
        /// <param name="permissionService">The permission service.</param>
        /// <param name="resourceService">The resource service.</param>
        public AuthController(
            IUserProvider provider, 
            IUserService userService, 
            IResourceService resourceService,
            IPermissionService permissionService)
        {
            Contract.Requires(provider != null, "The provider must not be null.");
            Contract.Requires(userService != null, "The user service must not be null.");
            Contract.Requires(resourceService != null, "The resource service must not be null.");
            Contract.Requires(permissionService != null, "The permission model service must not be null.");
            this.provider = provider;
            this.userService = userService;
            this.resourceService = resourceService;
            this.permissionService = permissionService;
        }

        /// <summary>
        /// Returns basic information about the currently authenticated user.
        /// </summary>
        /// <returns>Information about the currently authenticated user.</returns>
        [Authorize]
        [Route("api/auth/user/")]
        [ResponseType(typeof(UserViewModel))]
        public async Task<IHttpActionResult> GetUserAsync()
        {
            var currentUser = this.provider.GetCurrentUser();
            var camUser = await this.userService.GetUserByIdAsync(currentUser.Id);
            var viewModel = new UserViewModel();
            viewModel.UserId = currentUser.Id;
            viewModel.UserName = currentUser.GetUsername();
            if (camUser != null)
            {
                viewModel.IsRegistered = true;
                viewModel.DisplayName = camUser.DisplayName;
                viewModel.EcaUserId = camUser.PrincipalId;
            }
            else
            {
                viewModel.IsRegistered = false;
            }
            return Ok(viewModel);
        }

        /// <summary>
        /// Returns the user permissions for a resource given the type and id.
        /// </summary>
        /// <param name="type">The resource type e.g. Program or Project, etc.</param>
        /// <param name="id">The id of the resource i.e. ProgramId, ProjectId, etc.</param>
        /// <returns>The permissions granted to the current user for the resouce.</returns>
        [Authorize]
        [Route("api/auth/user/permissions")]
        [ResponseType(typeof(List<ResourcePermissionViewModel>))]
        public async Task<IHttpActionResult> GetUserPermissionsForResourceAsync(string type, int id)
        {
            var currentUser = this.provider.GetCurrentUser();
            return Ok(await GetUserPermissionsAsync(currentUser, type, id));
        }

        /// <summary>
        /// Allows a user with permissions to impersonate another user's premissions.  All other
        /// attributes about the user are still from the impersonator, only permissions are retrieved for
        /// the user.
        /// </summary>
        /// <param name="id">The id of the user to impersonate.</param>
        /// <returns>An Ok result to start impersonating.</returns>
        [Authorize]
        [Route("api/auth/user/impersonate/start")]
        public async Task<IHttpActionResult> PostStartImpersonationAsync(Guid id)
        {
            throw new HttpResponseException(HttpStatusCode.Unauthorized);
            //var currentUser = this.provider.GetCurrentUser();
            //await provider.ImpersonateAsync(currentUser, id);
            //return Ok();
        }

        /// <summary>
        /// Stops all impersonation of the current user.
        /// </summary>
        /// <returns>An Ok result..</returns>
        [Authorize]
        [Route("api/auth/user/impersonate/stop")]
        public IHttpActionResult PostStopImpersonation()
        {
            return this.PostLogout();
        }

        /// <summary>
        /// Returns permissions for the given user resource type and resource id.
        /// </summary>
        /// <param name="user">The user to retrieve permissions for.</param>
        /// <param name="type">The resource type.</param>
        /// <param name="id">The foreign resource id.</param>
        /// <returns>The permissions the user currently has on the resource.</returns>
        [NonAction]
        public async Task<List<ResourcePermissionViewModel>> GetUserPermissionsAsync(IWebApiUser user, string type, int id)
        {
            var principalId = await this.provider.GetPrincipalIdAsync(user);
            var resourceTypeId = this.resourceService.GetResourceTypeId(type);
            var models = new List<ResourcePermissionViewModel>();
            if(resourceTypeId.HasValue)
            {
                var userPermissions = (await this.provider.GetPermissionsAsync(user))
                    .Where(x => x.IsAllowed && x.ForeignResourceId == id && x.ResourceTypeId == resourceTypeId.Value)
                    .ToList();
                foreach (var p in userPermissions)
                {
                    var permissionModel = await this.permissionService.GetPermissionByIdAsync(p.PermissionId);
                    models.Add(new ResourcePermissionViewModel
                    {
                        PermissionName = permissionModel.Name,
                        PermissionId = p.PermissionId
                    });
                }
            }
            return models;
        }

        /// <summary>
        /// Logs the user out of the web api system, not the azure ad system. 
        /// </summary>
        /// <returns>Ok.</returns>
        [Authorize]
        [Route("api/auth/user/logout/")]
        public IHttpActionResult PostLogout()
        {
            var currentUser = this.provider.GetCurrentUser();
            this.provider.Clear(currentUser);
            return Ok();
        }

        /// <summary>
        /// Returns basic information about the currently authenticated user.
        /// </summary>
        /// <returns>Information about the currently authenticated user.</returns>
        [Authorize]
        [Route("api/auth/user/register")]
        public async Task<IHttpActionResult> PostRegisterAsync()
        {
            var currentUser = this.provider.GetCurrentUser();
            var camUser = await this.userService.GetUserByIdAsync(currentUser.Id);
            if (camUser == null)
            {
                this.userService.Create(currentUser.ToAzureUser());
                await this.userService.SaveChangesAsync();
            }
            return Ok();
        }
    }
}
