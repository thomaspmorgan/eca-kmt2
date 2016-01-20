using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Itineraries;
using ECA.Business.Service;
using System.Collections.Generic;

namespace ECA.Business.Test.Service.Itineraries
{
    [TestClass]
    public class ItineraryStopParticipantsTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var itineraryId = 1;
            var itineraryStopId = 10;
            var projectId = 2;
            var participantIds = new List<int> { 1, 1, 2 };
            var user = new User(5);
            var itineraryStopParticipants = new ItineraryStopParticipants(user, projectId, itineraryId, itineraryStopId, participantIds);
            Assert.AreEqual(itineraryId, itineraryStopParticipants.ItineraryId);
            Assert.AreEqual(projectId, itineraryStopParticipants.ProjectId);
            Assert.AreEqual(itineraryStopId, itineraryStopParticipants.ItineraryStopId);
            Assert.IsTrue(Object.ReferenceEquals(user, itineraryStopParticipants.Audit.User));
            CollectionAssert.AreEqual(participantIds.Distinct().ToList(), itineraryStopParticipants.ParticipantIds.ToList());
            Assert.IsInstanceOfType(itineraryStopParticipants.Audit, typeof(Update));
        }
    }
}
