using CAM.Business.Service;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECA.WebApi.Security
{
    public interface IUserProvider : IDisposable
    {
        IWebApiUser GetCurrentUser();

        ECA.Business.Service.User GetBusinessUser(IWebApiUser user);

        Task<IEnumerable<IPermission>> GetPermissionsAsync(IWebApiUser user);

        IEnumerable<IPermission> GetPermissions(IWebApiUser user);

        Task<int> GetPrincipalIdAsync(IWebApiUser user);

        int GetPrincipalId(IWebApiUser user);

        bool IsUserValid(IWebApiUser user);

        Task<bool> IsUserValidAsync(IWebApiUser user);

        void Clear(IWebApiUser user);

        void Clear(Guid userId);

        void Impersonate(IWebApiUser impersonator, Guid idOfUserToImpersonate);

        Task ImpersonateAsync(IWebApiUser impersonator, Guid idOfUserToImpersonate);
    }
}
