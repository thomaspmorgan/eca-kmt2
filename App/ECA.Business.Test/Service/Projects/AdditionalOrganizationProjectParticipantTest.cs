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
            var instance = new AdditionalOrganizationProjectParticipant(user, projectId, organizationId);
            Assert.IsTrue(Object.ReferenceEquals(user, instance.Audit.User));
            Assert.AreEqual(projectId, instance.ProjectId);
            Assert.AreEqual(organizationId, instance.OrganizationId);
        }

        [TestMethod]
        public void TestUpdateParticipant()
        {
            var user = new User(1);
            var projectId = 10;
            var organizationId = 20;
            var instance = new AdditionalOrganizationProjectParticipant(user, projectId, organizationId);

            var participant = new Participant();
            var participantType = new ParticipantType();

            instance.UpdateParticipant(participant, participantType);
            Assert.AreEqual(participantType.ParticipantTypeId, participant.ParticipantTypeId);
            Assert.IsFalse(participant.PersonId.HasValue);
            Assert.AreEqual(organizationId, participant.OrganizationId);
        }
    }
}
