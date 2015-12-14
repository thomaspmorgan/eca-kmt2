using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Itineraries;
using ECA.Business.Service;

namespace ECA.Business.Test.Service.Itineraries
{
    [TestClass]
    public class AddedEcaitineraryTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var startDate = DateTimeOffset.Now.AddDays(-1.0);
            var endDate = DateTimeOffset.Now.AddDays(1.0);
            var userId = 2;
            var arrivalLocationId = 3;
            var departureLocationId = 4;
            var name = "name";
            var projectId = 10;

            var model = new AddedEcaItinerary(new User(userId), projectId, startDate, endDate, name, arrivalLocationId, departureLocationId);
            Assert.AreEqual(projectId, model.ProjectId);
            Assert.IsInstanceOfType(model.Audit, typeof(Create));
            Assert.AreEqual(userId, model.Audit.User.Id);
            Assert.AreEqual(startDate, model.StartDate);
            Assert.AreEqual(endDate, model.EndDate);
            Assert.AreEqual(name, model.Name);
            Assert.AreEqual(arrivalLocationId, model.ArrivalLocationId);
            Assert.AreEqual(departureLocationId, model.DepartureLocationId);
        }
    }
}
