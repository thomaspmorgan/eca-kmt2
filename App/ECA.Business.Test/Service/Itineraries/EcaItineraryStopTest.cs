using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Itineraries;

namespace ECA.Business.Test.Service.Itineraries
{
    [TestClass]
    public class EcaItineraryStopTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var projectId = 1;
            var name = "name";
            var arrivalDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var departureDate = DateTimeOffset.UtcNow.AddDays(1.0);
            var destinationLocationId = 2;
            var timezoneId = "timezone";

            var instance = new EcaItineraryStop(projectId, name, arrivalDate, departureDate, destinationLocationId, timezoneId);
            Assert.AreEqual(projectId, instance.ProjectId);
            Assert.AreEqual(name, instance.Name);
            Assert.AreEqual(arrivalDate, instance.ArrivalDate);
            Assert.AreEqual(departureDate, instance.DepartureDate);
            Assert.AreEqual(timezoneId, instance.TimezoneId);
        }
    }
}
