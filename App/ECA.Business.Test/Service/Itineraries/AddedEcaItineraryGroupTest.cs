using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using ECA.Business.Service.Itineraries;
using ECA.Business.Service;

namespace ECA.Business.Test.Service.Itineraries
{
    [TestClass]
    public class AddedEcaItineraryGroupTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var userId = 10;
            var user = new User(userId);
            var projectId = 1;
            var name = "name";
            var participantIds = new List<int> { 1, 1, 2, 2 };
            var itineraryId = 2;
            var model = new AddedEcaItineraryGroup(user, projectId, itineraryId, name, participantIds);
            Assert.AreEqual(1, model.ProjectId);
            Assert.AreEqual(name, model.Name);
            Assert.AreEqual(2, model.ParticipantIds.Count());
            Assert.AreEqual(itineraryId, model.ItineraryId);

            Assert.IsNotNull(model.Audit);
            Assert.IsInstanceOfType(model.Audit, typeof(Create));
            Assert.AreEqual(userId, model.Audit.User.Id);
        }
    }
}
