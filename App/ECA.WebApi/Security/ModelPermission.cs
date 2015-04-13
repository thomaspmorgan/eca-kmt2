using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Web;

namespace ECA.WebApi.Security
{
    public class ModelPermission : PermissionBase
    {
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

        public string Property { get; private set; }

        public Type ModelType { get; private set; }

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
                    throw new NotSupportedException("There more than one action arguments for the web api.  You must specify the name of the variable in the property path.");
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

        public int GetResourceId(object model, string propertyName)
        {
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
                var propertyInfo = model.GetType().GetProperty(propertyName);
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

        public override string ToString()
        {
            return String.Format("{0}:{1}({2}#{3})", PermissionName, ResourceType, this.ModelType.Name, this.Property);
        }
    }
}