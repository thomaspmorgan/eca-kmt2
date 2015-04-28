using System;
namespace CAM.Business.Service
{
    public interface IPermissionModelService
    {
        System.Collections.Generic.List<PermissionModel> GetAllActivePermissions();
        System.Collections.Generic.List<PermissionModel> GetAllPermissions();
        PermissionModel GetPermission(int permissionId);
        System.Collections.Generic.List<PermissionModel> GetPermissionAssignedToUser(int principalId, System.Collections.Generic.List<int> resourceTypes);
        int GetPermissionIdByName(string permissionName);
        System.Collections.Generic.List<PermissionModel> GetPermissionsAssignedToRole(int roleId, int resourceTypeId, int resourceId);
        System.Collections.Generic.List<PermissionModel> GetPermissionsByResourceTypeResource(int resourceTypeId, int resourceId);
        System.Collections.Generic.List<PermissionModel> GetPermissionsByRole(int roleId);
        System.Collections.Generic.List<PermissionModel> GetPermissionsByRoleResourceTypeResource(int roleId, int resourceTypeId, int resourceId);
        System.Collections.Generic.List<PermissionModel> GetPermissionsUnassignedToRole(int roleId, int resourceTypeId, int resourceId);
        System.Collections.Generic.List<PermissionModel> GetPermissionUnAssignedToUser(int principalId, System.Collections.Generic.List<int> resourceTypes);
    }
}
