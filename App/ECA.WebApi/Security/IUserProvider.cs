using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.WebApi.Security
{
    public interface IUserProvider
    {
        WebApiUserBase GetCurrentUser();
    }
}
