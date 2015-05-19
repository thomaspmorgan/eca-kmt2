using System;
using FluentAssertions;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CAM.Business.Service;
using System.Collections.Generic;
using System.Runtime.Caching;
using Moq;
using CAM.Data;
using System.Threading.Tasks;
using ECA.Core.DynamicLinq.Sorter;
using CAM.Business.Model;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using ECA.Core.DynamicLinq.Filter;

namespace CAM.Business.Test.Service
{
    [TestClass]
    public class ResourceServiceTest
    {
        private Mock<ObjectCache> cache;
        private IDictionary<string, object> cacheDictionary;
        private int expectedTimeToLive = 10;

        private TestInMemoryCamModel context;
        private ResourceService service;

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

            context = new TestInMemoryCamModel();
            service = new ResourceService(context, cache.Object, expectedTimeToLive);
        }

        [TestMethod]
        public void TestGetKey_UsingIds()
        {
            var foreignResourceId = 1;
            var resourceTypeId = 2;
            var key = service.GetKey(foreignResourceId, resourceTypeId);
            var expectedKey = String.Format(ResourceService.CACHE_KEY_FORMAT, foreignResourceId, resourceTypeId);
            Assert.AreEqual(expectedKey, key);
        }

        [TestMethod]
        public void TestGetKey_UsingForeignResourceCache()
        {
            var foreignResourceId = 1;
            var resourceTypeId = 2;
            var resourceId = 3;

            var foreignResourceCache = new ForeignResourceCache(foreignResourceId, resourceId, resourceTypeId);
            var key = service.GetKey(foreignResourceCache);
            var expectedKey = String.Format(ResourceService.CACHE_KEY_FORMAT, foreignResourceId, resourceTypeId);
            Assert.AreEqual(expectedKey, key);
        }

        [TestMethod]
        public void TestIsCached_NothingCached()
        {   
            Assert.AreEqual(0, cacheDictionary.Count);
            var foreignResourceId = 1;
            var resourceTypeId = 2;
            Assert.IsFalse(service.IsCached(foreignResourceId, resourceTypeId));
        }

        [TestMethod]
        public void TestIsCached_OneItemCached()
        {
            Assert.AreEqual(0, cacheDictionary.Count);
            var foreignResourceId = 1;
            var resourceTypeId = 2;
            var resourceId = 3;
            var resourceCache = new ForeignResourceCache(foreignResourceId, resourceId, resourceTypeId);
            service.Add(resourceCache);
            Assert.AreEqual(1, cacheDictionary.Count);
            Assert.IsTrue(service.IsCached(foreignResourceId, resourceTypeId));
        }

        [TestMethod]
        public void TestIsCached_DifferentItemCached()
        {
            Assert.AreEqual(0, cacheDictionary.Count);
            var foreignResourceId = 1;
            var resourceTypeId = 2;
            var resourceId = 3;
            var resourceCache = new ForeignResourceCache(foreignResourceId, resourceId, resourceTypeId);
            service.Add(resourceCache);
            Assert.AreEqual(1, cacheDictionary.Count);
            Assert.IsFalse(service.IsCached(-1, -2));
        }

        [TestMethod]
        public void TestAdd()
        {
            Assert.AreEqual(0, cacheDictionary.Count);
            var foreignResourceId = 1;
            var resourceTypeId = 2;
            var resourceId = 3;
            var cacheItem = service.Add(foreignResourceId, resourceId, resourceTypeId);
            Assert.IsNotNull(cacheItem);
            var testCacheItem = service.GetForeignResourceCache(foreignResourceId, resourceTypeId);
            Assert.IsTrue(object.ReferenceEquals(cacheItem.Value, testCacheItem));
        }

        [TestMethod]
        public void TestItemRemovedCallback()
        {
            var policy = service.GetCacheItemPolicy();
            var callback = policy.RemovedCallback;
            Assert.IsNotNull(callback);
            policy.Invoking(x => x.RemovedCallback(new CacheEntryRemovedArguments(cache.Object, CacheEntryRemovedReason.Removed, new CacheItem("key", 1)))).ShouldNotThrow();
        }

        [TestMethod]
        public void TestGetForeignResourceCache_NoItemsCached()
        {
            var foreignResourceId = 1;
            var resourceTypeId = 2;
            var resourceId = 3;
            Assert.AreEqual(0, cacheDictionary.Count);
            Assert.IsNull(service.GetForeignResourceCache(foreignResourceId, resourceTypeId));
        }

