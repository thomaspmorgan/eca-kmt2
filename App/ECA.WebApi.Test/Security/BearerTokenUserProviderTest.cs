﻿using System;
using FluentAssertions;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.WebApi.Security;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.IO;
using Moq;
using System.Threading.Tasks;
using System.Collections.Generic;
using CAM.Business.Service;
using CAM.Data;
using System.Reflection;
using CAM.Business.Model;
using ECA.Core.Service;

namespace ECA.WebApi.Test.Security
{
    [TestClass]
    public class BearerTokenUserProviderTest
    {
        private Mock<IUserCacheService> cacheService;
        private Mock<IPermissionStore<IPermission>> permissionStore;
        private Mock<IUserService> userService;


        [TestInitialize]
        public void TestInit()
        {
            cacheService = new Mock<IUserCacheService>();
            permissionStore = new Mock<IPermissionStore<IPermission>>();
            userService = new Mock<IUserService>();
            HttpContext.Current = new HttpContext(
                new HttpRequest("", "http://localhost", ""),
                new HttpResponse(new StringWriter())
                );
        }

        [TestMethod]
        public void TestGetCurrentUser_NoCurrentUser()
        {
            var provider = new BearerTokenUserProvider(userService.Object, cacheService.Object, permissionStore.Object);
            var user = provider.GetCurrentUser();
            Assert.IsInstanceOfType(user, typeof(AnonymousUser));
        }

        [TestMethod]
        public void TestGetCurrentUser_HasCurrentUser()
        {
            var debugUser = SetDebugUser();
            var provider = new BearerTokenUserProvider(userService.Object, cacheService.Object, permissionStore.Object);
            var user = provider.GetCurrentUser();
            Assert.IsInstanceOfType(user, typeof(WebApiUser));
            Assert.AreEqual(debugUser.Id, user.Id);
        }

        [TestMethod]
        public async Task TestGetBusinessUser()
        {
            var camId = 1;
            var camUser = new User
            {
                PrincipalId = camId,
            };
            var user = new SimpleUser
            {
                Id = Guid.NewGuid()
            };
            var isUserValid = true;
            var userCache = new UserCache(user, camUser, isUserValid);
            cacheService.Setup(x => x.IsUserCached(It.IsAny<IWebApiUser>())).Returns(true);
            cacheService.Setup(x => x.GetUserCache(It.IsAny<IWebApiUser>())).Returns(userCache);

            var provider = new BearerTokenUserProvider(userService.Object, cacheService.Object, permissionStore.Object);
            Action<ECA.Business.Service.User> tester = (testUser) =>
            {
                Assert.AreEqual(camId, testUser.Id);
            };
            tester(provider.GetBusinessUser(user));
            tester(await provider.GetBusinessUserAsync(user));

        }

        [TestMethod]
        public async Task TestGetPermissions()
        {
            var camId = 1;
            var isUserValid = true;
            var camUser = new User
            {
                PrincipalId = camId,
            };
            var user = new SimpleUser
            {
                Id = Guid.NewGuid()
            };
            var permissions = new List<IPermission>();
            var userCache = new UserCache(user, camUser, isUserValid, permissions);
            cacheService.Setup(x => x.IsUserCached(It.IsAny<IWebApiUser>())).Returns(true);
            cacheService.Setup(x => x.GetUserCache(It.IsAny<IWebApiUser>())).Returns(userCache);

            var provider = new BearerTokenUserProvider(userService.Object, cacheService.Object, permissionStore.Object);
            Action<IEnumerable<IPermission>> tester = (testPermissions) =>
            {
                Assert.IsTrue(Object.ReferenceEquals(permissions, testPermissions));
            };
            tester(provider.GetPermissions(user));
            tester(await provider.GetPermissionsAsync(user));

        }

        [TestMethod]
        public async Task TestGetUserCache_UserIsCached()
        {
            var camId = 1;
            var isUserValid = true;
            var camUser = new User
            {
                PrincipalId = camId,
            };
            var user = new SimpleUser
            {
                Id = Guid.NewGuid()
            };
            var permissions = new List<IPermission>();
            var userCache = new UserCache(user, camUser, isUserValid, permissions);
            cacheService.Setup(x => x.IsUserCached(It.IsAny<IWebApiUser>())).Returns(true);
            cacheService.Setup(x => x.GetUserCache(It.IsAny<IWebApiUser>())).Returns(userCache);

            Action<UserCache> tester = (testCache) =>
            {
                Assert.IsTrue(Object.ReferenceEquals(userCache, testCache));
            };
            var provider = new BearerTokenUserProvider(userService.Object, cacheService.Object, permissionStore.Object);
            tester(provider.GetUserCache(user));
            tester(await provider.GetUserCacheAsync(user));
        }

