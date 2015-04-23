using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.WebApi.Security;
using System.Collections.Generic;

namespace ECA.WebApi.Test.Security
{
    [TestClass]
    public class StaticPermissionTest
    {
        [TestMethod]
        public void TestGetResourceId()
        {
            var value = 1;
            var permission = new StaticPermission();
            permission.ForeignResourceId = value;
            var resourceId = permission.GetResourceId(new Dictionary<string, object>());
            Assert.AreEqual(value, resourceId);
        }

        [TestMethod]
        public void TestToString()
        {
            var value = 1;
            var permission = new StaticPermission();
            permission.PermissionName = "Perm";
            permission.ResourceType = "Prog";
            permission.ForeignResourceId = value;
            var expectedMessage = String.Format("Permission Name:  {0}, Resource Type:  {1},  Foreign Resource Id:  {2}", 
                permission.PermissionName, 
                permission.ResourceType, 
                permission.ForeignResourceId);
            Assert.AreEqual(expectedMessage, permission.ToString());
        }
    }
}
