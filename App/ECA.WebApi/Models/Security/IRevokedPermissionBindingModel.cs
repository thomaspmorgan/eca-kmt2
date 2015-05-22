using CAM.Business.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.WebApi.Models.Security
{
    /// <summary>
    /// An IRevokedPermissionBindingModel is a permission binding that is created by a client intended to revoke a permission
    /// explicity.
    /// </summary>
    public interface IRevokedPermissionBindingModel
    {
        /// <summary>
        /// Returns a RevokedPermission.
        /// </summary>
        /// <param name="grantorUserId">The principal id of the user revoking the permission.</param>
        /// <returns>The RevokedPermission.</returns>
        RevokedPermission ToRevokedPermission(int grantorUserId);
    }
}
