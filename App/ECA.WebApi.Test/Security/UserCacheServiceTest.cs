using ECA.Core.Logging;
using FluentAssertions;
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

    //public class SimpleBusinessUserService : IBusinessUserService
    //{
    //    public SimpleBusinessUserService()
    //    {
    //        this.ResourcePermissions = new List<ResourcePermission>();
    //    }

    //    public List<ResourcePermission> ResourcePermissions { get; set; }

    //    public Task<List<ResourcePermission>> GetResourcePermissionsAsync(Guid userId)
    //    {
    //        return Task.FromResult<List<ResourcePermission>>(this.ResourcePermissions);
    //    }

    //    List<ResourcePermission> IBusinessUserService.GetResourcePermissions(Guid userId)
    //    {
    //        return this.ResourcePermissions;
    //    }
    //}

    [TestClass]
    public class UserCacheServiceTest
    {
        private Mock<ObjectCache> cache;
        private Mock<IBusinessUserService> userService;
        private IDictionary<string, object> cacheDictionary;
        private int expectedTimeToLive = 10;

        [TestInitialize]
        public void TestInit()
        {
            cacheDictionary = new Dictionary<string, object>();
            cache = new Mock<ObjectCache>();
            userService = new Mock<IBusinessUserService>();
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
        public async Task TestGetUserCache_MustLoad()
        {
            userService.Setup(x => x.GetResourcePermissions(It.IsAny<Guid>())).Returns(new List<ResourcePermission>());
            userService.Setup(x => x.GetResourcePermissionsAsync(It.IsAny<Guid>())).ReturnsAsync(new List<ResourcePermission>());
            var user = new SimpleUser
            {
                Id = Guid.NewGuid()
            };

            var logger = new TraceLogger();
            var testService = new UserCacheService(logger, userService.Object, cache.Object, expectedTimeToLive);

            Assert.AreEqual(0, testService.GetCount());
            userService.Verify(x => x.GetResourcePermissionsAsync(It.IsAny<Guid>()), Times.Never);
            userService.Verify(x => x.GetResourcePermissions(It.IsAny<Guid>()), Times.Never);
            
            var userCacheAsync = await testService.GetUserCacheAsync(user);
            Assert.AreEqual(1, testService.GetCount());
            cacheDictionary.Clear();
            var userCache = testService.GetUserCache(user);
            Assert.AreEqual(1, testService.GetCount());
            
            userService.Verify(x => x.GetResourcePermissionsAsync(It.IsAny<Guid>()), Times.Once);
            userService.Verify(x => x.GetResourcePermissions(It.IsAny<Guid>()), Times.Once);
            
        }

        [TestMethod]
        public async Task TestGetUserCache_UserCacheAlreadyExists()
        {
            userService.Setup(x => x.GetResourcePermissions(It.IsAny<Guid>())).Returns(new List<ResourcePermission>());
            userService.Setup(x => x.GetResourcePermissionsAsync(It.IsAny<Guid>())).ReturnsAsync(new List<ResourcePermission>());
            var user = new SimpleUser
            {
                Id = Guid.NewGuid()
            };

            var logger = new TraceLogger();
            var testService = new UserCacheService(logger, userService.Object, cache.Object, expectedTimeToLive);
            cacheDictionary.Add(testService.GetKey(user), new UserCache(user, null));

            Assert.AreEqual(1, testService.GetCount());
            userService.Verify(x => x.GetResourcePermissionsAsync(It.IsAny<Guid>()), Times.Never);
            userService.Verify(x => x.GetResourcePermissions(It.IsAny<Guid>()), Times.Never);

            Action<UserCache> tester = (c) =>
            {
                Assert.IsNotNull(c);
                Assert.AreEqual(user.Id, c.User.Id);                
            };

            var userCacheAsync = await testService.GetUserCacheAsync(user);
            var userCache = testService.GetUserCache(user);
            tester(userCacheAsync);
            tester(userCache);

            Assert.AreEqual(1, testService.GetCount());
            userService.Verify(x => x.GetResourcePermissionsAsync(It.IsAny<Guid>()), Times.Never);
            userService.Verify(x => x.GetResourcePermissions(It.IsAny<Guid>()), Times.Never);
        }


        [TestMethod]
        public void TestGetCacheKey()
        {
            var user = new SimpleUser
            {
                Id = Guid.NewGuid()
            };
            var logger = new TraceLogger();
            var testService = new UserCacheService(logger, userService.Object, cache.Object, expectedTimeToLive);
            Assert.AreEqual(user.Id.ToString(), testService.GetKey(user));
        }

        [TestMethod]
        public void TestAdd()
        {
            var user = new SimpleUser
            {
                Id = Guid.NewGuid()
            };
            var logger = new TraceLogger();
            var testService = new UserCacheService(logger, userService.Object, cache.Object, expectedTimeToLive);
            Assert.AreEqual(0, testService.GetCount());
            Assert.AreEqual(0, cacheDictionary.Count);

            var userCache = new UserCache(user, new List<ResourcePermission>());
            testService.Add(userCache);
            Assert.AreEqual(1, testService.GetCount());
            Assert.AreEqual(1, cacheDictionary.Count);
            Assert.IsInstanceOfType(cacheDictionary[testService.GetKey(user)], typeof(UserCache));
        }

        [TestMethod]
        public void TestGetCacheItemPolicy()
        {
            var logger = new TraceLogger();
            var testService = new UserCacheService(logger, userService.Object, cache.Object, expectedTimeToLive);
            var policy = testService.GetCacheItemPolicy();
            Assert.IsNotNull(policy);
            policy.AbsoluteExpiration.Should().BeCloseTo(DateTimeOffset.UtcNow.AddSeconds((double)expectedTimeToLive));
        }
    }
}
