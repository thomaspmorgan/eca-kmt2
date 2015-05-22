using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.WebApi.Models.Security;
using CAM.Data;
using ECA.WebApi.Models.Projects;

namespace ECA.WebApi.Test.Models.Projects
{
    [TestClass]
    public class CollaboratorBindingModelTest
    {
        [TestMethod]
        public void TestToDeletedPermission()
        {
            var model = new CollaboratorBindingModel();
            model.ProjectId = 1;
            model.PrincipalId = 2;
            model.PermissionId = 3;
            var grantorPrincipalId = 10;

            var instance = model.ToDeletedPermission(grantorPrincipalId);
            Assert.AreEqual(model.ProjectId, instance.ForeignResourceId);
            Assert.AreEqual(model.PrincipalId, instance.GranteePrincipalId);
            Assert.AreEqual(model.PermissionId, instance.PermissionId);
            Assert.AreEqual(ResourceType.Project.Value, instance.ResourceTypeAsString);
        }

        [TestMethod]
        public void TestToGrantedPermission()
        {
            var model = new CollaboratorBindingModel();
            model.ProjectId = 1;
            model.PrincipalId = 2;
            model.PermissionId = 3;
            var userId = 5;
            var entity = model.ToGrantedPermission(userId);
            Assert.AreEqual(model.ProjectId, entity.ForeignResourceId);
            Assert.AreEqual(model.PrincipalId, entity.GranteePrincipalId);
            Assert.AreEqual(model.PermissionId, entity.PermissionId);
            Assert.AreEqual(ResourceType.Project.Value, entity.ResourceTypeAsString);
            Assert.AreEqual(userId, entity.Audit.UserId);
        }

        [TestMethod]
        public void TestToRevokedPermission()
        {
            var model = new CollaboratorBindingModel();
            model.ProjectId = 1;
            model.PrincipalId = 2;
            model.PermissionId = 3;
            var userId = 5;
            var entity = model.ToRevokedPermission(userId);
            Assert.AreEqual(model.ProjectId, entity.ForeignResourceId);
            Assert.AreEqual(model.PrincipalId, entity.GranteePrincipalId);
            Assert.AreEqual(model.PermissionId, entity.PermissionId);
            Assert.AreEqual(ResourceType.Project.Value, entity.ResourceTypeAsString);
            Assert.AreEqual(userId, entity.Audit.UserId);
        }
    }
}
