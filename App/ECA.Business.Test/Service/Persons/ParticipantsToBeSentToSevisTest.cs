using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service;
using System.Collections.Generic;
using ECA.Business.Service.Persons;

namespace ECA.Business.Test.Service.Persons
{
    [TestClass]
    public class ParticipantsToBeSentToSevisTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var user = new User(1);
            var projectId = 2;
            var participantIds = new List<int> { 10, 20 };
            var sevisUsername = "user";
            var sevisOrgId = "org";

            var model = new ParticipantsToBeSentToSevis(user, projectId, participantIds, sevisUsername, sevisOrgId);
            Assert.IsInstanceOfType(model.Audit, typeof(Create));
            Assert.AreEqual(projectId, model.ProjectId);
            Assert.AreEqual(sevisUsername, model.SevisUsername);
            Assert.AreEqual(sevisOrgId, model.SevisOrgId);
            CollectionAssert.AreEqual(participantIds, model.ParticipantIds.ToList());
        }

        [TestMethod]
        public void TestConstructor_NullParticipantIds()
        {
            var user = new User(1);
            var projectId = 2;
            List<int> participantIds = null;
            var sevisUsername = "user";
            var sevisOrgId = "org";

            var model = new ParticipantsToBeSentToSevis(user, projectId, participantIds, sevisUsername, sevisOrgId);
            Assert.IsNotNull(model.ParticipantIds);
            Assert.AreEqual(0, model.ParticipantIds.Count());
        }
    }
}
