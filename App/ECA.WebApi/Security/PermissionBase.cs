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

        /// <summary>
        /// Parses a comma seperated string of permissions into PermissionBase instances.  This is useful when an action requires
        /// multiple permissions.
        /// 
        /// The string must be formatted in the following fashion:
        /// PermissionName:ResourceType(argumentName|int|TypeName#argumentName.PropertyName.PropertyName...), PermissionName:ResourceType(argumentName|int|TypeName#argumentName.PropertyName.PropertyName...), ...
        /// 
        /// PermissionName is the name of the permission e.g. Read, Delete, ...
        /// ResourceType is the type of resource e.g. Project, Program, ...
        /// 
        /// argumentName is the name of the action argument that represents the resource e.g. programId.
        /// or, int is the hard coded resource id
        /// or TypeName#argumentName.PropertyName.PropertyName where TypeName is the class name of the binding model,
        /// argumentName is the name of the web api action variable, and PropertyName is the model property
        /// where either a root property can be read or properties of properties can be read.
        /// 
        /// The following would be a single permission requiring Read Permission on a Program whose Action Argument
        /// is named programId:
        /// 
        /// Read:Program(programId)
        /// 
        /// The following would require Edit Permission on an Application with a resource id 1.
        /// Edit:Application(1)
        /// 
        /// A permission structured in the following manner would require Edit Permission on a Program by ProgramId, where the binding model
        /// has a property named ProgramId and the action parameter is named model:
        /// 
        /// Edit:Program(ProgramBindingModel#model.OwnerOrganizationId)
        /// 
        /// To require multiple permissions seperate groups by a comma e.g.
        /// Read:Program(programId), Edit:Project(projectId)
        /// 
        /// </summary>
        /// <param name="actionPermissionsString">The string containing action argument permissions.</param>
        /// <returns>The parsed action permissions.</returns>
        public static IEnumerable<PermissionBase> Parse(string actionPermissionsString)
        {
            Contract.Requires(actionPermissionsString != null, "The actionPermissionsString must not be null.");
            var actionPermissions = actionPermissionsString.Split(PERMISSIONS_DELIMITER, StringSplitOptions.RemoveEmptyEntries);
            foreach (var actionPermission in actionPermissions)
            {
                var trimmedPermission = actionPermission.Trim();
                trimmedPermission = trimmedPermission.Replace(" ", String.Empty);
                if (!PERMISSION_REGEX.IsMatch(trimmedPermission))
                {
                    throw new NotSupportedException(String.Format("The permission {0} is not valid.  It must be in the format PermissionName:ResourceType(argumentName|int)", actionPermission));
                }
                //an example permission
                //Read:Program(programId)

                //split on the semicolon
                var resourcePermissionSplits = trimmedPermission.Split(RESOURCE_PERMISSION_NAME_DELIMITER, StringSplitOptions.RemoveEmptyEntries);
                //permission name is first
                var permissionName = resourcePermissionSplits[0];

                //next split on first parenthesis
                var resourceTypeAndArgument = resourcePermissionSplits[1].Split(new string[] { "(" }, StringSplitOptions.RemoveEmptyEntries);
                var resourceType = resourceTypeAndArgument[0];

                //the value in the parenthesis should either be an integer or an action argument name.
                var resourceIdOrArgumentName = resourceTypeAndArgument[1].Replace(")", String.Empty);
                int resourceId;
                if (Int32.TryParse(resourceIdOrArgumentName, out resourceId))
                {
                    yield return new StaticPermission
                    {
                        PermissionName = permissionName,
                        ResourceType = resourceType,
                        ResourceId = resourceId,
                    };
                }
                else
                {
                    if (resourceIdOrArgumentName.Contains(MODEL_AND_MODEL_NAME_DELIMITER[0]))
                    {
                        var modelTypeAndVariableName = resourceIdOrArgumentName.Split(MODEL_AND_MODEL_NAME_DELIMITER, StringSplitOptions.RemoveEmptyEntries);
                        var modelTypeAsString = modelTypeAndVariableName[0];
                        var variableName = modelTypeAndVariableName[1];
                        yield return new ModelPermission(variableName, GetTypeByName(modelTypeAsString), permissionName, resourceType);
                    }
                    else
                    {
                        yield return new ActionPermission
                        {
                            ArgumentName = resourceIdOrArgumentName,
                            PermissionName = permissionName,
                            ResourceType = resourceType
                        };
                    }
                }
            }
        }

        /// <summary>
        /// Returns the type given a name of a type.
        /// </summary>
        /// <param name="name">The name of the type.</param>
        /// <returns>The type.</returns>
        public static Type GetTypeByName(string name)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var type = assembly.GetTypes().Where(x => x.Name.Contains(name)).SingleOrDefault();
            if (type == null)
            {
                throw new NotSupportedException("The web api does not have a type that contains the name " + name);
            }
            return type;
        }
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