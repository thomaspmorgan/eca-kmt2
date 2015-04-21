using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Security
{
    public enum AuthorizationResult
    {
        Allowed,
        Denied,
        ResourceDoesNotExist,
        InvalidCamUser
    }
}