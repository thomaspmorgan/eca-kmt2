using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Reflection;
using ECA.Core.Generation;

namespace ECA.Data.Test
{
    [TestClass]
    public class ParticipantStatusTest
    {
        [TestMethod]
        public void TestExchangeVisitorValidationParticipantStatuses()
        {
            var expectedParticipantStatuses = new List<StaticLookup>();
            expectedParticipantStatuses.Add(ParticipantStatus.Active);
            expectedParticipantStatuses.Add(ParticipantStatus.Nominee);
            expectedParticipantStatuses.Add(ParticipantStatus.Applicant);
            expectedParticipantStatuses.Add(ParticipantStatus.Alternate);
            expectedParticipantStatuses.Add(ParticipantStatus.Approved);
            expectedParticipantStatuses.Add(ParticipantStatus.Intention);
            expectedParticipantStatuses.Add(ParticipantStatus.Pending);

            var statuses = ParticipantStatus.EXCHANGE_VISITOR_VALIDATION_PARTICIPANT_STATUSES;
            CollectionAssert.AreEqual(expectedParticipantStatuses.OrderBy(x => x.Id).ToList(), statuses.OrderBy(x => x.Id).ToList());
        }
    }
}
