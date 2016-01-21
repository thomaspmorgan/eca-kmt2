using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.WebApi.Models.Itineraries;
using ECA.Business.Service;

namespace ECA.WebApi.Test.Models.Itineraries
{
    [TestClass]
    public class AddedEcaItineraryStopBindingModelTest
    {
        [TestMethod]
        public void TestToAddedEcaItineraryStop()
        {
            var user = new User(1);
            var itineraryId = 10;
            var projectId = 20;
            var model = new AddedEcaItineraryStopBindingModel();
            model.ArrivalDate = DateTimeOffset.Now.AddDays(1.0);
            model.DepartureDate = DateTimeOffset.Now.AddDays(10.0);
            model.DestinationLocationId = 1;
            model.Name = "name";
            model.TimezoneId = "timezone";

            var instance = model.ToAddedEcaItineraryStop(user, itineraryId, projectId);
            Assert.AreEqual(itineraryId, instance.ItineraryId);
            Assert.AreEqual(projectId, instance.ProjectId);
            Assert.AreEqual(model.ArrivalDate, instance.ArrivalDate);
            Assert.AreEqual(model.DepartureDate, instance.DepartureDate);
            Assert.AreEqual(model.DestinationLocationId, instance.DestinationLocationId);
            Assert.AreEqual(model.Name, instance.Name);
            Assert.AreEqual(model.TimezoneId, instance.TimezoneId);
        }
    }
}
