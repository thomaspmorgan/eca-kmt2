using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Data;
using ECA.Business.Queries.Itineraries;

namespace ECA.Business.Test.Queries.Itineraries
{
    [TestClass]
    public class ItineraryStopQueriesTest
    {
        private TestEcaContext context;

        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
        }

        [TestMethod]
        public void TestCreateGetItineraryStopsQueryTest_CheckProperties()
        {
            var project = new Project
            {
                ProjectId = 1,
            };
            var cityLocationType = new LocationType
            {
                LocationTypeId = LocationType.City.Id,
                LocationTypeName = LocationType.City.Value
            };
            var location = new Location
            {
                LocationId = 1,
                LocationName = "city",
                LocationType = cityLocationType,
                LocationTypeId = cityLocationType.LocationTypeId
            };
            var person1 = new Person
            {
                PersonId = 1,
                FullName = "person 1"
            };
            var participant1 = new Participant
            {
                ParticipantId = 1,
                PersonId = person1.PersonId,
                Person = person1
            };
            var person2 = new Person
            {
                PersonId = 2,
                FullName = "person 2"
            };
            var participant2 = new Participant
            {
                ParticipantId = 2,
                PersonId = person2.PersonId,
                Person = person2
            };
            var itineraryGroup = new ItineraryGroup
            {
                Name = "group1",
            };
            itineraryGroup.Participants.Add(participant1);
            var itinerary = new Itinerary
            {
                ItineraryId = 1,
                Name = "itinerary name",
                StartDate = DateTimeOffset.UtcNow.AddDays(-100.0),
                EndDate = DateTimeOffset.UtcNow.AddDays(100.0),
                ProjectId = project.ProjectId,
                Project = project
            };
            itinerary.ItineraryGroups.Add(itineraryGroup);
            itineraryGroup.Itinerary = itinerary;
            itineraryGroup.ItineraryId = itinerary.ItineraryId;
            var stop = new ItineraryStop
            {
                DateArrive = DateTimeOffset.UtcNow.AddDays(-10.0),
                DateLeave = DateTimeOffset.UtcNow.AddDays(10.0),
                Destination = location,
                DestinationId = location.LocationId,
                Itinerary = itinerary,
                ItineraryId = itinerary.ItineraryId,
                Name = "stop"
            };
            stop.History.RevisedOn = DateTimeOffset.UtcNow;
            stop.Groups.Add(itineraryGroup);
            stop.Participants.Add(participant2);

            context.ItineraryStops.Add(stop);
            context.Locations.Add(location);
            context.LocationTypes.Add(cityLocationType);
            var results = ItineraryStopQueries.CreateGetItineraryStopsQuery(context).ToList();
            Assert.AreEqual(1, results.Count);

            var firstResult = results.First();
            Assert.AreEqual(stop.DateArrive, firstResult.ArrivalDate);
            Assert.AreEqual(stop.DateLeave, firstResult.DepartureDate);
            Assert.AreEqual(itinerary.ItineraryId, firstResult.ItineraryId);
            Assert.AreEqual(stop.ItineraryStopId, firstResult.ItineraryStopId);
            Assert.AreEqual(stop.History.RevisedOn, firstResult.LastRevisedOn);
            Assert.AreEqual(stop.Name, firstResult.Name);
            Assert.AreEqual(2, firstResult.ParticipantsCount);
            Assert.AreEqual(project.ProjectId, firstResult.ProjectId);

            Assert.IsNotNull(firstResult.Destination);
            var destination = firstResult.Destination;
            Assert.AreEqual(location.LocationId, destination.Id);

            //check group participants
            Assert.AreEqual(1, firstResult.Groups.Count());
            var firstGroup = firstResult.Groups.First();
            Assert.AreEqual(itineraryGroup.ItineraryGroupId, firstGroup.ItineraryGroupId);
            Assert.AreEqual(itineraryGroup.Name, firstGroup.Name);
            Assert.AreEqual(1, firstGroup.Participants.Count());

            var groupParticipant = firstGroup.Participants.First();
            Assert.AreEqual(participant1.ParticipantId, groupParticipant.ParticipantId);
            Assert.AreEqual(person1.PersonId, groupParticipant.PersonId);
            Assert.AreEqual(person1.FullName, groupParticipant.FullName);
            Assert.AreEqual(-1, groupParticipant.ItineraryInformationId);
            Assert.IsNull(groupParticipant.TravelingFrom);

            //check participants
            Assert.AreEqual(1, firstResult.Participants.Count());
            var firstParticipant = firstResult.Participants.First();
            Assert.AreEqual(participant2.ParticipantId, firstParticipant.ParticipantId);
            Assert.AreEqual(person2.PersonId, firstParticipant.PersonId);
            Assert.AreEqual(person2.FullName, firstParticipant.FullName);
            Assert.AreEqual(-1, firstParticipant.ItineraryInformationId);
            Assert.IsNull(firstParticipant.TravelingFrom);
        }

