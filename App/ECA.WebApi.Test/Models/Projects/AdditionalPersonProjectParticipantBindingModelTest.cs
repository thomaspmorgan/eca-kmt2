using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.WebApi.Models.Projects;
using ECA.Business.Service;

namespace ECA.WebApi.Test.Models.Projects
{
    [TestClass]
    public class AdditionalPersonProjectParticipantBindingModelTest
    {
        [TestMethod]
        public void TestToAdditionalPersonProjectParticipant()
        {
            var model = new AdditionalPersonProjectParticipantBindingModel();
            model.PersonId = 1;
            model.ProjectId = 2;
            var user = new User(10);
            var businessEntity = model.ToAdditionalPersonProjectParticipant(user);
            Assert.AreEqual(model.PersonId, businessEntity.PersonId);
            Assert.AreEqual(model.ProjectId, businessEntity.ProjectId);
            Assert.IsTrue(Object.ReferenceEquals(user, businessEntity.Audit.User));
        }
    }
}
