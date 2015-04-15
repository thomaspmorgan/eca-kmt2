using CAM.Business.Service;
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

        ECA.Business.Service.User GetBusinessUser(IWebApiUser user);

        Task<IEnumerable<IPermission>> GetPermissionsAsync(IWebApiUser user);

        IEnumerable<IPermission> GetPermissions(IWebApiUser user);

        Task<int> GetPrincipalIdAsync(IWebApiUser user);

        int GetPrincipalId(IWebApiUser user);

        bool IsUserValid(IWebApiUser user);

        Task<bool> IsUserValidAsync(IWebApiUser user);

        void Clear(IWebApiUser user);

        void Clear(Guid userId);
    }
}
