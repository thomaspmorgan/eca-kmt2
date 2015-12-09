using ECA.Business.Queries.Admin;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Queries.Itineraries
{
    public static class ItineraryQueries
    {
        public static IQueryable<ItineraryDTO> CreateGetItinerariesQuery(EcaContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var locationsQuery = LocationQueries.CreateGetLocationsQuery(context);            
            var query = from itinerary in context.Itineraries

                        let hasArrival = itinerary.ArrivalLocationId.HasValue
                        let arrival = locationsQuery.Where(x => x.Id == itinerary.ArrivalLocationId).FirstOrDefault()

                        let hasDeparture = itinerary.DepartureLocationId.HasValue
                        let departure = locationsQuery.Where(x => x.Id == itinerary.DepartureLocationId).FirstOrDefault()

                        select new ItineraryDTO
                        {
                            ArrivalLocation = hasArrival ? arrival : null,
                            DepartureLocation = hasDeparture ? departure : null,
                            EndDate = itinerary.EndDate,
                            GroupsCount = 0,
                            Id = itinerary.ItineraryId,
                            LastRevisedOn = itinerary.History.RevisedOn,
                            Name = itinerary.Name,
                            ParticipantsCount = 0,
                            ProjectId = itinerary.ProjectId,
                            StartDate = itinerary.StartDate
                        };
            return query;
        }

        public static IQueryable<ItineraryDTO> CreateGetItinerariesByProjectIdQuery(EcaContext context, int projectId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            return CreateGetItinerariesQuery(context).Where(x => x.ProjectId == projectId).OrderByDescending(x => x.EndDate);
        }
    }
}
