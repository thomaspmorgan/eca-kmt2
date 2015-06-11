using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.WebApi.Models.Projects;
using ECA.Business.Service;

namespace ECA.WebApi.Test.Models.Projects
{
    [TestClass]
    public class AdditionalOrganizationProjectParticipantBindingModelTest
    {
        [TestMethod]
        public void TestToAdditionalOrganizationProjectParticipant()
        {
            var model = new AdditionalOrganizationProjectPariticipantBindingModel();
            model.OrganizationId = 1;
            model.ProjectId = 2;
            var user = new User(10);
            var businessEntity = model.ToAdditionalOrganizationProjectParticipant(user);
            Assert.AreEqual(model.OrganizationId, businessEntity.OrganizationId);
            Assert.AreEqual(model.ProjectId, businessEntity.ProjectId);
            Assert.IsTrue(Object.ReferenceEquals(user, businessEntity.Audit.User));
        }
    }
}