        [TestMethod]
        public void TestCreateGetItineraryStopsByItineraryIdQuery()
        {
            var project = new Project
            {
                ProjectId = 1,
            };
            var cityLocationType = new LocationType
            {
                LocationTypeId = LocationType.City.Id,
                LocationTypeName = LocationType.City.Value
            };
            var location = new Location
            {
                LocationId = 1,
                LocationName = "city",
                LocationType = cityLocationType,
                LocationTypeId = cityLocationType.LocationTypeId
            };
            var person1 = new Person
            {
                PersonId = 1,
                FullName = "person 1"
            };
            var participant1 = new Participant
            {
                ParticipantId = 1,
                PersonId = person1.PersonId,
                Person = person1
            };
            var person2 = new Person
            {
                PersonId = 2,
                FullName = "person 2"
            };
            var participant2 = new Participant
            {
                ParticipantId = 2,
                PersonId = person2.PersonId,
                Person = person2
            };
            var itineraryGroup = new ItineraryGroup
            {
                Name = "group1",
            };
            itineraryGroup.Participants.Add(participant1);
            var itinerary = new Itinerary
            {
                ItineraryId = 1,
                Name = "itinerary name",
                StartDate = DateTimeOffset.UtcNow.AddDays(-100.0),
                EndDate = DateTimeOffset.UtcNow.AddDays(100.0),
                ProjectId = project.ProjectId,
                Project = project
            };
            itinerary.ItineraryGroups.Add(itineraryGroup);
            itineraryGroup.Itinerary = itinerary;
            itineraryGroup.ItineraryId = itinerary.ItineraryId;
            var stop = new ItineraryStop
            {
                DateArrive = DateTimeOffset.UtcNow.AddDays(-10.0),
                DateLeave = DateTimeOffset.UtcNow.AddDays(10.0),
                Destination = location,
                DestinationId = location.LocationId,
                Itinerary = itinerary,
                ItineraryId = itinerary.ItineraryId,
                Name = "stop"
            };
            stop.History.RevisedOn = DateTimeOffset.UtcNow;
            stop.Groups.Add(itineraryGroup);
            stop.Participants.Add(participant2);

            context.ItineraryStops.Add(stop);
            context.Locations.Add(location);
            context.LocationTypes.Add(cityLocationType);
            var results = ItineraryStopQueries.CreateGetItineraryStopsByItineraryIdQuery(context, itinerary.ItineraryId).ToList();
            Assert.AreEqual(1, results.Count);
        }

        [TestMethod]
        public void TestCreateGetItineraryStopsByItineraryIdQuery_ItineraryDoesNotExist()
        {
            var project = new Project
            {
                ProjectId = 1,
            };
            var cityLocationType = new LocationType
            {
                LocationTypeId = LocationType.City.Id,
                LocationTypeName = LocationType.City.Value
            };
            var location = new Location
            {
                LocationId = 1,
                LocationName = "city",
                LocationType = cityLocationType,
                LocationTypeId = cityLocationType.LocationTypeId
            };
            var person1 = new Person
            {
                PersonId = 1,
                FullName = "person 1"
            };
            var participant1 = new Participant
            {
                ParticipantId = 1,
                PersonId = person1.PersonId,
                Person = person1
            };
            var person2 = new Person
            {
                PersonId = 2,
                FullName = "person 2"
            };
            var participant2 = new Participant
            {
                ParticipantId = 2,
                PersonId = person2.PersonId,
                Person = person2
            };
            var itineraryGroup = new ItineraryGroup
            {
                Name = "group1",
            };
            itineraryGroup.Participants.Add(participant1);
            var itinerary = new Itinerary
            {
                ItineraryId = 1,
                Name = "itinerary name",
                StartDate = DateTimeOffset.UtcNow.AddDays(-100.0),
                EndDate = DateTimeOffset.UtcNow.AddDays(100.0),
                ProjectId = project.ProjectId,
                Project = project
            };
            itinerary.ItineraryGroups.Add(itineraryGroup);
            itineraryGroup.Itinerary = itinerary;
            itineraryGroup.ItineraryId = itinerary.ItineraryId;
            var stop = new ItineraryStop
            {
                DateArrive = DateTimeOffset.UtcNow.AddDays(-10.0),
                DateLeave = DateTimeOffset.UtcNow.AddDays(10.0),
                Destination = location,
                DestinationId = location.LocationId,
                Itinerary = itinerary,
                ItineraryId = itinerary.ItineraryId,
                Name = "stop"
            };
            stop.History.RevisedOn = DateTimeOffset.UtcNow;
            stop.Groups.Add(itineraryGroup);
            stop.Participants.Add(participant2);

            context.ItineraryStops.Add(stop);
            context.Locations.Add(location);
            context.LocationTypes.Add(cityLocationType);
            var results = ItineraryStopQueries.CreateGetItineraryStopsByItineraryIdQuery(context, itinerary.ItineraryId + 1).ToList();
            Assert.AreEqual(0, results.Count);
        }
    }
}
