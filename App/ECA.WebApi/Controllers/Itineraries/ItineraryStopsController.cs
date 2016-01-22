using CAM.Data;
using ECA.Business.Queries.Models.Itineraries;
using ECA.Business.Service.Itineraries;
using ECA.WebApi.Models.Itineraries;
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

namespace ECA.WebApi.Controllers.Itineraries
{
    /// <summary>
    /// The ItineraryStopsController is used to perform crud operations on itinerary stops.
    /// </summary>
    [RoutePrefix("api")]
    [Authorize]
    public class ItineraryStopsController : ApiController
    {
        private IItineraryStopService itineraryStopService;
        private IUserProvider userProvider;

        /// <summary>
        /// Creates a new ItineraryStopsController with the given itinerary stop service.
        /// </summary>
        /// <param name="itineraryStopService">The itinerary stop service.</param>
        /// <param name="userProvider">The user provider.</param>
        public ItineraryStopsController(
            IItineraryStopService itineraryStopService,
            IUserProvider userProvider)
        {
            Contract.Requires(itineraryStopService != null, "The itinerary stop service must not be null.");
            this.itineraryStopService = itineraryStopService;
            this.userProvider = userProvider;
        }

        #region Get
        /// <summary>
        /// Returns the itinerary stops for the itinerary by id and project by id.
        /// </summary>
        /// <param name="projectId">The project id.</param>
        /// <param name="itineraryId">The itinerary id.</param>
        /// <returns>The itinerary stops.</returns>
        [Route("Projects/{projectId:int}/Itinerary/{itineraryId:int}/Stops")]
        [ResponseType(typeof(List<ItineraryStopDTO>))]
        public async Task<IHttpActionResult> GetItineraryStopDTOsAsync(int projectId, int itineraryId)
        {
            var dtos = await this.itineraryStopService.GetItineraryStopsByItineraryIdAsync(projectId, itineraryId);
            return Ok(dtos);
        }
        #endregion

        #region Create

        /// <summary>
        /// Adds the given itinerary stop to the itinerary.
        /// </summary>
        /// <returns>The newly saved itinerary stop.</returns>
        [Route("Projects/{projectId:int}/Itinerary/{itineraryId:int}/Stops")]
        [ResourceAuthorize(Permission.EDIT_PROJECT_VALUE, ResourceType.PROJECT_VALUE, "projectId")]
        public async Task<IHttpActionResult> PostCreateItineraryStopAsync(int projectId, int itineraryId, [FromBody]AddedEcaItineraryStopBindingModel model)
        {
            if (ModelState.IsValid)
            {
                var currentUser = this.userProvider.GetCurrentUser();
                var businessUser = this.userProvider.GetBusinessUser(currentUser);
                var itineraryStop = await this.itineraryStopService.CreateAsync(model.ToAddedEcaItineraryStop(businessUser, itineraryId, projectId));
                await this.itineraryStopService.SaveChangesAsync();
                var dto = await this.itineraryStopService.GetItineraryStopByIdAsync(itineraryStop.ItineraryStopId);
                return Ok(dto);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        #endregion

        #region Update
        /// <summary>
        /// Updates the given itinerary stop into the itinerary.
        /// </summary>
        /// <returns>The updated itinerary stop.</returns>
        [Route("Projects/{projectId:int}/Itinerary/{itineraryId:int}/Stops")]
        [ResourceAuthorize(Permission.EDIT_PROJECT_VALUE, ResourceType.PROJECT_VALUE, "projectId")]
        public async Task<IHttpActionResult> PutUpdateItineraryStopAsync(int projectId, int itineraryId, [FromBody]UpdatedEcaItineraryStopBindingModel model)
        {
            if (ModelState.IsValid)
            {
                var currentUser = this.userProvider.GetCurrentUser();
                var businessUser = this.userProvider.GetBusinessUser(currentUser);
                await this.itineraryStopService.UpdateAsync(model.ToUpdatedEcaItineraryStop(businessUser, projectId));
                await this.itineraryStopService.SaveChangesAsync();
                var dto = await this.itineraryStopService.GetItineraryStopByIdAsync(model.ItineraryStopId);
                return Ok(dto);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        #endregion

        #region Participants
        /// <summary>
        /// Sets the participants on the itinerary stop.
        /// </summary>
        /// <param name="itineraryId">The id of the itinerary.</param>
        /// <param name="projectId">The project id.</param>
        /// <param name="itineraryStopId">The itinerary stop id.</param>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [ResponseType(typeof(OkResult))]
        [Route("Projects/{projectId:int}/Itinerary/{itineraryId:int}/Stop/{itineraryStopId:int}/Participants")]
        [ResourceAuthorize(CAM.Data.Permission.EDIT_PROJECT_VALUE, CAM.Data.ResourceType.PROJECT_VALUE, "projectId")]
        public async Task<IHttpActionResult> PostSetItineraryParticipantsAsync(int itineraryId, int projectId, int itineraryStopId, [FromBody]ItineraryStopParticipantsBindingModel model)
        {
            if (ModelState.IsValid)
            {
                var user = this.userProvider.GetCurrentUser();
                var businessUser = this.userProvider.GetBusinessUser(user);
                await this.itineraryStopService.SetParticipantsAsync(model.ToItineraryStopParticipants(businessUser, projectId, itineraryId, itineraryStopId));
                await this.itineraryStopService.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        #endregion
    }
}