        [TestMethod]
        public void TestGetForeignResourceCache_DifferentItemCached()
        {
            var foreignResourceId = 1;
            var resourceTypeId = 2;
            var resourceId = 3;
            var foreignResource = new ForeignResourceCache(foreignResourceId, resourceId, resourceTypeId);
            service.Add(foreignResource);
            Assert.AreEqual(1, cacheDictionary.Count);
            Assert.IsNull(service.GetForeignResourceCache(-1, -1));
        }

        [TestMethod]
        public void TestGetForeignResourceCache()
        {
            var foreignResourceId = 1;
            var resourceTypeId = 2;
            var resourceId = 3;
            var foreignResource = new ForeignResourceCache(foreignResourceId, resourceId, resourceTypeId);
            service.Add(foreignResource);
            Assert.AreEqual(1, cacheDictionary.Count);

            var testItem = service.GetForeignResourceCache(foreignResourceId, resourceTypeId);
            Assert.IsNotNull(testItem);
            Assert.IsTrue(Object.ReferenceEquals(foreignResource, testItem));
        }

        [TestMethod]
        public void TestGetResourceTypeId()
        {
            var resourceTypeName = ResourceType.Application.Value;
            var id = service.GetResourceTypeId(resourceTypeName);
            Assert.IsTrue(id.HasValue);
            Assert.AreEqual(ResourceType.Application.Id, id);
        }

        [TestMethod]
        public void TestGetResourceTypeId_UnknownResource()
        {
            var resourceTypeName = "abc";
            var id = service.GetResourceTypeId(resourceTypeName);
            Assert.IsFalse(id.HasValue);
        }

        [TestMethod]
        public void TestGetResourceIdByForeignResourceId_NotCached()
        {
            var foreignResourceId = 1;
            var resourceId = 3;
            var resourceType = new ResourceType
            {
                ResourceTypeId = ResourceType.Application.Id,
                ResourceTypeName = ResourceType.Application.Value
            };
            var resource = new Resource
            {
                ResourceId = resourceId,
                ResourceTypeId = resourceType.ResourceTypeId,
                ResourceType = resourceType,
                ForeignResourceId = foreignResourceId
            };
            context.Resources.Add(resource);
            context.ResourceTypes.Add(resourceType);
            Assert.IsFalse(service.IsCached(foreignResourceId, resourceType.ResourceTypeId));

            var testResourceId = service.GetResourceIdByForeignResourceId(foreignResourceId, resourceType.ResourceTypeId);
            Assert.IsTrue(testResourceId.HasValue);
            Assert.AreEqual(resourceId, testResourceId.Value);

            Assert.IsTrue(service.IsCached(foreignResourceId, resourceType.ResourceTypeId));
        }

        [TestMethod]
        public async Task TestGetResourceIdByForeignResourceIdAsync_NotCached()
        {
            var foreignResourceId = 1;
            var resourceId = 3;
            var resourceType = new ResourceType
            {
                ResourceTypeId = ResourceType.Application.Id,
                ResourceTypeName = ResourceType.Application.Value
            };
            var resource = new Resource
            {
                ResourceId = resourceId,
                ResourceTypeId = resourceType.ResourceTypeId,
                ResourceType = resourceType,
                ForeignResourceId = foreignResourceId
            };
            context.Resources.Add(resource);
            context.ResourceTypes.Add(resourceType);
            Assert.IsFalse(service.IsCached(foreignResourceId, resourceType.ResourceTypeId));

            var testResourceId = await service.GetResourceIdByForeignResourceIdAsync(foreignResourceId, resourceType.ResourceTypeId);
            Assert.IsTrue(testResourceId.HasValue);
            Assert.AreEqual(resourceId, testResourceId.Value);

            Assert.IsTrue(service.IsCached(foreignResourceId, resourceType.ResourceTypeId));
        }