        [TestMethod]
        public async Task TestGetUserCache_UserIsNotCached_UserExistsInCam()
        {
            var camId = 1;
            var isUserValid = true;
            var camUser = new User
            {
                PrincipalId = camId,
            };
            var user = new SimpleUser
            {
                Id = Guid.NewGuid()
            };
            var permissions = new List<IPermission>();
            var userCache = new UserCache(user, camUser, isUserValid, permissions);
            cacheService.Setup(x => x.IsUserCached(It.IsAny<IWebApiUser>())).Returns(false);
            cacheService.Setup(x => x.GetUserCache(It.IsAny<IWebApiUser>())).Returns(userCache);
            permissionStore.SetupProperty(x => x.Permissions, permissions);
            userService.Setup(x => x.GetUserById(It.IsAny<Guid>())).Returns(camUser);
            userService.Setup(x => x.GetUserByIdAsync(It.IsAny<Guid>())).ReturnsAsync(camUser);
            userService.Setup(x => x.IsUserValid(It.IsAny<Guid>())).Returns(isUserValid);
            userService.Setup(x => x.IsUserValidAsync(It.IsAny<Guid>())).ReturnsAsync(isUserValid);

            Action<UserCache> tester = (testCache) =>
            {
                Assert.IsTrue(Object.ReferenceEquals(userCache, testCache));
            };
            var provider = new BearerTokenUserProvider(userService.Object, cacheService.Object, permissionStore.Object);
            tester(provider.GetUserCache(user));
            tester(await provider.GetUserCacheAsync(user));

            permissionStore.Verify(x => x.LoadUserPermissions(It.IsAny<int>()), Times.Exactly(2));
        }

        [TestMethod]
        public async Task TestGetUserCache_UserIsNotCached_UserDoesNotExistInCam()
        {
            var isUserValid = true;
            var user = new SimpleUser
            {
                Id = Guid.NewGuid()
            };
            UserCache cache = null;
            Action<UserCache> userCacheSetter = (uc) =>
                {
                    cache = uc;
                };
            Func<UserCache> getUserCache = () => cache;
            User nullUser = null;
            var permissions = new List<IPermission>();
            cacheService.Setup(x => x.IsUserCached(It.IsAny<IWebApiUser>())).Returns(cache != null);
            cacheService.Setup(x => x.Add(It.IsAny<UserCache>())).Callback(userCacheSetter);
            cacheService.Setup(x => x.GetUserCache(It.IsAny<IWebApiUser>())).Returns(getUserCache);

            permissionStore.SetupProperty(x => x.Permissions, permissions);

            userService.Setup(x => x.GetUserById(It.IsAny<Guid>())).Returns(nullUser);
            userService.Setup(x => x.GetUserByIdAsync(It.IsAny<Guid>())).ReturnsAsync(nullUser);
            userService.Setup(x => x.IsUserValid(It.IsAny<Guid>())).Returns(isUserValid);
            userService.Setup(x => x.IsUserValidAsync(It.IsAny<Guid>())).ReturnsAsync(isUserValid);

            Action<UserCache> tester = (testCache) =>
            {
                Assert.AreEqual(0, testCache.Permissions.Count());

            };
            var provider = new BearerTokenUserProvider(userService.Object, cacheService.Object, permissionStore.Object);
            var z = provider.GetUserCache(user);
            tester(provider.GetUserCache(user));
            tester(await provider.GetUserCacheAsync(user));

            permissionStore.Verify(x => x.LoadUserPermissions(It.IsAny<int>()), Times.Exactly(0));
        }

