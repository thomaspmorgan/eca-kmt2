﻿using System;
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
using CAM.Business.Queries.Models;
using ECA.Core.Exceptions;
using ECA.Core.Data;

namespace CAM.Business.Test.Service
{
    public class TestPermissableResource : IPermissable
    {
        public int Id { get; set; }

        public int? ParentId { get; set; }

        public PermissableType PermissableType { get; set; }

        public PermissableType ParentPermissableType { get; set; }

        public int GetId()
        {
            return this.Id;
        }

        public PermissableType GetPermissableType()
        {
            return this.PermissableType;
        }

        public int? GetParentId()
        {
            return this.ParentId;
        }

        public PermissableType GetParentPermissableType()
        {
            return this.ParentPermissableType;
        }
    }

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
            var parentResourceId = 4;
            var parentForeignResourceId = 5;
            var parentResourceTypeId = 6;

            var foreignResourceCache = new ForeignResourceCache(foreignResourceId, resourceId, resourceTypeId, parentForeignResourceId, parentResourceId, parentResourceTypeId);
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
            var parentResourceId = 4;
            var parentForeignResourceId = 5;
            var parentResourceTypeId = 6;

            var resourceCache = new ForeignResourceCache(foreignResourceId, resourceId, resourceTypeId, parentForeignResourceId, parentResourceId, parentResourceTypeId);
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
            var parentResourceId = 4;
            var parentForeignResourceId = 5;
            var parentResourceTypeId = 6;
            var resourceCache = new ForeignResourceCache(foreignResourceId, resourceId, resourceTypeId, parentForeignResourceId, parentResourceId, parentResourceTypeId);
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
            var parentResourceId = 4;
            var parentForeignResourceId = 5;
            var parentResourceTypeId = 6;
            var cacheItem = service.Add(foreignResourceId, resourceId, resourceTypeId, parentForeignResourceId, parentResourceId, parentResourceTypeId);
            Assert.IsNotNull(cacheItem);
            var testCacheItem = service.GetForeignResourceCache(foreignResourceId, resourceTypeId);
            Assert.IsTrue(object.ReferenceEquals(cacheItem.Value, testCacheItem));

            Assert.AreEqual(foreignResourceId, testCacheItem.ForeignResourceId);
            Assert.AreEqual(resourceTypeId, testCacheItem.ResourceTypeId);
            Assert.AreEqual(resourceId, testCacheItem.ResourceId);
            Assert.AreEqual(parentResourceId, testCacheItem.ParentResourceId);
            Assert.AreEqual(parentForeignResourceId, testCacheItem.ParentForeignResourceId);
            Assert.AreEqual(parentResourceTypeId, testCacheItem.ParentResourceTypeId);
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
            var parentResourceId = 4;
            var parentForeignResourceId = 5;
            var parentResourceTypeId = 6;
            var foreignResource = new ForeignResourceCache(foreignResourceId, resourceId, resourceTypeId, parentForeignResourceId, parentResourceId, parentResourceTypeId);
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
            var parentResourceId = 4;
            var parentForeignResourceId = 5;
            var parentResourceTypeId = 6;
            var foreignResource = new ForeignResourceCache(foreignResourceId, resourceId, resourceTypeId, parentForeignResourceId, parentResourceId, parentResourceTypeId);
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
        public async Task TestGetResourceIdByForeignResourceId_DoesNotHaveParent_NotCached()
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
            context.SetupActions.Add(() =>
            {
                cacheDictionary.Clear();
                context.Resources.Add(resource);
                context.ResourceTypes.Add(resourceType);
                Assert.IsFalse(service.IsCached(foreignResourceId, resourceType.ResourceTypeId));
            });
            Action<int?> tester = (testResourceId) =>
            {
                Assert.IsTrue(testResourceId.HasValue);
                Assert.AreEqual(resourceId, testResourceId.Value);
                Assert.IsTrue(service.IsCached(foreignResourceId, resourceType.ResourceTypeId));
                var cacheItem = cacheDictionary.First().Value as ForeignResourceCache;
                Assert.IsNotNull(cacheItem);
                Assert.AreEqual(foreignResourceId, cacheItem.ForeignResourceId);
                Assert.AreEqual(resourceId, cacheItem.ResourceId);
                Assert.AreEqual(resourceType.ResourceTypeId, cacheItem.ResourceTypeId);
                Assert.IsFalse(cacheItem.ParentForeignResourceId.HasValue);
                Assert.IsFalse(cacheItem.ParentResourceId.HasValue);
                Assert.IsFalse(cacheItem.ParentResourceTypeId.HasValue);
            };
            context.Revert();
            var serviceResult = service.GetResourceIdByForeignResourceId(foreignResourceId, resourceType.ResourceTypeId);
            tester(serviceResult);

