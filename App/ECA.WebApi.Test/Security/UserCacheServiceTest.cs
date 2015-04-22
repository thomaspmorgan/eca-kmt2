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
    public class SimpleUser : WebApiUserBase
    {
        public string Username { get; set; }

        public Guid Id 
        {
            get
            {
                return base.Id;
            }
            set
            {
                base.Id = value;
            }
        }

        public override string GetUsername()
        {
            return this.Username;
        }

        public override bool HasPermission(IPermission requestedPermission, IEnumerable<IPermission> allUserPermissions)
        {
            throw new NotImplementedException();
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
            var camUser = new TestCamUser
            {
                PrincipalId = camId,
                IsValid = true
            };
            var user = new SimpleUser
            {
                Id = Guid.NewGuid()
            };

            var testService = new UserCacheService(cache.Object, expectedTimeToLive);
            Assert.AreEqual(0, testService.GetCount());
            testService.Invoking(x => x.GetUserCache(user)).ShouldThrow<NotSupportedException>()
                .WithMessage("The user should have a cached object in the system cache.  Be sure use to the IsUserCached method and Add method for user cache logic.");
        }

        [TestMethod]
        public void TestGetUserCache()
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

            var testService = new UserCacheService(cache.Object, expectedTimeToLive);
            cacheDictionary.Add(testService.GetKey(user), new UserCache(user, camUser, camUser.IsValid, null));

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
            var camUser = new TestCamUser
            {
                PrincipalId = camId,
                IsValid = true
            };
            var user = new SimpleUser
            {
                Id = Guid.NewGuid()
            };
            var testService = new UserCacheService(cache.Object, expectedTimeToLive);
            Assert.AreEqual(0, testService.GetCount());
            Assert.AreEqual(0, cacheDictionary.Count);

            var userCache = new UserCache(user, camUser, camUser.IsValid, new List<IPermission>());
            testService.Add(userCache);
            Assert.AreEqual(1, testService.GetCount());
            Assert.AreEqual(1, cacheDictionary.Count);
            Assert.IsInstanceOfType(cacheDictionary[testService.GetKey(user)], typeof(UserCache));
        }

        [TestMethod]
        public void TestRemove()
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
            var testService = new UserCacheService(cache.Object, expectedTimeToLive);
            Assert.AreEqual(0, testService.GetCount());
            Assert.AreEqual(0, cacheDictionary.Count);

            var userCache = new UserCache(user, camUser, camUser.IsValid, new List<IPermission>());
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
            var camUser1 = new TestCamUser
            {
                PrincipalId = camId1,
                IsValid = true
            };
            var camUser2 = new TestCamUser
            {
                PrincipalId = camId2,
                IsValid = true
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

            var userCache1 = new UserCache(user1, camUser1, camUser1.IsValid, new List<IPermission>());
            var userCache2 = new UserCache(user2, camUser2, camUser2.IsValid, new List<IPermission>());
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
        }

        [TestMethod]
        public void TestIsUserCached_NoUserCached()
        {
            var user = new SimpleUser
            {
                Id = Guid.NewGuid()
            };
            var testService = new UserCacheService(cache.Object, expectedTimeToLive);
            Assert.AreEqual(0, testService.GetCount());
            Assert.AreEqual(0, cacheDictionary.Count);
            Assert.IsFalse(testService.IsUserCached(user));
        }

        [TestMethod]
        public void TestIsUserCached_MultipleUsersCached_RequestedUserNotCached()
        {
            var camId1 = 1;
            var camUser1 = new TestCamUser
            {
                PrincipalId = camId1,
                IsValid = true
            };
            var camId2 = 1;
            var camUser2 = new TestCamUser
            {
                PrincipalId = camId2,
                IsValid = true
            };
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
            var testService = new UserCacheService(cache.Object, expectedTimeToLive);
            Assert.AreEqual(0, testService.GetCount());
            Assert.AreEqual(0, cacheDictionary.Count);
            testService.Add(new UserCache(user1, camUser1, camUser1.IsValid));
            testService.Add(new UserCache(user2, camUser2, camUser2.IsValid));
            Assert.IsFalse(testService.IsUserCached(testUser));
        }

        [TestMethod]
        public void TestIsUserCached_MultipleUsersCached_UserIsCached()
        {
            var camId1 = 1;
            var camUser1 = new TestCamUser
            {
                PrincipalId = camId1,
                IsValid = true
            };
            var camId2 = 1;
            var camUser2 = new TestCamUser
            {
                PrincipalId = camId2,
                IsValid = true
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
            testService.Add(new UserCache(user1, camUser1, camUser1.IsValid));
            testService.Add(new UserCache(user2, camUser2, camUser2.IsValid));
            Assert.IsTrue(testService.IsUserCached(user1));
        }
    }
}
