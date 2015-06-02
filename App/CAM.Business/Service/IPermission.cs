
using System.Collections.Generic;


namespace CAM.Business.Service
{
    public interface IPermission
    {
        int PrincipalId { get; set; }
        int PermissionId { get; set; }
        int ResourceId { get; set; }
        bool IsAllowed { get; set; }
    }
}
