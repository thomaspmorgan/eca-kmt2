using CAM.Data;
using ECA.Core.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.Business.Service
{
    /// <summary>
    /// Provides extension methods to PermissableType enum.
    /// </summary>
    public static class PermissableTypeExtensions
    {
        /// <summary>
        /// Returns the CAM resource type id that maps to the permissable type.
        /// </summary>
        /// <param name="source">The permissable type.</param>
        /// <returns>The resource type id in CAM.</returns>
        public static int GetResourceTypeId(this PermissableType source)
        {
            var map = new Dictionary<PermissableType, int>();
            map.Add(PermissableType.Application, ResourceType.Application.Id);
            map.Add(PermissableType.Office, ResourceType.Office.Id);
            map.Add(PermissableType.Program, ResourceType.Program.Id);
            map.Add(PermissableType.Project, ResourceType.Project.Id);
            Contract.Assert(map.ContainsKey(source), "The permissable type does not have a matching resource type.");
            return map[source];
        }
    }
}
