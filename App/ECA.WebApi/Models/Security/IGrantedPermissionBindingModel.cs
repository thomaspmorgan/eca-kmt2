using CAM.Business.Model;
using ECA.WebApi.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace ECA.WebApi.Models.Security
{
    /// <summary>
    /// An IGrantedPermissionBindingModel is a model created by a client granting permission to a resource.
    /// </summary>
    public interface IGrantedPermissionBindingModel
    {
        /// <summary>
        /// Returns a granted permission instance from this.
        /// </summary>
        /// <param name="grantorUserId">The principal id of the user granting permission.</param>
        /// <returns>The GrantedPermission.</returns>
        GrantedPermission ToGrantedPermission(int grantorUserId);
    }
}