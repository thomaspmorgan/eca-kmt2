using System;
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

namespace ECA.WebApi.Controllers
{
    public class AuthBindingModel
    {
        public int ProgramId { get; set; }
        public OtherBindingModel Other { get; set; }
    }

    public class OtherBindingModel
    {
        public int OfficeId { get; set; }
    }

    [Authorize]
    public class AuthController : ApiController
    {
        private IUserProvider provider;        

        public AuthController(IUserProvider provider)
        {
            Contract.Requires(provider != null, "The provider must not be null.");
            this.provider = provider;
        }

        [Route("api/auth/user/")]
        [ResponseType(typeof(UserViewModel))]
        //[ResourceAuthorize("Read:Program(programId), Edit:Project(programId)")]
        [ResourceAuthorize("EditProgram", "Program", "programId")]
        //[ResourceAuthorize("Read", "Program", 1)]
        //[ResourceAuthorize("Read:Program(programId), Edit:Project(1)")]
        //[ResourceAuthorize("Read", "Program", "model.ProgramId")]
        //[ResourceAuthorize("model.OwnerOrganizationId", typeof(ECA.WebApi.Models.Programs.DraftProgramBindingModel), "Edit", "Office")]
        //[ResourceAuthorize("Edit", "Program", typeof(AuthBindingModel), "model.Other.OfficeId")]
        //[ResourceAuthorize("Edit:Office(DraftProgramBindingModel#model.OwnerOrganizationId")]
        public async Task<IHttpActionResult> GetUserAsync(int programId)
        {
            var currentUser = this.provider.GetCurrentUser();
            var businessUser = this.provider.GetBusinessUser(currentUser);
            var userPermissions = await this.provider.GetPermissionsAsync(currentUser);
            var viewModel = new UserViewModel
            {
                CamPrincipalId = businessUser.Id,
                //ResourcePermissions = userPermissionViewModels,
                UserId = currentUser.Id,
                UserName = currentUser.GetUsername()
            };
            return Ok(viewModel);
        }

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
