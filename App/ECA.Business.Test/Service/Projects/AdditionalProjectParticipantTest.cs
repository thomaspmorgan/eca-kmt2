using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Projects;
using ECA.Business.Service;
using ECA.Data;
using ECA.Core.Exceptions;

namespace ECA.Business.Test.Service.Projects
{
    public class TestAdditionalProjectParticipant : AdditionalProjectParticipant
    {
        public TestAdditionalProjectParticipant(User projectOwner, int projectId, int participantTypeId)
            : base(projectOwner, projectId, participantTypeId)
        {
            this.UpdateParticipantDetailsCalled = false;
        }

        public bool UpdateParticipantDetailsCalled { get; set; }

        protected override void UpdateParticipantDetails(Data.Participant participant, VisitorType visitorType, DefaultExchangeVisitorFunding defaultExchangeVisitorFunding)
        {
            this.UpdateParticipantDetailsCalled = true;
        }
    }

    [TestClass]
    public class AdditionalProjectParticipantTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var user = new User(1);
            var projectId = 10;
            var participantTypeId = ParticipantType.Individual.Id;
            var instance = new TestAdditionalProjectParticipant(user, projectId, participantTypeId);
            Assert.IsTrue(Object.ReferenceEquals(user, instance.Audit.User));
            Assert.AreEqual(projectId, instance.ProjectId);
            Assert.AreEqual(participantTypeId, instance.ParticipantTypeId);
        }

        [TestMethod]
        [ExpectedException(typeof(UnknownStaticLookupException))]
        public void TestConstructor_UnknownParticipantTypeId()
        {
            var user = new User(1);
            var projectId = 10;
            var participantTypeId = -1;
            var instance = new TestAdditionalProjectParticipant(user, projectId, participantTypeId);
        }

        [TestMethod]
        public void TestUpdateParticipant()
        {
            var user = new User(1);
            var projectId = 10;
            var participantTypeId = ParticipantType.Individual.Id;
            var instance = new TestAdditionalProjectParticipant(user, projectId, participantTypeId);
            var participant = new Participant();
            var participantType = new ParticipantType
            {
                ParticipantTypeId = ParticipantType.ForeignNonTravelingParticipant.Id,
                Name = ParticipantType.ForeignNonTravelingParticipant.Value
            };

            Assert.IsFalse(instance.UpdateParticipantDetailsCalled);
            instance.UpdateParticipant(participant, participantType, null, null);
            Assert.IsTrue(instance.UpdateParticipantDetailsCalled);
            Assert.AreEqual(participantType.ParticipantTypeId, participant.ParticipantTypeId);
            Assert.AreEqual(ParticipantStatus.Active.Id, participant.ParticipantStatusId);
            Assert.AreEqual(projectId, participant.ProjectId);
        }
    }
}
