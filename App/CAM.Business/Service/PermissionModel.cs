using System.Collections.Generic;
using System.Linq;
using CAM.Data;

namespace CAM.Business.Service
{
    public class PermissionModel
    {
        public string PermissionName { get; set; }
        public string PermissionDescription { get; set; }
        public bool IsActive { get; set; }
        public int PermissionId { get; set; }
        public string ApplicationName { get; set; }
        public int? ApplicationId { get; set; }
        public string ResourceType { get; set; }

        public int? ResourceId { get; set; }
        public int? ResourceTypeId { get; set; }
        public string ResourceName { get; set; }
    }

    public class PrincipalPermissionModel : PermissionModel
    {
        public bool IsAllowed { get; set; }
    }
}
