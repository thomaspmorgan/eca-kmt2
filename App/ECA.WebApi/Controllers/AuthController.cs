using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using ECA.WebApi.Common;

namespace ECA.WebApi.Controllers
{
    public class AuthController : ApiController
    {
        [HttpGet]
        [Route("api/auth/user")]
        public IHttpActionResult GetUser()
        {
            if (AppSettings.PdTrackerOpenNetMode)
            {
                return Ok(CurrentUser.OpenNetUser);
            }
            return Ok(CurrentUser.SignedInUser);
        }

        [HttpGet]
        [Route("api/auth/token")]
        public async Task<HttpResponseMessage> GetToken(string code, string redirect_uri)
        {
            var form = new HttpForm();

            form.Add("code", code);
            form.Add("client_id", AppSettings.AdClientId);
            form.Add("client_secret", AppSettings.AdClientSecret);
            form.Add("resource", AppSettings.AdAudience);
            form.Add("redirect_uri", redirect_uri);
            form.Add("grant_type", "authorization_code");

            var path = string.Format("/{0}/oauth2/token", AppSettings.AdTenantId);
            var request = new HttpRequestMessage(HttpMethod.Post, path);
            request.Content = form.Content;

            var client = new HttpClient();
            client.BaseAddress = new System.Uri("https://login.windows.net");
            return await client.SendAsync(request);
        }

        [HttpGet]
        [Route("api/auth/refresh")]
        public async Task<HttpResponseMessage> GetNewToken(string refresh_token)
        {
            var form = new HttpForm();

            form.Add("refresh_token", refresh_token);
            form.Add("client_id", AppSettings.AdClientId);
            form.Add("client_secret", AppSettings.AdClientSecret);
            form.Add("resource", AppSettings.AdAudience);
            form.Add("grant_type", "refresh_token");

            var path = string.Format("/{0}/oauth2/token", AppSettings.AdTenantId);
            var request = new HttpRequestMessage(HttpMethod.Post, path);
            request.Content = form.Content;

            var client = new HttpClient();
            client.BaseAddress = new System.Uri("https://login.windows.net");
            return await client.SendAsync(request);
        }
    }
}
