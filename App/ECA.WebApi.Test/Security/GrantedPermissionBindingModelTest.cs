using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.WebApi.Models.Security;
using CAM.Data;

namespace ECA.WebApi.Test.Security
{
    [TestClass]
    public class GrantedPermissionBindingModelTest
    {
        [TestMethod]
        public void TestToGrantedPermission()
        {
            var model = new GrantedPermissionBindingModel
            {
                ForeignResourceId = 1,
                GranteePrincipalId = 2,
                PermissionId = 3,
                ResourceType = ResourceType.Program.Value
            };
            var userId = 5;
            var entity = model.ToGrantedPermission(userId);
            Assert.AreEqual(model.ForeignResourceId, entity.ForeignResourceId);
            Assert.AreEqual(model.GranteePrincipalId, entity.GranteePrincipalId);
            Assert.AreEqual(model.PermissionId, entity.PermissionId);
            Assert.AreEqual(model.ResourceType, entity.ResourceTypeAsString);
            Assert.AreEqual(userId, entity.Audit.UserId);
        }
    }
}
