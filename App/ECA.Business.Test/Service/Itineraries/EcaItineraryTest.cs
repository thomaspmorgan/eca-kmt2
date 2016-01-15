using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Itineraries;
using System.Linq;
using System.Collections.Generic;

namespace ECA.Business.Test.Service.Itineraries
{
    [TestClass]
    public class EcaItineraryTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var startDate = DateTimeOffset.Now.AddDays(-1.0);
            var endDate = DateTimeOffset.Now.AddDays(1.0);
            var name = "name";
            var arrivalLocationId = 3;
            var departureLocationId = 4;
            var projectId = 5;
            var participantIds = new List<int> { 1, 1 };

            var model = new EcaItinerary(startDate, endDate, name, projectId, arrivalLocationId, departureLocationId, participantIds);
            Assert.AreEqual(startDate, model.StartDate);
            Assert.AreEqual(endDate, model.EndDate);
            Assert.AreEqual(name, model.Name);
            Assert.AreEqual(arrivalLocationId, model.ArrivalLocationId);
            Assert.AreEqual(departureLocationId, model.DepartureLocationId);
            Assert.AreEqual(projectId, model.ProjectId);
            CollectionAssert.AreEqual(participantIds.Distinct().ToList(), model.ParticipantIds.ToList());
        }

        [TestMethod]
        public void TestConstructor_NullListOfParticipantIds()
        {
            var startDate = DateTimeOffset.Now.AddDays(-1.0);
            var endDate = DateTimeOffset.Now.AddDays(1.0);
            var name = "name";
            var arrivalLocationId = 3;
            var departureLocationId = 4;
            var projectId = 5;
            List<int> participantIds = null;

            var model = new EcaItinerary(startDate, endDate, name, projectId, arrivalLocationId, departureLocationId, participantIds);
            Assert.IsNotNull(model.ParticipantIds);
            Assert.IsInstanceOfType(model.ParticipantIds, typeof(List<int>));
        }
    }
}
