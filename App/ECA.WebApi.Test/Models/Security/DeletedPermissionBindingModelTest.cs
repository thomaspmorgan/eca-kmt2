using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.WebApi.Models.Security;
using CAM.Data;

namespace ECA.WebApi.Test.Models.Security
{
    [TestClass]
    public class DeletedPermissionBindingModelTest
    {
        [TestMethod]
        public void TestToDeletedPermission()
        {
            var model = new DeletedPermissionBindingModel();
            model.ForeignResourceId = 1;
            model.PrincipalId = 2;
            model.PermissionId = 3;
            model.ResourceType = ResourceType.Project.Value;
            var grantorPrincipalId = 10;

            var instance = model.ToDeletedPermission(grantorPrincipalId);
            Assert.AreEqual(model.ForeignResourceId, instance.ForeignResourceId);
            Assert.AreEqual(model.PrincipalId, instance.GranteePrincipalId);
            Assert.AreEqual(model.PermissionId, instance.PermissionId);
            Assert.AreEqual(model.ResourceType, instance.ResourceTypeAsString);
        }
    }
}
