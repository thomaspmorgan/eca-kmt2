using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service;
using ECA.WebApi.Models.Itineraries;
using System.Collections.Generic;

namespace ECA.WebApi.Test.Models.Itineraries
{
    [TestClass]
    public class ItineraryStopParticipantsBindingModelTest
    {
        [TestMethod]
        public void TestToItineraryStopParticipants()
        {
            var user = new User(1);
            var projectId = 1;
            var itineraryId = 2;
            var itineraryStopId = 3;
            var participantIds = new List<int> { 1, 2 };

            var model = new ItineraryStopParticipantsBindingModel();
            model.ParticipantIds = participantIds;
            var instance = model.ToItineraryStopParticipants(user, projectId, itineraryId, itineraryStopId);

            Assert.AreEqual(projectId, instance.ProjectId);
            Assert.AreEqual(itineraryId, instance.ItineraryId);
            Assert.AreEqual(itineraryStopId, instance.ItineraryStopId);
            CollectionAssert.AreEqual(instance.ParticipantIds.ToList(), participantIds.ToList());
        }
    }
}
