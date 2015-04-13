using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace ECA.WebApi.Security
{
    /// <summary>
    /// An ActionPermission is a simple permission that retrieves a resource by an action argument directly.  This
    /// type of permission is best suited for Get methods that lookup an entity by Id for example.
    /// </summary>
    public class ActionPermission : PermissionBase
    {
        /// <summary>
        /// Gets or sets the name of the argument.
        /// </summary>
        public string ArgumentName { get; set; }

        /// <summary>
        /// Returns the formatted string of this permission.  This string format is the same format the Parse method expects.
        /// </summary>
        /// <returns>The formatted string.</returns>
        public override string ToString()
        {
            return String.Format("{0}:{1}({2})", PermissionName, ResourceType, ArgumentName);
        }

        /// <summary>
        /// Returns the resource id from the given action arguments by using the argument name and casting to 
        /// an integer.
        /// </summary>
        /// <param name="actionArguments">The web api action arguments.</param>
        /// <returns>The resource id.</returns>
        public override int GetResourceId(IDictionary<string, object> actionArguments)
        {
            Contract.Assert(actionArguments.ContainsKey(this.ArgumentName),
                String.Format("The argument named [{0}] does not exist in the action arguments.", this.ArgumentName));
            var actionArgumentValue = actionArguments[this.ArgumentName];
            if (actionArgumentValue.GetType() != typeof(int))
            {
                throw new NotSupportedException(String.Format("The action argument must be an integer."));
            }
            return (int)actionArgumentValue;
        }
    }

}