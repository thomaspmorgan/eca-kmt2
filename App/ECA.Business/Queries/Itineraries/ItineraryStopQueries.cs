using ECA.Business.Queries.Admin;
using ECA.Business.Queries.Models.Itineraries;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Queries.Itineraries
{
    public static class ItineraryStopQueries
    {
        /// <summary>
        /// Returns a query to retrieve itinerary stops from the given context.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <returns>The query to retrieve itinerary stops.</returns>
        public static IQueryable<ItineraryStopDTO> CreateGetItineraryStopsQuery(EcaContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var locationQuery = LocationQueries.CreateGetLocationsQuery(context);

            var query = from itineraryStop in context.ItineraryStops
                        select new ItineraryStopDTO
                        {
                            ArrivalDate = itineraryStop.DateArrive,
                            DepartureDate = itineraryStop.DateLeave,
                            Destination = locationQuery.Where(x => x.Id == itineraryStop.DestinationId).FirstOrDefault(),
                            Groups = null,
                            ItineraryId = itineraryStop.ItineraryId,
                            ItineraryStopId = itineraryStop.ItineraryStopId,
                            LastRevisedOn = itineraryStop.History.RevisedOn,
                            Name = itineraryStop.Name,
                            ParticipantsCount = 0,
                            ProjectId = itineraryStop.Itinerary.ProjectId
                        };
            return query;
        }
    }
}
