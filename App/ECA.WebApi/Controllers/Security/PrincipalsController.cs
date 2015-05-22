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
        public Task<IHttpActionResult> PostGrantPermissionAsync(PermissionBindingModel model)
        {
            return this.handler.HandleGrantedPermissionBindingModelAsync(model, this);
        }

        /// <summary>
        /// Revokes the given permission to the user.
        /// </summary>
        /// <returns>An ok result.</returns>
        [Route("Revoke/Permission")]
        [ResponseType(typeof(OkResult))]
        public Task<IHttpActionResult> PostRevokePermissionAsync(PermissionBindingModel model)
        {
            return this.handler.HandleRevokedPermissionBindingModelAsync(model, this);
        }

        /// <summary>
        /// Removes the given permissions to the user.
        /// </summary>
        /// <returns>An ok result.</returns>
        [Route("Remove/Permission")]
        [ResponseType(typeof(OkResult))]
        public Task<IHttpActionResult> PostDeletePermissionAsync(PermissionBindingModel model)
        {
            return this.handler.HandleDeletedPermissionBindingModelAsync(model, this);
        }
    }
}
