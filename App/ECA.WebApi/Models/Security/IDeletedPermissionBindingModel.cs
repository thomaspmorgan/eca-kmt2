using CAM.Business.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.WebApi.Models.Security
{
    /// <summary>
    /// An IDeletedPermissionBindingModel is a permission binding that is created by a client to delete a permission.
    /// </summary>
    public interface IDeletedPermissionBindingModel
    {
        /// <summary>
        /// Returns a DeletedPermission from this instance.
        /// </summary>
        /// <param name="grantorUserId">The principal id of the user deleting the permission.</param>
        /// <returns>The DeletedPermission.</returns>
        DeletedPermission ToDeletedPermission(int grantorUserId);
    }
}
