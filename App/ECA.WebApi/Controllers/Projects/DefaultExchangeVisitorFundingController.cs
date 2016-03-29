using CAM.Data;
using ECA.Business.Queries.Models.Admin;
using ECA.Business.Service.Projects;
using ECA.WebApi.Models.Projects;
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
        private IUserProvider userProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="service">The service to set</param>
        /// <param name="userProvider">The user provider to set</param>
        public DefaultExchangeVisitorFundingController(IDefaultExchangeVisitorFundingService service, IUserProvider userProvider)
        {
            Contract.Requires(service != null, "The default exchange visitor funding service must not be null.");
            this.service = service;
            this.userProvider = userProvider;
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

        /// <summary>
        /// Updates default exchange visitor funding
        /// </summary>
        /// <param name="model">The new model</param>
        /// <returns>The saved default exchange visitor funding</returns>
        [Route("Project/{projectId:int}/DefaultExchangeVisitorFunding")]
        [ResourceAuthorize(Permission.EDIT_PROJECT_VALUE, ResourceType.PROJECT_VALUE, typeof(UpdatedDefaultExchangeVisitorFundingBindingModel), "ProjectId")]
        public async Task<IHttpActionResult> PutDefaultExchangeVisitorFunding([FromBody]UpdatedDefaultExchangeVisitorFundingBindingModel model)
        {
            if (ModelState.IsValid)
            {
                var currentUser = userProvider.GetCurrentUser();
                var businessUser = userProvider.GetBusinessUser(currentUser);
                await service.UpdateAsync(model.ToUpdatedDefaultExchangeVisitorFunding(businessUser, model.ProjectId));
                await service.SaveChangesAsync();
                var dto = await service.GetDefaultExchangeVisitorFundingAsync(model.ProjectId);
                return Ok(dto);
            } 
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
