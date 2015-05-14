using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.Business.Model
{
    /// <summary>
    /// A RevokedPermission is permission that either must be explicity removed from a user.
    /// </summary>
    public class RevokedPermission : GrantedPermission
    {
        public RevokedPermission(int granteePrincipalId, int permissionId, int foreignResourceId, string resourceType, int grantorUserId)
            : base(granteePrincipalId, permissionId, foreignResourceId, resourceType, grantorUserId)
        {
            this.IsAllowed = false;
        }
    }
}
