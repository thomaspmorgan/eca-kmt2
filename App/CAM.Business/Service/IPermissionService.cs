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
        PermissionModel GetPermissionByIdAsync(int id);
    }
}
