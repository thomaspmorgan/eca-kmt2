using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Projects;
using ECA.Business.Service;
using ECA.Data;

namespace ECA.Business.Test.Service.Projects
{
    public class TestAdditionalProjectParticipant : AdditionalProjectParticipant
    {
        public TestAdditionalProjectParticipant(User projectOwner, int projectId)
            : base(projectOwner, projectId)
        {
            this.UpdateParticipantDetailsCalled = false;
        }

        public bool UpdateParticipantDetailsCalled { get; set; }

        protected override void UpdateParticipantDetails(Data.Participant participant)
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
            var instance = new TestAdditionalProjectParticipant(user, projectId);
            Assert.IsTrue(Object.ReferenceEquals(user, instance.Audit.User));
            Assert.AreEqual(projectId, instance.ProjectId);
        }

        [TestMethod]
        public void TestUpdateParticipant()
        {
            var user = new User(1);
            var projectId = 10;
            var instance = new TestAdditionalProjectParticipant(user, projectId);
            var participant = new Participant();
            var participantType = new ParticipantType
            {
                ParticipantTypeId = ParticipantType.ForeignEducationalInstitution.Id,
                Name = ParticipantType.ForeignEducationalInstitution.Value
            };

            Assert.IsFalse(instance.UpdateParticipantDetailsCalled);
            instance.UpdateParticipant(participant, participantType);
            Assert.IsTrue(instance.UpdateParticipantDetailsCalled);
            Assert.AreEqual(participantType.ParticipantTypeId, participant.ParticipantTypeId);
            Assert.AreEqual(ParticipantStatus.Active.Id, participant.ParticipantStatusId);
            Assert.Fail("This test needs to check the project.");
        }
    }
}
