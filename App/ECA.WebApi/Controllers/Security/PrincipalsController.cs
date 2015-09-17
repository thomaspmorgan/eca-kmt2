using CAM.Business.Model;
using CAM.Business.Service;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Core.Query;
using ECA.WebApi.Models.Query;
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
        private static ExpressionSorter<ResourceAuthorization> DEFAULT_SORTER = new ExpressionSorter<ResourceAuthorization>(x => x.DisplayName, SortDirection.Ascending);

        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IResourceAuthorizationHandler handler;
        private readonly IResourceService resourceService;


        /// <summary>
        /// Creates a new PrincipalsController given the user provider and principal service.
        /// </summary>
        /// <param name="handler">The resource authorization handler.</param>
        /// <param name="resourceService">The resource service.</param>
        public PrincipalsController(IResourceAuthorizationHandler handler, IResourceService resourceService)
        {
            Contract.Requires(handler != null, "The handler must not be null.");
            Contract.Requires(resourceService != null, "The resource service must not be null.");
            this.handler = handler;
            this.resourceService = resourceService;
        }

        /// <summary>
        /// Grants the given permission to the user.
        /// </summary>
        /// <param name="id">The application id.  KMT is 1.</param>
        /// <param name="model">The permission model.</param>
        /// <returns>An ok result.</returns>
        [Route("Application/{id}/Grant/Permission")]
        [ResponseType(typeof(OkResult))]
        [ResourceAuthorize(CAM.Data.Permission.ADMINISTRATOR_VALUE, CAM.Data.ResourceType.APPLICATION_VALUE)]
        public Task<IHttpActionResult> PostGrantPermissionAsync(int id, PermissionBindingModel model)
        {
            return this.handler.HandleGrantedPermissionBindingModelAsync(model, this);
        }

        /// <summary>
        /// Revokes the given permission to the user.
        /// </summary>
        /// <param name="id">The application id.  KMT is 1.</param>
        /// <param name="model">The permission.</param>
        /// <returns>An ok result.</returns>
        [Route("Application/{id}/Revoke/Permission")]
        [ResponseType(typeof(OkResult))]
        [ResourceAuthorize(CAM.Data.Permission.ADMINISTRATOR_VALUE, CAM.Data.ResourceType.APPLICATION_VALUE)]
        public Task<IHttpActionResult> PostRevokePermissionAsync(int id, PermissionBindingModel model)
        {
            return this.handler.HandleRevokedPermissionBindingModelAsync(model, this);
        }

        /// <summary>
        /// Removes the given permissions to the user.
        /// </summary>
        /// <param name="id">The application id.  KMT is 1.</param>
        /// <param name="model">The permission model.</param>
        /// <returns>An ok result.</returns>
        [Route("Application/{id}/Remove/Permission")]
        [ResponseType(typeof(OkResult))]
        [ResourceAuthorize(CAM.Data.Permission.ADMINISTRATOR_VALUE, CAM.Data.ResourceType.APPLICATION_VALUE)]
        public Task<IHttpActionResult> PostDeletePermissionAsync(int id, PermissionBindingModel model)
        {
            return this.handler.HandleDeletedPermissionBindingModelAsync(model, this);
        }

        /// <summary>
        /// Returns a list of all resource authorizations in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <param name="id">The id of the application id.  KMT is 1.</param>
        /// <returns>The list of authorizations.</returns>
        [Route("Application/{id}/Authorizations")]
        [ResponseType(typeof(PagedQueryResults<ResourceAuthorization>))]
        [ResourceAuthorize(CAM.Data.Permission.ADMINISTRATOR_VALUE, CAM.Data.ResourceType.APPLICATION_VALUE)]
        public async Task<IHttpActionResult> GetResourceAuthorizationsAsync(int id, [FromUri]PagingQueryBindingModel<ResourceAuthorization> queryOperator)
        {
            if (ModelState.IsValid)
            {
                var authorizations = await resourceService.GetResourceAuthorizationsAsync(queryOperator.ToQueryableOperator(DEFAULT_SORTER));
                return Ok(authorizations);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