        [TestMethod]
        public async Task TestIsUserValid_UserIsValid()
        {
            var camId = 1;
            var isUserValid = true;
            var camUser = new User
            {
                PrincipalId = camId,
            };
            var user = new SimpleUser
            {
                Id = Guid.NewGuid()
            };
            var permissions = new List<IPermission>();
            var userCache = new UserCache(user, camUser, isUserValid, permissions);
            cacheService.Setup(x => x.IsUserCached(It.IsAny<IWebApiUser>())).Returns(false);
            cacheService.Setup(x => x.GetUserCache(It.IsAny<IWebApiUser>())).Returns(userCache);
            userService.Setup(x => x.GetUserById(It.IsAny<Guid>())).Returns(camUser);
            userService.Setup(x => x.GetUserByIdAsync(It.IsAny<Guid>())).ReturnsAsync(camUser);

            permissionStore.SetupProperty(x => x.Permissions, permissions);
            var provider = new BearerTokenUserProvider(userService.Object, cacheService.Object, permissionStore.Object);

            Action<bool> tester = (userValidatity) =>
            {
                Assert.AreEqual(isUserValid, userValidatity);
            };
            tester(provider.IsUserValid(user));
            tester(await provider.IsUserValidAsync(user));
        }

        [TestMethod]
        public async Task TestIsUserValid_UserIsNotValid()
        {
            var camId = 1;
            var isUserValid = false;
            var camUser = new User
            {
                PrincipalId = camId,
            };
            var user = new SimpleUser
            {
                Id = Guid.NewGuid()
            };
            var permissions = new List<IPermission>();
            var userCache = new UserCache(user, camUser, isUserValid, permissions);
            cacheService.Setup(x => x.IsUserCached(It.IsAny<IWebApiUser>())).Returns(false);
            cacheService.Setup(x => x.GetUserCache(It.IsAny<IWebApiUser>())).Returns(userCache);
            userService.Setup(x => x.GetUserById(It.IsAny<Guid>())).Returns(camUser);
            userService.Setup(x => x.GetUserByIdAsync(It.IsAny<Guid>())).ReturnsAsync(camUser);

            permissionStore.SetupProperty(x => x.Permissions, permissions);
            var provider = new BearerTokenUserProvider(userService.Object, cacheService.Object, permissionStore.Object);

            Action<bool> tester = (userValidatity) =>
            {
                Assert.AreEqual(isUserValid, userValidatity);
            };
            tester(provider.IsUserValid(user));
            tester(await provider.IsUserValidAsync(user));
        }

        [TestMethod]
        public async Task TestGetPrincipalId()
        {
            var camId = 1;
            var isUserValid = true;
            var camUser = new User
            {
                PrincipalId = camId,
            };
            var user = new SimpleUser
            {
                Id = Guid.NewGuid()
            };
            var permissions = new List<IPermission>();
            var userCache = new UserCache(user, camUser, isUserValid, permissions);
            cacheService.Setup(x => x.IsUserCached(It.IsAny<IWebApiUser>())).Returns(false);
            cacheService.Setup(x => x.GetUserCache(It.IsAny<IWebApiUser>())).Returns(userCache);
            userService.Setup(x => x.GetUserById(It.IsAny<Guid>())).Returns(camUser);
            userService.Setup(x => x.GetUserByIdAsync(It.IsAny<Guid>())).ReturnsAsync(camUser);

            permissionStore.SetupProperty(x => x.Permissions, permissions);
            var provider = new BearerTokenUserProvider(userService.Object, cacheService.Object, permissionStore.Object);

            Action<int> tester = (testId) =>
            {
                Assert.AreEqual(camId, testId);
            };
            tester(provider.GetPrincipalId(user));
            tester(await provider.GetPrincipalIdAsync(user));
        }

        [TestMethod]
        public void TestClear_UserCacheIsPresent()
        {
            var camId = 1;
            var isUserValid = true;
            var camUser = new User
            {
                PrincipalId = camId,
            };
            var user = new SimpleUser
            {
                Id = Guid.NewGuid()
            };
            var permissions = new List<IPermission>();
            var userCache = new UserCache(user, camUser, isUserValid, permissions);
            cacheService.Setup(x => x.IsUserCached(It.IsAny<IWebApiUser>())).Returns(true);
            cacheService.Setup(x => x.IsUserCached(It.IsAny<Guid>())).Returns(true);
            cacheService.Setup(x => x.Remove(It.IsAny<IWebApiUser>()));
            cacheService.Setup(x => x.Remove(It.IsAny<Guid>()));

            permissionStore.SetupProperty(x => x.Permissions, permissions);
            var provider = new BearerTokenUserProvider(userService.Object, cacheService.Object, permissionStore.Object);
            provider.Clear(user);
            cacheService.Verify(x => x.Remove(It.IsAny<IWebApiUser>()), Times.Never());
            cacheService.Verify(x => x.Remove(It.IsAny<Guid>()), Times.Once());
        }