            context.Revert();
            var serviceResultAsync = await service.GetResourceIdByForeignResourceIdAsync(foreignResourceId, resourceType.ResourceTypeId);
            tester(serviceResult);
        }

        [TestMethod]
        public async Task TestGetResourceIdByForeignResourceId_HasParent_NotCached()
        {

            var resourceType = new ResourceType
            {
                ResourceTypeId = ResourceType.Project.Id,
                ResourceTypeName = ResourceType.Project.Value
            };
            var parentResourceType = new ResourceType
            {
                ResourceTypeId = ResourceType.Program.Id,
                ResourceTypeName = ResourceType.Program.Value
            };

            var parentForeignResourceId = 2;
            var parentResourceId = 10;
            var parentResource = new Resource
            {
                ResourceTypeId = parentResourceType.ResourceTypeId,
                ResourceId = parentResourceId,
                ForeignResourceId = parentForeignResourceId,
                ResourceType = parentResourceType
            };

            var foreignResourceId = 1;
            var resourceId = 3;
            var resource = new Resource
            {
                ResourceId = resourceId,
                ResourceTypeId = resourceType.ResourceTypeId,
                ResourceType = resourceType,
                ForeignResourceId = foreignResourceId,
                ParentResourceId = parentResourceId,
                ParentResource = parentResource
            };

            context.SetupActions.Add(() =>
            {
                cacheDictionary.Clear();
                context.Resources.Add(resource);
                context.ResourceTypes.Add(resourceType);
                context.Resources.Add(parentResource);
                context.ResourceTypes.Add(parentResourceType);
                Assert.IsFalse(service.IsCached(foreignResourceId, resourceType.ResourceTypeId));
            });
            Action<int?> tester = (testResourceId) =>
            {
                Assert.IsTrue(testResourceId.HasValue);
                Assert.AreEqual(resourceId, testResourceId.Value);
                Assert.IsTrue(service.IsCached(foreignResourceId, resourceType.ResourceTypeId));
                var cacheItem = cacheDictionary.First().Value as ForeignResourceCache;
                Assert.IsNotNull(cacheItem);
                Assert.AreEqual(foreignResourceId, cacheItem.ForeignResourceId);
                Assert.AreEqual(resourceId, cacheItem.ResourceId);
                Assert.AreEqual(resourceType.ResourceTypeId, cacheItem.ResourceTypeId);
                Assert.AreEqual(parentResource.ResourceId, cacheItem.ParentResourceId);
                Assert.AreEqual(parentResourceType.ResourceTypeId, cacheItem.ParentResourceTypeId);
                Assert.AreEqual(parentResource.ForeignResourceId, cacheItem.ParentForeignResourceId);
            };
            context.Revert();
            var serviceResult = service.GetResourceIdByForeignResourceId(foreignResourceId, resourceType.ResourceTypeId);
            tester(serviceResult);

            context.Revert();
            var serviceResultAsync = await service.GetResourceIdByForeignResourceIdAsync(foreignResourceId, resourceType.ResourceTypeId);
            tester(serviceResult);
        }

        [TestMethod]
        public async Task TestGetResourceIdByForeignResourceId_IsCached()
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
            service.Add(foreignResourceId, resourceId, resourceType.ResourceTypeId, null, null, null);
            Assert.IsTrue(service.IsCached(foreignResourceId, resourceType.ResourceTypeId));

            Action<int?> tester = (testResourceId) =>
            {
                Assert.IsTrue(testResourceId.HasValue);
                Assert.AreEqual(resourceId, testResourceId.Value);

                //if we know there are no resources or resource types in db we are pulling from cache
                Assert.AreEqual(0, context.Resources.Count());
                Assert.AreEqual(0, context.ResourceTypes.Count());
            };

            var serviceResult = service.GetResourceIdByForeignResourceId(foreignResourceId, resourceType.ResourceTypeId);
            var serviceResultAsync = await service.GetResourceIdByForeignResourceIdAsync(foreignResourceId, resourceType.ResourceTypeId);
            tester(serviceResult);
            tester(serviceResultAsync);

        }

        [TestMethod]
        public async Task TestGetResourceIdByForeignResourceId_ForeignResourceDoesNotExist()
        {
            var resourceTypeId = ResourceType.Application.Id;
            var foreignResourceId = 1;

            Assert.AreEqual(0, context.Resources.Count());
            Assert.AreEqual(0, context.ResourceTypes.Count());
            Assert.AreEqual(0, cacheDictionary.Count);
            Assert.IsFalse(service.IsCached(foreignResourceId, resourceTypeId));

            Action<int?> tester = (testResourceId) =>
            {
                Assert.IsFalse(testResourceId.HasValue);

                Assert.AreEqual(0, context.Resources.Count());
                Assert.AreEqual(0, context.ResourceTypes.Count());
                Assert.AreEqual(0, cacheDictionary.Count);
            };

            var serviceResult = service.GetResourceIdByForeignResourceId(foreignResourceId, resourceTypeId);
            var serviceResultAsync = await service.GetResourceIdByForeignResourceIdAsync(foreignResourceId, resourceTypeId);
            tester(serviceResult);
            tester(serviceResultAsync);
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
        public async Task TestGetResourceAuthorizationInfoDTO()
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
                PermissionId = CAM.Data.Permission.EditProject.Id,
                PermissionName = CAM.Data.Permission.EditProject.Value,
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

            context.Principals.Add(principal);
            context.UserAccounts.Add(userAccount);
            context.ResourceTypes.Add(resourceType);
            context.Resources.Add(resource);
            context.Permissions.Add(permission1);
            context.PermissionAssignments.Add(permissionAssignment1);

            Action<ResourceAuthorizationInfoDTO> tester = (dto) =>
            {
                Assert.IsNotNull(dto);
            };
            var serviceResult = service.GetResourceAuthorizationInfoDTO(resource.ResourceType.ResourceTypeName, resource.ForeignResourceId);
            var serviceResultAsync = await service.GetResourceAuthorizationInfoDTOAsync(resource.ResourceType.ResourceTypeName, resource.ForeignResourceId);
            tester(serviceResult);
            tester(serviceResultAsync);
        }

        [TestMethod]
        public async Task TestGetResourceAuthorizationInfoDTO_ResourceDoesNotExist()
        {
            Action<ResourceAuthorizationInfoDTO> tester = (dto) =>
            {
                Assert.IsNull(dto);
            };
            var serviceResult = service.GetResourceAuthorizationInfoDTO(ResourceType.Project.Value, 1);
            var serviceResultAsync = await service.GetResourceAuthorizationInfoDTOAsync(ResourceType.Project.Value, 1);
            tester(serviceResult);
            tester(serviceResultAsync);
        }

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
                PermissionId = CAM.Data.Permission.EditProject.Id,
                PermissionName = CAM.Data.Permission.EditProject.Value,
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
                PermissionId = CAM.Data.Permission.EditProgram.Id,
                PermissionName = CAM.Data.Permission.EditProgram.Value,
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
                PermissionId = CAM.Data.Permission.EditProject.Id,
                PermissionName = CAM.Data.Permission.EditProject.Value,
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
                PermissionId = CAM.Data.Permission.EditProgram.Id,
                PermissionName = CAM.Data.Permission.EditProgram.Value,
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
                PermissionId = CAM.Data.Permission.EditProject.Id,
                PermissionName = CAM.Data.Permission.EditProject.Value,
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
                PermissionId = CAM.Data.Permission.EditProgram.Id,
                PermissionName = CAM.Data.Permission.EditProgram.Value,
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
                PermissionId = CAM.Data.Permission.EditProject.Id,
                PermissionName = CAM.Data.Permission.EditProject.Value,
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
                PermissionId = CAM.Data.Permission.EditProgram.Id,
                PermissionName = CAM.Data.Permission.EditProgram.Value,
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

        [TestMethod]
        public async Task TestGetResourcePermissions_ForeignResourceIdIsNull()
        {
            var resourceType = new ResourceType
            {
                ResourceTypeName = ResourceType.Project.Value,
                ResourceTypeId = ResourceType.Project.Id
            };
            var permission = new CAM.Data.Permission
            {
                PermissionId = CAM.Data.Permission.EditProject.Id,
                PermissionName = CAM.Data.Permission.EditProject.Value,
                PermissionDescription = "desc",
                ResourceType = resourceType,
                ResourceTypeId = resourceType.ResourceTypeId
            };
            context.Permissions.Add(permission);
            context.ResourceTypes.Add(resourceType);

            Action<List<ResourcePermissionDTO>> tester = (results) =>
            {
                Assert.AreEqual(1, results.Count);
                var firstResult = results.First();
                Assert.AreEqual(permission.PermissionId, firstResult.PermissionId);
                Assert.AreEqual(permission.PermissionName, firstResult.PermissionName);
                Assert.AreEqual(permission.PermissionDescription, firstResult.PermissionDescription);
            };

            var serviceResults = service.GetResourcePermissions(resourceType.ResourceTypeName, null);
            var serviceResultsAsync = await service.GetResourcePermissionsAsync(resourceType.ResourceTypeName, null);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetResourcePermissions_ForeignResourceIdIsNotNull()
        {
            var resourceType = new ResourceType
            {
                ResourceTypeName = ResourceType.Project.Value,
                ResourceTypeId = ResourceType.Project.Id
            };
            var permission = new CAM.Data.Permission
            {
                PermissionId = CAM.Data.Permission.EditProject.Id,
                PermissionName = CAM.Data.Permission.EditProject.Value,
                PermissionDescription = "desc",
                ResourceType = resourceType,
                ResourceTypeId = resourceType.ResourceTypeId
            };
            var resource = new Resource
            {
                ForeignResourceId = 1,
                ResourceType = resourceType,
                ResourceTypeId = resourceType.ResourceTypeId
            };
            permission.Resource = resource;
            permission.ResourceId = resource.ResourceId;
            context.Resources.Add(resource);
            context.Permissions.Add(permission);
            context.ResourceTypes.Add(resourceType);

            Action<List<ResourcePermissionDTO>> tester = (results) =>
            {
                Assert.AreEqual(1, results.Count);
                var firstResult = results.First();
            };

            var serviceResults = service.GetResourcePermissions(resourceType.ResourceTypeName, resource.ForeignResourceId);
            var serviceResultsAsync = await service.GetResourcePermissionsAsync(resourceType.ResourceTypeName, resource.ForeignResourceId);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetResourcePermissions_ResourceTypeIsNotKnown()
        {
            var resourceType = "rt";
            Func<Task> f = async () =>
            {
                await service.GetResourcePermissionsAsync(resourceType, null);
            };

            service.Invoking(x => x.GetResourcePermissions(resourceType, null)).ShouldThrow<UnknownStaticLookupException>()
                .WithMessage(String.Format("The resource type [{0}] is not known.", resourceType));
            f.ShouldThrow<UnknownStaticLookupException>()
                .WithMessage(String.Format("The resource type [{0}] is not known.", resourceType));

        }

        [TestMethod]
        public async Task TestGetResourceTypes()
        {
            var resourceType = new ResourceType
            {
                ResourceTypeName = ResourceType.Project.Value,
                ResourceTypeId = ResourceType.Project.Id
            };
            context.ResourceTypes.Add(resourceType);
            Action<List<ResourceTypeDTO>> tester = (results) =>
            {
                Assert.AreEqual(1, results.Count);
                var first = results.First();
                Assert.AreEqual(resourceType.ResourceTypeId, first.Id);
                Assert.AreEqual(resourceType.ResourceTypeName, first.Name);
            };

            var serviceResults = service.GetResourceTypes();
            var serviceResultsAsync = await service.GetResourceTypesAsync();
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetResourceTypes_NoResourceTypes()
        {
            Action<List<ResourceTypeDTO>> tester = (results) =>
            {
                Assert.AreEqual(0, results.Count);
            };

            var serviceResults = service.GetResourceTypes();
            var serviceResultsAsync = await service.GetResourceTypesAsync();
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        #region IPermissableService


        [TestMethod]
        public void TestRemoveFromCache_HasParent()
        {
            var id = 1;
            var parentId = 2;
            var entity = new TestPermissableResource
            {
                Id = id,
                ParentId = parentId,
                ParentPermissableType = PermissableType.Program,
                PermissableType = PermissableType.Project
            };
            service.Add(entity.Id, 1, entity.PermissableType.GetResourceTypeId(), parentId, 2, entity.ParentPermissableType.GetResourceTypeId());
            service.Add(entity.ParentId.Value, 3, entity.ParentPermissableType.GetResourceTypeId(), null, null, null);
            Assert.AreEqual(2, cacheDictionary.Count);
            service.RemoveFromCache(entity);
            Assert.AreEqual(0, cacheDictionary.Count);
        }

        [TestMethod]
        public void TestRemoveFromCache_DoesNotHaveParent()
        {
            var id = 1;
            var entity = new TestPermissableResource
            {
                Id = id,
                PermissableType = PermissableType.Project
            };
            service.Add(entity.Id, 1, entity.PermissableType.GetResourceTypeId(), null, null, null);
            Assert.AreEqual(1, cacheDictionary.Count);
            service.RemoveFromCache(entity);
            Assert.AreEqual(0, cacheDictionary.Count);
        }

        [TestMethod]
        public void TestRemoveFromCache_DifferentResourcesCached()
        {
            var id = 1;
            var parentId = 2;
            var entity = new TestPermissableResource
            {
                Id = id,
                ParentId = parentId,
                ParentPermissableType = PermissableType.Program,
                PermissableType = PermissableType.Project
            };
            service.Add(entity.Id, 1, entity.PermissableType.GetResourceTypeId(), parentId, 2, entity.ParentPermissableType.GetResourceTypeId());
            service.Add(entity.ParentId.Value, 3, entity.ParentPermissableType.GetResourceTypeId(), null, null, null);
            Assert.AreEqual(2, cacheDictionary.Count);

            var differentEntity = new TestPermissableResource
            {
                Id = id - 1,
                PermissableType = PermissableType.Project
            };
            service.RemoveFromCache(differentEntity);
            Assert.AreEqual(2, cacheDictionary.Count);
        }

        [TestMethod]
        public async Task TestOnAdded_HasNewParent()
        {
            var id = 1;
            var parentId = 2;
            var entity = new TestPermissableResource
            {
                Id = id,
                ParentId = parentId,
                ParentPermissableType = PermissableType.Program,
                PermissableType = PermissableType.Project
            };
            var list = new List<IPermissable> { entity };
            Assert.AreEqual(0, context.Resources.Count());
            Assert.AreEqual(0, context.ResourceTypes.Count());

            context.SetupActions.Add(() =>
            {
                cacheDictionary.Clear();
            });

            Action tester = () =>
            {
                Assert.AreEqual(2, context.Resources.Count());
                var parent = context.Resources.Where(x => x.ForeignResourceId == parentId).First();
                var child = context.Resources.Where(x => x.ForeignResourceId == id).First();

                Assert.AreEqual(parentId, parent.ForeignResourceId);
                Assert.AreEqual(PermissableType.Program.GetResourceTypeId(), parent.ResourceTypeId);

                Assert.AreEqual(entity.Id, child.ForeignResourceId);
                Assert.AreEqual(PermissableType.Project.GetResourceTypeId(), child.ResourceTypeId);

                Assert.AreEqual(0, cacheDictionary.Count);
            };
            context.Revert();
            service.OnAdded(list);
            tester();

            context.Revert();
            await service.OnAddedAsync(list);
            tester();
        }

        [TestMethod]
        public async Task TestOnAdded_HasExistingParent()
        {
            var id = 1;
            var parentId = 2;
            var entity = new TestPermissableResource
            {
                Id = id,
                ParentId = parentId,
                ParentPermissableType = PermissableType.Program,
                PermissableType = PermissableType.Project
            };

            var programResourceType = new ResourceType
            {
                ResourceTypeId = ResourceType.Program.Id,
                ResourceTypeName = ResourceType.Program.Value
            };
            var parentResource = new Resource
            {
                ForeignResourceId = parentId,
                ResourceId = 10,
                ResourceTypeId = programResourceType.ResourceTypeId,
                ResourceType = programResourceType

            };

            var list = new List<IPermissable> { entity };

            context.SetupActions.Add(() =>
            {
                cacheDictionary.Clear();
                context.Resources.Add(parentResource);
                context.ResourceTypes.Add(programResourceType);
                Assert.AreEqual(1, context.Resources.Count());
                Assert.AreEqual(1, context.ResourceTypes.Count());
            });

            Action tester = () =>
            {
                Assert.AreEqual(2, context.Resources.Count());
                var parent = context.Resources.Where(x => x.ForeignResourceId == parentId).First();
                var child = context.Resources.Where(x => x.ForeignResourceId == id).First();

                Assert.AreEqual(parentId, parent.ForeignResourceId);
                Assert.AreEqual(PermissableType.Program.GetResourceTypeId(), parent.ResourceTypeId);

                Assert.AreEqual(entity.Id, child.ForeignResourceId);
                Assert.AreEqual(PermissableType.Project.GetResourceTypeId(), child.ResourceTypeId);
                Assert.AreEqual(parentResource.ResourceId, child.ParentResourceId);
                Assert.IsTrue(Object.ReferenceEquals(parentResource, child.ParentResource));
            };
            context.Revert();
            service.OnAdded(list);
            tester();

            context.Revert();
            await service.OnAddedAsync(list);
            tester();
        }

        [TestMethod]
        public async Task TestOnAdded_DoesNotHaveParent()
        {
            var id = 1;
            var entity = new TestPermissableResource
            {
                Id = id,
                PermissableType = PermissableType.Project
            };
            var list = new List<IPermissable> { entity };
            Assert.AreEqual(0, context.Resources.Count());
            Assert.AreEqual(0, context.ResourceTypes.Count());

            context.SetupActions.Add(() =>
            {
                cacheDictionary.Clear();
            });

            Action tester = () =>
            {
                Assert.AreEqual(1, context.Resources.Count());
                var resource = context.Resources.First();

                Assert.AreEqual(entity.Id, resource.ForeignResourceId);
                Assert.AreEqual(PermissableType.Project.GetResourceTypeId(), resource.ResourceTypeId);

                Assert.AreEqual(0, cacheDictionary.Count);
            };

            service.OnAdded(list);
            tester();

            context.Revert();
            await service.OnAddedAsync(list);
            tester();
        }

        [TestMethod]
        public async Task TestOnAdded_DoesNotHaveParent_ShouldRemoveFromCache()
        {
            var id = 1;
            var entity = new TestPermissableResource
            {
                Id = id,
                PermissableType = PermissableType.Project
            };
            var list = new List<IPermissable> { entity };
            Assert.AreEqual(0, context.Resources.Count());
            Assert.AreEqual(0, context.ResourceTypes.Count());

            context.SetupActions.Add(() =>
            {
                cacheDictionary.Add(service.GetKey(id, entity.PermissableType.GetResourceTypeId()), 1);
            });

            Action tester = () =>
            {
                Assert.AreEqual(0, cacheDictionary.Count);
            };
            context.Revert();
            Assert.AreEqual(1, cacheDictionary.Count);
            service.OnAdded(list);
            tester();

            context.Revert();
            Assert.AreEqual(1, cacheDictionary.Count);
            await service.OnAddedAsync(list);
            tester();
        }

        [TestMethod]
        public async Task TestOnAdded_HasParent_ShouldRemoveFromCache()
        {
            var id = 1;
            var parentId = 2;
            var entity = new TestPermissableResource
            {
                Id = id,
                ParentId = parentId,
                ParentPermissableType = PermissableType.Program,
                PermissableType = PermissableType.Project
            };
            var list = new List<IPermissable> { entity };
            Assert.AreEqual(0, context.Resources.Count());
            Assert.AreEqual(0, context.ResourceTypes.Count());

            context.SetupActions.Add(() =>
            {
                cacheDictionary.Add(service.GetKey(id, entity.PermissableType.GetResourceTypeId()), 1);
                cacheDictionary.Add(service.GetKey(parentId, entity.ParentPermissableType.GetResourceTypeId()), 2);
            });

            Action tester = () =>
            {
                Assert.AreEqual(0, cacheDictionary.Count);
            };
            context.Revert();
            Assert.AreEqual(2, cacheDictionary.Count);
            service.OnAdded(list);
            tester();

            context.Revert();
            Assert.AreEqual(2, cacheDictionary.Count);
            await service.OnAddedAsync(list);
            tester();
        }
        #endregion
    }
}
