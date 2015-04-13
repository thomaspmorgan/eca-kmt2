using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Security
{
    public class StaticPermission : PermissionBase
    {

        public int ResourceId { get; set; }

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