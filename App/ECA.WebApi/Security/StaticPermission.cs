using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Security
{
    /// <summary>
    /// A StaticPermission is a permission whose resource id will never change.  For example, the resource id of the
    /// application is always known.
    /// </summary>
    public class StaticPermission : PermissionBase
    {
        /// <summary>
        /// Gets or sets the resource id.
        /// </summary>
        public int ResourceId { get; set; }

        /// <summary>
        /// Returns the resource id.
        /// </summary>
        /// <param name="actionArguments">The web api action arguments.</param>
        /// <returns>The resource id.</returns>
        public override int GetResourceId(IDictionary<string, object> actionArguments)
        {
            return ResourceId;
        }

        /// <summary>
        /// Returns the formatted string of this permission.  This string format is the same format the Parse method expects.
        /// </summary>
        /// <returns>The formatted string.</returns>
        public override string ToString()
        {
            return String.Format("{0}:{1}({2})", PermissionName, ResourceType, ResourceId);
        }
    }
}