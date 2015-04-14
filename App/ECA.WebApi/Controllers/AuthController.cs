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
        private IUserCacheService cacheService;

        public AuthController(IUserProvider provider, IUserCacheService cacheService)
        {
            Contract.Requires(provider != null, "The provider must not be null.");
            Contract.Requires(cacheService != null, "The cache service must not be null.");
            this.provider = provider;
            this.cacheService = cacheService;
        }

        [Route("api/auth/user")]
        [ResponseType(typeof(IWebApiUser))]
        //[ResourceAuthorize("Read:Program(programId), Edit:Project(programId)")]
        //[ResourceAuthorize("Read", "Program", "programId")]
        //[ResourceAuthorize("Read", "Program", 1)]
        //[ResourceAuthorize("Read:Program(programId), Edit:Project(1)")]
        //[ResourceAuthorize("Read", "Program", "model.ProgramId")]
        

        //[ResourceAuthorize("model.OwnerOrganizationId", typeof(ECA.WebApi.Models.Programs.DraftProgramBindingModel), "Edit", "Office")]
        [ResourceAuthorize("Edit", "Program", typeof(AuthBindingModel), "model.Other.OfficeId")]
        //[ResourceAuthorize("Edit:Office(DraftProgramBindingModel#model.OwnerOrganizationId")]
        public async Task<IHttpActionResult> GetUserAsync([FromUri] AuthBindingModel model)
        {
            var user = provider.GetCurrentUser();
            var userCache = await cacheService.GetUserCacheAsync(user);
            return Ok(provider.GetCurrentUser());
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
