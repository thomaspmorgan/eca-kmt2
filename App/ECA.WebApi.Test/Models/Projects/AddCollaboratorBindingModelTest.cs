using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.WebApi.Models.Projects;
using CAM.Data;

namespace ECA.WebApi.Test.Models.Projects
{
    [TestClass]
    public class AddCollaboratorBindingModelTest
    {
        [TestMethod]
        public void TestToGrantedPermission()
        {
            var grantorId = 1;
            var model = new AddCollaboratorBindingModel();
            model.CollaboratorPrincipalId = 2;
            model.ProjectId = 3;
            var permission = model.ToGrantedPermission(grantorId);
            Assert.AreEqual(grantorId, permission.Audit.UserId);
            Assert.AreEqual(model.CollaboratorPrincipalId, permission.GranteePrincipalId);
            Assert.AreEqual(model.ProjectId, permission.ForeignResourceId);
            Assert.AreEqual(ResourceType.Project.Value, permission.ResourceTypeAsString);
            Assert.AreEqual(true, permission.IsAllowed);
        }
    }
}