        [TestMethod]
        public void TestGetResourceIdByForeignResourceId_IsCached()
        {
            var foreignResourceId = 1;
            var resourceId = 3;
            var resourceType = new ResourceType
            {
                ResourceTypeId = ResourceType.Application.Id,
                ResourceTypeName = ResourceType.Application.Value
            };
            var resource = new Resource
            {
                ResourceId = resourceId,
                ResourceTypeId = resourceType.ResourceTypeId,
                ResourceType = resourceType,
                ForeignResourceId = foreignResourceId
            };

            //if we know there are no resources or resource types in db we are pulling from cache
            Assert.AreEqual(0, context.Resources.Count());
            Assert.AreEqual(0, context.ResourceTypes.Count());
            service.Add(foreignResourceId, resourceId, resourceType.ResourceTypeId);
            Assert.IsTrue(service.IsCached(foreignResourceId, resourceType.ResourceTypeId));

            var testResourceId = service.GetResourceIdByForeignResourceId(foreignResourceId, resourceType.ResourceTypeId);
            Assert.IsTrue(testResourceId.HasValue);
            Assert.AreEqual(resourceId, testResourceId.Value);

            //if we know there are no resources or resource types in db we are pulling from cache
            Assert.AreEqual(0, context.Resources.Count());
            Assert.AreEqual(0, context.ResourceTypes.Count());
        }

        [TestMethod]
        public async Task TestGetResourceIdByForeignResourceIdAsync_IsCached()
        {
            var foreignResourceId = 1;
            var resourceId = 3;
            var resourceType = new ResourceType
            {
                ResourceTypeId = ResourceType.Application.Id,
                ResourceTypeName = ResourceType.Application.Value
            };
            var resource = new Resource
            {
                ResourceId = resourceId,
                ResourceTypeId = resourceType.ResourceTypeId,
                ResourceType = resourceType,
                ForeignResourceId = foreignResourceId
            };

            //if we know there are no resources or resource types in db we are pulling from cache
            Assert.AreEqual(0, context.Resources.Count());
            Assert.AreEqual(0, context.ResourceTypes.Count());
            service.Add(foreignResourceId, resourceId, resourceType.ResourceTypeId);
            Assert.IsTrue(service.IsCached(foreignResourceId, resourceType.ResourceTypeId));

            var testResourceId = await service.GetResourceIdByForeignResourceIdAsync(foreignResourceId, resourceType.ResourceTypeId);
            Assert.IsTrue(testResourceId.HasValue);
            Assert.AreEqual(resourceId, testResourceId.Value);

            //if we know there are no resources or resource types in db we are pulling from cache
            Assert.AreEqual(0, context.Resources.Count());
            Assert.AreEqual(0, context.ResourceTypes.Count());
        }

        [TestMethod]
        public void TestGetResourceIdByForeignResourceId_ForeignResourceDoesNotExist()
        {
            var resourceTypeId = ResourceType.Application.Id;
            var foreignResourceId = 1;

            Assert.AreEqual(0, context.Resources.Count());
            Assert.AreEqual(0, context.ResourceTypes.Count());
            Assert.AreEqual(0, cacheDictionary.Count);
            Assert.IsFalse(service.IsCached(foreignResourceId, resourceTypeId));

            var testResourceId = service.GetResourceIdByForeignResourceId(foreignResourceId, resourceTypeId);
            Assert.IsFalse(testResourceId.HasValue);

            Assert.AreEqual(0, context.Resources.Count());
            Assert.AreEqual(0, context.ResourceTypes.Count());
            Assert.AreEqual(0, cacheDictionary.Count);
        }

        [TestMethod]
        public async Task TestGetResourceIdByForeignResourceIdAsync_ForeignResourceDoesNotExist()
        {
            var resourceTypeId = ResourceType.Application.Id;
            var foreignResourceId = 1;

            Assert.AreEqual(0, context.Resources.Count());
            Assert.AreEqual(0, context.ResourceTypes.Count());
            Assert.AreEqual(0, cacheDictionary.Count);
            Assert.IsFalse(service.IsCached(foreignResourceId, resourceTypeId));

            var testResourceId = await service.GetResourceIdByForeignResourceIdAsync(foreignResourceId, resourceTypeId);
            Assert.IsFalse(testResourceId.HasValue);

            Assert.AreEqual(0, context.Resources.Count());
            Assert.AreEqual(0, context.ResourceTypes.Count());
            Assert.AreEqual(0, cacheDictionary.Count);
        }

