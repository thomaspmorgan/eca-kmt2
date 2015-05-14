using CAM.Business.Service;
using ECA.WebApi.Models.Security;
using ECA.WebApi.Security;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Results;

namespace ECA.WebApi.Controllers.Security
{
    /// <summary>
    /// The PrincipalsController provides methods for interacting with principals and permissions, roles, etc.
    /// </summary>
    [Authorize]
    [RoutePrefix("api/Principals")]
    public class PrincipalsController : ApiController
    {
        private IPrincipalService principalService;
        private IUserProvider userProvider;

        /// <summary>
        /// Creates a new PrincipalsController given the user provider and principal service.
        /// </summary>
        /// <param name="userProvider">The user service.</param>
        /// <param name="principalService">The principal service.</param>
        public PrincipalsController(IUserProvider userProvider, IPrincipalService principalService)
        {
            Contract.Requires(principalService != null, "The principal service must not be null.");
            Contract.Requires(userProvider != null, "The user provider must not be null.");
            this.principalService = principalService;
            this.userProvider = userProvider;
        }

        /// <summary>
        /// Grants the given permission to the user.
        /// </summary>
        /// <returns>An ok result.</returns>
        [Route("Grant/Permission")]
        [ResponseType(typeof(OkResult))]
        public Task<IHttpActionResult> PostGrantPermissionAsync(GrantedPermissionBindingModel model)
        {
            return PostGrantPermissionsAsync(new List<GrantedPermissionBindingModel> { model });
        }

        /// <summary>
        /// Grants the given permissions to the user.
        /// </summary>
        /// <returns>An ok result.</returns>
        [Route("Grant/Permissions")]
        [ResponseType(typeof(OkResult))]
        public async Task<IHttpActionResult> PostGrantPermissionsAsync(List<GrantedPermissionBindingModel> models)
        {
            if (ModelState.IsValid)
            {
                var currentUser = userProvider.GetCurrentUser();
                var user = userProvider.GetBusinessUser(currentUser);
                foreach(var model in models)
                {
                    await principalService.GrantPermissionsAsync(model.ToGrantedPermission(user.Id));
                }
                await principalService.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Grants the given permission to the user.
        /// </summary>
        /// <returns>An ok result.</returns>
        [Route("Revoke/Permission")]
        [ResponseType(typeof(OkResult))]
        public Task<IHttpActionResult> PostRevokePermissionAsync(RevokedPermissionBindingModel model)
        {
            return PostRevokePermissionsAsync(new List<RevokedPermissionBindingModel> { model });
        }

        /// <summary>
        /// Grants the given permissions to the user.
        /// </summary>
        /// <returns>An ok result.</returns>
        [Route("Revoke/Permissions")]
        [ResponseType(typeof(OkResult))]
        public async Task<IHttpActionResult> PostRevokePermissionsAsync(List<RevokedPermissionBindingModel> models)
        {
            if (ModelState.IsValid)
            {
                var currentUser = userProvider.GetCurrentUser();
                var user = userProvider.GetBusinessUser(currentUser);
                foreach (var model in models)
                {
                    await principalService.RevokePermissionAsync(model.ToRevokedPermission(user.Id));
                }
                await principalService.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
