using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.WebApi.Security;

namespace ECA.WebApi.Test.Security
{
    [TestClass]
    public class ActionPermissionTest
    {
        private string actionPermissionStringFormat = "{0}:{1}({2})";

        [TestMethod]
        public void TestParse_SingleActionPermission()
        {
            var actionArgument = "id";
            var permissionName = "Read";
            var resourceType = "Program";
            var permissionAsString = String.Format(actionPermissionStringFormat, permissionName, resourceType, actionArgument);

            var actionPermissions = ActionPermission.Parse(permissionAsString);
            Assert.AreEqual(1, actionPermissions.Count());
            var actionPermission = actionPermissions.First();
            Assert.AreEqual(actionArgument, actionPermission.ArgumentName);
            Assert.AreEqual(permissionName, actionPermission.PermissionName);
            Assert.AreEqual(resourceType, actionPermission.ResourceType);
        }

        [TestMethod]
        public void TestParse_TwoActionPermissions()
        {
            var actionArgument1 = "id";
            var permissionName1 = "Read";
            var resourceType1 = "Program";
            var permissionAsString1 = String.Format(actionPermissionStringFormat, permissionName1, resourceType1, actionArgument1);

            var actionArgument2 = "projectId";
            var permissionName2 = "Edit";
            var resourceType2 = "Project";
            var permissionAsString2 = String.Format(actionPermissionStringFormat, permissionName2, resourceType2, actionArgument2);

            var parsedPermissions = ActionPermission.Parse(String.Join(", ", permissionAsString1, permissionAsString2));
            Assert.AreEqual(2, parsedPermissions.Count());

            var parsedPermission1 = parsedPermissions.First();
            Assert.AreEqual(actionArgument1, parsedPermission1.ArgumentName);
            Assert.AreEqual(permissionName1, parsedPermission1.PermissionName);
            Assert.AreEqual(resourceType1, parsedPermission1.ResourceType);

            var parsedPermission2 = parsedPermissions.Last();
            Assert.AreEqual(actionArgument2, parsedPermission2.ArgumentName);
            Assert.AreEqual(permissionName2, parsedPermission2.PermissionName);
            Assert.AreEqual(resourceType2, parsedPermission2.ResourceType);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void TestParse_InvalidFormattedPermission()
        {
            var actionArgument1 = "id";
            var permissionName1 = "Read";
            var resourceType1 = "Program";
            var permissionAsString1 = String.Format("{0},{1}-{2}", permissionName1, resourceType1, actionArgument1);
            ActionPermission.Parse(permissionAsString1).ToList();
        }
    }
}
