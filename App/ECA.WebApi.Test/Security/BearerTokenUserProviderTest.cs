using System;
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

namespace ECA.WebApi.Test.Security
{
    [TestClass]
    public class BearerTokenUserProviderTest
    {
        private InMemoryCamModel camModel;
        private Mock<IUserCacheService> cacheService;
        private Mock<IPermissionStore<IPermission>> permissionStore;


        [TestInitialize]
        public void TestInit()
        {
            camModel = new InMemoryCamModel();
            cacheService = new Mock<IUserCacheService>();
            permissionStore = new Mock<IPermissionStore<IPermission>>();

            HttpContext.Current = new HttpContext(
                new HttpRequest("", "http://localhost", ""),
                new HttpResponse(new StringWriter())
                );
        }

        [TestMethod]
        public void TestGetCurrentUser_NoCurrentUser()
        {
            var provider = new BearerTokenUserProvider(camModel, cacheService.Object, permissionStore.Object);
            var user = provider.GetCurrentUser();
            Assert.IsInstanceOfType(user, typeof(AnonymousUser));
        }

        [TestMethod]
        public void TestGetCurrentUser_HasCurrentUser()
        {
            var debugUser = SetDebugUser();
            var provider = new BearerTokenUserProvider(camModel, cacheService.Object, permissionStore.Object);
            var user = provider.GetCurrentUser();
            Assert.IsInstanceOfType(user, typeof(WebApiUser));
            Assert.AreEqual(debugUser.Id, user.Id);
        }

        [TestMethod]
        public async Task TestGetBusinessUser()
        {
            var camId = 1;
            var camUser = new TestCamUser
            {
                PrincipalId = camId,
                IsValid = true
            };
            var user = new SimpleUser
            {
                Id = Guid.NewGuid()
            };
            var userCache = new UserCache(user, camUser, camUser.IsValid);
            cacheService.Setup(x => x.IsUserCached(It.IsAny<IWebApiUser>())).Returns(true);
            cacheService.Setup(x => x.GetUserCache(It.IsAny<IWebApiUser>())).Returns(userCache);

            var provider = new BearerTokenUserProvider(camModel, cacheService.Object, permissionStore.Object);
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
            var camUser = new TestCamUser
            {
                PrincipalId = camId,
                IsValid = true
            };
            var user = new SimpleUser
            {
                Id = Guid.NewGuid()
            };
            var permissions = new List<IPermission>();
            var userCache = new UserCache(user, camUser, camUser.IsValid, permissions);
            cacheService.Setup(x => x.IsUserCached(It.IsAny<IWebApiUser>())).Returns(true);
            cacheService.Setup(x => x.GetUserCache(It.IsAny<IWebApiUser>())).Returns(userCache);

            var provider = new BearerTokenUserProvider(camModel, cacheService.Object, permissionStore.Object);
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
            var camUser = new TestCamUser
            {
                PrincipalId = camId,
                IsValid = true
            };
            var user = new SimpleUser
            {
                Id = Guid.NewGuid()
            };
            var permissions = new List<IPermission>();
            var userCache = new UserCache(user, camUser, camUser.IsValid, permissions);
            cacheService.Setup(x => x.IsUserCached(It.IsAny<IWebApiUser>())).Returns(true);
            cacheService.Setup(x => x.GetUserCache(It.IsAny<IWebApiUser>())).Returns(userCache);

            Action<UserCache> tester = (testCache) =>
            {
                Assert.IsTrue(Object.ReferenceEquals(userCache, testCache));
            };
            var provider = new BearerTokenUserProvider(camModel, cacheService.Object, permissionStore.Object);
            tester(provider.GetUserCache(user));
            tester(await provider.GetUserCacheAsync(user));
        }

        [TestMethod]
        public async Task TestGetUserCache_UserIsNotCached_UserExistsInCam()
        {
            var camId = 1;
            var camUser = new TestCamUser
            {
                PrincipalId = camId,
                IsValid = true
            };
            var user = new SimpleUser
            {
                Id = Guid.NewGuid()
            };
            var permissions = new List<IPermission>();
            var userCache = new UserCache(user, camUser, camUser.IsValid, permissions);
            cacheService.Setup(x => x.IsUserCached(It.IsAny<IWebApiUser>())).Returns(false);
            cacheService.Setup(x => x.GetUserCache(It.IsAny<IWebApiUser>())).Returns(userCache);

            permissionStore.SetupProperty(x => x.Permissions, permissions);

            Action<UserCache> tester = (testCache) =>
            {
                Assert.IsTrue(Object.ReferenceEquals(userCache, testCache));
            };
            var provider = new BearerTokenUserProvider(camModel, cacheService.Object, permissionStore.Object);
            tester(provider.GetUserCache(user));
            tester(await provider.GetUserCacheAsync(user));

            permissionStore.Verify(x => x.LoadUserPermissions(It.IsAny<int>()), Times.Exactly(2));
        }

        [TestMethod]
        public async Task TestIsUserValid_UserIsValid()
        {
            var camId = 1;
            var camUser = new TestCamUser
            {
                PrincipalId = camId,
                IsValid = true
            };
            var user = new SimpleUser
            {
                Id = Guid.NewGuid()
            };
            var permissions = new List<IPermission>();
            var userCache = new UserCache(user, camUser, camUser.IsValid, permissions);
            cacheService.Setup(x => x.IsUserCached(It.IsAny<IWebApiUser>())).Returns(false);
            cacheService.Setup(x => x.GetUserCache(It.IsAny<IWebApiUser>())).Returns(userCache);

            permissionStore.SetupProperty(x => x.Permissions, permissions);
            var provider = new BearerTokenUserProvider(camModel, cacheService.Object, permissionStore.Object);

            Action<bool> tester = (userValidatity) =>
            {
                Assert.AreEqual(camUser.IsValid, userValidatity);
            };
            tester(provider.IsUserValid(user));
            tester(await provider.IsUserValidAsync(user));
        }

