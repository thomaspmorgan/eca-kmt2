using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Itineraries;
using ECA.Business.Service;

namespace ECA.Business.Test.Service.Itineraries
{
    [TestClass]
    public class UpdatedEcaItineraryTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var startDate = DateTimeOffset.Now.AddDays(-1.0);
            var endDate = DateTimeOffset.Now.AddDays(1.0);
            var id = 1;
            var userId = 2;
            var arrivalLocationId = 3;
            var departureLocationId = 4;
            var name = "name";
            var projectId = 5;

            var model = new UpdatedEcaItinerary(id, new User(userId), startDate, endDate, name, projectId, arrivalLocationId, departureLocationId);
            Assert.AreEqual(id, model.Id);
            Assert.IsInstanceOfType(model.Audit, typeof(Update));
            Assert.AreEqual(userId, model.Audit.User.Id);
            Assert.AreEqual(startDate, model.StartDate);
            Assert.AreEqual(endDate, model.EndDate);
            Assert.AreEqual(name, model.Name);
            Assert.AreEqual(arrivalLocationId, model.ArrivalLocationId);
            Assert.AreEqual(departureLocationId, model.DepartureLocationId);
            Assert.AreEqual(projectId, model.ProjectId);
        }
    }
}