        [TestMethod]
        public async Task GetResourceIdForApplicationId()
        {
            var resourceTypeId = ResourceType.Application.Id;
            var applicationId = 1;
            var resourceId = 10;
            var resourceType = new ResourceType
            {
                ResourceTypeId = ResourceType.Application.Id,
                ResourceTypeName = ResourceType.Application.Value
            };
            var resource = new Resource
            {
                ForeignResourceId = applicationId,
                ResourceType = resourceType,
                ResourceTypeId = resourceTypeId,
                ResourceId = resourceId
            };
            context.ResourceTypes.Add(resourceType);
            context.Resources.Add(resource);

            Assert.AreEqual(1, context.Resources.Count());
            Assert.AreEqual(1, context.ResourceTypes.Count());

            Action<int?> tester = (testId) =>
            {
                Assert.AreEqual(resourceId, testId);
            };

            var testResourceId = service.GetResourceIdForApplicationId(applicationId);
            var testResourceIdAsync = await service.GetResourceIdForApplicationIdAsync(applicationId);
            tester(testResourceId);
            tester(testResourceIdAsync);
        }

        [TestMethod]
        public async Task GetResourceIdForApplicationId_ApplicationDoesNotExist()
        {
            Assert.AreEqual(0, context.Resources.Count());
            Assert.AreEqual(0, context.ResourceTypes.Count());

            Action<int?> tester = (testId) =>
            {
                Assert.IsFalse(testId.HasValue);
            };

            var testResourceId = service.GetResourceIdForApplicationId(1);
            var testResourceIdAsync = await service.GetResourceIdForApplicationIdAsync(1);
            tester(testResourceId);
            tester(testResourceIdAsync);
        }

        [TestMethod]
        public void TestGetCacheItemPolicy()
        {
            var policy = service.GetCacheItemPolicy();
            Assert.IsNotNull(policy.SlidingExpiration);
        }

