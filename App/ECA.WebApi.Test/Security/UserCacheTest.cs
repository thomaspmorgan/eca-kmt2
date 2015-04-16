﻿using System;
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
            var principalId = 1;
            var isValidUser = true;
            var camUser = new TestCamUser();
            camUser.PrincipalId = principalId;
            camUser.IsValid = isValidUser;

            var user = new DebugWebApiUser(new TraceLogger());
            var userCache = new UserCache(user, camUser, isValidUser);
            Assert.AreEqual(principalId, userCache.CamPrincipalId);
            Assert.AreEqual(isValidUser, userCache.IsValidCamUser);
            Assert.AreEqual(user.Id, userCache.UserId);
            Assert.AreEqual(user.GetUsername(), userCache.UserName);
            Assert.IsNotNull(userCache.Permissions);
            Assert.AreEqual(0, userCache.Permissions.Count());
            DateTime.UtcNow.Should().BeCloseTo(userCache.DateCached, 1000);
        }

        [TestMethod]
        public void TestConstructor_NonNullPermissions()
        {
            var permissions = new List<IPermission>();
            var user = new DebugWebApiUser(new TraceLogger());
            var userCache = new UserCache(user, new TestCamUser(), true, permissions);
            Assert.IsTrue(Object.ReferenceEquals(permissions, userCache.Permissions));
            DateTime.UtcNow.Should().BeCloseTo(userCache.DateCached, 1000);
        }
    }
}
