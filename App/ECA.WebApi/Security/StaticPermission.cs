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
        /// Gets or sets the foreign resource id, e.g. the program id.
        /// </summary>
        public int ForeignResourceId { get; set; }

        /// <summary>
        /// Returns the resource id.
        /// </summary>
        /// <param name="actionArguments">The web api action arguments.</param>
        /// <returns>The foreign resource id.</returns>
        public override int GetResourceId(IDictionary<string, object> actionArguments)
        {
            return ForeignResourceId;
        }

        /// <summary>
        /// Returns the formatted string of this permission.  This string format is the same format the Parse method expects.
        /// </summary>
        /// <returns>The formatted string.</returns>
        public override string ToString()
        {
            return String.Format("Permission Name:  {0}, Resource Type:  {1},  Foreign Resource Id:  {2}", PermissionName, ResourceType, ForeignResourceId);
        }
    }
}