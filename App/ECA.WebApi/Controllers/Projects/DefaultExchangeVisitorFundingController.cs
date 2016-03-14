using ECA.Business.Queries.Models.Admin;
using ECA.Business.Service.Projects;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace ECA.WebApi.Controllers.Projects
{
    /// <summary>
    /// Controller for default exchange visitor funding 
    /// </summary>
    [RoutePrefix("api")]
    [Authorize]
    public class DefaultExchangeVisitorFundingController : ApiController
    {

        private IDefaultExchangeVisitorFundingService service;

        public DefaultExchangeVisitorFundingController(IDefaultExchangeVisitorFundingService service)
        {
            Contract.Requires(service != null, "The default exchange visitor funding service must not be null.");
            this.service = service;
        }

        /// <summary>
        /// Gets a default exchange visitor funding record
        /// </summary>
        /// <param name="projectId">The project id to lookup</param>
        /// <returns>Funding record for exchange visitor</returns>
        [ResponseType(typeof(DefaultExchangeVisitorFundingDTO))]
        [Route("Project/{projectId:int}/DefaultExchangeVisitorFunding")]
        public async Task<IHttpActionResult> GetDefaultExchangeVisitorFundingByIdAsync(int projectId)
        {
            var dto = await service.GetDefaultExchangeVisitorFundingAsync(projectId);
            if (dto != null)
            {
                return Ok(dto);
            } else
            {
                return NotFound();
            }
        }
    }
}
