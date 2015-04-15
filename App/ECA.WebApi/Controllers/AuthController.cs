using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using ECA.WebApi.Common;
using ECA.WebApi.Security;
using System.Diagnostics;
using System.Web.Http.Description;
using Swashbuckle.Swagger;
using System.Diagnostics.Contracts;
using ECA.WebApi.Models.Security;
using ECA.Data;

namespace ECA.WebApi.Controllers
{
    public class TestBindingModel
    {
        public int ProgramId { get; set; }
    }

    public class AuthController : ApiController
    {
        public const int APPLICATION_RESOURCE_ID = 7;

        private IUserProvider provider;        

        public AuthController(IUserProvider provider)
        {
            Contract.Requires(provider != null, "The provider must not be null.");
            this.provider = provider;
        }

        [Authorize]
        [Route("api/auth/user/")]
        [ResponseType(typeof(UserViewModel))]
        public async Task<IHttpActionResult> GetUserAsync()
        {
            var currentUser = this.provider.GetCurrentUser();
            var businessUser = this.provider.GetBusinessUser(currentUser);
            var userPermissions = (await this.provider.GetPermissionsAsync(currentUser))
                .ToList()
                .Select(x => new ResourcePermissionViewModel{
                    PermissionId = x.PermissionId,
                    ResourceId = x.ResourceId
                }).ToList();
            var viewModel = new UserViewModel
            {
                PrincipalId = businessUser.Id,
                ResourcePermissions = userPermissions,
                UserId = currentUser.Id,
                UserName = currentUser.GetUsername()
            };
            return Ok(viewModel);
        }

        [Authorize]
        [Route("api/auth/logout/")]
        public async Task<IHttpActionResult> PostLogout()
        {
            var currentUser = this.provider.GetCurrentUser();
            this.provider.Clear(currentUser);
            return Ok();
        }

        [Authorize]
        [Route("api/auth/user/{id}")]
        [ResourceAuthorize("EditProgram", "Program", 1009)]
        [ResourceAuthorize("EditProgram", "Program", "id")]
        [ResourceAuthorize("EditProgram", "Program")]
        [ResourceAuthorize("EditProgram", "Program", typeof(TestBindingModel), "model.ProgramId")]//model.ProgramId because we have more than one argument
        public IHttpActionResult PostTestResourceAuthorizeModelType([FromBody]TestBindingModel model, int id)
        {
            return Ok();
        }

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
