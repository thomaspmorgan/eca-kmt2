using ECA.Core.Logging;
using ECA.WebApi.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace ECA.WebApi.Test.Security
{
    public class SimpleUser : IWebApiUser
    {
        public Guid Id { get; set; }
    }

    public class SimpleBusinessUserService : IBusinessUserService
    {
        public SimpleBusinessUserService()
        {
            this.ResourcePermissions = new List<ResourcePermission>();
        }

        public List<ResourcePermission> ResourcePermissions { get; set; }

        public Task<List<ResourcePermission>> GetResourcePermissionsAsync(Guid userId)
        {
            return Task.FromResult<List<ResourcePermission>>(this.ResourcePermissions);
        }

        List<ResourcePermission> IBusinessUserService.GetResourcePermissions(Guid userId)
        {
            return this.ResourcePermissions;
        }
    }

    [TestClass]
    public class UserPermissionsServiceTest
    {
        private Mock<ObjectCache> cache;
        private IDictionary<string, object> cacheDictionary;
        private int expectedTimeToLive = 10;

        [TestInitialize]
        public void TestInit()
        {
            cacheDictionary = new Dictionary<string, object>();
            cache = new Mock<ObjectCache>();

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
        }

        [TestMethod]
        public async Task TestGetPermissions_UserHasNoPermissions()
        {
            var user = new SimpleUser
            {
                Id = Guid.NewGuid()
            };

            var logger = new TraceLogger();
            var service = new SimpleBusinessUserService();
            var testService = new UserPermissionsService(logger, expectedTimeToLive, service, cache.Object);

            Assert.AreEqual(0, testService.GetCount());
            var permissionsAsync = await testService.GetResourcePermissionsAsync(user);
            Assert.AreEqual(0, permissionsAsync.Count());

            cacheDictionary.Clear();
            var permissions = testService.GetResourcePermissions(user);
            Assert.AreEqual(0, permissions.Count());
        }

        [TestMethod]
        public async Task TestGetPermissions_MustLoadPermissions()
        {
            var user = new SimpleUser
            {
                Id = Guid.NewGuid()
            };

            var logger = new TraceLogger();
            var service = new SimpleBusinessUserService();
            var testService = new UserPermissionsService(logger, expectedTimeToLive, service, cache.Object);

            Assert.AreEqual(0, testService.GetCount());

            var grantedPermission = new ResourcePermission();
            grantedPermission.PermissionName = "Name";
            grantedPermission.ResourceId = 1;
            grantedPermission.ResourceType = "type";
            service.ResourcePermissions.Add(grantedPermission);

            Action<IEnumerable<ResourcePermission>> tester = (results) =>
            {
                Assert.AreEqual(1, results.Count());
            };

            var permissionsAsync = await testService.GetResourcePermissionsAsync(user);
            tester(permissionsAsync);
            cacheDictionary.Clear();
            var permissions = testService.GetResourcePermissions(user);
            tester(permissions);
        }

        [TestMethod]
        public async Task TestGetPermissions_PermissionsShouldBeCached()
        {
            var user = new SimpleUser
            {
                Id = Guid.NewGuid()
            };

            var logger = new TraceLogger();
            var service = new SimpleBusinessUserService();
            var grantedPermission = new ResourcePermission();
            grantedPermission.PermissionName = "Name";
            grantedPermission.ResourceId = 1;
            grantedPermission.ResourceType = "type";
            service.ResourcePermissions.Add(grantedPermission);
            cacheDictionary.Add(user.Id.ToString(), service.ResourcePermissions);

            var testService = new UserPermissionsService(logger, expectedTimeToLive, service, cache.Object);
            Assert.AreEqual(1, testService.GetCount());

            Action<IEnumerable<ResourcePermission>> tester = (results) =>
            {
                Assert.AreEqual(1, results.Count());
            };

            var permissionsAsync = await testService.GetResourcePermissionsAsync(user);
            var permissions = testService.GetResourcePermissions(user);
            tester(permissionsAsync);
            tester(permissions);

        }

        [TestMethod]
        public void TestGetCacheKey()
        {
            var user = new SimpleUser
            {
                Id = Guid.NewGuid()
            };
            var logger = new TraceLogger();
            var service = new SimpleBusinessUserService();
            var testService = new UserPermissionsService(logger, expectedTimeToLive, service, cache.Object);
            Assert.AreEqual(user.Id.ToString(), testService.GetCacheKey(user));
        }

        //[TestMethod]
        //public void TestGetCacheItemPolicy()
        //{
        //    Assert.Fail("Fail here...");
        //}

        //[TestMethod]
        //public void TestAdd()
        //{
        //    Assert.Fail("Fail here...");
        //}
    }
}
