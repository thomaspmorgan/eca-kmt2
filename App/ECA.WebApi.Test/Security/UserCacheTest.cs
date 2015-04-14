using System;
using FluentAssertions;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.WebApi.Security;
using ECA.Core.Logging;
using System.Collections.Generic;
using CAM.Business.Service;

namespace ECA.WebApi.Test.Security
{
    [TestClass]
    public class UserCacheTest
    {
        [TestMethod]
        public void TestConstructor_NullPermissons()
        {
            var camId = 1;
            var user = new DebugWebApiUser(new TraceLogger());
            var userCache = new UserCache(user, camId);
            Assert.AreEqual(camId, userCache.CamPrincipalId);
            Assert.AreEqual(user.Id, userCache.UserId);
            Assert.AreEqual(user.GetUsername(), userCache.UserName);
            Assert.IsNotNull(userCache.Permissions);
            Assert.AreEqual(0, userCache.Permissions.Count());
            DateTime.UtcNow.Should().BeCloseTo(userCache.DateCached, 1000);
        }

        [TestMethod]
        public void TestConstructor_NonNullPermissions()
        {
            var camId = 1;
            var permissions = new List<IPermission>();
            var user = new DebugWebApiUser(new TraceLogger());
            var userCache = new UserCache(user, camId, permissions);
            Assert.IsTrue(Object.ReferenceEquals(permissions, userCache.Permissions));
            DateTime.UtcNow.Should().BeCloseTo(userCache.DateCached, 1000);
        }
    }
}
