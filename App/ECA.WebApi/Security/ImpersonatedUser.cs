using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Security
{
    public class ImpersonatedUser : IWebApiUser
    {
        private string impersonatorUsername;

        public ImpersonatedUser(Guid impersonatorUserId, Guid impersonatedUserId, string impersonatorUserName)
        {
            this.ImpersonatedUserId = impersonatedUserId;
            this.ImpersonatorUserId = impersonatorUserId;
            this.impersonatorUsername = impersonatorUserName;
        }

        public Guid Id
        {
            get
            {
                return ImpersonatedUserId;
            }
        }

        public Guid ImpersonatedUserId
        {
            get; private set;
        }

        public Guid ImpersonatorUserId
        {
            get;
            private set;
        }

        public string GetUsername()
        {
            return impersonatorUsername;
        }
    }
}