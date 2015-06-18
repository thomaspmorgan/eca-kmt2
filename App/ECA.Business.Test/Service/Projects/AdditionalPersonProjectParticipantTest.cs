using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service;
using ECA.Business.Service.Projects;
using ECA.Data;

namespace ECA.Business.Test.Service.Projects
{
    [TestClass]
    public class AdditionalPersonProjectParticipantTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var user = new User(1);
            var projectId = 10;
            var personId = 20;
            var participantTypeId = ParticipantType.Individual.Id;

            var instance = new AdditionalPersonProjectParticipant(user, projectId, personId, participantTypeId);
            Assert.IsTrue(Object.ReferenceEquals(user, instance.Audit.User));
            Assert.AreEqual(projectId, instance.ProjectId);
            Assert.AreEqual(personId, instance.PersonId);
            Assert.AreEqual(participantTypeId, instance.ParticipantTypeId);
        }

        [TestMethod]
        public void TestUpdateParticipant()
        {
            var user = new User(1);
            var projectId = 10;
            var personId = 20;
            var participantTypeId = ParticipantType.Individual.Id;
            var instance = new AdditionalPersonProjectParticipant(user, projectId, personId, participantTypeId);

            var participant = new Participant();
            var participantType = new ParticipantType();

            instance.UpdateParticipant(participant, participantType);
            Assert.AreEqual(participantType.ParticipantTypeId, participant.ParticipantTypeId);
            Assert.IsFalse(participant.OrganizationId.HasValue);
            Assert.AreEqual(personId, participant.PersonId);
            Assert.AreEqual(participantTypeId, instance.ParticipantTypeId);
        }
    }
}
