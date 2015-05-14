using FluentAssertions;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CAM.Business.Model;
using CAM.Data;
using ECA.Core.Exceptions;

namespace CAM.Business.Test.Model
{
    [TestClass]
    public class GrantedPermissionTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var granteePrincipalId = 1;
            var grantorPrincipalId = 2;
            var permissionId = 3;
            var foreignResourceId = 4;
            var resourceType = ResourceType.Program.Value;

            var grantedPermission = new GrantedPermission(granteePrincipalId, permissionId, foreignResourceId, resourceType, grantorPrincipalId);
            Assert.IsNotNull(grantedPermission.Audit);
            Assert.IsTrue(grantedPermission.IsAllowed);
            Assert.AreEqual(granteePrincipalId, grantedPermission.GranteePrincipalId);
            Assert.AreEqual(grantorPrincipalId, grantedPermission.Audit.UserId);
            Assert.AreEqual(permissionId, grantedPermission.PermissionId);
            Assert.AreEqual(foreignResourceId, grantedPermission.ForeignResourceId);
            Assert.AreEqual(resourceType, grantedPermission.ResourceTypeAsString);
            DateTimeOffset.Now.Should().BeCloseTo(grantedPermission.Audit.Date, 2000);
        }

        [TestMethod]
        [ExpectedException(typeof(UnknownStaticLookupException))]
        public void TestConstructor_UnknownResourceType()
        {
            var granteePrincipalId = 1;
            var grantorPrincipalId = 2;
            var permissionId = 3;
            var foreignResourceId = 4;
            var resourceType = "abc";
            var grantedPermission = new GrantedPermission(granteePrincipalId, permissionId, foreignResourceId, resourceType, grantorPrincipalId);
        }

        [TestMethod]
        public void TestGetResourceType()
        {
            var granteePrincipalId = 1;
            var grantorPrincipalId = 2;
            var permissionId = 3;
            var foreignResourceId = 4;
            var resourceType = ResourceType.Program;

            var grantedPermission = new GrantedPermission(granteePrincipalId, permissionId, foreignResourceId, resourceType.Value, grantorPrincipalId);
            Assert.AreEqual(resourceType, grantedPermission.GetResourceType());           
        }
    }
}

