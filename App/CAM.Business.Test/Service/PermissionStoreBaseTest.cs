using CAM.Business.Service;
using CAM.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.Business.Test.Service
{
    public class SimplePermissionStore : PermissionStoreBase
    {
        public SimplePermissionStore(CamModel context, IPermissionModelService permissionModelService, IResourceService resourceService)
            : base(context, permissionModelService, resourceService)
        {

        }

        public void LoadPermissionsByPrincipalId(int principalId)
        {
            this.Permissions = base.GetUserPermissions(principalId);
        }

        public async Task LoadPermissionsByPrincipalIdAsync(int principalId)
        {
            this.Permissions = await base.GetUserPermissionsAsync(principalId);
        }
    }

    [TestClass]
    public class PermissionStoreBaseTest
    {
        private Mock<IPermissionModelService> permissionModelService;
        private Mock<IResourceService> resourceService;
        private SimplePermissionStore store;
        private InMemoryCamModel context;

        [TestInitialize]
        public void TestInit()
        {
            resourceService = new Mock<IResourceService>();
            permissionModelService = new Mock<IPermissionModelService>();
            context = new InMemoryCamModel();
            store = new SimplePermissionStore(context, permissionModelService.Object, resourceService.Object);
        }

        [TestMethod]
        public async Task TestPermissions_PermissionIsAssigned()
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
            store.LoadPermissionsByPrincipalId(principal.PrincipalId);
            tester(store.Permissions);

            store.Permissions = new List<IPermission>();
            await store.LoadPermissionsByPrincipalIdAsync(principal.PrincipalId);
            tester(store.Permissions);
        }


        [TestMethod]
        public async Task TestPermissions_PermissionIsNotAllowed()
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
                Assert.AreEqual(0, testPermissions.Count);
            };
            store.LoadPermissionsByPrincipalId(principal.PrincipalId);
            tester(store.Permissions);

            store.Permissions = new List<IPermission>();
            await store.LoadPermissionsByPrincipalIdAsync(principal.PrincipalId);
            tester(store.Permissions);
        }

        [TestMethod]
        public async Task TestPermissions_PermissionIsNotAssignedToPrincipal()
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
            store.LoadPermissionsByPrincipalId(principal.PrincipalId);
            tester(store.Permissions);

            store.Permissions = new List<IPermission>();
            await store.LoadPermissionsByPrincipalIdAsync(principal.PrincipalId);
            tester(store.Permissions);
        }

        [TestMethod]
        public async Task TestPermissions_PrincipalHasRole()
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
            store.LoadPermissionsByPrincipalId(principal.PrincipalId);
            tester(store.Permissions);

            store.Permissions = new List<IPermission>();
            await store.LoadPermissionsByPrincipalIdAsync(principal.PrincipalId);
            tester(store.Permissions);
        }


        [TestMethod]
        public async Task TestPermissions_PrincipalHasRole_PrincipalHasAllowedPermissionAssignment()
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
            store.LoadPermissionsByPrincipalId(principal.PrincipalId);
            tester(store.Permissions);

            store.Permissions = new List<IPermission>();
            await store.LoadPermissionsByPrincipalIdAsync(principal.PrincipalId);
            tester(store.Permissions);
        }

        [TestMethod]
        public async Task TestPermissions_PrincipalHasRole_PrincipalHasNotAllowedPermissionAssignment()
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
                Assert.AreEqual(0, testPermissions.Count);
            };
            store.LoadPermissionsByPrincipalId(principal.PrincipalId);
            tester(store.Permissions);

            store.Permissions = new List<IPermission>();
            await store.LoadPermissionsByPrincipalIdAsync(principal.PrincipalId);
            tester(store.Permissions);
        }

        [TestMethod]
        public async Task TestPermissions_PrincipalHasRole_SomeOtherPermissionAssignmentNotAllowed()
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
            var someOtherPermission = new CAM.Data.Permission
            {
                PermissionId = CAM.Data.Permission.EditProgram.Id,
                PermissionName = CAM.Data.Permission.EditProgram.Value
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
                Permission = someOtherPermission,
                PermissionId = someOtherPermission.PermissionId,
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
            store.LoadPermissionsByPrincipalId(principal.PrincipalId);
            tester(store.Permissions);

            store.Permissions = new List<IPermission>();
            await store.LoadPermissionsByPrincipalIdAsync(principal.PrincipalId);
            tester(store.Permissions);
        }


        [TestMethod]
        public async Task TestPermissions_PrincipalHasRole_OtherPrincipalHasNotAllowedPermissionAssignment()
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
            var someOtherPrincipal = new Principal
            {
                PrincipalId = 100,
            };
            var permissionAssignment = new PermissionAssignment
            {
                IsAllowed = false,
                Permission = permission,
                PermissionId = permission.PermissionId,
                Principal = someOtherPrincipal,
                PrincipalId = someOtherPrincipal.PrincipalId,
                Resource = resource,
                ResourceId = resource.ResourceId
            };
            context.Principals.Add(someOtherPrincipal);
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
            store.LoadPermissionsByPrincipalId(principal.PrincipalId);
            tester(store.Permissions);

            store.Permissions = new List<IPermission>();
            await store.LoadPermissionsByPrincipalIdAsync(principal.PrincipalId);
            tester(store.Permissions);
        }

        [TestMethod]
        public async Task TestPermissions_PrincipalDoesNotHaveRole()
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
            store.LoadPermissionsByPrincipalId(principal.PrincipalId);
            tester(store.Permissions);

            store.Permissions = new List<IPermission>();
            await store.LoadPermissionsByPrincipalIdAsync(principal.PrincipalId);
            tester(store.Permissions);
        }

        [TestMethod]
        public async Task TestPermissions_RoleDoesNotHaveResourcePermission()
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
            store.LoadPermissionsByPrincipalId(principal.PrincipalId);
            tester(store.Permissions);

            store.Permissions = new List<IPermission>();
            await store.LoadPermissionsByPrincipalIdAsync(principal.PrincipalId);
            tester(store.Permissions);
        }
    }
}
