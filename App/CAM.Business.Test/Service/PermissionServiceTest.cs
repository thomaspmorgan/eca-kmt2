using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Runtime.Caching;
using System.Collections.Generic;
using System.Linq;
using CAM.Business.Service;
using CAM.Data;
using System.Threading.Tasks;

namespace CAM.Business.Test.Service
{
    [TestClass]
    public class PermissionServiceTest
    {
        private Mock<ObjectCache> cache;
        private IDictionary<string, object> cacheDictionary;
        private int expectedTimeToLive = 10;

        private TestInMemoryCamModel context;
        private PermissionService service;

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
            service = new PermissionService(context, cache.Object, expectedTimeToLive);
        }

        #region Caching

        [TestMethod]
        public void TestGetKey_PermissionModel()
        {
            var model = new PermissionModel
            {
                Id = Permission.EditOffice.Id,
                Name = Permission.EditOffice.Value
            };
            var expectedKey = String.Format(PermissionService.PERMISSION_CACHE_KEY_FORMAT, model.Id, model.Name);
            Assert.AreEqual(expectedKey, service.GetKey(model));
        }

        [TestMethod]
        public void TestGetKey_PermissionId()
        {
            var model = new PermissionModel
            {
                Id = Permission.EditOffice.Id,
                Name = Permission.EditOffice.Value
            };
            var expectedKey = String.Format(PermissionService.PERMISSION_CACHE_KEY_FORMAT, model.Id, model.Name);
            Assert.AreEqual(expectedKey, service.GetKey(model.Id));
        }

        [TestMethod]
        public void TestGetKey_PermissionName()
        {
            var model = new PermissionModel
            {
                Id = Permission.EditOffice.Id,
                Name = Permission.EditOffice.Value
            };
            var expectedKey = String.Format(PermissionService.PERMISSION_CACHE_KEY_FORMAT, model.Id, model.Name);
            Assert.AreEqual(expectedKey, service.GetKey(model.Name));
        }

        [TestMethod]
        public void TestGetKey_PermissionIdAndName()
        {
            var model = new PermissionModel
            {
                Id = Permission.EditOffice.Id,
                Name = Permission.EditOffice.Value
            };
            var expectedKey = String.Format(PermissionService.PERMISSION_CACHE_KEY_FORMAT, model.Id, model.Name);
            Assert.AreEqual(expectedKey, service.GetKey(model.Id, model.Name));
        }

        [TestMethod]
        public void TestGetCacheItemPolicy()
        {
            var policy = service.GetCacheItemPolicy();
            Assert.IsNotNull(policy.SlidingExpiration);
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
        public void TestAdd()
        {
            Assert.AreEqual(0, cacheDictionary.Count);
            var model = new PermissionModel
            {
                Id = Permission.EditOffice.Id,
                Name = Permission.EditOffice.Value,
                ParentPermissionId = 2,
                ParentResourceTypeId = 3,
                ResourceTypeId = 5
            };
            var cacheItem = service.Add(model);
            Assert.IsNotNull(cacheItem);
            Assert.IsTrue(Object.ReferenceEquals(cacheItem.Value, model));
            Assert.AreEqual(1, cacheDictionary.Count);
        }
        #endregion

        #region Get Permissions
        [TestMethod]
        public void TestCreateGetPermissionsByNameQuery_DoesNotHaveParent()
        {
            var resourceType = new ResourceType
            {
                ResourceTypeId = ResourceType.Project.Id,
                ResourceTypeName = ResourceType.Project.Value
            };
            var permission = new Permission
            {
                PermissionId = Permission.EditProject.Id,
                PermissionName = Permission.EditProject.Value,
                ResourceType = resourceType,
                ResourceTypeId = resourceType.ResourceTypeId
            };
            context.ResourceTypes.Add(resourceType);
            context.Permissions.Add(permission);

            var models = service.CreateGetPermissionModelsByNameQuery(permission.PermissionName).ToList();
            Assert.AreEqual(1, models.Count);
            var firstModel = models.First();
            Assert.AreEqual(resourceType.ResourceTypeId, firstModel.ResourceTypeId);
            Assert.AreEqual(permission.PermissionId, firstModel.Id);
            Assert.AreEqual(permission.PermissionName, firstModel.Name);
            Assert.IsFalse(firstModel.ParentPermissionId.HasValue);
            Assert.IsFalse(firstModel.ParentResourceTypeId.HasValue);
        }

        [TestMethod]
        public void TestCreateGetPermissionsByPermissionIdQuery_DoesNotHaveParent()
        {
            var resourceType = new ResourceType
            {
                ResourceTypeId = ResourceType.Project.Id,
                ResourceTypeName = ResourceType.Project.Value
            };
            var permission = new Permission
            {
                PermissionId = Permission.EditProject.Id,
                PermissionName = Permission.EditProject.Value,
                ResourceType = resourceType,
                ResourceTypeId = resourceType.ResourceTypeId
            };
            context.ResourceTypes.Add(resourceType);
            context.Permissions.Add(permission);

            var models = service.CreateGetPermissionModelsByPermissionIdQuery(permission.PermissionId).ToList();
            Assert.AreEqual(1, models.Count);
            var firstModel = models.First();
            Assert.AreEqual(resourceType.ResourceTypeId, firstModel.ResourceTypeId);
            Assert.AreEqual(permission.PermissionId, firstModel.Id);
            Assert.AreEqual(permission.PermissionName, firstModel.Name);
            Assert.IsFalse(firstModel.ParentPermissionId.HasValue);
            Assert.IsFalse(firstModel.ParentResourceTypeId.HasValue);
        }

        [TestMethod]
        public void TestCreateGetPermissionsByNameQuery_PermissionDoesNotExist()
        {
            var models = service.CreateGetPermissionModelsByNameQuery("permission").ToList();
            Assert.AreEqual(0, models.Count);
        }

        [TestMethod]
        public void TestCreateGetPermissionsByPermissionIdQuery_PermissionDoesNotExist()
        {
            var models = service.CreateGetPermissionModelsByPermissionIdQuery(0).ToList();
            Assert.AreEqual(0, models.Count);
        }


        [TestMethod]
        public void TestCreateGetPermissionsByNameQuery_HasParent()
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
            var permission = new Permission
            {
                PermissionId = Permission.EditProject.Id,
                PermissionName = Permission.EditProject.Value,
                ResourceType = resourceType,
                ResourceTypeId = resourceType.ResourceTypeId,
                ParentResourceTypeId = parentResourceType.ResourceTypeId,
                ParentResourceType = parentResourceType
            };
            var parentPermission = new Permission
            {
                PermissionId = Permission.EditProgram.Id,
                PermissionName = Permission.EditProgram.Value,
                ResourceType = parentResourceType,
                ResourceTypeId = parentResourceType.ResourceTypeId,
            };
            context.ResourceTypes.Add(resourceType);
            context.ResourceTypes.Add(parentResourceType);
            context.Permissions.Add(permission);
            context.Permissions.Add(parentPermission);

            var models = service.CreateGetPermissionModelsByNameQuery(permission.PermissionName).ToList();
            Assert.AreEqual(1, models.Count);
            var firstModel = models.First();
            Assert.AreEqual(resourceType.ResourceTypeId, firstModel.ResourceTypeId);
            Assert.AreEqual(permission.PermissionId, firstModel.Id);
            Assert.AreEqual(permission.PermissionName, firstModel.Name);
            Assert.AreEqual(parentResourceType.ResourceTypeId, firstModel.ParentResourceTypeId);
            Assert.AreEqual(parentPermission.PermissionId, firstModel.ParentPermissionId);
        }

