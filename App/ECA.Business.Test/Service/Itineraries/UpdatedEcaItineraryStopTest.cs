using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Itineraries;
using ECA.Business.Service;

namespace ECA.Business.Test.Service.Itineraries
{
    [TestClass]
    public class UpdatedEcaItineraryStopTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var user = new User(1);
            var projectId = 1;
            var name = "name";
            var arrivalDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var departureDate = DateTimeOffset.UtcNow.AddDays(1.0);
            var destinationLocationId = 2;
            var timezoneId = "timezone";
            var itineraryStopId = 3;

            var instance = new UpdatedEcaItineraryStop(user, itineraryStopId, projectId, name, arrivalDate, departureDate, destinationLocationId, timezoneId);
            Assert.AreEqual(projectId, instance.ProjectId);
            Assert.AreEqual(name, instance.Name);
            Assert.AreEqual(arrivalDate, instance.ArrivalDate);
            Assert.AreEqual(departureDate, instance.DepartureDate);
            Assert.AreEqual(timezoneId, instance.TimezoneId);
            Assert.IsTrue(Object.ReferenceEquals(user, instance.Audit.User));
            Assert.AreEqual(itineraryStopId, instance.ItineraryStopId);
        }
    }
}
