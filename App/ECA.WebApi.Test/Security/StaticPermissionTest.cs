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
            permission.ResourceId = value;
            var resourceId = permission.GetResourceId(new Dictionary<string, object>());
            Assert.AreEqual(value, resourceId);
        }
    }
}
