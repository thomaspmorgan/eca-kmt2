using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Itineraries;
using ECA.Business.Service;
using System.Collections.Generic;

namespace ECA.Business.Test.Service.Itineraries
{
    [TestClass]
    public class ItineraryParticipantsTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var itineraryId = 1;
            var projectId = 2;
            var participantIds = new List<int> { 1, 1, 2 };
            var user = new User(5);
            var itineraryParticipants = new ItineraryParticipants(user, projectId, itineraryId, participantIds);
            Assert.AreEqual(itineraryId, itineraryParticipants.ItineraryId);
            Assert.AreEqual(projectId, itineraryParticipants.ProjectId);
            Assert.IsTrue(Object.ReferenceEquals(user, itineraryParticipants.Audit.User));
            CollectionAssert.AreEqual(participantIds.Distinct().ToList(), itineraryParticipants.ParticipantIds.ToList());
            Assert.IsInstanceOfType(itineraryParticipants.Audit, typeof(Update));
        }

        [TestMethod]
        public void TestConstructor_NullList()
        {
            var itineraryId = 1;
            var projectId = 2;
            var user = new User(5);
            var itineraryParticipants = new ItineraryParticipants(user, projectId, itineraryId, null);
            Assert.IsNotNull(itineraryParticipants.ParticipantIds);
            Assert.IsInstanceOfType(itineraryParticipants.ParticipantIds, typeof(IEnumerable<int>));
            Assert.AreEqual(0, itineraryParticipants.ParticipantIds.Count());
        }
    }
}
