using ECA.Business.Models.Itineraries;
using ECA.Business.Queries.Itineraries;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using ECA.Core.Service;
using ECA.Data;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace ECA.Business.Service.Itineraries
{
    /// <summary>
    /// An ItineraryGroupService is a service capable of performing crud operations on itinerary groups with an EcaContext.
    /// </summary>
    public class ItineraryGroupService : EcaService, IItineraryGroupService
    {
        /// <summary>
        /// Creates a new ItineraryGroupService with the given context and save actions.
        /// </summary>
        /// <param name="context">The context to operate against.</param>
        /// <param name="saveActions">The save actions.</param>
        public ItineraryGroupService(EcaContext context, List<ISaveAction> saveActions = null)
            : base(context, saveActions)
        {
            Contract.Requires(context != null, "The context must not be null.");
        }

        #region Get

        /// <summary>
        /// Returns the itinerary groups for the given itinerary by id.
        /// </summary>
        /// <param name="itineraryId">The itinerary id.</param>
        /// <param name="projectId">The project id.  Used for security purposes.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The paged, filtered, sorted itinerary groups.</returns>
        public async Task<PagedQueryResults<ItineraryGroupDTO>> GetItineraryGroupsByItineraryIdAsync(int projectId, int itineraryId, QueryableOperator<ItineraryGroupDTO> queryOperator)
        {
            var results = await ItineraryGroupQueries.CreateGetItineraryGroupDTOByItineraryIdQuery(this.Context, projectId, itineraryId, queryOperator).ToPagedQueryResultsAsync(queryOperator.Start, queryOperator.Limit);
            return results;
        }

        /// <summary>
        /// Returns the itinerary groups for the given itinerary by id.
        /// </summary>
        /// <param name="itineraryId">The itinerary id.</param>
        /// <param name="projectId">The project id.  Used for security purposes.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The paged, filtered, sorted itinerary groups.</returns>
        public PagedQueryResults<ItineraryGroupDTO> GetItineraryGroupsByItineraryId(int projectId, int itineraryId, QueryableOperator<ItineraryGroupDTO> queryOperator)
        {
            var results = ItineraryGroupQueries.CreateGetItineraryGroupDTOByItineraryIdQuery(this.Context, projectId, itineraryId, queryOperator).ToPagedQueryResults(queryOperator.Start, queryOperator.Limit);
            return results;
        }

        #endregion
    }
}
