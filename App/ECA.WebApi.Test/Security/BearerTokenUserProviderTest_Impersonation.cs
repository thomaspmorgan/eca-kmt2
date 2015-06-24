using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Runtime.Caching;
using System.Collections.Generic;
using ECA.WebApi.Security;
using CAM.Business.Service;
using System.Threading.Tasks;

namespace ECA.WebApi.Test.Security
{
    [TestClass]
    public class BearerTokenUserProviderTest_Impersonation
    {

        private Mock<ObjectCache> cache;
        private IDictionary<string, object> cacheDictionary;
        private int expectedTimeToLive = 10;
        private UserCacheService cacheService;
        private Mock<IPermissionService> permissionService;
        private Mock<IUserService> userService;

        [TestInitialize]
        public void TestInit()
        {   
            cacheDictionary = new Dictionary<string, object>();
            cache = new Mock<ObjectCache>();
            permissionService = new Mock<IPermissionService>();
            userService = new Mock<IUserService>();
            Action<string, string> removeAction = (id, region) =>
            {
                cacheDictionary.Remove(id);
            };
            Action<CacheItem, CacheItemPolicy> setAction = (c, p) =>
            {
                cacheDictionary.Add(c.Key, c.Value);
            };
            Func<string, string, object> getByKey = (key, region) =>
            {
                if (cacheDictionary.ContainsKey(key))
                {
                    return cacheDictionary[key];
                }
                else
                {
                    return null;
                }
            };
            cache.Setup(x => x.Set(It.IsAny<CacheItem>(), It.IsAny<CacheItemPolicy>())).Callback(setAction);
            cache.Setup(x => x.Get(It.IsAny<string>(), It.IsAny<string>())).Returns(getByKey);
            cache.Setup(x => x.GetCount(It.IsAny<string>())).Returns(() =>
            {
                return cacheDictionary.Count;
            });
            cache.Setup(x => x.Remove(It.IsAny<string>(), It.IsAny<string>())).Callback(removeAction);
            cacheService = new UserCacheService(cache.Object, expectedTimeToLive);
        }

        
        [TestMethod]
        public void TestImpersonate_ImpersonatedUserIsValid()
        {
            Guid impersonatorId = Guid.NewGuid();
            var isImpersonatorCamUserValid = false;            
            var impersonatorCamUser = new User
            {
                PrincipalId = 1,
            };
            var impersonator = new SimpleUser{
                Id = impersonatorId,
                Username = "impersonator"
            };
            var impersonatorUserCache = new UserCache(impersonator, impersonatorCamUser, isImpersonatorCamUserValid);

            Guid impersonatedId = Guid.NewGuid();
            var isImpersonatedCamUserValid = true;
            var impersonatedUserPermissions = new List<IPermission>
            {
                new CAM.Business.Service.SimplePermission
                {
                    IsAllowed = true,
                    PermissionId = 1,
                    PrincipalId = 2,
                    ResourceId = 3
                }
            };
            var impersonatedCamUser = new User
            {
                PrincipalId = 2,
            };
            var impersonatedUser = new SimpleUser{
                Id = impersonatedId,
                Username = "impersonated"
            };
            permissionService.Setup(x => x.GetAllowedPermissionsByPrincipalId(It.IsAny<int>())).Returns(impersonatedUserPermissions);
            Func<Guid, User> getUserById = (id) =>
            {
                if (id == impersonatorId)
                {
                    return impersonatorCamUser;
                }
                else
                {
                    return impersonatedCamUser;
                }
            };
            Func<Guid, bool> getIsUserValid = (id) =>
            {
                if (id == impersonatorId)
                {
                    return isImpersonatorCamUserValid;
                }
                else
                {
                    return isImpersonatedCamUserValid;
                }
            };
            userService.Setup(x => x.GetUserById(It.IsAny<Guid>())).Returns(getUserById);
            userService.Setup(x => x.IsUserValid(It.IsAny<Guid>())).Returns(getIsUserValid);
            cacheDictionary.Add(impersonatorId.ToString(), new UserCache(impersonatedUser, impersonatedCamUser, isImpersonatedCamUserValid, impersonatedUserPermissions));

            var provider = new BearerTokenUserProvider(userService.Object, cacheService, permissionService.Object);
            provider.Impersonate(impersonator, impersonatedId);

            var userCache = provider.GetUserCache(impersonator);
            Assert.IsNotNull(userCache);
            CollectionAssert.AreEqual(impersonatedUserPermissions.ToList(), userCache.Permissions.ToList());
            Assert.AreEqual(impersonatorCamUser.PrincipalId, userCache.CamPrincipalId);
            Assert.AreEqual(impersonator.Id, userCache.UserId);
            Assert.AreEqual(impersonator.Username, userCache.UserName);
            Assert.AreEqual(isImpersonatedCamUserValid, userCache.IsValidCamUser);
        }
        [TestMethod]
        public async Task TestImpersonateAsync_ImpersonatedUserIsValid()
        {
            Guid impersonatorId = Guid.NewGuid();
            var isImpersonatorCamUserValid = false;
            var impersonatorCamUser = new User
            {
                PrincipalId = 1,
            };
            var impersonator = new SimpleUser
            {
                Id = impersonatorId,
                Username = "impersonator"
            };
            var impersonatorUserCache = new UserCache(impersonator, impersonatorCamUser, isImpersonatorCamUserValid);

            Guid impersonatedId = Guid.NewGuid();
            var isImpersonatedCamUserValid = true;
            var impersonatedUserPermissions = new List<IPermission>
            {
                new CAM.Business.Service.SimplePermission
                {
                    IsAllowed = true,
                    PermissionId = 1,
                    PrincipalId = 2,
                    ResourceId = 3
                }
            };
            var impersonatedCamUser = new User
            {
                PrincipalId = 2,
            };
            var impersonatedUser = new SimpleUser
            {
                Id = impersonatedId,
                Username = "impersonated"
            };
            permissionService.Setup(x => x.GetAllowedPermissionsByPrincipalIdAsync(It.IsAny<int>())).ReturnsAsync(impersonatedUserPermissions);
            
            userService.Setup(x => x.GetUserByIdAsync(It.Is<Guid>((id) => id == impersonatedId))).ReturnsAsync(impersonatedCamUser);
            userService.Setup(x => x.GetUserByIdAsync(It.Is<Guid>((id) => id == impersonatorId))).ReturnsAsync(impersonatorCamUser);
            userService.Setup(x => x.IsUserValidAsync(It.Is<Guid>((id) => id == impersonatedId))).ReturnsAsync(isImpersonatedCamUserValid);
            userService.Setup(x => x.IsUserValidAsync(It.Is<Guid>((id) => id == impersonatorId))).ReturnsAsync(isImpersonatorCamUserValid);
            
            cacheDictionary.Add(impersonatorId.ToString(), new UserCache(impersonatedUser, impersonatedCamUser, isImpersonatedCamUserValid, impersonatedUserPermissions));

            var provider = new BearerTokenUserProvider(userService.Object, cacheService, permissionService.Object);
            await provider.ImpersonateAsync(impersonator, impersonatedId);

            var userCache = provider.GetUserCache(impersonator);
            Assert.IsNotNull(userCache);
            CollectionAssert.AreEqual(impersonatedUserPermissions.ToList(), userCache.Permissions.ToList());
            Assert.AreEqual(impersonatorCamUser.PrincipalId, userCache.CamPrincipalId);
            Assert.AreEqual(impersonator.Id, userCache.UserId);
            Assert.AreEqual(impersonator.Username, userCache.UserName);
            Assert.AreEqual(isImpersonatedCamUserValid, userCache.IsValidCamUser);
        }

