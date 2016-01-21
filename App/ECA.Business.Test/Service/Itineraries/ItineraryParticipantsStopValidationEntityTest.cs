using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Itineraries;
using System.Collections.Generic;

namespace ECA.Business.Test.Service.Itineraries
{
    [TestClass]
    public class ItineraryStopParticipantsValidationEntityTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var ids = new List<int> { 1, 1, 3 };
            var model = new ItineraryStopParticipantsValidationEntity(ids);
            CollectionAssert.AreEqual(ids.Distinct().ToList(), model.NotAllowedParticipantsByParticipantId.ToList());
        }

        [TestMethod]
        public void TestConstructor_NullNotAllowedParticipantsByParticipantIds()
        {
            List<int> ids = null;
            var model = new ItineraryStopParticipantsValidationEntity(ids);
            Assert.IsNotNull(model.NotAllowedParticipantsByParticipantId);
        }
    }
}
