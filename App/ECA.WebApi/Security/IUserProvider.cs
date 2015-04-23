using CAM.Business.Service;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace ECA.WebApi.Security
{
    /// <summary>
    /// An IUserProvider is an object that can manage the three different cases of users in the ECA system i.e.
    /// the user from Azure, the ECA Business User, and the CAM user.  The provider has a method to retrieve the
    /// current user as an IWebApiUser, using this instance of an IWebApiUser the system can determine other
    /// facets of that user.
    /// </summary>
    [ContractClass(typeof(IUserProviderContract))]
    public interface IUserProvider : IDisposable
    {
        /// <summary>
        /// Returns the current system user.
        /// </summary>
        /// <returns>The current user.</returns>
        IWebApiUser GetCurrentUser();

        /// <summary>
        /// Returns an ECA Business user from the given IWebApiUser.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>The business user.</returns>
        ECA.Business.Service.User GetBusinessUser(IWebApiUser user);

        /// <summary>
        /// Returns the permissions of the given user.
        /// </summary>
        /// <param name="user">The user to retrieve permissions for.</param>
        /// <returns>The permissions.</returns>
        Task<IEnumerable<IPermission>> GetPermissionsAsync(IWebApiUser user);

        /// <summary>
        /// Returns the permissions of the given user.
        /// </summary>
        /// <param name="user">The user to retrieve permissions for.</param>
        /// <returns>The permissions.</returns>
        IEnumerable<IPermission> GetPermissions(IWebApiUser user);

        /// <summary>
        /// Returns the principal id i.e. the CAM id of the user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>The CAM principal id.</returns>
        Task<int> GetPrincipalIdAsync(IWebApiUser user);

        /// <summary>
        /// Returns the principal id i.e. the CAM id of the user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>The CAM principal id.</returns>
        int GetPrincipalId(IWebApiUser user);

        /// <summary>
        /// Returns true if the user is a valid user of the system.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>True, if the user is a valid user of the system, other wise false.</returns>
        bool IsUserValid(IWebApiUser user);

        /// <summary>
        /// Returns true if the user is a valid user of the system.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>True, if the user is a valid user of the system, other wise false.</returns>
        Task<bool> IsUserValidAsync(IWebApiUser user);

        /// <summary>
        /// Removes all data about the CAM user from the provider.
        /// </summary>
        /// <param name="user">The user to clear CAM data for.</param>
        void Clear(IWebApiUser user);

        /// <summary>
        /// Removes all data about the CAM user from the provider.
        /// </summary>
        /// <param name="userId">The user by id to clear CAM data for.</param>
        void Clear(Guid userId);

        /// <summary>
        /// Allows a user to impersonate another user by permissions.
        /// </summary>
        /// <param name="impersonator">The user who will be impersonating another.</param>
        /// <param name="idOfUserToImpersonate">The id of user to impersonate.</param>
        void Impersonate(IWebApiUser impersonator, Guid idOfUserToImpersonate);

        /// <summary>
        /// Allows a user to impersonate another user by permissions.
        /// </summary>
        /// <param name="impersonator">The user who will be impersonating another.</param>
        /// <param name="idOfUserToImpersonate">The id of user to impersonate.</param>
        Task ImpersonateAsync(IWebApiUser impersonator, Guid idOfUserToImpersonate);
    }

    /// <summary>
    /// 
    /// </summary>
    [ContractClassFor(typeof(IUserProvider))]
    public abstract class IUserProviderContract : IUserProvider
    {

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IWebApiUser GetCurrentUser()
        {
            Contract.Ensures(Contract.Result<IWebApiUser>() != null, "The returned user must nobe null.");
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Business.Service.User GetBusinessUser(IWebApiUser user)
        {
            Contract.Requires(user != null, "The user must not be null.");
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<IEnumerable<IPermission>> GetPermissionsAsync(IWebApiUser user)
        {
            Contract.Requires(user != null, "The user must not be null.");
            return Task.FromResult<IEnumerable<IPermission>>(new List<IPermission>());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public IEnumerable<IPermission> GetPermissions(IWebApiUser user)
        {
            Contract.Requires(user != null, "The user must not be null.");
            return new List<IPermission>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<int> GetPrincipalIdAsync(IWebApiUser user)
        {
            Contract.Requires(user != null, "The user must not be null.");
            return Task.FromResult<int>(0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int GetPrincipalId(IWebApiUser user)
        {
            Contract.Requires(user != null, "The user must not be null.");
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool IsUserValid(IWebApiUser user)
        {
            Contract.Requires(user != null, "The user must not be null.");
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<bool> IsUserValidAsync(IWebApiUser user)
        {
            Contract.Requires(user != null, "The user must not be null.");
            return Task.FromResult<bool>(false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        public void Clear(IWebApiUser user)
        {
            Contract.Requires(user != null, "The user must not be null.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        public void Clear(Guid userId)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="impersonator"></param>
        /// <param name="idOfUserToImpersonate"></param>
        public void Impersonate(IWebApiUser impersonator, Guid idOfUserToImpersonate)
        {
            Contract.Requires(impersonator != null, "The impersonator must not be null.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="impersonator"></param>
        /// <param name="idOfUserToImpersonate"></param>
        /// <returns></returns>
        public Task ImpersonateAsync(IWebApiUser impersonator, Guid idOfUserToImpersonate)
        {
            Contract.Requires(impersonator != null, "The impersonator must not be null.");
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            
        }
    }
}
