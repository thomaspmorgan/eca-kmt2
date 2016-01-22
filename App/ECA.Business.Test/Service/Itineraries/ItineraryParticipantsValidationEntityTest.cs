using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Itineraries;
using System.Collections.Generic;

namespace ECA.Business.Test.Service.Itineraries
{
    [TestClass]
    public class ItineraryParticipantsValidationEntityTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var orphanedIds = new List<int>();
            var nonPersonParticipantIds = new List<int>();
            var model = new ItineraryParticipantsValidationEntity(orphanedIds, nonPersonParticipantIds);
            Assert.IsTrue(Object.ReferenceEquals(orphanedIds, model.OrphanedParticipantsByParticipantId));
            Assert.IsTrue(Object.ReferenceEquals(nonPersonParticipantIds, model.NonPersonParticipantsByParticipantId));
        }
    }
}