        #region Resource Authorizations
        [TestMethod]
        public async Task TestGetResourceAuthorization_DefaultSort()
        {
            var principal = new Principal
            {
                PrincipalId = 1
            };
            var userAccount = new UserAccount
            {
                DisplayName = "display",
                Principal = principal,
                PrincipalId = principal.PrincipalId
            };
            principal.UserAccount = userAccount;
            userAccount.Principal = principal;

            var resourceType = new ResourceType
            {
                ResourceTypeName = ResourceType.Project.Value,
                ResourceTypeId = ResourceType.Project.Id
            };
            var resource = new Resource
            {
                ResourceId = 1,
                ResourceType = resourceType,
                ResourceTypeId = resourceType.ResourceTypeId,
                ForeignResourceId = 2,
            };
            var permission1 = new CAM.Data.Permission
            {
                PermissionId = CAM.Data.Permission.Editproject.Id,
                PermissionName = CAM.Data.Permission.Editproject.Value,
            };
            var permissionAssignment1 = new PermissionAssignment
            {
                IsAllowed = true,
                Permission = permission1,
                PermissionId = permission1.PermissionId,
                Principal = principal,
                PrincipalId = principal.PrincipalId,
                Resource = resource,
                ResourceId = resource.ResourceId
            };
            var permission2 = new CAM.Data.Permission
            {
                PermissionId = CAM.Data.Permission.Editprogram.Id,
                PermissionName = CAM.Data.Permission.Editprogram.Value,
            };
            var permissionAssignment2 = new PermissionAssignment
            {
                IsAllowed = true,
                Permission = permission2,
                PermissionId = permission2.PermissionId,
                Principal = principal,
                PrincipalId = principal.PrincipalId,
                Resource = resource,
                ResourceId = resource.ResourceId
            };

            context.Principals.Add(principal);
            context.UserAccounts.Add(userAccount);
            context.ResourceTypes.Add(resourceType);
            context.Resources.Add(resource);
            context.Permissions.Add(permission1);
            context.PermissionAssignments.Add(permissionAssignment1);
            context.Permissions.Add(permission2);
            context.PermissionAssignments.Add(permissionAssignment2);

            var defaultSort = new ExpressionSorter<ResourceAuthorization>(x => x.PermissionName, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<ResourceAuthorization>(0, 10, defaultSort);
            Action<PagedQueryResults<ResourceAuthorization>> tester = (results) =>
            {
                Assert.AreEqual(2, results.Total);
                Assert.AreEqual(2, results.Results.Count);
                Assert.AreEqual(permission2.PermissionId, results.Results.First().PermissionId);
            };
            var serviceResults = service.GetResourceAuthorizations(queryOperator);
            var serviceResultsAsync = await service.GetResourceAuthorizationsAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetResourceAuthorization_Sort()
        {
            var principal = new Principal
            {
                PrincipalId = 1
            };
            var userAccount = new UserAccount
            {
                DisplayName = "display",
                Principal = principal,
                PrincipalId = principal.PrincipalId
            };
            principal.UserAccount = userAccount;
            userAccount.Principal = principal;

            var resourceType = new ResourceType
            {
                ResourceTypeName = ResourceType.Project.Value,
                ResourceTypeId = ResourceType.Project.Id
            };
            var resource = new Resource
            {
                ResourceId = 1,
                ResourceType = resourceType,
                ResourceTypeId = resourceType.ResourceTypeId,
                ForeignResourceId = 2,
            };
            var permission1 = new CAM.Data.Permission
            {
                PermissionId = CAM.Data.Permission.Editproject.Id,
                PermissionName = CAM.Data.Permission.Editproject.Value,
            };
            var permissionAssignment1 = new PermissionAssignment
            {
                IsAllowed = true,
                Permission = permission1,
                PermissionId = permission1.PermissionId,
                Principal = principal,
                PrincipalId = principal.PrincipalId,
                Resource = resource,
                ResourceId = resource.ResourceId
            };
            var permission2 = new CAM.Data.Permission
            {
                PermissionId = CAM.Data.Permission.Editprogram.Id,
                PermissionName = CAM.Data.Permission.Editprogram.Value,
            };
            var permissionAssignment2 = new PermissionAssignment
            {
                IsAllowed = true,
                Permission = permission2,
                PermissionId = permission2.PermissionId,
                Principal = principal,
                PrincipalId = principal.PrincipalId,
                Resource = resource,
                ResourceId = resource.ResourceId
            };

            context.Principals.Add(principal);
            context.UserAccounts.Add(userAccount);
            context.ResourceTypes.Add(resourceType);
            context.Resources.Add(resource);
            context.Permissions.Add(permission1);
            context.PermissionAssignments.Add(permissionAssignment1);
            context.Permissions.Add(permission2);
            context.PermissionAssignments.Add(permissionAssignment2);

            var defaultSort = new ExpressionSorter<ResourceAuthorization>(x => x.PermissionName, SortDirection.Descending);
            var queryOperator = new QueryableOperator<ResourceAuthorization>(0, 10, defaultSort);
            Action<PagedQueryResults<ResourceAuthorization>> tester = (results) =>
            {
                Assert.AreEqual(2, results.Total);
                Assert.AreEqual(2, results.Results.Count);
                Assert.AreEqual(permission1.PermissionId, results.Results.First().PermissionId);
            };
            var serviceResults = service.GetResourceAuthorizations(queryOperator);
            var serviceResultsAsync = await service.GetResourceAuthorizationsAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetResourceAuthorization_Filter()
        {
            var principal = new Principal
            {
                PrincipalId = 1
            };
            var userAccount = new UserAccount
            {
                DisplayName = "display",
                Principal = principal,
                PrincipalId = principal.PrincipalId
            };
            principal.UserAccount = userAccount;
            userAccount.Principal = principal;

            var resourceType = new ResourceType
            {
                ResourceTypeName = ResourceType.Project.Value,
                ResourceTypeId = ResourceType.Project.Id
            };
            var resource = new Resource
            {
                ResourceId = 1,
                ResourceType = resourceType,
                ResourceTypeId = resourceType.ResourceTypeId,
                ForeignResourceId = 2,
            };
            var permission1 = new CAM.Data.Permission
            {
                PermissionId = CAM.Data.Permission.Editproject.Id,
                PermissionName = CAM.Data.Permission.Editproject.Value,
            };
            var permissionAssignment1 = new PermissionAssignment
            {
                IsAllowed = true,
                Permission = permission1,
                PermissionId = permission1.PermissionId,
                Principal = principal,
                PrincipalId = principal.PrincipalId,
                Resource = resource,
                ResourceId = resource.ResourceId
            };
            var permission2 = new CAM.Data.Permission
            {
                PermissionId = CAM.Data.Permission.Editprogram.Id,
                PermissionName = CAM.Data.Permission.Editprogram.Value,
            };
            var permissionAssignment2 = new PermissionAssignment
            {
                IsAllowed = true,
                Permission = permission2,
                PermissionId = permission2.PermissionId,
                Principal = principal,
                PrincipalId = principal.PrincipalId,
                Resource = resource,
                ResourceId = resource.ResourceId
            };

            context.Principals.Add(principal);
            context.UserAccounts.Add(userAccount);
            context.ResourceTypes.Add(resourceType);
            context.Resources.Add(resource);
            context.Permissions.Add(permission1);
            context.PermissionAssignments.Add(permissionAssignment1);
            context.Permissions.Add(permission2);
            context.PermissionAssignments.Add(permissionAssignment2);

            var defaultSort = new ExpressionSorter<ResourceAuthorization>(x => x.PermissionName, SortDirection.Descending);
            var queryOperator = new QueryableOperator<ResourceAuthorization>(0, 10, defaultSort);
            queryOperator.Filters.Add(new ExpressionFilter<ResourceAuthorization>(x => x.PermissionName, ComparisonType.Equal, permission2.PermissionName));
            Action<PagedQueryResults<ResourceAuthorization>> tester = (results) =>
            {
                Assert.AreEqual(1, results.Total);
                Assert.AreEqual(1, results.Results.Count);
                Assert.AreEqual(permission2.PermissionId, results.Results.First().PermissionId);
            };
            var serviceResults = service.GetResourceAuthorizations(queryOperator);
            var serviceResultsAsync = await service.GetResourceAuthorizationsAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetResourceAuthorization_Paged()
        {
            var principal = new Principal
            {
                PrincipalId = 1
            };
            var userAccount = new UserAccount
            {
                DisplayName = "display",
                Principal = principal,
                PrincipalId = principal.PrincipalId
            };
            principal.UserAccount = userAccount;
            userAccount.Principal = principal;

            var resourceType = new ResourceType
            {
                ResourceTypeName = ResourceType.Project.Value,
                ResourceTypeId = ResourceType.Project.Id
            };
            var resource = new Resource
            {
                ResourceId = 1,
                ResourceType = resourceType,
                ResourceTypeId = resourceType.ResourceTypeId,
                ForeignResourceId = 2,
            };
            var permission1 = new CAM.Data.Permission
            {
                PermissionId = CAM.Data.Permission.Editproject.Id,
                PermissionName = CAM.Data.Permission.Editproject.Value,
            };
            var permissionAssignment1 = new PermissionAssignment
            {
                IsAllowed = true,
                Permission = permission1,
                PermissionId = permission1.PermissionId,
                Principal = principal,
                PrincipalId = principal.PrincipalId,
                Resource = resource,
                ResourceId = resource.ResourceId
            };
            var permission2 = new CAM.Data.Permission
            {
                PermissionId = CAM.Data.Permission.Editprogram.Id,
                PermissionName = CAM.Data.Permission.Editprogram.Value,
            };
            var permissionAssignment2 = new PermissionAssignment
            {
                IsAllowed = true,
                Permission = permission2,
                PermissionId = permission2.PermissionId,
                Principal = principal,
                PrincipalId = principal.PrincipalId,
                Resource = resource,
                ResourceId = resource.ResourceId
            };

            context.Principals.Add(principal);
            context.UserAccounts.Add(userAccount);
            context.ResourceTypes.Add(resourceType);
            context.Resources.Add(resource);
            context.Permissions.Add(permission1);
            context.PermissionAssignments.Add(permissionAssignment1);
            context.Permissions.Add(permission2);
            context.PermissionAssignments.Add(permissionAssignment2);

            var defaultSort = new ExpressionSorter<ResourceAuthorization>(x => x.PermissionName, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<ResourceAuthorization>(0, 1, defaultSort);
            Action<PagedQueryResults<ResourceAuthorization>> tester = (results) =>
            {
                Assert.AreEqual(2, results.Total);
                Assert.AreEqual(1, results.Results.Count);
                Assert.AreEqual(permission2.PermissionId, results.Results.First().PermissionId);
            };
            var serviceResults = service.GetResourceAuthorizations(queryOperator);
            var serviceResultsAsync = await service.GetResourceAuthorizationsAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }
        #endregion
    }
}
