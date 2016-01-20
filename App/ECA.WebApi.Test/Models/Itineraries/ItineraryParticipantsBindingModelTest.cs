using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service;
using ECA.WebApi.Models.Itineraries;
using System.Collections.Generic;

namespace ECA.WebApi.Test.Models.Itineraries
{
    [TestClass]
    public class ItineraryParticipantsBindingModelTest
    {
        [TestMethod]
        public void TestToItineraryParticipants()
        {
            var user = new User(1);
            var projectId = 1;
            var itineraryId = 2;
            var participantIds = new List<int> { 1, 2 };

            var model = new ItineraryParticipantsBindingModel();
            model.ParticipantIds = participantIds;
            var instance = model.ToItineraryParticipants(user, projectId, itineraryId);

            Assert.AreEqual(projectId, instance.ProjectId);
            Assert.AreEqual(itineraryId, instance.ItineraryId);
            CollectionAssert.AreEqual(instance.ParticipantIds.ToList(), participantIds.ToList());
        }
    }
}
