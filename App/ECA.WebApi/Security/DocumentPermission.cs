using ECA.Business.Search;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Security
{
    public class DocumentPermission : PermissionBase
    {


        public string ArgumentName { get; set; }

        public override int GetResourceId(IDictionary<string, object> actionArguments)
        {
            Contract.Assert(this.ArgumentName != null, "The argument name must not be null.");
            if (!actionArguments.ContainsKey(this.ArgumentName))
            {
                var message = "The argument named [{0}] was not found in the given action arguments.  "
                + "If you did not specify an argument name then the default argument name [{1}] is assumed.  Either specify an argument name or refactor the argument name to the default.";
                throw new NotSupportedException(String.Format(message, this.ArgumentName, ResourceAuthorizeAttribute.DEFAULT_ID_ARGUMENT_NAME));
            }
            var actionArgumentValue = actionArguments[this.ArgumentName];
            var actionArgumentValueType = actionArgumentValue.GetType();
            if (actionArgumentValueType != typeof(string))
            {
                throw new NotSupportedException(String.Format("The action argument must be a string (DocumentKey).  It was a [{0}].", actionArgumentValueType));
            }

            Contract.Assert(actionArgumentValueType == typeof(string), "The action argument value type should be a string.");
            DocumentKey documentKey = null;
            var actionArgumentStringValue = (string)actionArgumentValue;
            var isDocumentKey = DocumentKey.TryParse(actionArgumentStringValue, out documentKey);
            if (!isDocumentKey)
            {
                throw new NotSupportedException(String.Format("The action argument string value [{0}] is not a valid document key.", actionArgumentStringValue));
            }
            if (documentKey.KeyType != DocumentKeyType.Int)
            {
                throw new NotSupportedException(String.Format("The document key - key type of [{0}] is not supported.", documentKey.KeyType));
            }
            return (int)documentKey.Value;
        }

        public override string GetResourceType(IDictionary<string, object> actionArguments)
        {
            throw new NotImplementedException();
        }
    }
}