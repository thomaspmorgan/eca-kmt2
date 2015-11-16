using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CAM.Data;
using CAM.Business.Queries;

namespace CAM.Business.Test.Queries
{
    [TestClass]
    public class UserQueriesTest
    {
        private TestInMemoryCamModel context;

        [TestInitialize]
        public void TestInit()
        {
            context = new TestInMemoryCamModel();
        }

        [TestMethod]
        public void TestCreateGetSimpleResourceAuthorizationsByPrincipalId_HasPermissionAssignment_CheckProperties()
        {
            var principalId = 1;
            var principal = new Principal
            {
                PrincipalId = principalId,
            };
            var resourceType = new ResourceType
            {
                ResourceTypeId = ResourceType.Project.Id,
                ResourceTypeName = ResourceType.Project.Value
            };
            var resource = new Resource
            {
                ResourceId = 1,
                ForeignResourceId = 2,
                ResourceTypeId = resourceType.ResourceTypeId,
                ResourceType = resourceType,
            };
            var permission = new Permission
            {
                PermissionId = 1,
                PermissionName = "Permission"
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
            context.Principals.Add(principal);
            context.ResourceTypes.Add(resourceType);
            context.Resources.Add(resource);
            context.Permissions.Add(permission);
            context.PermissionAssignments.Add(permissionAssignment);

            var results = UserQueries.CreateGetSimpleResourceAuthorizationsByPrincipalId(context, principalId).ToList();
            Assert.AreEqual(1, results.Count);
            var firstResult = results.First();
            Assert.AreEqual(principal.PrincipalId, firstResult.PrincipalId);
            Assert.AreEqual(permission.PermissionId, firstResult.PermissionId);
            Assert.AreEqual(resource.ResourceId, firstResult.ResourceId);
            Assert.AreEqual(resource.ForeignResourceId, firstResult.ForeignResourceId);
            Assert.IsTrue(firstResult.IsAllowed);
        }

        [TestMethod]
        public void TestCreateGetSimpleResourceAuthorizationsByPrincipalId_DoesNotHavePermissionAssignment()
        {
            var principalId = 1;
            var principal = new Principal
            {
                PrincipalId = principalId,
            };
            var resourceType = new ResourceType
            {
                ResourceTypeId = ResourceType.Project.Id,
                ResourceTypeName = ResourceType.Project.Value
            };
            var resource = new Resource
            {
                ResourceId = 1,
                ForeignResourceId = 2,
                ResourceTypeId = resourceType.ResourceTypeId,
                ResourceType = resourceType,
            };
            var permission = new Permission
            {
                PermissionId = 1,
                PermissionName = "Permission"
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
            context.Principals.Add(principal);
            context.ResourceTypes.Add(resourceType);
            context.Resources.Add(resource);
            context.Permissions.Add(permission);
            context.PermissionAssignments.Add(permissionAssignment);

            var results = UserQueries.CreateGetSimpleResourceAuthorizationsByPrincipalId(context, principalId).ToList();
            Assert.AreEqual(1, results.Count);
            var firstResult = results.First();
            Assert.IsFalse(firstResult.IsAllowed);
        }

        [TestMethod]
        public void TestCreateGetSimpleResourceAuthorizationsByPrincipalId_PermissionAssignmentDifferentPrincipal()
        {
            var principalId = 1;
            var principal = new Principal
            {
                PrincipalId = principalId,
            };
            var resourceType = new ResourceType
            {
                ResourceTypeId = ResourceType.Project.Id,
                ResourceTypeName = ResourceType.Project.Value
            };
            var resource = new Resource
            {
                ResourceId = 1,
                ForeignResourceId = 2,
                ResourceTypeId = resourceType.ResourceTypeId,
                ResourceType = resourceType,
            };
            var permission = new Permission
            {
                PermissionId = 1,
                PermissionName = "Permission"
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
            context.Principals.Add(principal);
            context.ResourceTypes.Add(resourceType);
            context.Resources.Add(resource);
            context.Permissions.Add(permission);
            context.PermissionAssignments.Add(permissionAssignment);

            var results = UserQueries.CreateGetSimpleResourceAuthorizationsByPrincipalId(context, 0).ToList();
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void TestCreateGetSimpleResourceAuthorizationsByPrincipalId_PrincipalHasRole()
        {
            var principalId = 1;
            var principal = new Principal
            {
                PrincipalId = principalId,
            };
            var resourceType = new ResourceType
            {
                ResourceTypeId = ResourceType.Project.Id,
                ResourceTypeName = ResourceType.Project.Value
            };
            var resource = new Resource
            {
                ResourceId = 1,
                ForeignResourceId = 2,
                ResourceTypeId = resourceType.ResourceTypeId,
                ResourceType = resourceType,
            };
            var permission = new Permission
            {
                PermissionId = 1,
                PermissionName = "Permission"
            };
            var role = new Role
            {
                IsActive = true,
                RoleId = 1,
                RoleName = "role name"
            };
            var principalRole = new PrincipalRole
            {
                RoleId = role.RoleId,
                Role = role,
                Principal = principal,
                PrincipalId = principal.PrincipalId
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
            context.ResourceTypes.Add(resourceType);
            context.Resources.Add(resource);
            context.Permissions.Add(permission);
            context.Roles.Add(role);
            context.PrincipalRoles.Add(principalRole);
            context.RoleResourcePermissions.Add(roleResourcePermission);

            var results = UserQueries.CreateGetSimpleResourceAuthorizationsByPrincipalId(context, principalId).ToList();
            Assert.AreEqual(1, results.Count);
            var firstResult = results.First();
            Assert.AreEqual(principal.PrincipalId, firstResult.PrincipalId);
            Assert.AreEqual(permission.PermissionId, firstResult.PermissionId);
            Assert.AreEqual(resource.ResourceId, firstResult.ResourceId);
            Assert.AreEqual(resource.ForeignResourceId, firstResult.ForeignResourceId);
            Assert.IsTrue(firstResult.IsAllowed);
        }

        [TestMethod]
        public void TestCreateGetSimpleResourceAuthorizationsByPrincipalId_PrincipalHasRole_RoleIsInactive()
        {
            var principalId = 1;
            var principal = new Principal
            {
                PrincipalId = principalId,
            };
            var resourceType = new ResourceType
            {
                ResourceTypeId = ResourceType.Project.Id,
                ResourceTypeName = ResourceType.Project.Value
            };
            var resource = new Resource
            {
                ResourceId = 1,
                ForeignResourceId = 2,
                ResourceTypeId = resourceType.ResourceTypeId,
                ResourceType = resourceType,
            };
            var permission = new Permission
            {
                PermissionId = 1,
                PermissionName = "Permission"
            };
            var role = new Role
            {
                IsActive = false,
                RoleId = 1,
                RoleName = "role name"
            };
            var principalRole = new PrincipalRole
            {
                RoleId = role.RoleId,
                Role = role,
                Principal = principal,
                PrincipalId = principal.PrincipalId
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
            context.ResourceTypes.Add(resourceType);
            context.Resources.Add(resource);
            context.Permissions.Add(permission);
            context.Roles.Add(role);
            context.PrincipalRoles.Add(principalRole);
            context.RoleResourcePermissions.Add(roleResourcePermission);

            var results = UserQueries.CreateGetSimpleResourceAuthorizationsByPrincipalId(context, principalId).ToList();
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void TestCreateGetSimpleResourceAuthorizationsByPrincipalId_PrincipalDoesHaveNotRoles()
        {
            var principalId = 1;
            var principal = new Principal
            {
                PrincipalId = principalId,
            };
            var resourceType = new ResourceType
            {
                ResourceTypeId = ResourceType.Project.Id,
                ResourceTypeName = ResourceType.Project.Value
            };
            var resource = new Resource
            {
                ResourceId = 1,
                ForeignResourceId = 2,
                ResourceTypeId = resourceType.ResourceTypeId,
                ResourceType = resourceType,
            };
            var permission = new Permission
            {
                PermissionId = 1,
                PermissionName = "Permission"
            };
            var role = new Role
            {
                IsActive = true,
                RoleId = 1,
                RoleName = "role name"
            };
            var principalRole = new PrincipalRole
            {
                RoleId = role.RoleId,
                Role = role,
                Principal = principal,
                PrincipalId = principal.PrincipalId
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
            context.ResourceTypes.Add(resourceType);
            context.Resources.Add(resource);
            context.Permissions.Add(permission);
            context.Roles.Add(role);
            context.PrincipalRoles.Add(principalRole);
            context.RoleResourcePermissions.Add(roleResourcePermission);

            var results = UserQueries.CreateGetSimpleResourceAuthorizationsByPrincipalId(context, principalId + 1).ToList();
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void TestCreateGetSimpleResourceAuthorizationsByPrincipalId_PrincipalHasRoleAndPermissionAssignment()
        {
            var principalId = 1;
            var principal = new Principal
            {
                PrincipalId = principalId,
            };
            var resourceType = new ResourceType
            {
                ResourceTypeId = ResourceType.Project.Id,
                ResourceTypeName = ResourceType.Project.Value
            };
            var resource = new Resource
            {
                ResourceId = 1,
                ForeignResourceId = 2,
                ResourceTypeId = resourceType.ResourceTypeId,
                ResourceType = resourceType,
            };
            var permission = new Permission
            {
                PermissionId = 1,
                PermissionName = "Permission"
            };
            var rolePermission = new Permission
            {
                PermissionId = 2,
                PermissionName = "Role Permission"
            };
            var role = new Role
            {
                IsActive = true,
                RoleId = 1,
                RoleName = "role name"
            };
            var principalRole = new PrincipalRole
            {
                RoleId = role.RoleId,
                Role = role,
                Principal = principal,
                PrincipalId = principal.PrincipalId
            };
            var roleResourcePermission = new RoleResourcePermission
            {
                Permission = rolePermission,
                PermissionId = rolePermission.PermissionId,
                Resource = resource,
                ResourceId = resource.ResourceId,
                Role = role,
                RoleId = role.RoleId
            };
            var permissionAssignment = new PermissionAssignment
            {
                IsAllowed = true,
                Permission = permission,
                PermissionId = permission.PermissionId,
                Principal = principal,
                Resource = resource,
                ResourceId = resource.ResourceId,
                PrincipalId = principal.PrincipalId,
            };

            context.Principals.Add(principal);
            context.ResourceTypes.Add(resourceType);
            context.Resources.Add(resource);
            context.Permissions.Add(rolePermission);
            context.Permissions.Add(permission);
            context.Roles.Add(role);
            context.PrincipalRoles.Add(principalRole);
            context.RoleResourcePermissions.Add(roleResourcePermission);
            context.PermissionAssignments.Add(permissionAssignment);

            var results = UserQueries.CreateGetSimpleResourceAuthorizationsByPrincipalId(context, principalId).ToList();
            Assert.AreEqual(2, results.Count);
            Assert.AreEqual(2, results.Where(x => x.IsAllowed).Count());
        }

        [TestMethod]
        public void TestCreateGetSimpleResourceAuthorizationsByPrincipalId_PrincipalHasRoleAndPermissionAssignment_PermissionIsInRoleAndPermissionAssignment()
        {
            var principalId = 1;
            var principal = new Principal
            {
                PrincipalId = principalId,
            };
            var resourceType = new ResourceType
            {
                ResourceTypeId = ResourceType.Project.Id,
                ResourceTypeName = ResourceType.Project.Value
            };
            var resource = new Resource
            {
                ResourceId = 1,
                ForeignResourceId = 2,
                ResourceTypeId = resourceType.ResourceTypeId,
                ResourceType = resourceType,
            };
            var permission = new Permission
            {
                PermissionId = 1,
                PermissionName = "Permission"
            };
            var role = new Role
            {
                IsActive = true,
                RoleId = 1,
                RoleName = "role name"
            };
            var principalRole = new PrincipalRole
            {
                RoleId = role.RoleId,
                Role = role,
                Principal = principal,
                PrincipalId = principal.PrincipalId
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
            var permissionAssignment = new PermissionAssignment
            {
                IsAllowed = true,
                Permission = permission,
                PermissionId = permission.PermissionId,
                Principal = principal,
                Resource = resource,
                ResourceId = resource.ResourceId,
                PrincipalId = principal.PrincipalId,
            };

            context.Principals.Add(principal);
            context.ResourceTypes.Add(resourceType);
            context.Resources.Add(resource);
            context.Permissions.Add(permission);
            context.Roles.Add(role);
            context.PrincipalRoles.Add(principalRole);
            context.RoleResourcePermissions.Add(roleResourcePermission);
            context.PermissionAssignments.Add(permissionAssignment);

            var results = UserQueries.CreateGetSimpleResourceAuthorizationsByPrincipalId(context, principalId).ToList();
            Assert.AreEqual(1, results.Count);
            var firstResult = results.First();
            Assert.AreEqual(principal.PrincipalId, firstResult.PrincipalId);
            Assert.AreEqual(permission.PermissionId, firstResult.PermissionId);
            Assert.AreEqual(resource.ResourceId, firstResult.ResourceId);
            Assert.AreEqual(resource.ForeignResourceId, firstResult.ForeignResourceId);
            Assert.IsTrue(firstResult.IsAllowed);
        }

        [TestMethod]
        public void TestCreateGetSimpleResourceAuthorizationsByPrincipalId_PrincipalHasRoleAndPermissionAssignment_PermissionAssignmentIsNotAllowed()
        {
            var principalId = 1;
            var principal = new Principal
            {
                PrincipalId = principalId,
            };
            var resourceType = new ResourceType
            {
                ResourceTypeId = ResourceType.Project.Id,
                ResourceTypeName = ResourceType.Project.Value
            };
            var resource = new Resource
            {
                ResourceId = 1,
                ForeignResourceId = 2,
                ResourceTypeId = resourceType.ResourceTypeId,
                ResourceType = resourceType,
            };
            var permission = new Permission
            {
                PermissionId = 1,
                PermissionName = "Permission"
            };
            var role = new Role
            {
                IsActive = true,
                RoleId = 1,
                RoleName = "role name"
            };
            var principalRole = new PrincipalRole
            {
                RoleId = role.RoleId,
                Role = role,
                Principal = principal,
                PrincipalId = principal.PrincipalId
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
            var permissionAssignment = new PermissionAssignment
            {
                IsAllowed = false,
                Permission = permission,
                PermissionId = permission.PermissionId,
                Principal = principal,
                Resource = resource,
                ResourceId = resource.ResourceId,
                PrincipalId = principal.PrincipalId,
            };

            context.Principals.Add(principal);
            context.ResourceTypes.Add(resourceType);
            context.Resources.Add(resource);
            context.Permissions.Add(permission);
            context.Roles.Add(role);
            context.PrincipalRoles.Add(principalRole);
            context.RoleResourcePermissions.Add(roleResourcePermission);
            context.PermissionAssignments.Add(permissionAssignment);

            var results = UserQueries.CreateGetSimpleResourceAuthorizationsByPrincipalId(context, principalId).ToList();
            Assert.AreEqual(1, results.Count);
            var firstResult = results.First();
            Assert.AreEqual(principal.PrincipalId, firstResult.PrincipalId);
            Assert.AreEqual(permission.PermissionId, firstResult.PermissionId);
            Assert.AreEqual(resource.ResourceId, firstResult.ResourceId);
            Assert.AreEqual(resource.ForeignResourceId, firstResult.ForeignResourceId);
            Assert.IsFalse(firstResult.IsAllowed);
        }

        [TestMethod]
        public void TestCreateGetSimpleResourceAuthorizationsByPrincipalId_PrincipalHasRoleAndPermissionAssignment_RoleIsInactive()
        {
            var principalId = 1;
            var principal = new Principal
            {
                PrincipalId = principalId,
            };
            var resourceType = new ResourceType
            {
                ResourceTypeId = ResourceType.Project.Id,
                ResourceTypeName = ResourceType.Project.Value
            };
            var resource = new Resource
            {
                ResourceId = 1,
                ForeignResourceId = 2,
                ResourceTypeId = resourceType.ResourceTypeId,
                ResourceType = resourceType,
            };
            var permission = new Permission
            {
                PermissionId = 1,
                PermissionName = "Permission"
            };
            var role = new Role
            {
                IsActive = false,
                RoleId = 1,
                RoleName = "role name"
            };
            var principalRole = new PrincipalRole
            {
                RoleId = role.RoleId,
                Role = role,
                Principal = principal,
                PrincipalId = principal.PrincipalId
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
            var permissionAssignment = new PermissionAssignment
            {
                IsAllowed = true,
                Permission = permission,
                PermissionId = permission.PermissionId,
                Principal = principal,
                Resource = resource,
                ResourceId = resource.ResourceId,
                PrincipalId = principal.PrincipalId,
            };

            context.Principals.Add(principal);
            context.ResourceTypes.Add(resourceType);
            context.Resources.Add(resource);
            context.Permissions.Add(permission);
            context.Roles.Add(role);
            context.PrincipalRoles.Add(principalRole);
            context.RoleResourcePermissions.Add(roleResourcePermission);
            context.PermissionAssignments.Add(permissionAssignment);

            var results = UserQueries.CreateGetSimpleResourceAuthorizationsByPrincipalId(context, principalId).ToList();
            Assert.AreEqual(1, results.Count);
            var firstResult = results.First();
            Assert.AreEqual(principal.PrincipalId, firstResult.PrincipalId);
            Assert.AreEqual(permission.PermissionId, firstResult.PermissionId);
            Assert.AreEqual(resource.ResourceId, firstResult.ResourceId);
            Assert.AreEqual(resource.ForeignResourceId, firstResult.ForeignResourceId);
            Assert.IsTrue(firstResult.IsAllowed);
        }

        [TestMethod]
        public void TestCreateGetSimpleResourceAuthorizationsByPrincipalId_PrincipalHasRoleAndPermissionAssignment_RoleIsInactive_PermissionIsNotAllowed()
        {
            var principalId = 1;
            var principal = new Principal
            {
                PrincipalId = principalId,
            };
            var resourceType = new ResourceType
            {
                ResourceTypeId = ResourceType.Project.Id,
                ResourceTypeName = ResourceType.Project.Value
            };
            var resource = new Resource
            {
                ResourceId = 1,
                ForeignResourceId = 2,
                ResourceTypeId = resourceType.ResourceTypeId,
                ResourceType = resourceType,
            };
            var permission = new Permission
            {
                PermissionId = 1,
                PermissionName = "Permission"
            };
            var role = new Role
            {
                IsActive = false,
                RoleId = 1,
                RoleName = "role name"
            };
            var principalRole = new PrincipalRole
            {
                RoleId = role.RoleId,
                Role = role,
                Principal = principal,
                PrincipalId = principal.PrincipalId
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
            var permissionAssignment = new PermissionAssignment
            {
                IsAllowed = false,
                Permission = permission,
                PermissionId = permission.PermissionId,
                Principal = principal,
                Resource = resource,
                ResourceId = resource.ResourceId,
                PrincipalId = principal.PrincipalId,
            };

            context.Principals.Add(principal);
            context.ResourceTypes.Add(resourceType);
            context.Resources.Add(resource);
            context.Permissions.Add(permission);
            context.Roles.Add(role);
            context.PrincipalRoles.Add(principalRole);
            context.RoleResourcePermissions.Add(roleResourcePermission);
            context.PermissionAssignments.Add(permissionAssignment);

            var results = UserQueries.CreateGetSimpleResourceAuthorizationsByPrincipalId(context, principalId).ToList();
            Assert.AreEqual(1, results.Count);
            var firstResult = results.First();
            Assert.AreEqual(principal.PrincipalId, firstResult.PrincipalId);
            Assert.AreEqual(permission.PermissionId, firstResult.PermissionId);
            Assert.AreEqual(resource.ResourceId, firstResult.ResourceId);
            Assert.AreEqual(resource.ForeignResourceId, firstResult.ForeignResourceId);
            Assert.IsFalse(firstResult.IsAllowed);
        }
    }
}
