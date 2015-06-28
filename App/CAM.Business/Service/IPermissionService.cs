using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace CAM.Business.Service
{
    public interface IPermissionService
    {
        PermissionModel GetPermissionByName(string permissionName);
        Task<PermissionModel> GetPermissionByNameAsync(string permissionName);
        PermissionModel GetPermissionById(int id);
        Task<PermissionModel> GetPermissionByIdAsync(int id);

        List<IPermission> GetAllowedPermissionsByPrincipalId(int principalId);

        Task<List<IPermission>> GetAllowedPermissionsByPrincipalIdAsync(int principalId);

        bool HasPermission(int resourceId, int? parentResourceId, int permissionId, List<IPermission> grantedPermissions);
    }
}
