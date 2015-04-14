using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.WebApi.Security;
using ECA.Core.Logging;
using System.Collections.Generic;

namespace ECA.WebApi.Test.Security
{
    [TestClass]
    public class UserCacheTest
    {
        [TestMethod]
        public void TestConstructor_NullPermissons()
        {
            var user = new DebugWebApiUser(new TraceLogger());
            var userCache = new UserCache(user);
            Assert.IsTrue(Object.ReferenceEquals(user, userCache.User));
            Assert.IsNotNull(userCache.Permissions);
            Assert.AreEqual(0, userCache.Permissions.Count());
        }

        [TestMethod]
        public void TestConstructor_NonNullPermissions()
        {
            var permissions = new List<ResourcePermission>();
            var user = new DebugWebApiUser(new TraceLogger());
            var userCache = new UserCache(user, permissions);
            Assert.IsTrue(Object.ReferenceEquals(permissions, userCache.Permissions));
        }
    }
}
