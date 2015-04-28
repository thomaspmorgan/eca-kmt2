using CAM.Data;
using ECA.Core.Service;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.Business.Service
{
    public class PermissionModelService : DbContextService<CamModel>, CAM.Business.Service.IPermissionModelService
    {
        public PermissionModelService(CamModel model)
            : base(model)
        {
            Contract.Requires(model != null, "The model must not be null.");
        }

        #region Public methods


        public List<PermissionModel> GetAllPermissions()
        {
            var results = (from c in Context.Permissions
                           orderby c.PermissionName
                           select new PermissionModel()
                           {
                               PermissionName = c.PermissionName,
                               PermissionDescription = c.PermissionDescription,
                               IsActive = c.IsActive,
                               PermissionId = c.PermissionId,
                               ApplicationId = c.ResourceId,
                               ResourceTypeId = c.ResourceTypeId,
                               ApplicationName = (c.ResourceTypeId == 1 ? c.Resource.Application.ApplicationName : "Other Resource"),
                               ResourceType = c.ResourceType.ResourceTypeName,
                               ResourceId = c.ResourceId,
                               ResourceName = (c.ResourceTypeId == 1 ? c.Resource.Application.ApplicationName : "Other Resource")
                           }).ToList();
            return results.OrderBy(s => s.PermissionId).ToList();

        }


        public List<PermissionModel> GetAllActivePermissions()
        {
            return GetAllPermissions().FindAll(p => p.IsActive.Equals(true));
        }

        public PermissionModel GetPermission(int permissionId)
        {
            return (from c in Context.Permissions
                    where c.PermissionId == permissionId
                    select new PermissionModel()
                    {
                        PermissionName = c.PermissionName,
                        PermissionDescription = c.PermissionDescription,
                        IsActive = c.IsActive,

                    }).FirstOrDefault();
        }

        public List<PermissionModel> GetPermissionsByResourceTypeResource(int resourceTypeId, int resourceId)
        {
            return (from c in Context.Permissions
                    where ((c.ResourceId == resourceId || c.ResourceId == null) && c.ResourceTypeId == resourceTypeId)
                    orderby c.PermissionName
                    select new PermissionModel
                    {
                        PermissionId = c.PermissionId,
                        PermissionName = c.PermissionName,
                        PermissionDescription = c.PermissionDescription
                    }).ToList();
        }

        public List<PermissionModel> GetPermissionsByRoleResourceTypeResource(int roleId, int resourceTypeId, int resourceId)
        {
            List<PermissionModel> retList = null;

            retList = (from r in Context.RoleResourcePermissions
                       where r.RoleId == roleId
                       && r.ResourceId == resourceId
                       select new PermissionModel()
                       {
                           PermissionId = r.PermissionId,
                           PermissionName = r.Permission.PermissionName,
                           PermissionDescription = r.Permission.PermissionDescription
                       }).ToList();
            return retList;

        }

        public List<PermissionModel> GetPermissionsByRole(int roleId)
        {
            List<PermissionModel> retList = null;

            retList = (from r in Context.RoleResourcePermissions
                       where r.RoleId == roleId
                       select new PermissionModel()
                       {
                           PermissionId = r.PermissionId,
                           PermissionName = r.Permission.PermissionName,
                           PermissionDescription = r.Permission.PermissionDescription,
                           ResourceId = r.ResourceId,
                           ResourceTypeId = r.Resource.ResourceTypeId,
                           ResourceType = r.Resource.ResourceType.ResourceTypeName,
                           IsActive = r.Permission.IsActive,
                           ResourceName = (r.Resource.ResourceTypeId == 1 ? r.Resource.Application.ApplicationName : "Other Resource")

                       }).ToList();
            return retList;

        }

        public List<PermissionModel> GetPermissionsAssignedToRole(int roleId, int resourceTypeId, int resourceId)
        {
            return GetPermissionsByRoleResourceTypeResource(roleId, resourceTypeId, resourceId);
        }

        public List<PermissionModel> GetPermissionsUnassignedToRole(int roleId, int resourceTypeId, int resourceId)
        {
            List<PermissionModel> allPermissions = GetPermissionsByResourceTypeResource(resourceTypeId, resourceId);
            List<PermissionModel> rolePermissions = GetPermissionsByRole(roleId); // GetPermissionsByRoleResourceTypeResource(roleId, resourceTypeId, resourceId);
            ResultsComparer comparer = new ResultsComparer();
            allPermissions.RemoveAll(s => rolePermissions.Contains(s, comparer));

            return allPermissions;
        }

        public int GetPermissionIdByName(string permissionName)
        {
            return
                (from p in Context.Permissions where p.PermissionName == permissionName select p.PermissionId)
                    .FirstOrDefault();
        }


        public List<PermissionModel> GetPermissionAssignedToUser(int principalId, List<int> resourceTypes)
        {
            List<PermissionModel> retList = new List<PermissionModel>();
            retList = (from c in Context.PermissionAssignments
                       where c.PrincipalId == principalId
                       && resourceTypes.Contains(c.Resource.ResourceTypeId)
                       select new PermissionModel()
                       {
                           PermissionName = c.Permission.PermissionName,
                           PermissionId = c.PermissionId,
                           ResourceId = c.ResourceId,
                           ApplicationId = c.ResourceId,
                           ResourceType = c.Permission.ResourceType.ResourceTypeName,
                           ResourceTypeId = c.Resource.ResourceTypeId,
                           ResourceName = (c.Resource.ResourceTypeId == 1 ? c.Resource.Application.ApplicationName : string.Empty),
                           IsActive = c.Permission.IsActive,
                           IsAllowed = c.IsAllowed
                       }).ToList();

            return retList;
        }

        public List<PermissionModel> GetPermissionUnAssignedToUser(int principalId, List<int> resourceTypes)
        {
            ResultsComparer comparer = new ResultsComparer();
            List<PermissionModel> assignedPermissions = GetPermissionAssignedToUser(principalId, resourceTypes);
            List<PermissionModel> unAssignedPermissions = GetAllPermissions();

            unAssignedPermissions.RemoveAll(s => assignedPermissions.Contains(s, comparer));
            List<PermissionModel> retList = unAssignedPermissions.FindAll(s => resourceTypes.Contains((int)s.ResourceTypeId));

            return retList.OrderBy(s => s.PermissionId).ToList(); ;
        }


        #endregion

        #region Private Methods

        private class ResultsComparer : IEqualityComparer<PermissionModel>
        {

            public int GetHashCode(PermissionModel obj)
            {
                return obj.PermissionId + (int)obj.ApplicationId;
            }

            public bool Equals(PermissionModel x, PermissionModel y)
            {
                if (x.PermissionId == y.PermissionId && x.ApplicationId == y.ApplicationId)
                    return true;
                return false;
            }
        }

        #endregion
    }
}
