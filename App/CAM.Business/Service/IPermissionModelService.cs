using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace CAM.Business.Service
{
    public interface IPermissionModelService
    {
        int GetPermissionIdByName(string permissionName);
        string GetPermissionNameById(int id);
    }
}
