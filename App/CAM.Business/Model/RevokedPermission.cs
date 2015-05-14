using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.Business.Model
{
    /// <summary>
    /// A RevokedPermission is permission that must be explicity denied to a user.
    /// </summary>
    public class RevokedPermission : GrantedPermission
    {
        /// <summary>
        /// Creates a new RevokedPermission business entity.  The IsAllowed property is set to false.
        /// </summary>
        /// <param name="granteePrincipalId">The principal id of the Grantee, i.e. the user being given the permission.</param>
        /// <param name="permissionId">The permission id.</param>
        /// <param name="foreignResourceId">The id of the resource not in Cam, e.g. the Program Id or the Project Id..</param>
        /// <param name="resourceType">The resource type as a string.</param>
        /// <param name="grantorUserId">The principal id of the Grantor, i.e. the user giving the permission to another principal.</param>
        public RevokedPermission(int granteePrincipalId, int permissionId, int foreignResourceId, string resourceType, int grantorUserId)
            : base(granteePrincipalId, permissionId, foreignResourceId, resourceType, grantorUserId)
        {
            this.IsAllowed = false;
        }
    }
}
