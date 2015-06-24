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

        public Guid Id
        {
            get;
            set;
        }

        public string GetUsername()
        {
            return this.Username;
        }


        public CAM.Business.Model.AzureUser ToAzureUser()
        {
            return new CAM.Business.Model.AzureUser(this.Id, "", "", "", "");
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
        }

        [TestMethod]
        public void TestGetUserCache_UserCacheDoesNotExist()
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

            var testService = new UserCacheService(cache.Object, expectedTimeToLive);
            Assert.AreEqual(0, testService.GetCount());
            Assert.IsNull(testService.GetUserCache(user));
        }

        [TestMethod]
        public void TestGetUserCache()
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

            var testService = new UserCacheService(cache.Object, expectedTimeToLive);
            cacheDictionary.Add(testService.GetKey(user), new UserCache(user, camUser, isUserValid, null));

            Assert.AreEqual(1, testService.GetCount());
            Action<UserCache> tester = (c) =>
            {
                Assert.IsNotNull(c);
                Assert.AreEqual(user.Id, c.UserId);
            };

            var userCache = testService.GetUserCache(user);
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
            var testService = new UserCacheService(cache.Object, expectedTimeToLive);
            Assert.AreEqual(user.Id.ToString(), testService.GetKey(user));
        }

        [TestMethod]
        public void TestGetCacheKey_UserId()
        {
            var user = new SimpleUser
            {
                Id = Guid.NewGuid()
            };
            var testService = new UserCacheService(cache.Object, expectedTimeToLive);
            Assert.AreEqual(user.Id.ToString(), testService.GetKey(user.Id));
        }

        [TestMethod]
        public void TestAdd()
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
            var testService = new UserCacheService(cache.Object, expectedTimeToLive);
            Assert.AreEqual(0, testService.GetCount());
            Assert.AreEqual(0, cacheDictionary.Count);

            var userCache = new UserCache(user, camUser, isUserValid, new List<IPermission>());
            testService.Add(userCache);
            Assert.AreEqual(1, testService.GetCount());
            Assert.AreEqual(1, cacheDictionary.Count);
            Assert.IsInstanceOfType(cacheDictionary[testService.GetKey(user)], typeof(UserCache));
        }

        [TestMethod]
        public void TestRemove()
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
            var testService = new UserCacheService(cache.Object, expectedTimeToLive);
            Assert.AreEqual(0, testService.GetCount());
            Assert.AreEqual(0, cacheDictionary.Count);

            var userCache = new UserCache(user, camUser, isUserValid, new List<IPermission>());
            testService.Add(userCache);
            Assert.AreEqual(1, testService.GetCount());
            Assert.AreEqual(1, cacheDictionary.Count);

            testService.Remove(user);
            Assert.AreEqual(0, testService.GetCount());
            Assert.AreEqual(0, cacheDictionary.Count);
        }

        [TestMethod]
        public void TestRemove_MultipleUsers()
        {
            var camId1 = 1;
            var camId2 = 2;
            var isCamUser1Valid = true;
            var isCamUser2Valid = true;
            var camUser1 = new User
            {
                PrincipalId = camId1,
            };
            var camUser2 = new User
            {
                PrincipalId = camId2,
            };
            var user1 = new SimpleUser
            {
                Id = Guid.NewGuid()
            };
            var user2 = new SimpleUser
            {
                Id = Guid.NewGuid()
            };
            var testService = new UserCacheService(cache.Object, expectedTimeToLive);
            Assert.AreEqual(0, testService.GetCount());
            Assert.AreEqual(0, cacheDictionary.Count);

            var userCache1 = new UserCache(user1, camUser1, isCamUser1Valid, new List<IPermission>());
            var userCache2 = new UserCache(user2, camUser2, isCamUser2Valid, new List<IPermission>());
            testService.Add(userCache1);
            testService.Add(userCache2);
            Assert.AreEqual(2, testService.GetCount());
            Assert.AreEqual(2, cacheDictionary.Count);

            testService.Remove(user1);
            Assert.AreEqual(1, testService.GetCount());
            Assert.AreEqual(1, cacheDictionary.Count);
            Assert.IsTrue(cacheDictionary.ContainsKey(testService.GetKey(user2)));
            Assert.AreEqual(1, testService.GetCount());
        }

        [TestMethod]
        public void TestRemove_NoUsers()
        {

            var testService = new UserCacheService(cache.Object, expectedTimeToLive);
            Assert.AreEqual(0, testService.GetCount());
            Assert.AreEqual(0, cacheDictionary.Count);

            testService.Remove(new SimpleUser { Id = Guid.NewGuid() });
            Assert.AreEqual(0, testService.GetCount());
            Assert.AreEqual(0, cacheDictionary.Count);
        }

        [TestMethod]
        public void TestGetCacheItemPolicy()
        {
            var testService = new UserCacheService(cache.Object, expectedTimeToLive);
            var policy = testService.GetCacheItemPolicy();
            Assert.IsNotNull(policy);
            policy.AbsoluteExpiration.Should().BeCloseTo(DateTimeOffset.UtcNow.AddSeconds((double)expectedTimeToLive));
            Assert.IsNotNull(policy.RemovedCallback);
            policy.Invoking(x => x.RemovedCallback(new CacheEntryRemovedArguments(cache.Object, CacheEntryRemovedReason.Removed, new CacheItem(Guid.NewGuid().ToString())))).ShouldNotThrow();
        }
    }
}
