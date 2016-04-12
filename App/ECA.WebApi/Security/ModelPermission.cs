using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace ECA.WebApi.Security
{
    /// <summary>
    /// A ModelPermission is used when a WebApi action's method parameters is expecting a class i.e. BindingModel
    /// and resource id's are properties of the BindingModel.  For example, the DraftProgramBindingModel has
    /// an OwnerOrganizationId property.  The value of OwnerOrganizationId represents the office, and therefore
    /// creation of the Program may depend on the user's access to that office.  Using a ModelPermission
    /// can restrict the creation of that program to a certain office, by setting the property to OwnerOrganizationId,
    /// the type to DraftProgramBindingModel, and the correct permission name and resource type.
    /// 
    /// When defining the property, the property may either be a direct property on the model itself or a nested property
    /// of the model.  To define a nested property seperate the names of the property by a period.  For example, MyModel is a class,
    /// and MyModel has property named Program, which has a Property named ProgramId.  Therefore, the property path would be defined as:
    /// 
    /// Program.ProgramId.
    /// 
    /// If the api action has more than one method argument defined it is necessary to specify the name of the method variable in the property path.
    /// For example, the web api may have a definition 
    /// 
    /// public void GetById(MyModel model, int id)
    /// 
    /// therefore; the property path for the ProgramId would be
    /// model.Program.ProgramId
    /// </summary>
    public class ModelPermission : PermissionBase
    {
        /// <summary>
        /// Creates a new ModelPermission with the name of the property and the model type to find the property in.
        /// </summary>
        /// <param name="property">The name of the property to find the foreign resource id.</param>
        /// <param name="modelType">The model type that contains the foreign resource id.</param>
        /// <param name="permissionName">The permission name.</param>
        /// <param name="resourceType">The resource type.</param>
        public ModelPermission(string property, Type modelType, string permissionName, string resourceType)
        {
            Contract.Requires(property != null, "The property of the model must not be null.");
            Contract.Requires(modelType != null, "The model type must not be null.");
            Contract.Requires(permissionName != null, "The permission name must not be null.");
            Contract.Requires(resourceType != null, "The resource type must not be null.");
            this.Property = property;
            this.ModelType = modelType;
            this.PermissionName = permissionName;
            this.ResourceType = resourceType;
        }

        /// <summary>
        /// Gets the property.
        /// </summary>
        public string Property { get; private set; }

        /// <summary>
        /// Gets the model type.
        /// </summary>
        public Type ModelType { get; private set; }

        /// <summary>
        /// Recursively scans the action arguments for the property defined in the instance's property path for
        /// the resource id.  The property maybe at the root of the model or a child property of another property.
        /// </summary>
        /// <param name="actionArguments">The web api action arguments.</param>
        /// <returns>The resource id.</returns>
        public override int GetResourceId(IDictionary<string, object> actionArguments)
        {
            var prop = this.Property;
            //if there are more than one keys passed in this dictionary we can't assume the property
            //starts with a certain model; therefore, the Property must contain the key in the action arguments
            if (actionArguments.Keys.Count > 1)
            {
                var propertyContainsDictionaryKey = false;
                var allKeys = actionArguments.Keys.ToList();
                foreach (var key in allKeys)
                {
                    if (this.Property.Contains(key))
                    {
                        propertyContainsDictionaryKey = true;
                        break;
                    }
                }
                if (!propertyContainsDictionaryKey)
                {
                    throw new NotSupportedException("There are more than one action arguments for the web api.  You must specify the name of the variable in the property path.");
                }
            }
            //otherwise lets append the key to the property if the developer didn't specify it.
            else if (actionArguments.Keys.Count == 1 && !this.Property.Contains(actionArguments.Keys.First()))
            {
                prop = actionArguments.Keys.First() + "." + prop;
            }
            else if(actionArguments.Keys.Count == 0)
            {
                throw new NotSupportedException("There are no action arguments to check permissions with.  There must be at least one action argument.");
            }
            var argumentKey = prop;
            var propertyKey = prop;
            var index = prop.IndexOf('.');
            if (index >= 0)
            {
                argumentKey = prop.Substring(0, index);
                propertyKey = prop.Substring(index + 1);
            }
            return GetResourceId(actionArguments[argumentKey], propertyKey);
        }

        private int GetResourceId(object model, string propertyName)
        {
            Contract.Requires(model != null, "The model must not be null.");
            Contract.Requires(propertyName != null, "The property name must not be null.");
            var index = propertyName.IndexOf('.');
            if (index >= 0)
            {
                var key = propertyName.Substring(0, index);
                var remainingProperties = propertyName.Substring(index + 1);
                var property = model.GetType().GetProperty(key);
                var propertyValue = property.GetValue(model);
                return GetResourceId(propertyValue, remainingProperties);
            }
            else
            {
                var propertyInfo = model.GetType().GetProperties().Where(x => String.Equals(x.Name, propertyName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                if (propertyInfo == null)
                {
                    throw new NotSupportedException(String.Format("The property [{0}] does not exist in the model of type [{1}].", propertyName, model.GetType().Name));
                }
                if (propertyInfo.PropertyType != typeof(int))
                {
                    throw new NotSupportedException(String.Format("The nested property [{0}] in the model's property hierarchy [{1}] must be an integer.", propertyName, this.Property));
                }
                return (int)propertyInfo.GetValue(model);
            }
        }

        /// <summary>
        /// Returns a formatted string of this permission.  This format can also be parsed back to a model permission.
        /// </summary>
        /// <returns>A formatted string of this permission.</returns>
        public override string ToString()
        {
            return String.Format("Permission Name:  [{0}], Resource Type:  [{1}], Model Type:  [{2}], Property:  [{3}]", PermissionName, ResourceType, this.ModelType.Name, this.Property);
        }
    }
}