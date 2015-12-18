using ECA.Business.Queries.Models.Itineraries;
using ECA.Business.Service.Itineraries;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Sorter;
using ECA.WebApi.Models.Query;
using ECA.WebApi.Security;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace ECA.WebApi.Controllers.Itineraries
{
    /// <summary>
    /// The ItineraryGroupsController is used to manage itinerary groups for itineraries in a project.
    /// </summary>
    [RoutePrefix("api")]
    [Authorize]
    public class ItineraryGroupsController : ApiController
    {
        /// <summary>
        /// The default sorter for itinerary groups.
        /// </summary>
        public static ExpressionSorter<ItineraryGroupDTO> DEFAULT_SORTER = new ExpressionSorter<ItineraryGroupDTO>(x => x.ItineraryGroupName, SortDirection.Ascending);

        private IItineraryGroupService itineraryGroupService;
        private IUserProvider userProvider;

        /// <summary>
        /// Creates a new ItinerariesController with the given itinerary service.
        /// </summary>
        /// <param name="itineraryGroupService">The itinerary group service.</param>
        /// <param name="userProvider">The user provider.</param>
        public ItineraryGroupsController(
            IItineraryGroupService itineraryGroupService,
            IUserProvider userProvider)
        {
            Contract.Requires(itineraryGroupService != null, "The itinerary service must not be null.");
            this.itineraryGroupService = itineraryGroupService;
            this.userProvider = userProvider;
        }

        #region Get
        /// <summary>
        /// Returns a listing of the itinerary groups.
        /// </summary>
        /// <param name="projectId">The project id.</param>
        /// <param name="itineraryId">The itinerary id.</param>
        /// <param name="queryModel">The query model to page, filter, and sort.</param>
        /// <returns>The list of project itineraries.</returns>
        [ResponseType(typeof(List<ItineraryGroupDTO>))]
        [Route("Projects/{projectId:int}/Itinerary/{itineraryId:int}/Groups")]
        public async Task<IHttpActionResult> GetItinerariesByProjectIdAsync(int projectId, int itineraryId, [FromUri]PagingQueryBindingModel<ItineraryGroupDTO> queryModel)
        {
            if (ModelState.IsValid)
            {
                var results = await this.itineraryGroupService.GetItineraryGroupsByItineraryIdAsync(projectId, itineraryId, queryModel.ToQueryableOperator(DEFAULT_SORTER));
                return Ok(results);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        #endregion
    }
}
