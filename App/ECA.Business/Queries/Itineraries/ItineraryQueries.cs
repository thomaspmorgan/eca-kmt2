using ECA.Business.Queries.Admin;
using ECA.Business.Queries.Models.Itineraries;
using ECA.Data;
using System.Diagnostics.Contracts;
using System.Linq;

namespace ECA.Business.Queries.Itineraries
{
    /// <summary>
    /// The ItineraryQueries class provides linq queries for an eca context to query itinerary related data.
    /// </summary>
    public static class ItineraryQueries
    {
        /// <summary>
        /// Returns a query to get ItineraryDTOs from the given context.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <returns>The query to get itineraries from the given context.</returns>
        public static IQueryable<ItineraryDTO> CreateGetItinerariesQuery(EcaContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var locationsQuery = LocationQueries.CreateGetLocationsQuery(context);            
            var query = from itinerary in context.Itineraries

                        let hasArrival = itinerary.ArrivalLocationId.HasValue
                        let arrival = locationsQuery.Where(x => x.Id == itinerary.ArrivalLocationId).FirstOrDefault()

                        let hasDeparture = itinerary.DepartureLocationId.HasValue
                        let departure = locationsQuery.Where(x => x.Id == itinerary.DepartureLocationId).FirstOrDefault()

                        let groups = itinerary.ItineraryGroups
                        let groupsCount = groups.Count()

                        let groupParticipants = groups.SelectMany(x => x.Participants)
                        let itineraryStopPartipants = itinerary.Stops.SelectMany(x => x.Participants)
                        let distinctParticipants = groupParticipants.Union(itineraryStopPartipants).Distinct()

                        let participantsCount = distinctParticipants.Count()

                        select new ItineraryDTO
                        {
                            ArrivalLocation = hasArrival ? arrival : null,
                            DepartureLocation = hasDeparture ? departure : null,
                            EndDate = itinerary.EndDate,
                            GroupsCount = groupsCount,
                            Id = itinerary.ItineraryId,
                            LastRevisedOn = itinerary.History.RevisedOn,
                            Name = itinerary.Name,
                            ParticipantsCount = participantsCount,
                            ProjectId = itinerary.ProjectId,
                            StartDate = itinerary.StartDate
                        };
            return query;
        }

        /// <summary>
        /// Returns a query to get ItineraryDTOs from the given context and project with the given id.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="projectId">The id of the project to get itineraries for.</param>
        /// <returns>The query to get itineraries from the given context with the project id.</returns>
        public static IQueryable<ItineraryDTO> CreateGetItinerariesByProjectIdQuery(EcaContext context, int projectId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            return CreateGetItinerariesQuery(context).Where(x => x.ProjectId == projectId).OrderByDescending(x => x.EndDate);
        }
    }
}
