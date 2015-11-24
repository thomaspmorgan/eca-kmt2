using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service;
using ECA.Business.Service.Persons;

namespace ECA.Business.Test.Service.Persons
{
    [TestClass]
    public class DeletedParticipantTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var user = new User(1);
            var projectId = 2;
            var participantId = 3;
            var deletedParticipant = new DeletedParticipant(user, projectId, participantId);
            Assert.IsTrue(Object.ReferenceEquals(user, deletedParticipant.Audit.User));
            Assert.AreEqual(projectId, deletedParticipant.ProjectId);
            Assert.AreEqual(participantId, deletedParticipant.ParticipantId);
        }
    }
}
