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
    public class BearerTokenUserProvider : IUserProvider, IDisposable
    {
        private static readonly string COMPONENT_NAME = typeof(BearerTokenUserProvider).FullName;

        private readonly ILogger logger;
        private readonly IUserCacheService cacheService;
        private readonly IPermissionStore<IPermission> permissionStore;
        private CamModel camContext;

        /// <summary>
        /// Creates a new BearerTokenUserProvider instance with the given logger instance.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        public BearerTokenUserProvider(
            ILogger logger,
            CamModel camContext,
            IUserCacheService cacheService,
            IPermissionStore<IPermission> permissionStore)
        {
            Contract.Requires(logger != null, "The logger must not be null.");
            Contract.Requires(cacheService != null, "The cache service must not be null.");
            Contract.Requires(permissionStore != null, "The permissionStore must not be null.");
            Contract.Requires(camContext != null, "The camContext must not be null.");
            this.logger = logger;
            this.cacheService = cacheService;
            this.permissionStore = permissionStore;
            this.camContext = camContext;
        }

        /// <summary>
        /// Returns the current user or an anonymous user if now user is currently set.
        /// </summary>
        /// <returns>The current user, or an anonymous user.</returns>
        public WebApiUserBase GetCurrentUser()
        {
            var currentUser = HttpContext.Current.User;
            if (currentUser != null)
            {
                //Contract.Assert(ClaimsPrincipal.Current != null, "The claims principal must not be null.");
                return new WebApiUser(logger, ClaimsPrincipal.Current);
            }
            else
            {
                return new AnonymousUser();
            }
        }

        public bool IsUserValid(IWebApiUser user)
        {
            var cache = GetUserCache(user);
            return cache.IsValidCamUser;
        }

        public Task<bool> IsUserValidAsync(IWebApiUser user)
        {
            return Task.FromResult<bool>(IsUserValid(user));
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
                var stopWatch = Stopwatch.StartNew();
                logger.Information("Caching user information...");
                bool isValidUser;
                var camUser = await GetCamUserAsync(user, out isValidUser);
                var permissions = await GetUserPermissionsAsync(camUser.PrincipalId);
                cacheService.Add(new UserCache(user, camUser, isValidUser, permissions));
                stopWatch.Stop();
                logger.TraceApi(COMPONENT_NAME, stopWatch.Elapsed);
            }
            return cacheService.GetUserCache(user);
        }

        public UserCache GetUserCache(IWebApiUser user)
        {
            var isUserCached = cacheService.IsUserCached(user);
            if (!isUserCached)
            {
                var stopWatch = Stopwatch.StartNew();
                logger.Information("Caching user information...");
                bool isValidUser;
                var camUser = GetCamUser(user, out isValidUser);
                var permissions = GetUserPermissions(camUser.PrincipalId);
                cacheService.Add(new UserCache(user, camUser, isValidUser, permissions));
                stopWatch.Stop();
                logger.TraceApi(COMPONENT_NAME, stopWatch.Elapsed);
            }
            return cacheService.GetUserCache(user);
        }

        private Task<IEnumerable<IPermission>> GetUserPermissionsAsync(int principalId)
        {
            return Task.FromResult<IEnumerable<IPermission>>(GetUserPermissions(principalId));
        }

        private IEnumerable<IPermission> GetUserPermissions(int principalId)
        {
            this.permissionStore.LoadUserPermissions(principalId);
            return this.permissionStore.Permissions;
        }

        private User GetCamUser(IWebApiUser user, out bool isValid)
        {
            var camUser = new User();
            isValid = camUser.AuthenticateUserWithGuid(user.Id, this.camContext);
            return camUser;
        }

        private Task<User> GetCamUserAsync(IWebApiUser user, out bool isValid)
        {
            return Task.FromResult<User>(GetCamUser(user, out isValid));
        }

        public void Clear(IWebApiUser user)
        {
            if (this.cacheService.IsUserCached(user))
            {
                this.cacheService.Remove(user);
            }
        }

        #region IDispose

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.camContext.Dispose();
                this.camContext = null;
            }
        }

        #endregion


        
    }
}