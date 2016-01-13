using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Itineraries;

namespace ECA.Business.Test.Service.Itineraries
{
    [TestClass]
    public class EcaItineraryStopValidationEntityTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var itineraryStartDate = DateTimeOffset.UtcNow.AddDays(-100.0);
            var itineraryEndDate = DateTimeOffset.UtcNow.AddDays(100.0);
            var stopArrivalDate = DateTimeOffset.UtcNow.AddDays(-10.0);
            var stopDepartureDate = DateTimeOffset.UtcNow.AddDays(10.0);
            var timezoneId = "timezone";

            var model = new EcaItineraryStopValidationEntity(itineraryStartDate, itineraryEndDate, stopArrivalDate, stopDepartureDate, timezoneId);
            Assert.AreEqual(itineraryStartDate, model.ItineraryStartDate);
            Assert.AreEqual(itineraryEndDate, model.ItineraryEndDate);
            Assert.AreEqual(stopArrivalDate, model.ItineraryStopArrivalDate);
            Assert.AreEqual(stopDepartureDate, model.ItineraryStopDepartureDate);
            Assert.AreEqual(timezoneId, model.TimezoneId);
        }
    }
}
