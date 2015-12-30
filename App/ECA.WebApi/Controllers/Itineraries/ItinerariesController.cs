using ECA.Business.Queries.Models.Itineraries;
using ECA.Business.Service.Itineraries;
using ECA.WebApi.Models.Itineraries;
using ECA.WebApi.Security;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace ECA.WebApi.Controllers.Itineraries
{
    /// <summary>
    /// The ItinerariesController is used for managing project itineraries in the ECA system.
    /// </summary>
    [RoutePrefix("api")]
    [Authorize]
    public class ItinerariesController : ApiController
    {
        private IItineraryService itineraryService;
        private IUserProvider userProvider;

        /// <summary>
        /// Creates a new ItinerariesController with the given itinerary service.
        /// </summary>
        /// <param name="itineraryService">The itinerary service.</param>
        /// <param name="userProvider">The user provider.</param>
        public ItinerariesController(
            IItineraryService itineraryService,
            IUserProvider userProvider)
        {
            Contract.Requires(itineraryService != null, "The itinerary service must not be null.");
            this.itineraryService = itineraryService;
            this.userProvider = userProvider;
        }

        #region Get
        /// <summary>
        /// Returns a listing of the project itineraries.
        /// </summary>
        /// <param name="id">The project id.</param>
        /// <returns>The list of project itineraries.</returns>
        [ResponseType(typeof(List<ItineraryDTO>))]
        [Route("Projects/{id:int}/Itineraries")]
        public async Task<IHttpActionResult> GetItinerariesByProjectIdAsync(int id)
        {
            var results = await this.itineraryService.GetItinerariesByProjectIdAsync(id);
            return Ok(results);
        }

        #endregion

        #region Create
        /// <summary>
        /// Creates a new itinerary.
        /// </summary>
        /// <param name="id">The id of the project.</param>
        /// <param name="model">The new itinerary.</param>
        /// <returns>The created itinerary.</returns>
        [ResponseType(typeof(ItineraryDTO))]
        [Route("Projects/{id:int}/Itinerary")]
        [ResourceAuthorize(CAM.Data.Permission.EDIT_PROJECT_VALUE, CAM.Data.ResourceType.PROJECT_VALUE)]
        public async Task<IHttpActionResult> PostCreateItineraryAsync([FromUri]int id, [FromBody]AddedItineraryBindingModel model)
        {
            if (ModelState.IsValid)
            {
                var user = this.userProvider.GetCurrentUser();
                var businessUser = this.userProvider.GetBusinessUser(user);
                var itinerary = await this.itineraryService.CreateAsync(model.ToAddedEcaItinerary(id, businessUser));
                await this.itineraryService.SaveChangesAsync();
                var dto = await this.itineraryService.GetItineraryByIdAsync(id, itinerary.ItineraryId);
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
        /// Updates the system's itinerary with the given updated itinerary.
        /// </summary>
        /// <param name="id">The id of the project.</param>
        /// <param name="model">The updated itinerary.</param>
        /// <returns>The updated itinerary.</returns>
        [ResponseType(typeof(ItineraryDTO))]
        [Route("Projects/{id:int}/Itinerary")]
        [ResourceAuthorize(CAM.Data.Permission.EDIT_PROJECT_VALUE, CAM.Data.ResourceType.PROJECT_VALUE)]
        public async Task<IHttpActionResult> PutUpdateItineraryAsync([FromUri]int id, [FromBody]UpdatedItineraryBindingModel model)
        {
            if (ModelState.IsValid)
            {
                var user = this.userProvider.GetCurrentUser();
                var businessUser = this.userProvider.GetBusinessUser(user);
                await this.itineraryService.UpdateAsync(model.ToUpdatedEcaItinerary(id, businessUser));
                await this.itineraryService.SaveChangesAsync();
                var dto = await this.itineraryService.GetItineraryByIdAsync(id, model.Id);
                return Ok(dto);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        #endregion

    }
}
