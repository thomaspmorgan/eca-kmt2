using CAM.Business.Model;
using CAM.Business.Queries.Models;
using CAM.Business.Service;
using CAM.Data;
using ECA.Business.Queries.Itineraries;
using ECA.Business.Queries.Models.Admin;
using ECA.Business.Service.Admin;
using ECA.Business.Service.Itineraries;
using ECA.Business.Service.Projects;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Filter;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Core.Query;
using ECA.WebApi.Models.Projects;
using ECA.WebApi.Models.Query;
using ECA.WebApi.Security;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Results;

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
        /// <param name="itineraryService">The project service.</param>
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
        
    }
}
