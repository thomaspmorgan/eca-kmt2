using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.WebApi.Models.Security;
using CAM.Data;
using ECA.WebApi.Models.Projects;
using ECA.WebApi.Models.Programs;

namespace ECA.WebApi.Test.Models.Programs
{
    [TestClass]
    public class ProgramCollaboratorBindingModelTest
    {
        [TestMethod]
        public void TestToDeletedPermission()
        {
            var model = new ProgramCollaboratorBindingModel();
            model.ProgramId = 1;
            model.PrincipalId = 2;
            model.PermissionId = 3;
            var grantorPrincipalId = 10;

            var instance = model.ToDeletedPermission(grantorPrincipalId);
            Assert.AreEqual(model.ProgramId, instance.ForeignResourceId);
            Assert.AreEqual(model.PrincipalId, instance.GranteePrincipalId);
            Assert.AreEqual(model.PermissionId, instance.PermissionId);
            Assert.AreEqual(ResourceType.Program.Value, instance.ResourceTypeAsString);
        }

        [TestMethod]
        public void TestToGrantedPermission()
        {
            var model = new ProgramCollaboratorBindingModel();
            model.ProgramId = 1;
            model.PrincipalId = 2;
            model.PermissionId = 3;
            var userId = 5;
            var entity = model.ToGrantedPermission(userId);
            Assert.AreEqual(model.ProgramId, entity.ForeignResourceId);
            Assert.AreEqual(model.PrincipalId, entity.GranteePrincipalId);
            Assert.AreEqual(model.PermissionId, entity.PermissionId);
            Assert.AreEqual(ResourceType.Program.Value, entity.ResourceTypeAsString);
            Assert.AreEqual(userId, entity.Audit.UserId);
        }

        [TestMethod]
        public void TestToRevokedPermission()
        {
            var model = new ProgramCollaboratorBindingModel();
            model.ProgramId = 1;
            model.PrincipalId = 2;
            model.PermissionId = 3;
            var userId = 5;
            var entity = model.ToRevokedPermission(userId);
            Assert.AreEqual(model.ProgramId, entity.ForeignResourceId);
            Assert.AreEqual(model.PrincipalId, entity.GranteePrincipalId);
            Assert.AreEqual(model.PermissionId, entity.PermissionId);
            Assert.AreEqual(ResourceType.Program.Value, entity.ResourceTypeAsString);
            Assert.AreEqual(userId, entity.Audit.UserId);
        }
    }
}
