using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Projects;
using ECA.Data;
using ECA.Business.Service;

namespace ECA.Business.Test.Service.Projects
{
    [TestClass]
    public class AdditionalOrganizationProjectParticipantTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var user = new User(1);
            var projectId = 10;
            var organizationId = 20;
            var participantTypeId = ParticipantType.Individual.Id;
            var instance = new AdditionalOrganizationProjectParticipant(user, projectId, organizationId, participantTypeId);
            Assert.IsTrue(Object.ReferenceEquals(user, instance.Audit.User));
            Assert.AreEqual(projectId, instance.ProjectId);
            Assert.AreEqual(organizationId, instance.OrganizationId);
            Assert.AreEqual(participantTypeId, instance.ParticipantTypeId);
        }

        [TestMethod]
        public void TestUpdateParticipant()
        {
            var user = new User(1);
            var projectId = 10;
            var organizationId = 20;
            var participantTypeId = ParticipantType.Individual.Id;
            var instance = new AdditionalOrganizationProjectParticipant(user, projectId, organizationId, participantTypeId);

            var participant = new Participant();
            var participantType = new ParticipantType();

            instance.UpdateParticipant(participant, participantType, null, null);
            Assert.AreEqual(participantType.ParticipantTypeId, participant.ParticipantTypeId);
            Assert.IsFalse(participant.PersonId.HasValue);
            Assert.AreEqual(organizationId, participant.OrganizationId);
            Assert.AreEqual(participantTypeId, instance.ParticipantTypeId);
        }
    }
}
