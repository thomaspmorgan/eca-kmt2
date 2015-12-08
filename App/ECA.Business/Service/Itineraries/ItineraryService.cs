using ECA.Business.Queries.Itineraries;
using System.Data.Entity;
using ECA.Core.Service;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Itineraries
{
    /// <summary>
    /// An ItineraryService is used to perform crud operations for project itineraries.
    /// </summary>
    public class ItineraryService : EcaService, IItineraryService
    {
        /// <summary>
        /// Creates a new service instance with the given context and save actions.
        /// </summary>
        /// <param name="context">The context to perform crud operations against.</param>
        /// <param name="saveActions">The context save actions.</param>
        public ItineraryService(EcaContext context, List<ISaveAction> saveActions = null)
            : base(context, saveActions)
        {
            Contract.Requires(context != null, "The context must not be null.");
        }

        #region Get

        /// <summary>
        /// Returns the itineraries for the given project by project id.
        /// </summary>
        /// <param name="projectId">The id of the project.</param>
        /// <returns>The itineraries of the project.</returns>
        public Task<List<ItineraryDTO>> GetItinerariesByProjectIdAsync(int projectId)
        {
            var itineraries = ItineraryQueries.CreateGetItinerariesByProjectIdQuery(this.Context, projectId).ToListAsync();
            return itineraries;
        }

        /// <summary>
        /// Returns the itineraries for the given project by project id.
        /// </summary>
        /// <param name="projectId">The id of the project.</param>
        /// <returns>The itineraries of the project.</returns>
        public List<ItineraryDTO> GetItinerariesByProjectId(int projectId)
        {
            var itineraries = ItineraryQueries.CreateGetItinerariesByProjectIdQuery(this.Context, projectId).ToList();
            return itineraries;
        }
        #endregion
    }
}
