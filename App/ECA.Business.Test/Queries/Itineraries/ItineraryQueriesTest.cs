using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Data;
using ECA.Business.Queries.Itineraries;

namespace ECA.Business.Test.Queries.Itineraries
{
    [TestClass]
    public class ItineraryQueriesTest
    {
        private TestEcaContext context;

        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
        }

        [TestMethod]
        public void TestCreateGetItinerariesQuery_NoItineraries()
        {
            Assert.AreEqual(0, context.Itineraries.Count());
            var results = ItineraryQueries.CreateGetItinerariesQuery(context);
            Assert.AreEqual(0, results.Count());
        }

        [TestMethod]
        public void TestCreateGetItinerariesQuery_CheckProperties()
        {
            var cityLocationType = new LocationType
            {
                LocationTypeId = LocationType.City.Id,
                LocationTypeName = LocationType.City.Value
            };
            var countryLocationType = new LocationType
            {
                LocationTypeId = LocationType.Country.Id,
                LocationTypeName = LocationType.Country.Value
            };
            var arrival = new Location
            {
                LocationId = 1,
                LocationName = "city 1",
                LocationType = cityLocationType,
                LocationTypeId = cityLocationType.LocationTypeId
            };
            var departureDestination = new Location
            {
                LocationId = 2,
                LocationName = "country 1",
                LocationType = countryLocationType,
                LocationTypeId = countryLocationType.LocationTypeId
            };
            var project = new Project
            {
                ProjectId = 1
            };
            var itinerary = new Itinerary
            {
                Arrival = arrival,
                ArrivalLocationId = arrival.LocationId,
                Departure = departureDestination,
                DepartureLocationId = departureDestination.LocationId,
                EndDate = DateTimeOffset.Now.AddDays(1.0),
                ItineraryId = 1,
                Name = "name",
                ProjectId = project.ProjectId,
                Project = project,
                StartDate = DateTimeOffset.Now.AddDays(-10.0),
            };
            itinerary.History.RevisedOn = DateTimeOffset.Now.AddDays(-2.0);
            context.LocationTypes.Add(cityLocationType);
            context.LocationTypes.Add(countryLocationType);
            context.Locations.Add(arrival);
            context.Locations.Add(departureDestination);
            context.Projects.Add(project);
            context.Itineraries.Add(itinerary);

            var results = ItineraryQueries.CreateGetItinerariesQuery(context);
            Assert.AreEqual(1, results.Count());

            var firstResult = results.First();
            Assert.AreEqual(arrival.LocationId, firstResult.ArrivalLocation.Id);
            Assert.AreEqual(departureDestination.LocationId, firstResult.DepartureLocation.Id);
            Assert.AreEqual(project.ProjectId, firstResult.ProjectId);
            Assert.AreEqual(itinerary.History.RevisedOn, firstResult.LastRevisedOn);
            Assert.AreEqual(itinerary.EndDate, firstResult.EndDate);
            Assert.AreEqual(itinerary.ItineraryId, firstResult.Id);
            Assert.AreEqual(itinerary.Name, firstResult.Name);
            Assert.AreEqual(itinerary.StartDate, firstResult.StartDate);
            Assert.AreEqual(0, firstResult.ParticipantsCount);
            Assert.AreEqual(0, firstResult.GroupsCount);
        }

        [TestMethod]
        public void TestCreateGetItinerariesQuery_NoArrivalOrDepartureLocation()
        {   
            var project = new Project
            {
                ProjectId = 1
            };
            var itinerary = new Itinerary
            {
                EndDate = DateTimeOffset.Now.AddDays(1.0),
                ItineraryId = 1,
                Name = "name",
                ProjectId = project.ProjectId,
                Project = project,
                StartDate = DateTimeOffset.Now.AddDays(-10.0),
            };
            itinerary.History.RevisedOn = DateTimeOffset.Now.AddDays(-2.0);
            context.Projects.Add(project);
            context.Itineraries.Add(itinerary);

            var results = ItineraryQueries.CreateGetItinerariesQuery(context);
            Assert.AreEqual(1, results.Count());
            var firstResult = results.First();
        }

        [TestMethod]
        public void TestCreateGetItinerariesByProjectIdQuery()
        {
            var project = new Project
            {
                ProjectId = 1
            };
            var itinerary = new Itinerary
            {
                EndDate = DateTimeOffset.Now.AddDays(1.0),
                ItineraryId = 1,
                Name = "name",
                ProjectId = project.ProjectId,
                Project = project,
                StartDate = DateTimeOffset.Now.AddDays(-10.0),
            };
            itinerary.History.RevisedOn = DateTimeOffset.Now.AddDays(-2.0);
            context.Projects.Add(project);
            context.Itineraries.Add(itinerary);

            var results = ItineraryQueries.CreateGetItinerariesByProjectIdQuery(context, project.ProjectId);
            Assert.AreEqual(1, results.Count());
            var firstResult = results.First();
        }

        [TestMethod]
        public void TestCreateGetItinerariesByProjectIdQuery_ProjectDoesNotExist()
        {
            var project = new Project
            {
                ProjectId = 1
            };
            var itinerary = new Itinerary
            {
                EndDate = DateTimeOffset.Now.AddDays(1.0),
                ItineraryId = 1,
                Name = "name",
                ProjectId = project.ProjectId,
                Project = project,
                StartDate = DateTimeOffset.Now.AddDays(-10.0),
            };
            itinerary.History.RevisedOn = DateTimeOffset.Now.AddDays(-2.0);
            context.Projects.Add(project);
            context.Itineraries.Add(itinerary);

            var results = ItineraryQueries.CreateGetItinerariesByProjectIdQuery(context, project.ProjectId + 1);
            Assert.AreEqual(0, results.Count());
        }
    }
}
