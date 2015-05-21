using CAM.Business.Queries.Models;
using CAM.Business.Service;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace ECA.WebApi.Controllers.Security
{
    /// <summary>
    /// The ResourcesController is used to handle authorization information on resources in CAM.
    /// </summary>
    [Authorize]
    [RoutePrefix("Resources")]
    public class ResourcesController : ApiController
    {
        private IResourceService resourceService;

        /// <summary>
        /// Creates a new ResourcesController with the resource service.
        /// </summary>
        /// <param name="resourceService">The resource service.</param>
        public ResourcesController(IResourceService resourceService)
        {
            Contract.Requires(resourceService != null, "The resource service must not be null.");
            this.resourceService = resourceService;
        }

        /// <summary>
        /// Returns the permissions for the given resource type.  If the resourceId 
        /// </summary>
        /// <param name="resourceType">The resource type.</param>
        /// <returns>The possible permissions.</returns>
        [Route("Permissions/{resourceType}")]
        [ResponseType(typeof(List<ResourcePermissionDTO>))]
        public async Task<IHttpActionResult> GetPermissionsAsync(string resourceType)
        {
            var dtos = await this.resourceService.GetResourcePermissionsAsync(resourceType, null);
            return Ok(dtos);
        }

        /// <summary>
        /// Returns the permissions for the given resource type.  If the resourceId is given then permissions for that specific
        /// resource will also be retrieved.
        /// </summary>
        /// <param name="resourceType">The resource type.</param>
        /// <param name="foreignResourceId">The foreign resource id.</param>
        /// <returns>The possible permissions.</returns>
        [ResponseType(typeof(List<ResourcePermissionDTO>))]
        [Route("Permissions/{resourceType}/{foreignResourceId}")]
        public async Task<IHttpActionResult> GetPermissionsAsync(string resourceType, int foreignResourceId)
        {
            var dtos = await this.resourceService.GetResourcePermissionsAsync(resourceType, foreignResourceId);
            return Ok(dtos);
        }

        /// <summary>
        /// Returns the resource types currently cataloged in CAM.
        /// </summary>
        /// <returns>The resource types.</returns>
        [ResponseType(typeof(List<ResourceTypeDTO>))]
        [Route("Types")]
        public async Task<IHttpActionResult> GetResourceTypesAsync()
        {
            var dtos = await this.resourceService.GetResourceTypesAsync();
            return Ok(dtos);
        }
    }
}
