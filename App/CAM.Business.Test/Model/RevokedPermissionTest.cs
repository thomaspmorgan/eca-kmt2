using FluentAssertions;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CAM.Business.Model;
using CAM.Data;
using ECA.Core.Exceptions;

namespace CAM.Business.Test.Model
{
    [TestClass]
    public class RevokedPermissionTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var granteePrincipalId = 1;
            var grantorPrincipalId = 2;
            var permissionId = 3;
            var foreignResourceId = 4;
            var resourceType = ResourceType.Program.Value;

            var grantedPermission = new RevokedPermission(granteePrincipalId, permissionId, foreignResourceId, resourceType, grantorPrincipalId);
            Assert.IsNotNull(grantedPermission.Audit);
            Assert.IsFalse(grantedPermission.IsAllowed);
            Assert.AreEqual(granteePrincipalId, grantedPermission.GranteePrincipalId);
            Assert.AreEqual(grantorPrincipalId, grantedPermission.Audit.UserId);
            Assert.AreEqual(permissionId, grantedPermission.PermissionId);
            Assert.AreEqual(foreignResourceId, grantedPermission.ForeignResourceId);
            Assert.AreEqual(resourceType, grantedPermission.ResourceTypeAsString);
            DateTimeOffset.Now.Should().BeCloseTo(grantedPermission.Audit.Date, 2000);
        }
    }
}

