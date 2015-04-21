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
        private InMemoryCamModel camModel;
        private Mock<IPermissionStore<IPermission>> permissionStore;

        [TestInitialize]
        public void TestInit()
        {
            
            camModel = new InMemoryCamModel();
            cacheDictionary = new Dictionary<string, object>();
            cache = new Mock<ObjectCache>();
            permissionStore = new Mock<IPermissionStore<IPermission>>();
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
        public void TestImpersonate()
        {
            Guid impersonatorId = Guid.NewGuid();
            var isImpersonatorCamUserValid = false;            
            var impersonatorCamUser = new TestCamUser
            {
                PrincipalId = 1,
                IsValid = isImpersonatorCamUserValid,
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
                new CAM.Business.Service.Permission
                {
                    IsAllowed = true,
                    PermissionId = 1,
                    PrincipalId = 2,
                    ResourceId = 3
                    
                }
            };
            var impersonatededCamUser = new TestCamUser
            {
                PrincipalId = 2,
                IsValid = isImpersonatedCamUserValid
            };
            var impersonatedUser = new SimpleUser{
                Id = impersonatedId,
                Username = "impersonated"
            };
            permissionStore.SetupProperty(x => x.Permissions, impersonatedUserPermissions);
            var provider = new BearerTokenUserProvider(camModel, cacheService, permissionStore.Object);
            BearerTokenUserProvider.UserFactory = (userId) => {

                if (userId == impersonatorId)
                {
                    return impersonatorCamUser;
                }
                else
                {
                    return impersonatededCamUser;
                }
            };

            cacheDictionary.Add(impersonatorId.ToString(), new UserCache(impersonatedUser, impersonatededCamUser, isImpersonatedCamUserValid, impersonatedUserPermissions));
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
        public async Task TestImpersonateAsync()
        {
            Guid impersonatorId = Guid.NewGuid();
            var isImpersonatorCamUserValid = false;
            var impersonatorCamUser = new TestCamUser
            {
                PrincipalId = 1,
                IsValid = isImpersonatorCamUserValid,
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
                new CAM.Business.Service.Permission
                {
                    IsAllowed = true,
                    PermissionId = 1,
                    PrincipalId = 2,
                    ResourceId = 3
                    
                }
            };
            var impersonatededCamUser = new TestCamUser
            {
                PrincipalId = 2,
                IsValid = isImpersonatedCamUserValid
            };
            var impersonatedUser = new SimpleUser
            {
                Id = impersonatedId,
                Username = "impersonated"
            };
            permissionStore.SetupProperty(x => x.Permissions, impersonatedUserPermissions);
            var provider = new BearerTokenUserProvider(camModel, cacheService, permissionStore.Object);
            BearerTokenUserProvider.UserFactory = (userId) =>
            {

                if (userId == impersonatorId)
                {
                    return impersonatorCamUser;
                }
                else
                {
                    return impersonatededCamUser;
                }
            };

            cacheDictionary.Add(impersonatorId.ToString(), new UserCache(impersonatedUser, impersonatededCamUser, isImpersonatedCamUserValid, impersonatedUserPermissions));
            await provider.ImpersonateAsync(impersonator, impersonatedId);

            var userCache = provider.GetUserCache(impersonator);
            Assert.IsNotNull(userCache);
            CollectionAssert.AreEqual(impersonatedUserPermissions.ToList(), userCache.Permissions.ToList());
            Assert.AreEqual(impersonatorCamUser.PrincipalId, userCache.CamPrincipalId);
            Assert.AreEqual(impersonator.Id, userCache.UserId);
            Assert.AreEqual(impersonator.Username, userCache.UserName);
            Assert.AreEqual(isImpersonatedCamUserValid, userCache.IsValidCamUser);
        }
    }
}
