using CAM.Business.Service;
using ECA.WebApi.Models.Security;
using ECA.WebApi.Security;
using NLog;
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
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private IResourceAuthorizationHandler handler;

        /// <summary>
        /// Creates a new PrincipalsController given the user provider and principal service.
        /// </summary>
        /// <param name="handler">The resource authorization handler.</param>
        public PrincipalsController(IResourceAuthorizationHandler handler)
        {
            Contract.Requires(handler != null, "The handler must not be null.");
            this.handler = handler;
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
                foreach(var model in models)
                {
                    await handler.GrantPermissionAsync(model);
                }
                await handler.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Revokes the given permission to the user.
        /// </summary>
        /// <returns>An ok result.</returns>
        [Route("Revoke/Permission")]
        [ResponseType(typeof(OkResult))]
        public Task<IHttpActionResult> PostRevokePermissionAsync(RevokedPermissionBindingModel model)
        {
            return PostRevokePermissionsAsync(new List<RevokedPermissionBindingModel> { model });
        }

        /// <summary>
        /// Revokes the given permissions to the user.
        /// </summary>
        /// <returns>An ok result.</returns>
        [Route("Revoke/Permissions")]
        [ResponseType(typeof(OkResult))]
        public async Task<IHttpActionResult> PostRevokePermissionsAsync(List<RevokedPermissionBindingModel> models)
        {
            if (ModelState.IsValid)
            {
                foreach (var model in models)
                {
                    await handler.RevokePermissionAsync(model);
                }
                await handler.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Removes the given permissions to the user.
        /// </summary>
        /// <returns>An ok result.</returns>
        [Route("Remove/Permission")]
        [ResponseType(typeof(OkResult))]
        public Task<IHttpActionResult> DeletePermissionAsync(DeletedPermissionBindingModel model)
        {
            return DeletePermissionsAsync(new List<DeletedPermissionBindingModel> { model });
        }

        /// <summary>
        /// Removes the given permissions to the user.
        /// </summary>
        /// <returns>An ok result.</returns>
        [Route("Remove/Permissions")]
        [ResponseType(typeof(OkResult))]
        public async Task<IHttpActionResult> DeletePermissionsAsync(List<DeletedPermissionBindingModel> models)
        {
            if (ModelState.IsValid)
            {
                foreach (var model in models)
                {
                    await handler.DeletePermissionAsync(model);
                }
                await handler.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
