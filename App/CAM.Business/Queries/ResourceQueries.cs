using CAM.Business.Model;
using CAM.Business.Queries.Models;
using CAM.Business.Service;
using CAM.Data;
using ECA.Core.DynamicLinq;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
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
        /// The role id to be set when a permission is granted by a permission assignment and not a role.
        /// </summary>
        public const int AUTHORIZATION_ASSIGNED_BY_PERMISSION_ROLE_ID = -1;

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
        /// Permissions that are not allowed in PermissionAssignments will be removed by this query.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <returns>The query to retrieve all ResourceAuthroizations from the Cam.</returns>
        public static IQueryable<SimpleResourceAuthorization> CreateGetResourceAuthorizationsByRoleQuery(CamModel context)
        {
            Contract.Requires(context != null, "The context must not be null.");
       

            var query = from role in context.Roles

                        join roleResourcePermission in context.RoleResourcePermissions
                        on role.RoleId equals roleResourcePermission.RoleId

                        join principalRole in context.PrincipalRoles
                        on roleResourcePermission.RoleId equals principalRole.RoleId                        

                        where role.IsActive

                        select new SimpleResourceAuthorization
                        {
                            AssignedOn = roleResourcePermission.AssignedOn,
                            IsAllowed = true,
                            IsGrantedByInheritance = false,
                            IsGrantedByPermission = false,
                            IsGrantedByRole = true,
                            PermissionId = roleResourcePermission.PermissionId,
                            PrincipalId = principalRole.PrincipalId,
                            ResourceId = roleResourcePermission.ResourceId,
                            RoleId = role.RoleId,
                        };


            return query;
        }

        /// <summary>
        /// Creates a query to retrieve resource authorizations that are granted by a role and inherited by the parent resource.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <returns>The query to retrieve all ResourceAuthroizations from the Cam.</returns>
        public static IQueryable<SimpleResourceAuthorization> CreateGetResourceAuthorizationsByInheritedRolePermissionsQuery(CamModel context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var query = from resource in context.Resources

                        join roleResourcePermission in context.RoleResourcePermissions
                        on resource.ParentResourceId equals roleResourcePermission.ResourceId

                        join permission in context.Permissions
                        on roleResourcePermission.PermissionId equals permission.PermissionId

                        join role in context.Roles
                        on roleResourcePermission.RoleId equals role.RoleId

                        join principalRole in context.PrincipalRoles
                        on roleResourcePermission.RoleId equals principalRole.RoleId

                        where role.IsActive

                        select new SimpleResourceAuthorization
                        {
                            AssignedOn = roleResourcePermission.AssignedOn,
                            IsAllowed = true,
                            IsGrantedByInheritance = true,
                            IsGrantedByPermission = false,
                            IsGrantedByRole = true,
                            PermissionId = permission.PermissionId,
                            PrincipalId = principalRole.PrincipalId,
                            ResourceId = resource.ResourceId,
                            RoleId = roleResourcePermission.RoleId,
                        };

            return query;
        }


        /// <summary>
        /// Creates a query to retrieve all ResourceAuthorizations from the CAM granted by role.  Use this query in conjunction with another to further filter the results.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <returns>The query to retrieve all ResourceAuthroizations from the Cam.</returns>
        public static IQueryable<SimpleResourceAuthorization> CreateGetResourceAuthorizationsByPermissionAssignmentsQuery(CamModel context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var query = from permissionAssignment in context.PermissionAssignments
                        select new SimpleResourceAuthorization
                        {
                            AssignedOn = permissionAssignment.AssignedOn,
                            IsAllowed = permissionAssignment.IsAllowed,
                            IsGrantedByInheritance = false,
                            IsGrantedByPermission = true,
                            IsGrantedByRole = false,
                            PermissionId = permissionAssignment.PermissionId,
                            PrincipalId = permissionAssignment.PrincipalId,
                            ResourceId = permissionAssignment.ResourceId,
                            RoleId = AUTHORIZATION_ASSIGNED_BY_PERMISSION_ROLE_ID,
                        };
            return query;
        }

        /// <summary>
        /// Creates a query to retrieve all ResourceAuthorizations from the CAM granted permission assignments and inherited.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <returns>The query to retrieve all ResourceAuthroizations from the Cam.</returns>
        public static IQueryable<SimpleResourceAuthorization> CreateGetResourceAuthorizationsByInheritedPermissionAssignmentQuery(CamModel context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var query = from resource in context.Resources

                        join parentPermissionAssignment in context.PermissionAssignments
                        on resource.ParentResourceId equals parentPermissionAssignment.ResourceId

                        join permission in context.Permissions
                        on parentPermissionAssignment.PermissionId equals permission.PermissionId

                        where permission.ResourceTypeId == resource.ResourceTypeId

                        select new SimpleResourceAuthorization
                        {
                            AssignedOn = parentPermissionAssignment.AssignedOn,
                            IsAllowed = parentPermissionAssignment.IsAllowed,
                            IsGrantedByInheritance = true,
                            IsGrantedByPermission = true,
                            IsGrantedByRole = false,
                            PermissionId = permission.PermissionId,
                            PrincipalId = parentPermissionAssignment.PrincipalId,
                            ResourceId = resource.ResourceId,
                            RoleId = AUTHORIZATION_ASSIGNED_BY_PERMISSION_ROLE_ID,
                        };
            return query;
        }

        /// <summary>
        /// Creates a query to return all resource authorizations from the given context.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <returns>All resource authorizations in the context.</returns>
        public static IQueryable<ResourceAuthorization> CreateGetResourceAuthorizationsQuery(CamModel context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var query = CreateGetResourceAuthorizationsByPermissionAssignmentsQuery(context)
                .Union(CreateGetResourceAuthorizationsByRoleQuery(context))
                .Union(CreateGetResourceAuthorizationsByInheritedPermissionAssignmentQuery(context))
                .Union(CreateGetResourceAuthorizationsByInheritedRolePermissionsQuery(context));

            var resourceAuthQuery = from simplePermission in query

                                    join resource in context.Resources
                                    on simplePermission.ResourceId equals resource.ResourceId

                                    join permission in context.Permissions
                                    on simplePermission.PermissionId equals permission.PermissionId

                                    join resourceType in context.ResourceTypes
                                    on resource.ResourceTypeId equals resourceType.ResourceTypeId

                                    join principal in context.Principals
                                    on simplePermission.PrincipalId equals principal.PrincipalId

                                    join tempUserAccount in context.UserAccounts
                                    on principal.PrincipalId equals tempUserAccount.PrincipalId into joinedUserAccounts
                                    from joinedUserAccount in joinedUserAccounts.DefaultIfEmpty()

                                    join tempRole in context.Roles
                                    on simplePermission.RoleId equals tempRole.RoleId into joinedRoles
                                    from joinedRole in joinedRoles.DefaultIfEmpty()

                                    select new ResourceAuthorization
                                    {
                                        AssignedOn = simplePermission.AssignedOn,
                                        DisplayName = joinedUserAccount != null ? joinedUserAccount.DisplayName : null,
                                        EmailAddress = joinedUserAccount != null ? joinedUserAccount.EmailAddress : null,
                                        ForeignResourceId = resource.ForeignResourceId,
                                        IsAllowed = simplePermission.IsAllowed,
                                        IsGrantedByInheritance = simplePermission.IsGrantedByInheritance,
                                        IsGrantedByPermission = simplePermission.IsGrantedByPermission,
                                        IsGrantedByRole = simplePermission.IsGrantedByRole,
                                        PermissionDescription = permission.PermissionDescription,
                                        PermissionId = simplePermission.PermissionId,
                                        PermissionName = permission.PermissionName,
                                        PrincipalId = principal.PrincipalId,
                                        ResourceId = simplePermission.ResourceId,
                                        ResourceType = resourceType.ResourceTypeName,
                                        ResourceTypeId = resource.ResourceTypeId,
                                        RoleId = simplePermission.RoleId,
                                        RoleName = joinedRole != null ? joinedRole.RoleName : null
                                    };
            return resourceAuthQuery;
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
            var query = CreateGetResourceAuthorizationsQuery(context);
            query = query.Apply(queryOperator);
            return query;
        }

        /// <summary>
        /// Returns a query to return a resource authorization info dto that contains information regarding a resource and it's principals related to it.  For example,
        /// use this query to determine the last revised on date for a resource and all of it's permissions.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <returns>The info dto.</returns>
        public static IQueryable<ResourceAuthorizationInfoDTO> CreateGetResourceAuthorizationInfoDTOQuery(CamModel context, string resourceType, int foreignResourceId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(resourceType != null, "The resource type must not be null.");
            var query = CreateGetResourceAuthorizationsQuery(context);
            query = query.Where(x => x.IsAllowed && x.ResourceType == resourceType && x.ForeignResourceId == foreignResourceId);
            var groupedQuery = from resourceAuthorization in query
                               group resourceAuthorization by resourceAuthorization.ResourceId into g
                               select new ResourceAuthorizationInfoDTO
                               {
                                   AllowedPrincipalsCount = g.Select(x => x.PrincipalId).Distinct().Count(),
                                   LastRevisedOn = g.Max(x => x.AssignedOn)
                               };
            return groupedQuery;
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
                        where (p.ResourceTypeId == resourceTypeId || p.ParentResourceTypeId == resourceTypeId)
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

        public class SimpleResourceAuthorization
        {
            /// <summary>
            /// Gets or sets the date the permission was assigned.
            /// </summary>
            public DateTimeOffset AssignedOn { get; set; }

            /// <summary>
            /// Gets or sets whether or not this resource authorization is granted by a role.
            /// </summary>
            public bool IsGrantedByRole { get; set; }

            /// <summary>
            /// Gets or sets whether or not this resource authorization is granted by a permission explicity.
            /// </summary>
            public bool IsGrantedByPermission { get; set; }

            /// <summary>
            /// Gets or sets the value indicating whether this permission is granted by a resource's parent permission.
            /// </summary>
            public bool IsGrantedByInheritance { get; set; }

            /// <summary>
            /// Gets or sets the role id this authorization is granted by.
            /// </summary>
            public int RoleId { get; set; }

            /// <summary>
            /// Gets or sets the principal id.
            /// </summary>
            public int PrincipalId { get; set; }

            /// <summary>
            /// Gets or sets the permission id.
            /// </summary>
            public int PermissionId { get; set; }

            /// <summary>
            /// Gets or sets the resource id.
            /// </summary>
            public int ResourceId { get; set; }

            /// <summary>
            /// Gets or sets is allowed.
            /// </summary>
            public bool IsAllowed { get; set; }
        }
    }
}
