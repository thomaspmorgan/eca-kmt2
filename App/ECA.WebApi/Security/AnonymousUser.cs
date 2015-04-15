using CAM.Business.Service;
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
            this.Id = Guid.Empty;
        }

        public override bool HasPermission(IPermission requestedPermission, IEnumerable<IPermission> allUserPermissions)
        {
            return false;
        }

        public override string GetUsername()
        {
            return ANONYMOUS_USER_NAME;
        }
    }
}