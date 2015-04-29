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

namespace ECA.WebApi.Controllers
{
    //public class TestBindingModel
    //{
    //    public int ProgramId { get; set; }
    //}

    /// <summary>
    /// The AuthController provide user authentication and authorization details.
    /// </summary>
    public class AuthController : ApiController
    {
        private IUserProvider provider;
        private IPermissionStore<IPermission> permissionStore;
        private IUserService userService;
        private IResourceService resourceService;

        /// <summary>
        /// The AuthController provides user authentication and authorization details.
        /// </summary>
        /// <param name="provider">The user provider.</param>
        /// <param name="permissionStore">The permissions store.</param>
        /// <param name="userService">The user service.</param>
        public AuthController(IUserProvider provider, IPermissionStore<IPermission> permissionStore, IUserService userService, IResourceService resourceService)
        {
            Contract.Requires(provider != null, "The provider must not be null.");
            Contract.Requires(permissionStore != null, "The permission store must not be null.");
            Contract.Requires(userService != null, "The user service must not be null.");
            Contract.Requires(resourceService != null, "The resource service must not be null.");
            this.provider = provider;
            this.permissionStore = permissionStore;
            this.userService = userService;
            this.resourceService = resourceService;
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
                var resourceId = await this.resourceService.GetResourceIdByForeignResourceIdAsync(id, resourceTypeId.Value);
                if (resourceId.HasValue)
                {
                    this.permissionStore.LoadUserPermissions(principalId);
                    (await this.provider.GetPermissionsAsync(user)).Where(x => x.IsAllowed
                        && x.PrincipalId == principalId
                        && x.ResourceId == resourceId.Value)
                        .ToList()
                        .ForEach((p) =>
                        {
                            var permissionName = this.permissionStore.GetPermissionNameById(p.PermissionId);
                            models.Add(new ResourcePermissionViewModel
                            {
                                PermissionName = permissionName,
                                PermissionId = p.PermissionId
                            });
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

        /// <summary>
        /// A simple test method of the user permissions and authorization.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="id">The id.</param>
        /// <returns>An Ok if the user is authorized.</returns>
        //[Authorize]
        //[Route("api/auth/user/{id}")]
        //[ResourceAuthorize(OrganizationType.BRANCH_VALUE, "Program", 1009)]
        //[ResourceAuthorize("EditProgram", "Program", 1009)]
        //[ResourceAuthorize("EditProgram", "Program", "id")]
        //[ResourceAuthorize("EditProgram", "Program")]
        //[ResourceAuthorize("EditProgram", "Program", typeof(TestBindingModel), "model.ProgramId")]//model.ProgramId because we have more than one argument
        //public IHttpActionResult PostTestResourceAuthorizeModelType([FromBody]TestBindingModel model, int id)
        //{
        //    return Ok();
        //}

        //[Authorize]
        //[Route("api/auth/logout/{id}")]
        //[ResourceAuthorize("Admin", "Application", APPLICATION_RESOURCE_ID)]
        //public async Task<IHttpActionResult> PostLogout(Guid id)
        //{
        //    this.provider.Clear(id);
        //    return Ok();
        //}

        //[HttpGet]
        //[Route("api/auth/token")]
        //public async Task<HttpResponseMessage> GetToken(string code, string redirect_uri)
        //{
        //    var form = new HttpForm();

        //    form.Add("code", code);
        //    form.Add("client_id", AppSettings.AdClientId);
        //    form.Add("client_secret", AppSettings.AdClientSecret);
        //    form.Add("resource", AppSettings.AdAudience);
        //    form.Add("redirect_uri", redirect_uri);
        //    form.Add("grant_type", "authorization_code");

        //    var path = string.Format("/{0}/oauth2/token", AppSettings.AdTenantId);
        //    var request = new HttpRequestMessage(HttpMethod.Post, path);
        //    request.Content = form.Content;

        //    var client = new HttpClient();
        //    client.BaseAddress = new System.Uri("https://login.windows.net");
        //    return await client.SendAsync(request);
        //}

        //[HttpGet]
        //[Route("api/auth/refresh")]
        //public async Task<HttpResponseMessage> GetNewToken(string refresh_token)
        //{
        //    var form = new HttpForm();

        //    form.Add("refresh_token", refresh_token);
        //    form.Add("client_id", AppSettings.AdClientId);
        //    form.Add("client_secret", AppSettings.AdClientSecret);
        //    form.Add("resource", AppSettings.AdAudience);
        //    form.Add("grant_type", "refresh_token");

        //    var path = string.Format("/{0}/oauth2/token", AppSettings.AdTenantId);
        //    var request = new HttpRequestMessage(HttpMethod.Post, path);
        //    request.Content = form.Content;

        //    var client = new HttpClient();
        //    client.BaseAddress = new System.Uri("https://login.windows.net");
        //    return await client.SendAsync(request);
        //}
    }
}
