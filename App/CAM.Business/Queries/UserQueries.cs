using CAM.Business.Queries.Models;
using CAM.Business.Service;
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
    /// UserQueries contains queries for retrieving users from CAM.
    /// </summary>
    public static class UserQueries
    {
        /// <summary>
        /// Returns a query to retrieve user dto's from the cam model.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The query to retrieve the users.</returns>
        public static IQueryable<UserDTO> CreateGetUsersQuery(CamModel context, QueryableOperator<UserDTO> queryOperator)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(queryOperator != null, "The query operator must not be null.");
            var query = context.UserAccounts.Select(x => new UserDTO
            {
                AdGuid = x.AdGuid,
                DisplayName = x.DisplayName,
                FirstName = x.FirstName,
                PrincipalId = x.PrincipalId,
                LastName = x.LastName,
                Email = x.EmailAddress,
            });
            query = query.Apply(queryOperator);
            return query;
        }

        /// <summary>
        /// Returns the permissions for the principal with the given id.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="principalId">The id of the principal.</param>
        /// <returns>The permissions for the principal with the given id.</returns>
        public static IQueryable<SimplePermission> CreateGetSimpleResourceAuthorizationsByPrincipalId(CamModel context, int principalId)
        {
            Contract.Requires(context != null, "The context must not be null.");

            //Get permission assignments for the principal by principal id
            var permissionAssignmentQuery = from permissionAssignment in context.PermissionAssignments

                                            join resource in context.Resources
                                            on permissionAssignment.ResourceId equals resource.ResourceId

                                            where permissionAssignment.PrincipalId == principalId

                                            group permissionAssignment by new
                                            {
                                                PrincipalId = permissionAssignment.PrincipalId,
                                                PermissionId = permissionAssignment.PermissionId,
                                                ResourceId = permissionAssignment.ResourceId,
                                                ForeignResourceId = resource.ForeignResourceId,
                                                ResourceTypeId = resource.ResourceTypeId
                                            } into g
                                            select new SimplePermission
                                            {
                                                PrincipalId = g.Key.PrincipalId,
                                                PermissionId = g.Key.PermissionId,
                                                ResourceId = g.Key.ResourceId,
                                                ForeignResourceId = g.Key.ForeignResourceId,
                                                ResourceTypeId = g.Key.ResourceTypeId,
                                                IsAllowed = !(g.Count(x => !x.IsAllowed) > 0)
                                            };
            //get permissions based on roles by principal id
            var roleResourcePermissionQuery = from roleResourcePermission in context.RoleResourcePermissions

                                              join resource in context.Resources
                                              on roleResourcePermission.ResourceId equals resource.ResourceId

                                              join principalRole in context.PrincipalRoles
                                              on roleResourcePermission.RoleId equals principalRole.RoleId

                                              join role in context.Roles
                                              on roleResourcePermission.RoleId equals role.RoleId

                                              where principalRole.PrincipalId == principalId
                                              && role.IsActive

                                              group roleResourcePermission by new
                                              {
                                                  PrincipalId = principalRole.PrincipalId,
                                                  PermissionId = roleResourcePermission.PermissionId,
                                                  ResourceId = roleResourcePermission.ResourceId,
                                                  ForeignResourceId = resource.ForeignResourceId,
                                                  ResourceTypeId = resource.ResourceTypeId
                                              } into g
                                              select new SimplePermission
                                              {
                                                  PrincipalId = g.Key.PrincipalId,
                                                  PermissionId = g.Key.PermissionId,
                                                  ResourceId = g.Key.ResourceId,
                                                  ForeignResourceId = g.Key.ForeignResourceId,
                                                  ResourceTypeId = g.Key.ResourceTypeId,
                                                  IsAllowed = true
                                              };

            var sendToSevisPermissionQuery = from principal in context.Principals
                                             let hasSevisUserAccounts = principal.SevisAccounts.Count() > 0
                                             let sendToSevisPermission = context.Permissions.Where(x => x.PermissionId == Permission.SEND_TO_SEVIS_ID).FirstOrDefault()
                                             let applicationResource = sendToSevisPermission != null ? sendToSevisPermission.Resource : null

                                             where sendToSevisPermission != null 
                                             && sendToSevisPermission.ResourceId.HasValue
                                             && hasSevisUserAccounts
                                             && principal.PrincipalId == principalId

                                             select new SimplePermission
                                             {
                                                 PrincipalId = principal.PrincipalId,
                                                 PermissionId = sendToSevisPermission.PermissionId,
                                                 ResourceId = applicationResource.ResourceId,
                                                 ForeignResourceId = applicationResource.ForeignResourceId,
                                                 ResourceTypeId = applicationResource.ResourceTypeId,
                                                 IsAllowed = true,
                                             };

            //get the union of the two queries
            var unionQuery = from groupedPermission in permissionAssignmentQuery.Union(roleResourcePermissionQuery).Union(sendToSevisPermissionQuery)
                             group groupedPermission by new
                             {
                                 PrincipalId = groupedPermission.PrincipalId,
                                 PermissionId = groupedPermission.PermissionId,
                                 ResourceId = groupedPermission.ResourceId,
                                 ForeignResourceId = groupedPermission.ForeignResourceId,
                                 ResourceTypeId = groupedPermission.ResourceTypeId
                             } into g
                             select new SimplePermission
                             {
                                 PrincipalId = g.Key.PrincipalId,
                                 PermissionId = g.Key.PermissionId,
                                 ResourceId = g.Key.ResourceId,
                                 ForeignResourceId = g.Key.ForeignResourceId,
                                 ResourceTypeId = g.Key.ResourceTypeId,
                                 IsAllowed = !(g.Count(x => !x.IsAllowed) > 0)
                             };
            return unionQuery;
        }
    }
}