        [TestMethod]
        public async Task TestIsUserValid_UserIsNotValid()
        {
            var camId = 1;
            var camUser = new TestCamUser
            {
                PrincipalId = camId,
                IsValid = false
            };
            var user = new SimpleUser
            {
                Id = Guid.NewGuid()
            };
            var permissions = new List<IPermission>();
            var userCache = new UserCache(user, camUser, camUser.IsValid, permissions);
            cacheService.Setup(x => x.IsUserCached(It.IsAny<IWebApiUser>())).Returns(false);
            cacheService.Setup(x => x.GetUserCache(It.IsAny<IWebApiUser>())).Returns(userCache);

            permissionStore.SetupProperty(x => x.Permissions, permissions);
            var provider = new BearerTokenUserProvider(camModel, cacheService.Object, permissionStore.Object);

            Action<bool> tester = (userValidatity) =>
            {
                Assert.AreEqual(camUser.IsValid, userValidatity);
            };
            tester(provider.IsUserValid(user));
            tester(await provider.IsUserValidAsync(user));
        }

        [TestMethod]
        public async Task TestGetPrincipalId()
        {
            var camId = 1;
            var camUser = new TestCamUser
            {
                PrincipalId = camId,
                IsValid = false
            };
            var user = new SimpleUser
            {
                Id = Guid.NewGuid()
            };
            var permissions = new List<IPermission>();
            var userCache = new UserCache(user, camUser, camUser.IsValid, permissions);
            cacheService.Setup(x => x.IsUserCached(It.IsAny<IWebApiUser>())).Returns(false);
            cacheService.Setup(x => x.GetUserCache(It.IsAny<IWebApiUser>())).Returns(userCache);

            permissionStore.SetupProperty(x => x.Permissions, permissions);
            var provider = new BearerTokenUserProvider(camModel, cacheService.Object, permissionStore.Object);

            Action<int> tester = (testId) =>
            {
                Assert.AreEqual(camId, testId);
            };
            tester(provider.GetPrincipalId(user));
            tester(await provider.GetPrincipalIdAsync(user));
        }

        [TestMethod]
        public async Task TestClear_UserCacheIsPresent()
        {
            var camId = 1;
            var camUser = new TestCamUser
            {
                PrincipalId = camId,
                IsValid = false
            };
            var user = new SimpleUser
            {
                Id = Guid.NewGuid()
            };
            var permissions = new List<IPermission>();
            var userCache = new UserCache(user, camUser, camUser.IsValid, permissions);
            cacheService.Setup(x => x.IsUserCached(It.IsAny<IWebApiUser>())).Returns(true);
            cacheService.Setup(x => x.IsUserCached(It.IsAny<Guid>())).Returns(true);
            cacheService.Setup(x => x.Remove(It.IsAny<IWebApiUser>()));
            cacheService.Setup(x => x.Remove(It.IsAny<Guid>()));

            permissionStore.SetupProperty(x => x.Permissions, permissions);
            var provider = new BearerTokenUserProvider(camModel, cacheService.Object, permissionStore.Object);
            provider.Clear(user);
            cacheService.Verify(x => x.Remove(It.IsAny<IWebApiUser>()), Times.Never());
            cacheService.Verify(x => x.Remove(It.IsAny<Guid>()), Times.Once());
        }

        [TestMethod]
        public async Task TestClear_UserCacheIsNotPresent()
        {
            var camId = 1;
            var camUser = new TestCamUser
            {
                PrincipalId = camId,
                IsValid = false
            };
            var user = new SimpleUser
            {
                Id = Guid.NewGuid()
            };
            var permissions = new List<IPermission>();
            var userCache = new UserCache(user, camUser, camUser.IsValid, permissions);
            cacheService.Setup(x => x.IsUserCached(It.IsAny<IWebApiUser>())).Returns(false);
            cacheService.Setup(x => x.Remove(It.IsAny<IWebApiUser>()));

            permissionStore.SetupProperty(x => x.Permissions, permissions);
            var provider = new BearerTokenUserProvider(camModel, cacheService.Object, permissionStore.Object);
            provider.Clear(user);
            cacheService.Verify(x => x.Remove(It.IsAny<IWebApiUser>()), Times.Never());
        }


        #region Dispose
        [TestMethod]
        public void TestDispose_Context()
        {
            var testContext = new InMemoryCamModel();
            var testService = new BearerTokenUserProvider(testContext, cacheService.Object, permissionStore.Object);

            var contextField = typeof(BearerTokenUserProvider).GetField("camContext", BindingFlags.Instance | BindingFlags.NonPublic);
            var contextValue = contextField.GetValue(testService);
            Assert.IsNotNull(contextField);
            Assert.IsNotNull(contextValue);

            testService.Dispose();
            contextValue = contextField.GetValue(testService);
            Assert.IsNull(contextValue);


        }
        #endregion

        private TestUser SetDebugUser()
        {
            var debugUser = new TestUser();
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(debugUser.GetClaims(), "Bearer"));
            Thread.CurrentPrincipal = claimsPrincipal;
            HttpContext.Current.User = claimsPrincipal;
            return debugUser;
        }
    }
}
