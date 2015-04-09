using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Security
{
    public class AnonymousUser : WebApiUserBase
    {
        public const string ANONYMOUS_USER_NAME = "Anonymous";

        public AnonymousUser()
        {

        }

        public Guid Id
        {
            get { return Guid.Empty; }
        }

        public override Business.Service.User ToBusinessUser()
        {
            throw new NotImplementedException();
        }

        public override bool HasPermission(ResourcePermission requestedPermission, IEnumerable<ResourcePermission> allUserPermissions)
        {
            return false;
        }

        public override string GetUsername()
        {
            return ANONYMOUS_USER_NAME;
        }
    }
}