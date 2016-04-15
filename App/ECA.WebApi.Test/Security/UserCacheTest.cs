using CAM.Business.Model;
using CAM.Business.Service;
using ECA.WebApi.Security;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ECA.WebApi.Test.Security
{
    [TestClass]
    public class UserCacheTest
    {
        [TestMethod]
        public void TestConstructor_UserHasSevisAccounts()
        {
            var principalId = 1;
            var isValidUser = true;
            var camUser = new User();
            camUser.PrincipalId = principalId;

            var user = new DebugWebApiUser();
            var firstSevisAccount = new SevisUserAccount
            {
                OrgId = "org 1",
                Username = "user 1"
            };
            var secondSevisAccount = new SevisUserAccount
            {
                OrgId = "org 2",
                Username = "user 2"
            };
            var accounts = new List<SevisUserAccount> { firstSevisAccount, secondSevisAccount };
            camUser.SevisUserAccounts = accounts;
            var userCache = new UserCache(user, camUser, isValidUser);
            Assert.AreEqual(2, userCache.SevisUserAccounts.Count());
            Assert.IsTrue(Object.ReferenceEquals(firstSevisAccount, userCache.SevisUserAccounts.First()));
            Assert.IsTrue(Object.ReferenceEquals(secondSevisAccount, userCache.SevisUserAccounts.Last()));
        }

        [TestMethod]
        public void TestConstructor_NullPermissons()
        {
            var principalId = 1;
            var isValidUser = true;
            var camUser = new User();
            camUser.PrincipalId = principalId;

            var user = new DebugWebApiUser();
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
            var user = new DebugWebApiUser();
            var userCache = new UserCache(user, new User(), true, permissions);
            Assert.IsTrue(Object.ReferenceEquals(permissions, userCache.Permissions));
            DateTime.UtcNow.Should().BeCloseTo(userCache.DateCached, 1000);
        }
    }
}
