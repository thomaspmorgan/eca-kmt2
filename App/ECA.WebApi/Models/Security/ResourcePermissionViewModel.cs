using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Models.Security
{
    /// <summary>
    /// A ResourcePermissionViewModel is used to allow a client to show permissions granted to a resource.
    /// </summary>
    public class ResourcePermissionViewModel
    {
        /// <summary>
        /// The name of the permission.
        /// </summary>
        public string PermissionName { get; set; }

        /// <summary>
        /// The id of the permission.
        /// </summary>
        public int PermissionId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            var otherType = obj as ResourcePermissionViewModel;
            if (otherType == null)
            {
                return false;
            }
            return this.PermissionId == otherType.PermissionId;
        }

        /// <summary>
        /// Returns the hash code.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            return this.PermissionId.GetHashCode();
        }
    }
}