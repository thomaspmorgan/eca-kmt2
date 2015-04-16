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
    }
}