        [TestMethod]
        public void TestClear_UserCacheIsNotPresent()
        {
            var camId = 1;
            var isUserValid = true;
            var camUser = new User
            {
                PrincipalId = camId,
            };
            var user = new SimpleUser
            {
                Id = Guid.NewGuid()
            };
            var permissions = new List<IPermission>();
            var userCache = new UserCache(user, camUser, isUserValid, permissions);
            cacheService.Setup(x => x.IsUserCached(It.IsAny<IWebApiUser>())).Returns(false);
            cacheService.Setup(x => x.Remove(It.IsAny<IWebApiUser>()));

            permissionStore.SetupProperty(x => x.Permissions, permissions);
            var provider = new BearerTokenUserProvider(userService.Object, cacheService.Object, permissionStore.Object);
            provider.Clear(user);
            cacheService.Verify(x => x.Remove(It.IsAny<IWebApiUser>()), Times.Never());
        }

        #region Dispose
        [TestMethod]
        public void TestDispose_UserService()
        {
            userService.As<IDisposable>();
            cacheService.As<IDisposable>();
            permissionStore.As<IDisposable>();
            var testService = new BearerTokenUserProvider(userService.Object, cacheService.Object, permissionStore.Object);

            var serviceField = typeof(BearerTokenUserProvider).GetField("userService", BindingFlags.Instance | BindingFlags.NonPublic);
            var serviceValue = serviceField.GetValue(testService);
            Assert.IsNotNull(serviceField);
            Assert.IsNotNull(serviceValue);

            testService.Dispose();
            serviceValue = serviceField.GetValue(testService);
            Assert.IsNull(serviceValue);

        }

        [TestMethod]
        public void TestDispose_UserCacheService()
        {
            userService.As<IDisposable>();
            cacheService.As<IDisposable>();
            permissionStore.As<IDisposable>();
            var testService = new BearerTokenUserProvider(userService.Object, cacheService.Object, permissionStore.Object);

            var serviceField = typeof(BearerTokenUserProvider).GetField("cacheService", BindingFlags.Instance | BindingFlags.NonPublic);
            var serviceValue = serviceField.GetValue(testService);
            Assert.IsNotNull(serviceField);
            Assert.IsNotNull(serviceValue);

            testService.Dispose();
            serviceValue = serviceField.GetValue(testService);
            Assert.IsNull(serviceValue);

        }

        [TestMethod]
        public void TestDispose_PermissionStore()
        {
            userService.As<IDisposable>();
            cacheService.As<IDisposable>();
            permissionStore.As<IDisposable>();
            var testService = new BearerTokenUserProvider(userService.Object, cacheService.Object, permissionStore.Object);

            var storeField = typeof(BearerTokenUserProvider).GetField("permissionStore", BindingFlags.Instance | BindingFlags.NonPublic);
            var storeValue = storeField.GetValue(testService);
            Assert.IsNotNull(storeField);
            Assert.IsNotNull(storeValue);

            testService.Dispose();
            storeValue = storeField.GetValue(testService);
            Assert.IsNull(storeValue);

        }

        [TestMethod]
        public void TestDispose_DisposingAgainShouldNotThrow()
        {
            userService.As<IDisposable>();
            cacheService.As<IDisposable>();
            permissionStore.As<IDisposable>();
            var testService = new BearerTokenUserProvider(userService.Object, cacheService.Object, permissionStore.Object);
            testService.Dispose();
            testService.Invoking(x => x.Dispose()).ShouldNotThrow();
        }
        #endregion

        private DebugWebApiUser SetDebugUser()
        {
            var debugUser = new DebugWebApiUser();
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(debugUser.GetClaims(), "Bearer"));
            Thread.CurrentPrincipal = claimsPrincipal;
            HttpContext.Current.User = claimsPrincipal;
            return debugUser;
        }
    }
}
