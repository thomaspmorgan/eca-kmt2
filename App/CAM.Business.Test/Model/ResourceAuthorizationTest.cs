using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CAM.Business.Model;
using CAM.Data;

namespace CAM.Business.Test.Model
{
    [TestClass]
    public class ResourceAuthorizationTest
    {

        [TestMethod]
        public void TestToString()
        {
            var authorization = new ResourceAuthorization
            {
                ForeignResourceId = 1,
                IsAllowed = true,
                PermissionId = Permission.EditProject.Id,
                PrincipalId = 2,
                ResourceId = 3,
                ResourceTypeId = ResourceType.Project.Id
            };
            var s = authorization.ToString();
            Assert.IsNotNull(s);
        }

        [TestMethod]
        public void TestToString_ResourceTypeUnknown()
        {
            var authorization = new ResourceAuthorization
            {
                ForeignResourceId = 1,
                IsAllowed = true,
                PermissionId = Permission.EditProject.Id,
                PrincipalId = 2,
                ResourceId = 3,
                ResourceTypeId = 0
            };
            var s = authorization.ToString();
            Assert.IsNotNull(s);
        }

        [TestMethod]
        public void TestToString_PermissionIdUnknown()
        {
            var authorization = new ResourceAuthorization
            {
                ForeignResourceId = 1,
                IsAllowed = true,
                PermissionId = 0,
                PrincipalId = 2,
                ResourceId = 3,
                ResourceTypeId = ResourceType.Project.Id
            };
            var s = authorization.ToString();
            Assert.IsNotNull(s);
        }
    }
}
