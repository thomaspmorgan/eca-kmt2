using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CAM.Data;
using ECA.WebApi.Models.Security;

namespace ECA.WebApi.Test.Models.Security
{
    [TestClass]
    public class RevokedPermissionBindingModelTest
    {

        [TestMethod]
        public void TestToGrantedPermission()
        {
            var model = new RevokedPermissionBindingModel
            {

                ForeignResourceId = 1,
                GranteePrincipalId = 2,
                PermissionId = 3,
                ResourceType = ResourceType.Program.Value
            };
            var userId = 5;
            var entity = model.ToRevokedPermission(userId);
            Assert.AreEqual(model.ForeignResourceId, entity.ForeignResourceId);
            Assert.AreEqual(model.GranteePrincipalId, entity.GranteePrincipalId);
            Assert.AreEqual(model.PermissionId, entity.PermissionId);
            Assert.AreEqual(model.ResourceType, entity.ResourceTypeAsString);
            Assert.AreEqual(userId, entity.Audit.UserId);
        }
    }
}
