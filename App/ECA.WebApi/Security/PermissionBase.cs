using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;

namespace ECA.WebApi.Security
{
    /// <summary>
    /// A PermissionBase is a class for maintaining the resource type and permission name required for accessing a resource.  Implement this
    /// class to provide a way of retrieving the resource id.
    /// </summary>
    [ContractClass(typeof(PermissionBaseContract))]
    public abstract class PermissionBase
    {
        /// <summary>
        /// The character that splits multiple permissions in a string.
        /// </summary>
        public static readonly string[] PERMISSIONS_DELIMITER = new string[] { "," };

        /// <summary>
        /// The character that splits the permission name and the resource type.
        /// </summary>
        public static readonly string[] RESOURCE_PERMISSION_NAME_DELIMITER = new string[] { ":" };

        /// <summary>
        /// The character that splits the model type and model variable name.
        /// </summary>
        public static readonly string[] MODEL_AND_MODEL_NAME_DELIMITER = new string[] { "#" };

        /// <summary>
        /// The regular expression to check a single action permission in a string i.e. PermissionName:ResourceType(argumentName)
        /// </summary>
        public static readonly Regex PERMISSION_REGEX = new Regex(@"[A-Za-z]+:{1}[A-Za-z]+[(]{1}[A-Za-z0-9#.]+[)]{1}");

        /// <summary>
        /// Gets or sets the type of the resource.
        /// </summary>
        public string ResourceType { get; set; }

        /// <summary>
        /// Gets or sets the permission name.
        /// </summary>
        public string PermissionName { get; set; }

        /// <summary>
        /// Returns the resource id given the action arguments from the controller's action.
        /// </summary>
        /// <param name="actionArguments">The action arguments.</param>
        /// <returns>The resource id.</returns>
        public abstract int GetResourceId(IDictionary<string, object> actionArguments);
    }

    /// <summary>
    /// 
    /// </summary>
    [ContractClassFor(typeof(PermissionBase))]
    public abstract class PermissionBaseContract : PermissionBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionArguments"></param>
        /// <returns></returns>
        public override int GetResourceId(IDictionary<string, object> actionArguments)
        {
            Contract.Requires(actionArguments != null, "The action arguments must not be null.");
            return -1;
        }
    }
}