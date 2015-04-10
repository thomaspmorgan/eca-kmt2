using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace ECA.WebApi.Security
{
    /// <summary>
    /// An ActionPermission is a permission designated to an action that relates to a resource.
    /// </summary>
    public class ActionPermission
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
        /// The regular expression to check a single action permission in a string i.e. PermissionName:ResourceType(argumentName)
        /// </summary>
        public static readonly Regex PERMISSION_REGEX = new Regex(@"[A-Za-z]+:{1}[A-Za-z]+[(]{1}[A-Za-z]+[)]{1}");  

        /// <summary>
        /// Gets or sets the name of the argument.
        /// </summary>
        public string ArgumentName { get; set; }

        /// <summary>
        /// Gets or sets the type of the resource.
        /// </summary>
        public string ResourceType { get; set; }

        /// <summary>
        /// Gets or sets the permission name.
        /// </summary>
        public string PermissionName { get; set; }

        /// <summary>
        /// Returns the formatted string of this permission.  This string format is the same format the Parse method expects.
        /// </summary>
        /// <returns>The formatted string.</returns>
        public override string ToString()
        {
            return String.Format("{0}:{1}({2})", PermissionName, ResourceType, ArgumentName);
        }

        /// <summary>
        /// Parses an a comma seperated string of permissions into action permissions.  This is useful when an action requires
        /// multiple permissions.
        /// 
        /// The string must be formatted in the following fashion:
        /// PermissionName:ResourceType(argumentName), PermissionName:ResourceType(argumentName), ...
        /// 
        /// PermissionName is the name of the permission e.g. Read, Delete, ...
        /// ResourceType is the type of resource e.g. Project, Program, ...
        /// argumentName is the name of the action argument that represents the resource e.g. programId.
        /// 
        /// So the following would be a single permission requiring Read Permission on a Program whose Action Argument
        /// is named programId:
        /// 
        /// Read:Program(programId)
        /// 
        /// To require multiple permissions seperate groups by a comma e.g.
        /// Read:Program(programId), Edit:Project(projectId)
        /// 
        /// </summary>
        /// <param name="actionPermissionsString">The string containing action argument permissions.</param>
        /// <returns>The parsed action permissions.</returns>
        public static IEnumerable<ActionPermission> Parse(string actionPermissionsString)
        {
            var actionPermissions = actionPermissionsString.Split(PERMISSIONS_DELIMITER, StringSplitOptions.RemoveEmptyEntries);
            foreach (var actionPermission in actionPermissions)
            {
                var trimmedPermission = actionPermission.Trim();
                trimmedPermission = trimmedPermission.Replace(" ", String.Empty);
                if (!PERMISSION_REGEX.IsMatch(trimmedPermission))
                {
                    throw new NotSupportedException(String.Format("The permission {0} is not valid.  It must be in the format PermissionName:ResourceType(argumentName)", actionPermission));
                }
                //an example permission
                //Read:Program(programId)

                //split on the semicolon
                var resourcePermissionSplits = trimmedPermission.Split(RESOURCE_PERMISSION_NAME_DELIMITER, StringSplitOptions.RemoveEmptyEntries);
                //permission name is first
                var permissionName = resourcePermissionSplits[0];

                //next split on first parenthesis
                var resourceTypeAndArgumentName = resourcePermissionSplits[1].Split(new string[] { "(" }, StringSplitOptions.RemoveEmptyEntries);
                var resourceType = resourceTypeAndArgumentName[0];
                var argumentName = resourceTypeAndArgumentName[1].Replace(")", String.Empty);

                yield return new ActionPermission
                {
                    ArgumentName = argumentName,
                    PermissionName = permissionName,
                    ResourceType = resourceType
                };
            }
        }
    }

}