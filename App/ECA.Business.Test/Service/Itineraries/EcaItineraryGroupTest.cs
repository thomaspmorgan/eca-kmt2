using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Itineraries;
using System.Collections.Generic;

namespace ECA.Business.Test.Service.Itineraries
{
    [TestClass]
    public class EcaItineraryGroupTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var projectId = 1;
            var name = "name";
            var participantIds = new List<int> { 1, 1, 2, 2 };
            var model = new EcaItineraryGroup(projectId, name, participantIds);
            Assert.AreEqual(1, model.ProjectId);
            Assert.AreEqual(name, model.Name);
            Assert.AreEqual(2, model.ParticipantIds.Count());
            Assert.IsTrue(model.ParticipantIds.Contains(1));
            Assert.IsTrue(model.ParticipantIds.Contains(2));
        }

        [TestMethod]
        public void TestConstructor_NullParticipantIds()
        {
            var projectId = 1;
            var name = "name";
            var model = new EcaItineraryGroup(projectId, name, null);
            Assert.IsNotNull(model.ParticipantIds);
        }
    }
}
