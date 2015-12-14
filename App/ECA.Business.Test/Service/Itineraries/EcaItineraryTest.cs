﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Itineraries;

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

            var model = new EcaItinerary(startDate, endDate, name, arrivalLocationId, departureLocationId);
            Assert.AreEqual(startDate, model.StartDate);
            Assert.AreEqual(endDate, model.EndDate);
            Assert.AreEqual(name, model.Name);
            Assert.AreEqual(arrivalLocationId, model.ArrivalLocationId);
            Assert.AreEqual(departureLocationId, model.DepartureLocationId);
        }
    }
}
