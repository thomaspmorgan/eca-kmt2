using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.WebApi.Models.Itineraries;
using System.Collections.Generic;
using ECA.Business.Service;

namespace ECA.WebApi.Test.Models.Itineraries
{
    [TestClass]
    public class AddedItineryGroupBindingModelTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var model = new AddedItineraryGroupBindingModel();
            model.Name = "name";
            model.ParticipantIds = new List<int> { 1 };
            var projectId = 1;
            var itineraryId = 2;
            var user = new User(1);

            var instance = model.ToAddedEcaItineraryGroup(user, projectId, itineraryId);
            Assert.AreEqual(itineraryId, instance.ItineraryId);
            Assert.AreEqual(projectId, instance.ProjectId);
            Assert.IsTrue(Object.ReferenceEquals(user, instance.Audit.User));
            Assert.AreEqual(model.Name, instance.Name);
            CollectionAssert.AreEqual(model.ParticipantIds.ToList(), instance.ParticipantIds.ToList());
        }
    }
}
