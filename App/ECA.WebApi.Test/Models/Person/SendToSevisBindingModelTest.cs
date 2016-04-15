using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.WebApi.Models.Person;
using ECA.Business.Service;
using System.Collections.Generic;

namespace ECA.WebApi.Test.Models.Person
{
    [TestClass]
    public class SendToSevisBindingModelTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var model = new SendToSevisBindingModel
            {
                ParticipantIds = new List<int> { 1 },
                SevisOrgId = "org",
                SevisUsername = "user"
            };
            var user = new User(100);
            var projectId = 10;
            var instance = model.ToParticipantsToBeSentToSevis(user, projectId);
            Assert.AreEqual(model.SevisOrgId, instance.SevisOrgId);
            Assert.AreEqual(model.SevisUsername, instance.SevisUsername);
            Assert.AreEqual(projectId, instance.ProjectId);
            Assert.IsTrue(Object.ReferenceEquals(user, instance.Audit.User));
            CollectionAssert.AreEqual(model.ParticipantIds.ToList(), instance.ParticipantIds.ToList());
        }
    }
}