        [TestMethod]
        public void TestImpersonate_ImpersonatedUserIsNotValid()
        {
            Guid impersonatorId = Guid.NewGuid();
            var isImpersonatorCamUserValid = false;
            var impersonatorCamUser = new User
            {
                PrincipalId = 1,
            };
            var impersonator = new SimpleUser
            {
                Id = impersonatorId,
                Username = "impersonator"
            };
            var impersonatorUserCache = new UserCache(impersonator, impersonatorCamUser, isImpersonatorCamUserValid);

            Guid impersonatedId = Guid.NewGuid();
            var isImpersonatedCamUserValid = false;
            var impersonatedUserPermissions = new List<IPermission>
            {
                new CAM.Business.Service.SimplePermission
                {
                    IsAllowed = true,
                    PermissionId = 1,
                    PrincipalId = 2,
                    ResourceId = 3
                }
            };
            var impersonatedCamUser = new User
            {
                PrincipalId = 2,
            };
            var impersonatedUser = new SimpleUser
            {
                Id = impersonatedId,
                Username = "impersonated"
            };
            permissionService.Setup(x => x.GetAllowedPermissionsByPrincipalIdAsync(It.IsAny<int>())).ReturnsAsync(impersonatedUserPermissions);
            Func<Guid, User> getUserById = (id) =>
            {
                if (id == impersonatorId)
                {
                    return impersonatorCamUser;
                }
                else
                {
                    return impersonatedCamUser;
                }
            };
            Func<Guid, bool> getIsUserValid = (id) =>
            {
                if (id == impersonatorId)
                {
                    return isImpersonatorCamUserValid;
                }
                else
                {
                    return isImpersonatedCamUserValid;
                }
            };
            userService.Setup(x => x.GetUserById(It.IsAny<Guid>())).Returns(getUserById);
            userService.Setup(x => x.IsUserValid(It.IsAny<Guid>())).Returns(getIsUserValid);
            cacheDictionary.Add(impersonatorId.ToString(), new UserCache(impersonatedUser, impersonatedCamUser, isImpersonatedCamUserValid, impersonatedUserPermissions));

            var provider = new BearerTokenUserProvider(userService.Object, cacheService, permissionService.Object);
            provider.Impersonate(impersonator, impersonatedId);

            var userCache = provider.GetUserCache(impersonator);
            Assert.IsNotNull(userCache);
            Assert.AreEqual(0, userCache.Permissions.Count());
}
        [TestMethod]
        public async Task TestImpersonateAsync_ImpersonatedUserIsNotValid()
        {
            Guid impersonatorId = Guid.NewGuid();
            var isImpersonatorCamUserValid = false;
            var impersonatorCamUser = new User
            {
                PrincipalId = 1,
            };
            var impersonator = new SimpleUser
            {
                Id = impersonatorId,
                Username = "impersonator"
            };
            var impersonatorUserCache = new UserCache(impersonator, impersonatorCamUser, isImpersonatorCamUserValid);

            Guid impersonatedId = Guid.NewGuid();
            var isImpersonatedCamUserValid = false;
            var impersonatedUserPermissions = new List<IPermission>
            {
                new CAM.Business.Service.SimplePermission
                {
                    IsAllowed = true,
                    PermissionId = 1,
                    PrincipalId = 2,
                    ResourceId = 3
                }
            };
            var impersonatedCamUser = new User
            {
                PrincipalId = 2,
            };
            var impersonatedUser = new SimpleUser
            {
                Id = impersonatedId,
                Username = "impersonated"
            };
            permissionService.Setup(x => x.GetAllowedPermissionsByPrincipalIdAsync(It.IsAny<int>())).ReturnsAsync(impersonatedUserPermissions);

            userService.Setup(x => x.GetUserByIdAsync(It.Is<Guid>((id) => id == impersonatedId))).ReturnsAsync(impersonatedCamUser);
            userService.Setup(x => x.GetUserByIdAsync(It.Is<Guid>((id) => id == impersonatorId))).ReturnsAsync(impersonatorCamUser);
            userService.Setup(x => x.IsUserValidAsync(It.Is<Guid>((id) => id == impersonatedId))).ReturnsAsync(isImpersonatedCamUserValid);
            userService.Setup(x => x.IsUserValidAsync(It.Is<Guid>((id) => id == impersonatorId))).ReturnsAsync(isImpersonatorCamUserValid);

            cacheDictionary.Add(impersonatorId.ToString(), new UserCache(impersonatedUser, impersonatedCamUser, isImpersonatedCamUserValid, impersonatedUserPermissions));

            var provider = new BearerTokenUserProvider(userService.Object, cacheService, permissionService.Object);
            await provider.ImpersonateAsync(impersonator, impersonatedId);

            var userCache = provider.GetUserCache(impersonator);
            Assert.IsNotNull(userCache);
            Assert.AreEqual(0, userCache.Permissions.Count());
        }
    }
}
