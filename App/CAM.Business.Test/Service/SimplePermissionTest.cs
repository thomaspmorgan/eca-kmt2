using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CAM.Business.Service;
using CAM.Data;

namespace CAM.Business.Test.Service
{
    [TestClass]
    public class SimplePermissionTest
    {
        [TestMethod]
        public void TestToString()
        {
            var permission = new SimplePermission
            {
                ForeignResourceId = 1,
                IsAllowed = true,
                PermissionId = Permission.EditProject.Id,
                PrincipalId = 2,
                ResourceId = 3,
                ResourceTypeId = ResourceType.Project.Id
            };
            var s = permission.ToString();
            Assert.IsNotNull(s);
        }

        [TestMethod]
        public void TestToString_ResourceTypeUnknown()
        {
            var permission = new SimplePermission
            {
                ForeignResourceId = 1,
                IsAllowed = true,
                PermissionId = Permission.EditProject.Id,
                PrincipalId = 2,
                ResourceId = 3,
                ResourceTypeId = 0
            };
            var s = permission.ToString();
            Assert.IsNotNull(s);
        }

        [TestMethod]
        public void TestToString_PermissionIdUnknown()
        {
            var permission = new SimplePermission
            {
                ForeignResourceId = 1,
                IsAllowed = true,
                PermissionId = 0,
                PrincipalId = 2,
                ResourceId = 3,
                ResourceTypeId = ResourceType.Project.Id
            };
            var s = permission.ToString();
            Assert.IsNotNull(s);
        }
    }
}
