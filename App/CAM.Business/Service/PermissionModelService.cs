using CAM.Data;
using System.Data.Entity;
using ECA.Core.Service;
using NLog.Interface;
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
        private readonly ILogger logger = new LoggerAdapter(NLog.LogManager.GetCurrentClassLogger());

        private static int APPLICATION_RESOURCE_TYPE_ID = ResourceType.Application.Id;

        private const string OTHER_RESOURCE_NAME = "Other Resource";

        public PermissionModelService(CamModel model)
            : base(model)
        {
            Contract.Requires(model != null, "The model must not be null.");
        }

        #region Public methods

        //public List<PermissionModel> GetAllPermissions()
        //{
        //    var results = (from c in Context.Permissions
        //                   orderby c.PermissionName
        //                   select new PermissionModel()
        //                   {
        //                       PermissionName = c.PermissionName,
        //                       PermissionDescription = c.PermissionDescription,
        //                       IsActive = c.IsActive,
        //                       PermissionId = c.PermissionId,
        //                       ApplicationId = c.ResourceId,
        //                       ResourceTypeId = c.ResourceTypeId,
        //                       ApplicationName = (c.ResourceTypeId == APPLICATION_RESOURCE_TYPE_ID ? c.Resource.Application.ApplicationName : "Other Resource"),
        //                       ResourceType = c.ResourceType.ResourceTypeName,
        //                       ResourceId = c.ResourceId,
        //                       ResourceName = (c.ResourceTypeId == APPLICATION_RESOURCE_TYPE_ID ? c.Resource.Application.ApplicationName : "Other Resource")
        //                   }).ToList();
        //    return results.OrderBy(s => s.PermissionId).ToList();

        //}


        //public List<PermissionModel> GetAllActivePermissions()
        //{
        //    return GetAllPermissions().FindAll(p => p.IsActive.Equals(true));
        //}

        //public PermissionModel GetPermission(int permissionId)
        //{
        //    return (from c in Context.Permissions
        //            where c.PermissionId == permissionId
        //            select new PermissionModel()
        //            {
        //                PermissionName = c.PermissionName,
        //                PermissionDescription = c.PermissionDescription,
        //                IsActive = c.IsActive,

        //            }).FirstOrDefault();
        //}

        //public List<PermissionModel> GetPermissionsByResourceTypeResource(int resourceTypeId, int resourceId)
        //{
        //    return (from c in Context.Permissions
        //            where ((c.ResourceId == resourceId || c.ResourceId == null) && c.ResourceTypeId == resourceTypeId)
        //            orderby c.PermissionName
        //            select new PermissionModel
        //            {
        //                PermissionId = c.PermissionId,
        //                PermissionName = c.PermissionName,
        //                PermissionDescription = c.PermissionDescription
        //            }).ToList();
        //}

        //public List<PermissionModel> GetPermissionsByRoleResourceTypeResource(int roleId, int resourceTypeId, int resourceId)
        //{
        //    List<PermissionModel> retList = null;

        //    retList = (from r in Context.RoleResourcePermissions
        //               where r.RoleId == roleId
        //               && r.ResourceId == resourceId
        //               select new PermissionModel()
        //               {
        //                   PermissionId = r.PermissionId,
        //                   PermissionName = r.Permission.PermissionName,
        //                   PermissionDescription = r.Permission.PermissionDescription
        //               }).ToList();
        //    return retList;

        //}

        //public List<PermissionModel> GetPermissionsByRole(int roleId)
        //{
        //    List<PermissionModel> retList = null;

        //    retList = (from r in Context.RoleResourcePermissions
        //               where r.RoleId == roleId
        //               select new PermissionModel()
        //               {
        //                   PermissionId = r.PermissionId,
        //                   PermissionName = r.Permission.PermissionName,
        //                   PermissionDescription = r.Permission.PermissionDescription,
        //                   ResourceId = r.ResourceId,
        //                   ResourceTypeId = r.Resource.ResourceTypeId,
        //                   ResourceType = r.Resource.ResourceType.ResourceTypeName,
        //                   IsActive = r.Permission.IsActive,
        //                   ResourceName = (r.Resource.ResourceTypeId == APPLICATION_RESOURCE_TYPE_ID ? r.Resource.Application.ApplicationName : "Other Resource")

        //               }).ToList();
        //    return retList;

        //}

        //public List<PermissionModel> GetPermissionsAssignedToRole(int roleId, int resourceTypeId, int resourceId)
        //{
        //    return GetPermissionsByRoleResourceTypeResource(roleId, resourceTypeId, resourceId);
        //}

        //public List<PermissionModel> GetPermissionsUnassignedToRole(int roleId, int resourceTypeId, int resourceId)
        //{
        //    List<PermissionModel> allPermissions = GetPermissionsByResourceTypeResource(resourceTypeId, resourceId);
        //    List<PermissionModel> rolePermissions = GetPermissionsByRole(roleId); 
        //    ResultsComparer comparer = new ResultsComparer();
        //    allPermissions.RemoveAll(s => rolePermissions.Contains(s, comparer));

        //    return allPermissions;
        //}

        public int GetPermissionIdByName(string permissionName)
        {
            var lookup = CAM.Data.Permission.GetStaticLookup(permissionName);
            Contract.Assert(lookup != null, "The permission name must exist.");
            return lookup.Id;
        }

        public string GetPermissionNameById(int id)
        {
            var lookup = CAM.Data.Permission.GetStaticLookup(id);
            Contract.Assert(lookup != null, "The permission name must exist.");
            return lookup.Value;
        }

        //private IQueryable<PrincipalPermissionModel> CreateGetPrincipalPermissionModelQuery(int principalId, IEnumerable<int> resourceTypes)
        //{
        //    var query = from c in Context.PermissionAssignments
        //                let permission = c.Permission
        //                let resource = c.Resource
        //                where c.PrincipalId == principalId
        //                && resourceTypes.Contains(resource.ResourceTypeId)
        //                orderby permission.PermissionName
        //                select new PrincipalPermissionModel
        //                {
        //                    PermissionName = c.Permission.PermissionName,
        //                    PermissionId = c.PermissionId,
        //                    ResourceId = c.ResourceId,
        //                    ApplicationId = c.ResourceId,
        //                    ResourceType = c.Permission.ResourceType.ResourceTypeName,
        //                    ResourceTypeId = c.Resource.ResourceTypeId,
        //                    ResourceName = (c.Resource.ResourceTypeId == APPLICATION_RESOURCE_TYPE_ID ? c.Resource.Application.ApplicationName : OTHER_RESOURCE_NAME),
        //                    IsActive = c.Permission.IsActive,
        //                    IsAllowed = c.IsAllowed
        //                };
        //    return query;
        //}

        //public List<PrincipalPermissionModel> GetPermissionsAssignedToUser(int principalId, IEnumerable<int> resourceTypes)
        //{
        //    return CreateGetPrincipalPermissionModelQuery(principalId, resourceTypes).ToList();
        //}

        //public Task<List<PrincipalPermissionModel>> GetPermissionsAssignedToUserAsync(int principalId, IEnumerable<int> resourceTypes)
        //{
        //    return CreateGetPrincipalPermissionModelQuery(principalId, resourceTypes).ToListAsync();
        //}

        //public List<PermissionModel> GetPermissionUnAssignedToUser(int principalId, List<int> resourceTypes)
        //{
        //    ResultsComparer comparer = new ResultsComparer();
        //    List<PermissionModel> assignedPermissions = GetPermissionAssignedToUser(principalId, resourceTypes);
        //    List<PermissionModel> unAssignedPermissions = GetAllPermissions();

        //    unAssignedPermissions.RemoveAll(s => assignedPermissions.Contains(s, comparer));
        //    List<PermissionModel> retList = unAssignedPermissions.FindAll(s => resourceTypes.Contains((int)s.ResourceTypeId));

        //    return retList.OrderBy(s => s.PermissionId).ToList();
        //}


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
