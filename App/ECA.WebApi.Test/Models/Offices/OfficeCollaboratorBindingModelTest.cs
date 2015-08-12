using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.WebApi.Models.Security;
using CAM.Data;
using ECA.WebApi.Models.Projects;
using ECA.WebApi.Models.Programs;
using ECA.WebApi.Models.Offices;

namespace ECA.WebApi.Test.Models.Programs
{
    [TestClass]
    public class OfficeCollaboratorBindingModelTest
    {
        [TestMethod]
        public void TestToDeletedPermission()
        {
            var model = new OfficeCollaboratorBindingModel();
            model.OfficeId = 1;
            model.PrincipalId = 2;
            model.PermissionId = 3;
            var grantorPrincipalId = 10;

            var instance = model.ToDeletedPermission(grantorPrincipalId);
            Assert.AreEqual(model.OfficeId, instance.ForeignResourceId);
            Assert.AreEqual(model.PrincipalId, instance.GranteePrincipalId);
            Assert.AreEqual(model.PermissionId, instance.PermissionId);
            Assert.AreEqual(ResourceType.Office.Value, instance.ResourceTypeAsString);
        }

        [TestMethod]
        public void TestToGrantedPermission()
        {
            var model = new OfficeCollaboratorBindingModel();
            model.OfficeId = 1;
            model.PrincipalId = 2;
            model.PermissionId = 3;
            var userId = 5;
            var entity = model.ToGrantedPermission(userId);
            Assert.AreEqual(model.OfficeId, entity.ForeignResourceId);
            Assert.AreEqual(model.PrincipalId, entity.GranteePrincipalId);
            Assert.AreEqual(model.PermissionId, entity.PermissionId);
            Assert.AreEqual(ResourceType.Office.Value, entity.ResourceTypeAsString);
            Assert.AreEqual(userId, entity.Audit.UserId);
        }

        [TestMethod]
        public void TestToRevokedPermission()
        {
            var model = new OfficeCollaboratorBindingModel();
            model.OfficeId = 1;
            model.PrincipalId = 2;
            model.PermissionId = 3;
            var userId = 5;
            var entity = model.ToRevokedPermission(userId);
            Assert.AreEqual(model.OfficeId, entity.ForeignResourceId);
            Assert.AreEqual(model.PrincipalId, entity.GranteePrincipalId);
            Assert.AreEqual(model.PermissionId, entity.PermissionId);
            Assert.AreEqual(ResourceType.Office.Value, entity.ResourceTypeAsString);
            Assert.AreEqual(userId, entity.Audit.UserId);
        }
    }
}
