using CAM.Business.Model;
using CAM.Business.Queries.Models;
using CAM.Data;
using ECA.Core.DynamicLinq;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.Business.Queries
{
    /// <summary>
    /// The ResourceQueries class contains queries for CAM resources in a CamModel.
    /// </summary>
    public static class ResourceQueries
    {
        /// <summary>
        /// Returns a query to retrieve a resource by foreign resource id and resource type id.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="foreignResourceId">The foreign resource id.</param>
        /// <param name="resourceTypeId">The resource type id.</param>
        /// <returns>The query to retrieve the resource.</returns>
        public static IQueryable<Resource> CreateGetResourceByForeignResourceIdQuery(CamModel context, int foreignResourceId, int resourceTypeId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var query = from resource in context.Resources
                        where
                            resource.ResourceTypeId == resourceTypeId &&
                            resource.ForeignResourceId == foreignResourceId
                        select resource;
            return query;
        }

        /// <summary>
        /// Creates a query to retrieve all ResourceAuthorizations from the CAM granted by role.  Use this query in conjunction with another to further filter the results.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <returns>The query to retrieve all ResourceAuthroizations from the Cam.</returns>
        public static IQueryable<ResourceAuthorization> CreateGetResourceAuthorizationsByRoleQuery(CamModel context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var query = from resource in context.Resources

                        join resourceType in context.ResourceTypes
                        on resource.ResourceTypeId equals resourceType.ResourceTypeId

                        join roleResourcePermission in context.RoleResourcePermissions
                        on resource.ResourceId equals roleResourcePermission.ResourceId

                        join permission in context.Permissions
                        on roleResourcePermission.PermissionId equals permission.PermissionId

                        join role in context.Roles
                        on roleResourcePermission.RoleId equals role.RoleId

                        join principalRole in context.PrincipalRoles
                        on role.RoleId equals principalRole.RoleId

                        join principal in context.Principals
                        on principalRole.PrincipalId equals principal.PrincipalId

                        join userAccount in context.UserAccounts
                        on principal.PrincipalId equals userAccount.PrincipalId

                        join permissionAssignment in context.PermissionAssignments
                        on resource equals permissionAssignment.Resource into permissionAssignments
                        from tempPermissionAssignment in permissionAssignments
                            .Where(x => !x.IsAllowed 
                                && x.PrincipalId == principal.PrincipalId 
                                && x.PermissionId == permission.PermissionId).DefaultIfEmpty()

                        select new ResourceAuthorization
                        {
                            DisplayName = userAccount.DisplayName,
                            EmailAddress = userAccount.EmailAddress,
                            ForeignResourceId = resource.ForeignResourceId,
                            IsAllowed = tempPermissionAssignment == null ? true : tempPermissionAssignment.IsAllowed,
                            IsGrantedByPermission = false,
                            IsGrantedByRole = true,
                            PermissionDescription = permission.PermissionDescription,
                            PermissionId = permission.PermissionId,
                            PermissionName = permission.PermissionName,
                            PrincipalId = principal.PrincipalId,
                            ResourceId = resource.ResourceId,
                            ResourceType = resourceType.ResourceTypeName,
                            ResourceTypeId = resource.ResourceTypeId,
                            RoleId = role.RoleId,
                            RoleName = role.RoleName,
                        };
            return query;
        }


        /// <summary>
        /// Creates a query to retrieve all ResourceAuthorizations from the CAM granted by role.  Use this query in conjunction with another to further filter the results.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <returns>The query to retrieve all ResourceAuthroizations from the Cam.</returns>
        public static IQueryable<ResourceAuthorization> CreateGetResourceAuthorizationsByPermissionAssignmentsQuery(CamModel context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var query = from resource in context.Resources

                        join resourceType in context.ResourceTypes
                        on resource.ResourceTypeId equals resourceType.ResourceTypeId

                        join permissionAssignment in context.PermissionAssignments
                        on resource.ResourceId equals permissionAssignment.ResourceId

                        join permission in context.Permissions
                        on permissionAssignment.PermissionId equals permission.PermissionId
                        
                        join principal in context.Principals
                        on permissionAssignment.PrincipalId equals principal.PrincipalId

                        join userAccount in context.UserAccounts
                        on principal.PrincipalId equals userAccount.PrincipalId

                        select new ResourceAuthorization
                        {
                            DisplayName = userAccount.DisplayName,
                            EmailAddress = userAccount.EmailAddress,
                            ForeignResourceId = resource.ForeignResourceId,
                            IsAllowed = permissionAssignment.IsAllowed,
                            IsGrantedByPermission = true,
                            IsGrantedByRole = false,
                            PermissionDescription = permission.PermissionDescription,
                            PermissionId = permission.PermissionId,
                            PermissionName = permission.PermissionName,
                            PrincipalId = principal.PrincipalId,
                            ResourceId = resource.ResourceId,
                            ResourceType = resourceType.ResourceTypeName,
                            ResourceTypeId = resource.ResourceTypeId,
                            RoleId = -1,
                            RoleName = null
                        };
            return query;
        }


        /// <summary>
        /// Returns a filtered and sorted query of resource authorizations currently in the cam granted by both permissions and roles.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The query to retrieve filterd and sorted resource authorizations granted by permissions and roles in the CAM.</returns>
        public static IQueryable<ResourceAuthorization> CreateGetResourceAuthorizationsQuery(CamModel context, QueryableOperator<ResourceAuthorization> queryOperator)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var query = CreateGetResourceAuthorizationsByPermissionAssignmentsQuery(context).Union(CreateGetResourceAuthorizationsByRoleQuery(context));
            query = query.Apply(queryOperator);
            return query;
        }

        /// <summary>
        /// Creates a query to get the possible permissions for a resource in CAM.  If the resourceId has a value, then 
        /// permissions designated specifically for that resource will also be included in the query.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="resourceType">The resource type.</param>
        /// <param name="resourceId">The resource id.  Or null, if resource type permissions only should be included.</param>
        /// <returns>The query to retrieve resource permissions dtos.</returns>
        public static IQueryable<ResourcePermissionDTO> CreateGetResourcePermissionsQuery(CamModel context, string resourceType, int? resourceId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(ResourceType.GetStaticLookup(resourceType) != null, "The resource type must be valid.");
            var resourceTypeId = ResourceType.GetStaticLookup(resourceType).Id;
            var query = from p in context.Permissions
                        where p.ResourceTypeId == resourceTypeId
                        && !p.ResourceId.HasValue
                        select new ResourcePermissionDTO
                        {
                            PermissionDescription = p.PermissionDescription,
                            PermissionId = p.PermissionId,
                            PermissionName = p.PermissionName,
                        };
            if (resourceId.HasValue)
            {
                var resourceIdPermissionsQuery = from p in context.Permissions
                                                 let resource = p.Resource
                                                 where resource.ResourceId == resourceId.Value
                                                 select new ResourcePermissionDTO
                                                 {
                                                     PermissionDescription = p.PermissionDescription,
                                                     PermissionId = p.PermissionId,
                                                     PermissionName = p.PermissionName
                                                 };
                query = query.Union(resourceIdPermissionsQuery);
            }
            query = query.OrderBy(x => x.PermissionName);
            return query;
        }
    }
}
