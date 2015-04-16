using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Security
{
    public class ImpersonatedUser : IWebApiUser
    {
        public Guid Id
        {
            get { throw new NotImplementedException(); }
        }

        public string GetUsername()
        {
            throw new NotImplementedException();
        }
    }
}