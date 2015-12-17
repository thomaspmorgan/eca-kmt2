using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.WebApi.Models.Itineraries;
using ECA.Business.Service;

namespace ECA.WebApi.Test.Models.Itineraries
{
    [TestClass]
    public class UpdateItineraryBindingModelTest
    {
        [TestMethod]
        public void TestToUpdatedEcaItinerary()
        {
            var model = new UpdatedItineraryBindingModel();
            model.ArrivalLocationId = 1;
            model.DepartureLocationId = 2;
            model.EndDate = DateTimeOffset.Now.AddDays(1.0);
            model.Name = "name";
            model.StartDate = DateTimeOffset.Now.AddDays(10.0);
            model.Id = 5;

            var projectId = 10;
            var userId = 20;
            var user = new User(userId);

            var instance = model.ToUpdatedEcaItinerary(projectId, user);
            Assert.IsTrue(Object.ReferenceEquals(user, instance.Audit.User));
            Assert.AreEqual(projectId, instance.ProjectId);
            Assert.AreEqual(model.ArrivalLocationId, instance.ArrivalLocationId);
            Assert.AreEqual(model.DepartureLocationId, instance.DepartureLocationId);
            Assert.AreEqual(model.EndDate, instance.EndDate);
            Assert.AreEqual(model.Name, instance.Name);
            Assert.AreEqual(model.StartDate, instance.StartDate);
            Assert.AreEqual(model.Id, instance.Id);
        }
    }
}
