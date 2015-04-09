using ECA.Core.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace ECA.WebApi.Security
{
    public class BearerTokenUserProvider : IUserProvider
    {
        private readonly ILogger logger;

        public BearerTokenUserProvider(ILogger logger)
        {
            Contract.Requires(logger != null, "The logger must not be null.");
            this.logger = logger;
            
        }
        public WebApiUserBase GetCurrentUser()
        {
            var currentUser = HttpContext.Current.User;
            if (currentUser != null)
            {
                Contract.Assert(ClaimsPrincipal.Current != null, "The claims principal must not be null.");
                return new WebApiUser(logger, ClaimsPrincipal.Current);
            }
            else
            {
                return new AnonymousUser();
            }
            
        }
    }
}