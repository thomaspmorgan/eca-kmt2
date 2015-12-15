using ECA.Business.Service.Admin;
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

namespace ECA.WebApi.Controllers.Admin
{
    /// <summary>
    /// Controller for data point configurations
    /// </summary>
    [Authorize]
    [RoutePrefix("api/dataPointConfigurations")]
    public class DataPointConfigurationsController : ApiController
    {
        private IDataPointConfigurationService service;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="service">The data point configuration service</param>
        public DataPointConfigurationsController(IDataPointConfigurationService service)
        {
            Contract.Requires(service != null, "The bookmarks service must not be null.");
            this.service = service;
        }

        /// <summary>
        /// Deletes a data point configuration
        /// </summary>
        /// <param name="id">The id to delete</param>
        /// <returns>OkResult</returns>
        [ResponseType(typeof(OkResult))]
        public async Task<IHttpActionResult> DeleteDataPointConfigurationAsync(int id)
        {
            await service.DeleteDataPointConfigurationAsync(id);
            await service.SaveChangesAsync();
            return Ok();
        }
    }
}
