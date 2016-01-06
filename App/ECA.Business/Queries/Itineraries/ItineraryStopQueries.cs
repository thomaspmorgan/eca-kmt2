using ECA.Business.Queries.Admin;
using ECA.Business.Queries.Models.Admin;
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
    /// <summary>
    /// ItineraryStopQueries contains queries for retrieving itinerary stops from the eca context.
    /// </summary>
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

                        let groupParticipants = itineraryStop.Groups.SelectMany(x => x.Participants)
                        let directParticipants = itineraryStop.Participants
                        let distinctParticipants = groupParticipants.Union(directParticipants).Distinct()
                        let participantsCount = distinctParticipants.Count()

                        let hasDestination = itineraryStop.DestinationId.HasValue
                        let destination = hasDestination ? locationQuery.Where(x => x.Id == itineraryStop.DestinationId.Value).FirstOrDefault() : null

                        select new ItineraryStopDTO
                        {
                            ArrivalDate = itineraryStop.DateArrive,
                            DepartureDate = itineraryStop.DateLeave,
                            DestinationLocation = destination,
                            Groups = itineraryStop.Groups.Select(x => new ItineraryStopGroupDTO
                            {
                                ItineraryGroupId = x.ItineraryGroupId,
                                Name = x.Name,
                                Participants = x.Participants.Where(p => p.PersonId.HasValue).Select(p => new ItineraryStopParticipantDTO
                                {
                                    FullName = p.Person.FullName,
                                    ItineraryInformationId = -1,
                                    ItineraryStopId = itineraryStop.ItineraryStopId,
                                    ParticipantId = p.ParticipantId,
                                    PersonId = p.Person.PersonId,
                                    //TravelingFrom = null
                                }).OrderBy(p => p.FullName)
                            }).OrderBy(g => g.Name),
                            Participants = itineraryStop.Participants.Where(p => p.PersonId.HasValue).Select(p => new ItineraryStopParticipantDTO
                            {
                                FullName = p.Person.FullName,
                                ItineraryInformationId = -1,
                                ItineraryStopId = itineraryStop.ItineraryStopId,
                                ParticipantId = p.ParticipantId,
                                PersonId = p.Person.PersonId,
                                //TravelingFrom = null
                            }).OrderBy(p => p.FullName),
                            ItineraryId = itineraryStop.ItineraryId,
                            ItineraryStopId = itineraryStop.ItineraryStopId,
                            LastRevisedOn = itineraryStop.History.RevisedOn,
                            Name = itineraryStop.Name,
                            ParticipantsCount = participantsCount,
                            ProjectId = itineraryStop.Itinerary.ProjectId
                        };
            return query;
        }

        /// <summary>
        /// Returns the itinerary stops for the itinerary with the given id.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="itineraryId">The itinerary id.</param>
        /// <returns>The itinerary stops for the itinerary with the given id.</returns>
        public static IQueryable<ItineraryStopDTO> CreateGetItineraryStopsByItineraryIdQuery(EcaContext context, int itineraryId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            return CreateGetItineraryStopsQuery(context).Where(x => x.ItineraryId == itineraryId);
        }
    }
}
