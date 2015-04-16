using System.Collections.Generic;
using System.Linq;

namespace CAM.Business.Service
{
    public interface IPermissionStore<T>
    {
        List<IPermission> Permissions { get; set; }
        bool HasPermission(IPermission permission);
        bool HasPermission(int principalId, int permissionId, int resourceId);
        bool HasPermission(int principalId, string permissionName, int resourceId);
        bool HasPermission(int principalId, int resourceId);
        bool HasPermission(string permissionName);
        bool HasPermissionForApplication(int principalId, string permissionName, int applicationId);
        bool HasPermissionForApplication(int principalId, string permissionName);
        bool HasPermissionForApplication(List<string> excludedPermissionNames);

        int GetPermissionIdByName(string permissionName);
        string GetPermissionNameById(int permissionId);
        int GetResourceIdForApplicationId(int applicationId);
        int? GetResourceIdByForeignResourceId(int foreignResourceId, int resourceTypeId);
        int? GetResourceTypeId(string resourceTypeName);

        void LoadUserPermissionsForResource(int principleId, int resourceId);
        void LoadUserPermissions(int principalId);

        int? ResourceId { get; set; }
        int? PrincipalId { get; set; }

    }
}
