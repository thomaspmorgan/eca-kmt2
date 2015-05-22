using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.WebApi.Models.Security;
using CAM.Data;

namespace ECA.WebApi.Test.Security
{
    [TestClass]
    public class PermissionBindingModelTest
    {
        [TestMethod]
        public void TestToGrantedPermission()
        {
            var model = new PermissionBindingModel();
            model.ForeignResourceId = 1;
            model.PrincipalId = 2;
            model.PermissionId = 3;
            model.ResourceType = ResourceType.Project.Value;
            var userId = 5;
            var grantedPermission = model.ToGrantedPermission(userId);

            Assert.AreEqual(model.ForeignResourceId, grantedPermission.ForeignResourceId);
            Assert.AreEqual(model.PrincipalId, grantedPermission.GranteePrincipalId);
            Assert.AreEqual(model.PermissionId, grantedPermission.PermissionId);
            Assert.AreEqual(model.ResourceType, grantedPermission.ResourceTypeAsString);
            Assert.AreEqual(userId, grantedPermission.Audit.UserId);
        }

        [TestMethod]
        public void TestToRevokedPermission()
        {
            var model = new PermissionBindingModel();
            model.ForeignResourceId = 1;
            model.PrincipalId = 2;
            model.PermissionId = 3;
            model.ResourceType = ResourceType.Project.Value;
            var userId = 5;
            var grantedPermission = model.ToRevokedPermission(userId);

            Assert.AreEqual(model.ForeignResourceId, grantedPermission.ForeignResourceId);
            Assert.AreEqual(model.PrincipalId, grantedPermission.GranteePrincipalId);
            Assert.AreEqual(model.PermissionId, grantedPermission.PermissionId);
            Assert.AreEqual(model.ResourceType, grantedPermission.ResourceTypeAsString);
            Assert.AreEqual(userId, grantedPermission.Audit.UserId);
        }

        [TestMethod]
        public void TestToDeletedPermission()
        {
            var model = new PermissionBindingModel();
            model.ForeignResourceId = 1;
            model.PrincipalId = 2;
            model.PermissionId = 3;
            model.ResourceType = ResourceType.Project.Value;
            var userId = 5;
            var grantedPermission = model.ToDeletedPermission(userId);

            Assert.AreEqual(model.ForeignResourceId, grantedPermission.ForeignResourceId);
            Assert.AreEqual(model.PrincipalId, grantedPermission.GranteePrincipalId);
            Assert.AreEqual(model.PermissionId, grantedPermission.PermissionId);
            Assert.AreEqual(model.ResourceType, grantedPermission.ResourceTypeAsString);
        }
    }
}
