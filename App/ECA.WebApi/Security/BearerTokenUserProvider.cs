using CAM.Business.Service;
using CAM.Data;
using NLog;
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

        public static Func<Guid, User> UserFactory = (userId) => { return new User(); };

        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IUserCacheService cacheService;
        private readonly IPermissionStore<IPermission> permissionStore;
        private CamModel camContext;

        /// <summary>
        /// Creates a new BearerTokenUserProvider instance with the given logger instance.
        /// </summary>
        public BearerTokenUserProvider(
            CamModel camContext,
            IUserCacheService cacheService,
            IPermissionStore<IPermission> permissionStore)
        {
            Contract.Requires(cacheService != null, "The cache service must not be null.");
            Contract.Requires(permissionStore != null, "The permissionStore must not be null.");
            Contract.Requires(camContext != null, "The camContext must not be null.");
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
                Debug.Assert(ClaimsPrincipal.Current != null, "The claims principal must not be null.");
                return new WebApiUser(ClaimsPrincipal.Current);
            }
            else
            {
                return new AnonymousUser();
            }
        }

        public bool IsUserValid(IWebApiUser user)
        {
            var cache = GetUserCache(user);            
            var valid = cache.IsValidCamUser;
            logger.Info("User {0} is valid in CAM:  {1}", user, valid);
            return valid;
        }

        public Task<bool> IsUserValidAsync(IWebApiUser user)
        {
            var valid = Task.FromResult<bool>(IsUserValid(user));
            logger.Info("User {0} is valid in CAM:  {1}", user, valid);
            return valid;
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
            var principalId = (await GetUserCacheAsync(user)).CamPrincipalId;
            logger.Info("User {0} cam principal id:  {1}", user, principalId);
            return principalId;
        }

        public int GetPrincipalId(IWebApiUser user)
        {
            var principalId = GetUserCache(user).CamPrincipalId;
            logger.Info("User {0} cam principal id:  {1}", user, principalId);
            return principalId;
        }

        public async Task<UserCache> GetUserCacheAsync(IWebApiUser user)
        {
            var isUserCached = cacheService.IsUserCached(user);
            logger.Trace("User {0} is cached:  {1}.", user, isUserCached);
            if (!isUserCached)
            {
                logger.Info("Caching user information...");
                bool isValidUser;
                var camUser = await GetCamUserAsync(user, out isValidUser);
                var permissions = await GetUserPermissionsAsync(camUser.PrincipalId);
                cacheService.Add(new UserCache(user, camUser, isValidUser, permissions));
            }
            return cacheService.GetUserCache(user);
        }

        public UserCache GetUserCache(IWebApiUser user)
        {
            var isUserCached = cacheService.IsUserCached(user);
            logger.Trace("User {0} is cached:  {1}.", user, isUserCached);
            if (!isUserCached)
            {
                logger.Info("Caching user information...");
                bool isValidUser;
                var camUser = GetCamUser(user, out isValidUser);
                var permissions = GetUserPermissions(camUser.PrincipalId);
                cacheService.Add(new UserCache(user, camUser, isValidUser, permissions));
            }
            return cacheService.GetUserCache(user);
        }

        private Task<IEnumerable<IPermission>> GetUserPermissionsAsync(int principalId)
        {
            var permissions = Task.FromResult<IEnumerable<IPermission>>(GetUserPermissions(principalId));
            logger.Trace("Retrieved user with principal id {0} permissions.", principalId);
            return permissions;
        }

        private IEnumerable<IPermission> GetUserPermissions(int principalId)
        {
            this.permissionStore.LoadUserPermissions(principalId);
            var permissions = this.permissionStore.Permissions;
            logger.Trace("Retrieved user with principal id {0} permissions.", principalId);
            return permissions;
        }

        private User GetCamUser(IWebApiUser user, out bool isValid)
        {
            return GetCamUser(user.Id, out isValid);
        }

        private Task<User> GetCamUserAsync(IWebApiUser user, out bool isValid)
        {
            return Task.FromResult<User>(GetCamUser(user.Id, out isValid));
        }

        private User GetCamUser(Guid userId, out bool isValid)
        {
            var camUser = UserFactory(userId);
            isValid = camUser.AuthenticateUserWithGuid(userId, this.camContext);
            return camUser;
        }

        private Task<User> GetCamUserAsync(Guid userId, out bool isValid)
        {
            return Task.FromResult<User>(GetCamUser(userId, out isValid));
        }

        public void Clear(IWebApiUser user)
        {
            Clear(user.Id);
        }

        public void Clear(Guid userId)
        {
            if (this.cacheService.IsUserCached(userId))
            {
                this.cacheService.Remove(userId);
                logger.Trace("Removed user {0} from cache.", userId);
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
                if (this.camContext != null)
                {
                    this.camContext.Dispose();
                    this.camContext = null;
                }
            }
        }

        #endregion


        public void Impersonate(IWebApiUser impersonator, Guid idOfUserToImpersonate)
        {
            logger.Info("User {0} is going to impersonate user with id {1}.", impersonator, idOfUserToImpersonate);
            BeforeImpersonateUser(impersonator.Id);
            var impersonatedUser = new ImpersonatedUser(impersonatorUserId: impersonator.Id, impersonatedUserId: idOfUserToImpersonate, impersonatorUserName: impersonator.GetUsername());
            bool isImpersonatedUserValid;
            bool isImpersonatorValid;
            var impersonatedCamUser = GetCamUser(impersonatedUser, out isImpersonatedUserValid);
            var impersonatorCamUser = GetCamUser(impersonator.Id, out isImpersonatorValid);
            var impersonatedUserPermissions = GetPermissions(impersonatedUser);
            CacheImpersonatedUser(impersonator, impersonatorCamUser, isImpersonatedUserValid, impersonatedUserPermissions.ToList());
            logger.Info("User {0} is no impersonating user with id {1}.", impersonator, idOfUserToImpersonate);
        }

        public async Task ImpersonateAsync(IWebApiUser impersonator, Guid idOfUserToImpersonate)
        {
            logger.Info("User {0} is going to impersonate user with id {1}.", impersonator, idOfUserToImpersonate);
            BeforeImpersonateUser(impersonator.Id);
            var impersonatedUser = new ImpersonatedUser(impersonatorUserId: impersonator.Id, impersonatedUserId: idOfUserToImpersonate, impersonatorUserName: impersonator.GetUsername());
            bool isImpersonatedUserValid;
            bool isImpersonatorValid;
            var impersonatedCamUser = await GetCamUserAsync(impersonatedUser, out isImpersonatedUserValid);
            var impersonatorCamUser = await GetCamUserAsync(impersonator.Id, out isImpersonatorValid);
            var impersonatedUserPermissions = await GetPermissionsAsync(impersonatedUser);
            CacheImpersonatedUser(impersonator, impersonatorCamUser, isImpersonatedUserValid, impersonatedUserPermissions.ToList());
            logger.Info("User {0} is no impersonating user with id {1}.", impersonator, idOfUserToImpersonate);
        }

        private void CacheImpersonatedUser(IWebApiUser impersonator, User impersonatorCamUser, bool isImpersonatedUserValid, IEnumerable<IPermission> impersonatedUserPermissions)
        {
            var modifiedCache = new UserCache(impersonator, impersonatorCamUser, isImpersonatedUserValid, impersonatedUserPermissions.ToList());
            this.cacheService.Add(modifiedCache);

        }

        private void BeforeImpersonateUser(Guid impersonatorId)
        {
            var isUserCached = cacheService.IsUserCached(impersonatorId);
            if (isUserCached)
            {
                this.Clear(impersonatorId);
            }
        }
    }
}