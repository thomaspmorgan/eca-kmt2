using ECA.Business.Queries.Itineraries;
using System.Data.Entity;
using System.Linq;
using ECA.Business.Queries.Models.Itineraries;
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

        /// <summary>
        /// Returns the itinerary groups and participant persons given the project id and itinerary id.
        /// </summary>
        /// <param name="projectId">The id of the project.  Used for security purposes.</param>
        /// <param name="itineraryId">The itinerary id.</param>
        /// <returns>The list of itinerary groups and participant persons per group.</returns>
        public async Task<List<ItineraryGroupParticipantsDTO>> GetItineraryGroupPersonsByItineraryIdAsync(int projectId, int itineraryId)
        {
            var results = await ItineraryGroupQueries.CreateGetItineraryGroupParticipantsQuery(this.Context, projectId, itineraryId).ToListAsync();
            return results;
        }

        /// <summary>
        /// Returns the itinerary groups and participant persons given the project id and itinerary id.
        /// </summary>
        /// <param name="projectId">The id of the project.  Used for security purposes.</param>
        /// <param name="itineraryId">The itinerary id.</param>
        /// <returns>The list of itinerary groups and participant persons per group.</returns>
        public List<ItineraryGroupParticipantsDTO> GetItineraryGroupPersonsByItineraryId(int projectId, int itineraryId)
        {
            var results = ItineraryGroupQueries.CreateGetItineraryGroupParticipantsQuery(this.Context, projectId, itineraryId).ToList();
            return results;
        }

        #endregion
    }
}
