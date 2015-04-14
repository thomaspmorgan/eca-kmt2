using CAM.Business.Service;
using CAM.Data;
using ECA.Core.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;

namespace ECA.WebApi.Security
{
    /// <summary>
    /// The BearerTokenUserProvider is used to retrieve bearer token objects issued by the Microsoft Azure service.
    /// </summary>
    public class BearerTokenUserProvider : IUserProvider
    {
        private readonly ILogger logger;
        private readonly IUserCacheService cacheService;
        private readonly IPermissionStore<IPermission> permissionStore;

        /// <summary>
        /// Creates a new BearerTokenUserProvider instance with the given logger instance.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        public BearerTokenUserProvider(
            ILogger logger,
            IUserCacheService cacheService,
            IPermissionStore<IPermission> permissionStore)
        {
            Contract.Requires(logger != null, "The logger must not be null.");
            Contract.Requires(cacheService != null, "The cache service must not be null.");
            Contract.Requires(permissionStore != null, "The permissionStore must not be null.");
            this.logger = logger;
            this.cacheService = cacheService;
            this.permissionStore = permissionStore;
        }

        //private Func<Guid, User> getUserDelegate;
        //public Func<Guid, User> GetUserDelegate
        //{
        //    get
        //    {
        //        if (this.getUserDelegate == null)
        //        {
        //            this.getUserDelegate = (id) => {
        //                var user = new User();
        //            }
        //        }
        //    }
        //    set
        //    {
        //        this.getUserDelegate = value;
        //    }
        //}

        /// <summary>
        /// Returns the current user or an anonymous user if now user is currently set.
        /// </summary>
        /// <returns>The current user, or an anonymous user.</returns>
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


        public Business.Service.User GetBusinessUser(IWebApiUser user)
        {
            var userCache = GetUserCache(user);
            return new Business.Service.User(userCache.CamPrincipalId);
        }

        public async Task<Business.Service.User> GetBusinessUserAsync(IWebApiUser user)
        {
            var userCache = await GetUserCacheAsync(user);
            return new Business.Service.User(userCache.CamPrincipalId);
        }

        public async Task<IEnumerable<IPermission>> GetPermissionsAsync(IWebApiUser user)
        {
            return (await GetUserCacheAsync(user)).Permissions;
        }

        public IEnumerable<IPermission> GetPermissions(IWebApiUser user)
        {
            return GetUserCache(user).Permissions;
        }

        public async Task<int> GetPrincipalIdAsync(IWebApiUser user)
        {
            return (await GetUserCacheAsync(user)).CamPrincipalId;
        }

        public int GetPrincipalId(IWebApiUser user)
        {
            return GetUserCache(user).CamPrincipalId;
        }

        public async Task<UserCache> GetUserCacheAsync(IWebApiUser user)
        {
            var isUserCached = cacheService.IsUserCached(user);
            if (!isUserCached)
            {
                var camUser = await GetCamUserAsync(user);
                if (camUser == null)
                {
                    //camUser = await this.userService.CreateUserAsync(user);
                    throw new NotSupportedException("Currently not implmented is a way to add users who are authorized through azure.");
                }
                var permissions = GetUserPermissions(camUser.PrincipalId);
                cacheService.Add(new UserCache(user, camUser.PrincipalId, permissions));
            }
            return cacheService.GetUserCache(user);
        }

        public UserCache GetUserCache(IWebApiUser user)
        {
            var isUserCached = cacheService.IsUserCached(user);
            if (!isUserCached)
            {
                var camUser = GetCamUser(user);
                if (camUser == null)
                {
                    //camUser = this.userService.CreateUser(user);
                    throw new NotSupportedException("Currently not implmented is a way to add users who are authorized through azure.");
                }
                var permissions = GetUserPermissions(camUser.PrincipalId);
                cacheService.Add(new UserCache(user, camUser.PrincipalId, permissions));
            }
            return cacheService.GetUserCache(user);
        }

        private IEnumerable<IPermission> GetUserPermissions(int principalId)
        {
            this.permissionStore.LoadUserPermissions(principalId);
            return this.permissionStore.Permissions;
        }

        private User GetCamUser(IWebApiUser user)
        {
            //return this.userService.GetUserById(user.Id);
            //return new UserAccount
            //{
            //    AdGuid = user.Id
            //};
            var camUser = new User();
            //camUser.AuthenticateUserWithGuid(user.Id);
            return camUser;
        }

        private Task<User> GetCamUserAsync(IWebApiUser user)
        {
            //return this.userService.GetUserByIdAsync(user.Id);
            return Task.FromResult<User>(GetCamUser(user));
        }
    }
}