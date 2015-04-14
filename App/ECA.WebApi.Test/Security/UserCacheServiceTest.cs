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
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Core.DynamicLinq.Filter;
using CAM.Business.Service;

namespace ECA.WebApi.Test.Security
{
    public class SimpleUser : IWebApiUser
    {
        public string Username { get; set; }

        public Guid Id { get; set; }

        public string GetUsername()
        {
            return this.Username;
        }
    }

    [TestClass]
    public class UserCacheServiceTest
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
        public void TestGetUserCache()
        {
            var user = new SimpleUser
            {
                Id = Guid.NewGuid()
            };

            var logger = new TraceLogger();
            var testService = new UserCacheService(logger, cache.Object, expectedTimeToLive);
            cacheDictionary.Add(testService.GetKey(user), new UserCache(user, 1, null));

            Assert.AreEqual(1, testService.GetCount());
            Action<UserCache> tester = (c) =>
            {
                Assert.IsNotNull(c);
                Assert.AreEqual(user.Id, c.UserId);
            };

            var userCache = testService.GetUserCache(user); ;
            tester(userCache);

            Assert.AreEqual(1, testService.GetCount());
        }


        [TestMethod]
        public void TestGetCacheKey_User()
        {
            var user = new SimpleUser
            {
                Id = Guid.NewGuid()
            };
            var logger = new TraceLogger();
            var testService = new UserCacheService(logger, cache.Object, expectedTimeToLive);
            Assert.AreEqual(user.Id.ToString(), testService.GetKey(user));
        }

        [TestMethod]
        public void TestGetCacheKey_UserId()
        {
            var user = new SimpleUser
            {
                Id = Guid.NewGuid()
            };
            var logger = new TraceLogger();
            var testService = new UserCacheService(logger, cache.Object, expectedTimeToLive);
            Assert.AreEqual(user.Id.ToString(), testService.GetKey(user.Id));
        }

        [TestMethod]
        public void TestAdd()
        {
            var user = new SimpleUser
            {
                Id = Guid.NewGuid()
            };
            var logger = new TraceLogger();
            var testService = new UserCacheService(logger, cache.Object, expectedTimeToLive);
            Assert.AreEqual(0, testService.GetCount());
            Assert.AreEqual(0, cacheDictionary.Count);

            var userCache = new UserCache(user, 1, new List<IPermission>());
            testService.Add(userCache);
            Assert.AreEqual(1, testService.GetCount());
            Assert.AreEqual(1, cacheDictionary.Count);
            Assert.IsInstanceOfType(cacheDictionary[testService.GetKey(user)], typeof(UserCache));
        }

        [TestMethod]
        public void TestGetCacheItemPolicy()
        {
            var logger = new TraceLogger();
            var testService = new UserCacheService(logger, cache.Object, expectedTimeToLive);
            var policy = testService.GetCacheItemPolicy();
            Assert.IsNotNull(policy);
            policy.AbsoluteExpiration.Should().BeCloseTo(DateTimeOffset.UtcNow.AddSeconds((double)expectedTimeToLive));
        }

        [TestMethod]
        public void TestIsUserCached_NoUserCached()
        {
            var user = new SimpleUser
            {
                Id = Guid.NewGuid()
            };
            var logger = new TraceLogger();
            var testService = new UserCacheService(logger, cache.Object, expectedTimeToLive);
            Assert.AreEqual(0, testService.GetCount());
            Assert.AreEqual(0, cacheDictionary.Count);
            Assert.IsFalse(testService.IsUserCached(user));
        }

        [TestMethod]
        public void TestIsUserCached_MultipleUsersCached_RequestedUserNotCached()
        {
            var user1 = new SimpleUser
            {
                Id = Guid.NewGuid()
            };
            var user2 = new SimpleUser
            {
                Id = Guid.NewGuid()
            };
            var testUser = new SimpleUser
            {
                Id = Guid.NewGuid()
            };
            var logger = new TraceLogger();
            var testService = new UserCacheService(logger, cache.Object, expectedTimeToLive);
            Assert.AreEqual(0, testService.GetCount());
            Assert.AreEqual(0, cacheDictionary.Count);
            testService.Add(new UserCache(user1, 1));
            testService.Add(new UserCache(user2, 2));
            Assert.IsFalse(testService.IsUserCached(testUser));
        }

        [TestMethod]
        public void TestIsUserCached_MultipleUsersCached_UserIsCached()
        {
            var user1 = new SimpleUser
            {
                Id = Guid.NewGuid()
            };
            var user2 = new SimpleUser
            {
                Id = Guid.NewGuid()
            };
            var logger = new TraceLogger();
            var testService = new UserCacheService(logger, cache.Object, expectedTimeToLive);
            Assert.AreEqual(0, testService.GetCount());
            Assert.AreEqual(0, cacheDictionary.Count);
            testService.Add(new UserCache(user1, 1));
            testService.Add(new UserCache(user2, 2));
            Assert.IsTrue(testService.IsUserCached(user1));
        }
    }
}