        [TestMethod]
        public void TestCreateGetPermissionsByPermissionIdQuery_HasParent()
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
            var permission = new Permission
            {
                PermissionId = Permission.EditProject.Id,
                PermissionName = Permission.EditProject.Value,
                ResourceType = resourceType,
                ResourceTypeId = resourceType.ResourceTypeId,
                ParentResourceTypeId = parentResourceType.ResourceTypeId,
                ParentResourceType = parentResourceType
            };
            var parentPermission = new Permission
            {
                PermissionId = Permission.EditProgram.Id,
                PermissionName = Permission.EditProgram.Value,
                ResourceType = parentResourceType,
                ResourceTypeId = parentResourceType.ResourceTypeId,
            };
            context.ResourceTypes.Add(resourceType);
            context.ResourceTypes.Add(parentResourceType);
            context.Permissions.Add(permission);
            context.Permissions.Add(parentPermission);

            var models = service.CreateGetPermissionModelsByPermissionIdQuery(permission.PermissionId).ToList();
            Assert.AreEqual(1, models.Count);
            var firstModel = models.First();
            Assert.AreEqual(resourceType.ResourceTypeId, firstModel.ResourceTypeId);
            Assert.AreEqual(permission.PermissionId, firstModel.Id);
            Assert.AreEqual(permission.PermissionName, firstModel.Name);
            Assert.AreEqual(parentResourceType.ResourceTypeId, firstModel.ParentResourceTypeId);
            Assert.AreEqual(parentPermission.PermissionId, firstModel.ParentPermissionId);
        }

        [TestMethod]
        public async Task TestGetPermissionByName_Cached()
        {
            var permission = new PermissionModel
            {
                Id = Permission.EditProject.Id,
                Name = Permission.EditProject.Value,
            };
            context.SetupActions.Add(() =>
            {
                cacheDictionary.Clear();
                service.Add(permission);
            });
            Action<PermissionModel> tester = (testModel) =>
            {
                Assert.IsTrue(Object.ReferenceEquals(permission, testModel));
                Assert.AreEqual(0, context.Permissions.Count());
            };
            context.Revert();
            var results = service.GetPermissionByName(permission.Name);
            tester(results);

            context.Revert();
            var resultsAsync = await service.GetPermissionByNameAsync(permission.Name);
            tester(resultsAsync);
        }

        [TestMethod]
        public async Task TestGetPermissionByPermissionId_Cached()
        {
            var permission = new PermissionModel
            {
                Id = Permission.EditProject.Id,
                Name = Permission.EditProject.Value,
            };
            context.SetupActions.Add(() =>
            {
                cacheDictionary.Clear();
                service.Add(permission);
            });
            Action<PermissionModel> tester = (testModel) =>
            {
                Assert.IsTrue(Object.ReferenceEquals(permission, testModel));
                Assert.AreEqual(0, context.Permissions.Count());
            };
            context.Revert();
            var results = service.GetPermissionById(permission.Id);
            tester(results);

            context.Revert();
            var resultsAsync = await service.GetPermissionByIdAsync(permission.Id);
            tester(resultsAsync);
        }

        [TestMethod]
        public async Task TestGetPermissionByName_NotCached()
        {
            var resourceType = new ResourceType
            {
                ResourceTypeId = ResourceType.Project.Id,
                ResourceTypeName = ResourceType.Project.Value
            };
            var permission = new Permission
            {
                PermissionId = Permission.EditProject.Id,
                PermissionName = Permission.EditProject.Value
            };

            context.SetupActions.Add(() =>
            {
                cacheDictionary.Clear();
                context.Permissions.Add(permission);
                context.ResourceTypes.Add(resourceType);
            });
            Action<PermissionModel> tester = (testModel) =>
            {
                Assert.AreEqual(1, cacheDictionary.Count);
                Assert.IsTrue(Object.ReferenceEquals(cacheDictionary.Values.First(), testModel));
            };
            context.Revert();
            var results = service.GetPermissionByName(permission.PermissionName);
            tester(results);

            context.Revert();
            var resultsAsync = await service.GetPermissionByNameAsync(permission.PermissionName);
            tester(resultsAsync);
        }

        [TestMethod]
        public async Task TestGetPermissionByPermissionId_NotCached()
        {
            var resourceType = new ResourceType
            {
                ResourceTypeId = ResourceType.Project.Id,
                ResourceTypeName = ResourceType.Project.Value
            };
            var permission = new Permission
            {
                PermissionId = Permission.EditProject.Id,
                PermissionName = Permission.EditProject.Value
            };

            context.SetupActions.Add(() =>
            {
                cacheDictionary.Clear();
                context.Permissions.Add(permission);
                context.ResourceTypes.Add(resourceType);
            });
            Action<PermissionModel> tester = (testModel) =>
            {
                Assert.AreEqual(1, cacheDictionary.Count);
                Assert.IsTrue(Object.ReferenceEquals(cacheDictionary.Values.First(), testModel));
            };
            context.Revert();
            var results = service.GetPermissionById(permission.PermissionId);
            tester(results);

            context.Revert();
            var resultsAsync = await service.GetPermissionByIdAsync(permission.PermissionId);
            tester(resultsAsync);
        }

        #endregion

        #region Get User Permissions

        [TestMethod]
        public void TestCreateGetAllowedPermissionsByPrincipalIdQuery_PermissionIsAssigned()
        {
            var principal = new Principal
            {
                PrincipalId = 1
            };
            var userAccount = new UserAccount
            {
                DisplayName = "display",
                Principal = principal,
                PrincipalId = principal.PrincipalId,
                EmailAddress = "someone@isp.com"
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
            var permission = new CAM.Data.Permission
            {
                PermissionId = CAM.Data.Permission.EditProject.Id,
                PermissionName = CAM.Data.Permission.EditProject.Value,
                PermissionDescription = "desc"
            };
            var permissionAssignment = new PermissionAssignment
            {
                IsAllowed = true,
                Permission = permission,
                PermissionId = permission.PermissionId,
                Principal = principal,
                PrincipalId = principal.PrincipalId,
                Resource = resource,
                ResourceId = resource.ResourceId,
                AssignedOn = DateTimeOffset.UtcNow,
            };

            context.Principals.Add(principal);
            context.UserAccounts.Add(userAccount);
            context.ResourceTypes.Add(resourceType);
            context.Resources.Add(resource);
            context.Permissions.Add(permission);
            context.PermissionAssignments.Add(permissionAssignment);

            Action<IList<IPermission>> tester = (testPermissions) =>
            {
                Assert.AreEqual(1, testPermissions.Count);
                var firstPermission = testPermissions.First();
                Assert.AreEqual(principal.PrincipalId, firstPermission.PrincipalId);
                Assert.IsTrue(firstPermission.IsAllowed);
                Assert.AreEqual(resource.ResourceId, firstPermission.ResourceId);
                Assert.AreEqual(permission.PermissionId, firstPermission.PermissionId);
            };
            var results = service.CreateGetAllowedPermissionsByPrincipalIdQuery(principal.PrincipalId).ToList();
            tester(results);
        }

        [TestMethod]
        public void TestCreateGetAllowedPermissionsByPrincipalIdQuery_CheckSendToSevisPermission_PrincipalHasSevisUserAccounts()
        {
            var principal = new Principal
            {
                PrincipalId = 1
            };
            var sevisAccount = new SevisAccount
            {
                Id = 1,
                OrgId = "org id",
                Principal = principal,
                PrincipalId = principal.PrincipalId,
                Username = "username"
            };
            var userAccount = new UserAccount
            {
                DisplayName = "display",
                Principal = principal,
                PrincipalId = principal.PrincipalId,
                EmailAddress = "someone@isp.com"
            };
            principal.SevisAccounts.Add(sevisAccount);
            principal.UserAccount = userAccount;
            userAccount.Principal = principal;

            var applicationResourceType = new ResourceType
            {
                ResourceTypeName = ResourceType.Application.Value,
                ResourceTypeId = ResourceType.Application.Id
            };
            var applicationResource = new Resource
            {
                ResourceId = 1,
                ResourceType = applicationResourceType,
                ResourceTypeId = applicationResourceType.ResourceTypeId,
                ForeignResourceId = 2,
            };
            var permission = new CAM.Data.Permission
            {
                PermissionId = CAM.Data.Permission.SendToSevis.Id,
                PermissionName = CAM.Data.Permission.SendToSevis.Value,
                PermissionDescription = "desc",
                Resource = applicationResource,
                ResourceId = applicationResource.ResourceId,
                ResourceType = applicationResourceType,
                ResourceTypeId = applicationResourceType.ResourceTypeId
            };

            context.Principals.Add(principal);
            context.UserAccounts.Add(userAccount);
            context.ResourceTypes.Add(applicationResourceType);
            context.Resources.Add(applicationResource);
            context.Permissions.Add(permission);
            context.SevisAccounts.Add(sevisAccount);

            Action<IList<IPermission>> tester = (testPermissions) =>
            {
                Assert.AreEqual(1, testPermissions.Count);
                var firstPermission = testPermissions.First();
                Assert.AreEqual(principal.PrincipalId, firstPermission.PrincipalId);
                Assert.IsTrue(firstPermission.IsAllowed);
                Assert.AreEqual(applicationResource.ResourceId, firstPermission.ResourceId);
                Assert.AreEqual(permission.PermissionId, firstPermission.PermissionId);
                Assert.AreEqual(applicationResourceType.ResourceTypeId, firstPermission.ResourceTypeId);
            };
            var results = service.CreateGetAllowedPermissionsByPrincipalIdQuery(principal.PrincipalId).ToList();
            tester(results);
        }

        [TestMethod]
        public void TestCreateGetAllowedPermissionsByPrincipalIdQuery_CheckSendToSevisPermission_PrincipalDoesNotHaveSevisUserAccounts()
        {
            var principal = new Principal
            {
                PrincipalId = 1
            };            
            var userAccount = new UserAccount
            {
                DisplayName = "display",
                Principal = principal,
                PrincipalId = principal.PrincipalId,
                EmailAddress = "someone@isp.com"
            };
            principal.UserAccount = userAccount;
            userAccount.Principal = principal;

            var applicationResourceType = new ResourceType
            {
                ResourceTypeName = ResourceType.Application.Value,
                ResourceTypeId = ResourceType.Application.Id
            };
            var applicationResource = new Resource
            {
                ResourceId = 1,
                ResourceType = applicationResourceType,
                ResourceTypeId = applicationResourceType.ResourceTypeId,
                ForeignResourceId = 2,
            };
            var permission = new CAM.Data.Permission
            {
                PermissionId = CAM.Data.Permission.SendToSevis.Id,
                PermissionName = CAM.Data.Permission.SendToSevis.Value,
                PermissionDescription = "desc",
                Resource = applicationResource,
                ResourceId = applicationResource.ResourceId,
                ResourceType = applicationResourceType,
                ResourceTypeId = applicationResourceType.ResourceTypeId
            };

            context.Principals.Add(principal);
            context.UserAccounts.Add(userAccount);
            context.ResourceTypes.Add(applicationResourceType);
            context.Resources.Add(applicationResource);
            context.Permissions.Add(permission);
            

            Action<IList<IPermission>> tester = (testPermissions) =>
            {
                Assert.AreEqual(0, testPermissions.Count);
            };
            var results = service.CreateGetAllowedPermissionsByPrincipalIdQuery(principal.PrincipalId).ToList();
            tester(results);
        }

        [TestMethod]
        public void TestCreateGetAllowedPermissionsByPrincipalIdQuery_CheckSendToSevisPermission_SendToSevisPermissionDoesNotExist()
        {
            var principal = new Principal
            {
                PrincipalId = 1
            };
            var sevisAccount = new SevisAccount
            {
                Id = 1,
                OrgId = "org id",
                Principal = principal,
                PrincipalId = principal.PrincipalId,
                Username = "username"
            };
            var userAccount = new UserAccount
            {
                DisplayName = "display",
                Principal = principal,
                PrincipalId = principal.PrincipalId,
                EmailAddress = "someone@isp.com"
            };
            principal.SevisAccounts.Add(sevisAccount);
            principal.UserAccount = userAccount;
            userAccount.Principal = principal;

            var applicationResourceType = new ResourceType
            {
                ResourceTypeName = ResourceType.Application.Value,
                ResourceTypeId = ResourceType.Application.Id
            };
            var applicationResource = new Resource
            {
                ResourceId = 1,
                ResourceType = applicationResourceType,
                ResourceTypeId = applicationResourceType.ResourceTypeId,
                ForeignResourceId = 2,
            };

            context.Principals.Add(principal);
            context.UserAccounts.Add(userAccount);
            context.ResourceTypes.Add(applicationResourceType);
            context.Resources.Add(applicationResource);
            context.SevisAccounts.Add(sevisAccount);

            Action<IList<IPermission>> tester = (testPermissions) =>
            {
                Assert.AreEqual(0, testPermissions.Count);
            };
            var results = service.CreateGetAllowedPermissionsByPrincipalIdQuery(principal.PrincipalId).ToList();
            tester(results);
        }

        [TestMethod]
        public void TestCreateGetAllowedPermissionsByPrincipalIdQuery_CheckSendToSevisPermission_ApplicationResourceDoesNotExist()
        {
            var principal = new Principal
            {
                PrincipalId = 1
            };
            var sevisAccount = new SevisAccount
            {
                Id = 1,
                OrgId = "org id",
                Principal = principal,
                PrincipalId = principal.PrincipalId,
                Username = "username"
            };
            var userAccount = new UserAccount
            {
                DisplayName = "display",
                Principal = principal,
                PrincipalId = principal.PrincipalId,
                EmailAddress = "someone@isp.com"
            };
            principal.SevisAccounts.Add(sevisAccount);
            principal.UserAccount = userAccount;
            userAccount.Principal = principal;

            var applicationResourceType = new ResourceType
            {
                ResourceTypeName = ResourceType.Application.Value,
                ResourceTypeId = ResourceType.Application.Id
            };
            var permission = new CAM.Data.Permission
            {
                PermissionId = CAM.Data.Permission.SendToSevis.Id,
                PermissionName = CAM.Data.Permission.SendToSevis.Value,
                PermissionDescription = "desc",
                ResourceType = applicationResourceType,
                ResourceTypeId = applicationResourceType.ResourceTypeId
            };

            context.Principals.Add(principal);
            context.UserAccounts.Add(userAccount);
            context.ResourceTypes.Add(applicationResourceType);
            context.Permissions.Add(permission);
            context.SevisAccounts.Add(sevisAccount);

            Action<IList<IPermission>> tester = (testPermissions) =>
            {
                Assert.AreEqual(0, testPermissions.Count);
            };
            var results = service.CreateGetAllowedPermissionsByPrincipalIdQuery(principal.PrincipalId).ToList();
            tester(results);
        }

        [TestMethod]
        public void TestCreateGetAllowedPermissionsByPrincipalIdQuery_PermissionIsNotAllowed()
        {
            var principal = new Principal
            {
                PrincipalId = 1
            };
            var userAccount = new UserAccount
            {
                DisplayName = "display",
                Principal = principal,
                PrincipalId = principal.PrincipalId,
                EmailAddress = "someone@isp.com"
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
            var permission = new CAM.Data.Permission
            {
                PermissionId = CAM.Data.Permission.EditProject.Id,
                PermissionName = CAM.Data.Permission.EditProject.Value,
                PermissionDescription = "desc"
            };
            var permissionAssignment = new PermissionAssignment
            {
                IsAllowed = false,
                Permission = permission,
                PermissionId = permission.PermissionId,
                Principal = principal,
                PrincipalId = principal.PrincipalId,
                Resource = resource,
                ResourceId = resource.ResourceId,
                AssignedOn = DateTimeOffset.UtcNow,
            };

            context.Principals.Add(principal);
            context.UserAccounts.Add(userAccount);
            context.ResourceTypes.Add(resourceType);
            context.Resources.Add(resource);
            context.Permissions.Add(permission);
            context.PermissionAssignments.Add(permissionAssignment);

            Action<IList<IPermission>> tester = (testPermissions) =>
            {
                Assert.AreEqual(1, testPermissions.Count);
                var firstPermission = testPermissions.First();
                Assert.IsFalse(firstPermission.IsAllowed);
            };
            var results = service.CreateGetAllowedPermissionsByPrincipalIdQuery(principal.PrincipalId).ToList();
            tester(results);
        }

        [TestMethod]
        public void TestCreateGetAllowedPermissionsByPrincipalIdQuery_PermissionIsNotAssignedToPrincipal()
        {
            var principal = new Principal
            {
                PrincipalId = 1
            };
            var userAccount = new UserAccount
            {
                DisplayName = "display",
                Principal = principal,
                PrincipalId = principal.PrincipalId,
                EmailAddress = "someone@isp.com"
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
            var permission = new CAM.Data.Permission
            {
                PermissionId = CAM.Data.Permission.EditProject.Id,
                PermissionName = CAM.Data.Permission.EditProject.Value,
                PermissionDescription = "desc"
            };

            var someOtherPrincipal = new Principal
            {
                PrincipalId = 100,

            };
            var permissionAssignment = new PermissionAssignment
            {
                IsAllowed = true,
                Permission = permission,
                PermissionId = permission.PermissionId,
                Principal = someOtherPrincipal,
                PrincipalId = someOtherPrincipal.PrincipalId,
                Resource = resource,
                ResourceId = resource.ResourceId,
                AssignedOn = DateTimeOffset.UtcNow,
            };

            context.Principals.Add(someOtherPrincipal);
            context.Principals.Add(principal);
            context.UserAccounts.Add(userAccount);
            context.ResourceTypes.Add(resourceType);
            context.Resources.Add(resource);
            context.Permissions.Add(permission);
            context.PermissionAssignments.Add(permissionAssignment);

            Action<IList<IPermission>> tester = (testPermissions) =>
            {
                Assert.AreEqual(0, testPermissions.Count);
            };
            var results = service.CreateGetAllowedPermissionsByPrincipalIdQuery(principal.PrincipalId).ToList();
            tester(results);
        }

        [TestMethod]
        public void TestCreateGetAllowedPermissionsByPrincipalIdQuery_PrincipalHasRole()
        {
            var principal = new Principal
            {
                PrincipalId = 1
            };
            var userAccount = new UserAccount
            {
                DisplayName = "display",
                Principal = principal,
                PrincipalId = principal.PrincipalId,
                EmailAddress = "someone@isp.com"
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
            var permission = new CAM.Data.Permission
            {
                PermissionId = CAM.Data.Permission.EditProject.Id,
                PermissionName = CAM.Data.Permission.EditProject.Value,
                PermissionDescription = "desc"
            };
            var role = new Role
            {
                RoleName = "role",
                RoleId = 1,
                IsActive = true
            };
            var roleResourcePermission = new RoleResourcePermission
            {
                Permission = permission,
                PermissionId = permission.PermissionId,
                Resource = resource,
                ResourceId = resource.ResourceId,
                Role = role,
                RoleId = role.RoleId,
                AssignedOn = DateTimeOffset.UtcNow,
            };
            var principalRole = new PrincipalRole
            {
                Principal = principal,
                PrincipalId = principal.PrincipalId,
                Role = role,
                RoleId = role.RoleId
            };
            context.Principals.Add(principal);
            context.UserAccounts.Add(userAccount);
            context.ResourceTypes.Add(resourceType);
            context.Resources.Add(resource);
            context.Permissions.Add(permission);
            context.Roles.Add(role);
            context.RoleResourcePermissions.Add(roleResourcePermission);
            context.PrincipalRoles.Add(principalRole);

            Action<IList<IPermission>> tester = (testPermissions) =>
            {
                Assert.AreEqual(1, testPermissions.Count);
                var firstPermission = testPermissions.First();
                Assert.AreEqual(principal.PrincipalId, firstPermission.PrincipalId);
                Assert.IsTrue(firstPermission.IsAllowed);
                Assert.AreEqual(resource.ResourceId, firstPermission.ResourceId);
                Assert.AreEqual(permission.PermissionId, firstPermission.PermissionId);
            };
            var results = service.CreateGetAllowedPermissionsByPrincipalIdQuery(principal.PrincipalId).ToList();
            tester(results);
        }

        [TestMethod]
        public void TestCreateGetAllowedPermissionsByPrincipalIdQuery_PrincipalHasRole_RoleIsInactive()
        {
            var principal = new Principal
            {
                PrincipalId = 1
            };
            var userAccount = new UserAccount
            {
                DisplayName = "display",
                Principal = principal,
                PrincipalId = principal.PrincipalId,
                EmailAddress = "someone@isp.com"
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
            var permission = new CAM.Data.Permission
            {
                PermissionId = CAM.Data.Permission.EditProject.Id,
                PermissionName = CAM.Data.Permission.EditProject.Value,
                PermissionDescription = "desc"
            };
            var role = new Role
            {
                RoleName = "role",
                RoleId = 1,
                IsActive = false
            };
            var roleResourcePermission = new RoleResourcePermission
            {
                Permission = permission,
                PermissionId = permission.PermissionId,
                Resource = resource,
                ResourceId = resource.ResourceId,
                Role = role,
                RoleId = role.RoleId,
                AssignedOn = DateTimeOffset.UtcNow,
            };
            var principalRole = new PrincipalRole
            {
                Principal = principal,
                PrincipalId = principal.PrincipalId,
                Role = role,
                RoleId = role.RoleId
            };
            context.Principals.Add(principal);
            context.UserAccounts.Add(userAccount);
            context.ResourceTypes.Add(resourceType);
            context.Resources.Add(resource);
            context.Permissions.Add(permission);
            context.Roles.Add(role);
            context.RoleResourcePermissions.Add(roleResourcePermission);
            context.PrincipalRoles.Add(principalRole);

            Action<IList<IPermission>> tester = (testPermissions) =>
            {
                Assert.AreEqual(0, testPermissions.Count);
            };
            var results = service.CreateGetAllowedPermissionsByPrincipalIdQuery(principal.PrincipalId).ToList();
            tester(results);
        }

        [TestMethod]
        public void TestCreateGetAllowedPermissionsByPrincipalIdQuery_PrincipalHasRole_PrincipalHasAllowedPermissionAssignment()
        {
            var principal = new Principal
            {
                PrincipalId = 1
            };
            var userAccount = new UserAccount
            {
                DisplayName = "display",
                Principal = principal,
                PrincipalId = principal.PrincipalId,
                EmailAddress = "someone@isp.com"
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
            var permission = new CAM.Data.Permission
            {
                PermissionId = CAM.Data.Permission.EditProject.Id,
                PermissionName = CAM.Data.Permission.EditProject.Value,
            };
            var role = new Role
            {
                RoleName = "role",
                RoleId = 1,
            };
            var roleResourcePermission = new RoleResourcePermission
            {
                Permission = permission,
                PermissionId = permission.PermissionId,
                Resource = resource,
                ResourceId = resource.ResourceId,
                Role = role,
                RoleId = role.RoleId
            };
            var principalRole = new PrincipalRole
            {
                Principal = principal,
                PrincipalId = principal.PrincipalId,
                Role = role,
                RoleId = role.RoleId
            };
            var permissionAssignment = new PermissionAssignment
            {
                IsAllowed = true,
                Permission = permission,
                PermissionId = permission.PermissionId,
                Principal = principal,
                PrincipalId = principal.PrincipalId,
                Resource = resource,
                ResourceId = resource.ResourceId
            };
            context.PermissionAssignments.Add(permissionAssignment);
            context.Principals.Add(principal);
            context.UserAccounts.Add(userAccount);
            context.ResourceTypes.Add(resourceType);
            context.Resources.Add(resource);
            context.Permissions.Add(permission);
            context.Roles.Add(role);
            context.RoleResourcePermissions.Add(roleResourcePermission);
            context.PrincipalRoles.Add(principalRole);

            Action<IList<IPermission>> tester = (testPermissions) =>
            {
                Assert.AreEqual(1, testPermissions.Count);
                var firstPermission = testPermissions.First();
                Assert.AreEqual(principal.PrincipalId, firstPermission.PrincipalId);
                Assert.IsTrue(firstPermission.IsAllowed);
                Assert.AreEqual(resource.ResourceId, firstPermission.ResourceId);
                Assert.AreEqual(permission.PermissionId, firstPermission.PermissionId);
            };
            var results = service.CreateGetAllowedPermissionsByPrincipalIdQuery(principal.PrincipalId).ToList();
            tester(results);
        }

        [TestMethod]
        public void TestCreateGetAllowedPermissionsByPrincipalIdQuery_PrincipalHasRole_PrincipalHasNotAllowedPermissionAssignment()
        {
            var principal = new Principal
            {
                PrincipalId = 1
            };
            var userAccount = new UserAccount
            {
                DisplayName = "display",
                Principal = principal,
                PrincipalId = principal.PrincipalId,
                EmailAddress = "someone@isp.com"
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
            var permission = new CAM.Data.Permission
            {
                PermissionId = CAM.Data.Permission.EditProject.Id,
                PermissionName = CAM.Data.Permission.EditProject.Value,
            };
            var role = new Role
            {
                RoleName = "role",
                RoleId = 1,
            };
            var roleResourcePermission = new RoleResourcePermission
            {
                Permission = permission,
                PermissionId = permission.PermissionId,
                Resource = resource,
                ResourceId = resource.ResourceId,
                Role = role,
                RoleId = role.RoleId
            };
            var principalRole = new PrincipalRole
            {
                Principal = principal,
                PrincipalId = principal.PrincipalId,
                Role = role,
                RoleId = role.RoleId
            };
            var permissionAssignment = new PermissionAssignment
            {
                IsAllowed = false,
                Permission = permission,
                PermissionId = permission.PermissionId,
                Principal = principal,
                PrincipalId = principal.PrincipalId,
                Resource = resource,
                ResourceId = resource.ResourceId
            };
            context.PermissionAssignments.Add(permissionAssignment);
            context.Principals.Add(principal);
            context.UserAccounts.Add(userAccount);
            context.ResourceTypes.Add(resourceType);
            context.Resources.Add(resource);
            context.Permissions.Add(permission);
            context.Roles.Add(role);
            context.RoleResourcePermissions.Add(roleResourcePermission);
            context.PrincipalRoles.Add(principalRole);

            Action<IList<IPermission>> tester = (testPermissions) =>
            {
                Assert.AreEqual(1, testPermissions.Count);
                var firstPermission = testPermissions.First();
                Assert.IsFalse(firstPermission.IsAllowed);
            };
            var results = service.CreateGetAllowedPermissionsByPrincipalIdQuery(principal.PrincipalId).ToList();
            tester(results);
        }

        [TestMethod]
        public void TestCreateGetAllowedPermissionsByPrincipalIdQuery_PrincipalDoesNotHaveRole()
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
            var permission = new CAM.Data.Permission
            {
                PermissionId = CAM.Data.Permission.EditProject.Id,
                PermissionName = CAM.Data.Permission.EditProject.Value,
            };
            var role = new Role
            {
                RoleName = "role",
                RoleId = 1,
            };
            var roleResourcePermission = new RoleResourcePermission
            {
                Permission = permission,
                PermissionId = permission.PermissionId,
                Resource = resource,
                ResourceId = resource.ResourceId,
                Role = role,
                RoleId = role.RoleId
            };
            context.Principals.Add(principal);
            context.UserAccounts.Add(userAccount);
            context.ResourceTypes.Add(resourceType);
            context.Resources.Add(resource);
            context.Permissions.Add(permission);
            context.Roles.Add(role);
            context.RoleResourcePermissions.Add(roleResourcePermission);

            Action<IList<IPermission>> tester = (testPermissions) =>
            {
                Assert.AreEqual(0, testPermissions.Count);
            };
            var results = service.CreateGetAllowedPermissionsByPrincipalIdQuery(principal.PrincipalId).ToList();
            tester(results);
        }

        [TestMethod]
        public void TestCreateGetAllowedPermissionsByPrincipalIdQuery_RoleDoesNotHaveResourcePermission()
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
            var permission = new CAM.Data.Permission
            {
                PermissionId = CAM.Data.Permission.EditProject.Id,
                PermissionName = CAM.Data.Permission.EditProject.Value,
            };
            var role = new Role
            {
                RoleName = "role",
                RoleId = 1,
            };
            var principalRole = new PrincipalRole
            {
                Principal = principal,
                PrincipalId = principal.PrincipalId,
                Role = role,
                RoleId = role.RoleId
            };
            context.Principals.Add(principal);
            context.UserAccounts.Add(userAccount);
            context.ResourceTypes.Add(resourceType);
            context.Resources.Add(resource);
            context.Permissions.Add(permission);
            context.Roles.Add(role);
            context.PrincipalRoles.Add(principalRole);

            Action<IList<IPermission>> tester = (testPermissions) =>
            {
                Assert.AreEqual(0, testPermissions.Count);
            };
            var results = service.CreateGetAllowedPermissionsByPrincipalIdQuery(principal.PrincipalId).ToList();
            tester(results);
        }

        [TestMethod]
        public async Task TestGetAllowedPermissionsByPrincipalIdAsync()
        {
            var principal = new Principal
            {
                PrincipalId = 1
            };
            var userAccount = new UserAccount
            {
                DisplayName = "display",
                Principal = principal,
                PrincipalId = principal.PrincipalId,
                EmailAddress = "someone@isp.com"
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
            var permission = new CAM.Data.Permission
            {
                PermissionId = CAM.Data.Permission.EditProject.Id,
                PermissionName = CAM.Data.Permission.EditProject.Value,
                PermissionDescription = "desc"
            };
            var permissionAssignment = new PermissionAssignment
            {
                IsAllowed = true,
                Permission = permission,
                PermissionId = permission.PermissionId,
                Principal = principal,
                PrincipalId = principal.PrincipalId,
                Resource = resource,
                ResourceId = resource.ResourceId,
                AssignedOn = DateTimeOffset.UtcNow,
            };

            context.Principals.Add(principal);
            context.UserAccounts.Add(userAccount);
            context.ResourceTypes.Add(resourceType);
            context.Resources.Add(resource);
            context.Permissions.Add(permission);
            context.PermissionAssignments.Add(permissionAssignment);

            Action<IList<IPermission>> tester = (testPermissions) =>
            {
                Assert.AreEqual(1, testPermissions.Count);
            };
            var results = service.GetAllowedPermissionsByPrincipalId(userAccount.PrincipalId);
            tester(results);

            var resultsAsync = await service.GetAllowedPermissionsByPrincipalIdAsync(userAccount.PrincipalId);
            tester(resultsAsync);
        }

        #endregion

        #region HasPermission
        [TestMethod]
        public void TestHasPermission_PermissionsAllowed_DoesNotHaveParent()
        {
            var principalId = 1;
            var resourceId = 1;
            var permissionId = Permission.EditProject.Id;
            var simplePermission = new SimplePermission
            {
                IsAllowed = true,
                PermissionId = Permission.EditProject.Id,
                PrincipalId = principalId,
                ResourceId = resourceId
            };
            var list = new List<IPermission> { simplePermission };
            var result = service.HasPermission(resourceId, null, permissionId, list);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestHasPermission_PermissionsNotAllowed()
        {
            var principalId = 1;
            var resourceId = 1;
            var permissionId = Permission.EditProject.Id;
            var simplePermission = new SimplePermission
            {
                IsAllowed = false,
                PermissionId = Permission.EditProject.Id,
                PrincipalId = principalId,
                ResourceId = resourceId
            };
            var list = new List<IPermission> { simplePermission };
            var result = service.HasPermission(resourceId, null, permissionId, list);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TestHasPermission_HasParent_PermissionIsAllowedOnParentResource()
        {
            var parentResourceId = 2;
            var permissionId = Permission.ViewProject.Id;
            var principalId = 2;
            var viewProjectPermission = new SimplePermission
            {
                ResourceId = parentResourceId,
                PrincipalId = principalId,
                IsAllowed = true,
                PermissionId = permissionId
            };
            var list = new List<IPermission> { viewProjectPermission };
            var result = service.HasPermission(1, parentResourceId, permissionId, list);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestHasPermission_NoPermissions()
        {   
            var list = new List<IPermission>();
            var result = service.HasPermission(1, 2, Permission.EditProject.Id, list);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TestHasPermission_PermissionsForADifferentResource()
        {
            var principalId = 1;
            var resourceId = 1;
            var permissionId = Permission.EditProject.Id;
            var simplePermission = new SimplePermission
            {
                IsAllowed = true,
                PermissionId = Permission.EditProject.Id,
                PrincipalId = principalId,
                ResourceId = resourceId
            };
            var list = new List<IPermission> { simplePermission };
            var result = service.HasPermission(resourceId - 1, null, permissionId, list);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TestHasPermission_PermissionsForADifferentPermission()
        {
            var principalId = 1;
            var resourceId = 1;
            var permissionId = Permission.EditProject.Id;
            var simplePermission = new SimplePermission
            {
                IsAllowed = true,
                PermissionId = Permission.EditProject.Id,
                PrincipalId = principalId,
                ResourceId = resourceId
            };
            var list = new List<IPermission> { simplePermission };
            var result = service.HasPermission(resourceId, null, Permission.EditOffice.Id, list);
            Assert.IsFalse(result);
        }
        #endregion
    }

}
