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
    public class BearerTokenUserProvider : IUserProvider
    {

        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private IUserCacheService cacheService;
        private IPermissionService permissionService;
        private IUserService userService;

        /// <summary>
        /// Creates a new BearerTokenUserProvider instance with the given logger instance.
        /// </summary>
        /// <param name="cacheService">The user cache service.</param>
        /// <param name="permissionService">The permission store.</param>
        /// <param name="userService">The user service.</param>
        public BearerTokenUserProvider(
            IUserService userService,
            IUserCacheService cacheService,
            IPermissionService permissionService)
        {
            Contract.Requires(cacheService != null, "The cache service must not be null.");
            Contract.Requires(permissionService != null, "The permissionStore must not be null.");
            Contract.Requires(userService != null, "The user service must not be null.");
            this.cacheService = cacheService;
            this.permissionService = permissionService;
            this.userService = userService;
        }

        /// <summary>
        /// Returns the current user or an anonymous user if now user is currently set.
        /// </summary>
        /// <returns>The current user, or an anonymous user.</returns>
        public IWebApiUser GetCurrentUser()
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

        /// <summary>
        /// Returns true if the user is valid according to CAM.
        /// </summary>
        /// <param name="user">The user to check.</param>
        /// <returns>True, if the user is valid according to cam.</returns>
        public bool IsUserValid(IWebApiUser user)
        {
            var cache = GetUserCache(user);            
            var valid = cache.IsValidCamUser;
            logger.Info("User {0} is valid in CAM:  {1}", user, valid);
            return valid;
        }

        /// <summary>
        /// Returns true if the user is valid according to CAM.
        /// </summary>
        /// <param name="user">The user to check.</param>
        /// <returns>True, if the user is valid according to cam.</returns>
        public async Task<bool> IsUserValidAsync(IWebApiUser user)
        {
            var cache = await GetUserCacheAsync(user);
            var valid = cache.IsValidCamUser;
            logger.Info("User {0} is valid in CAM:  {1}", user, valid);
            return valid;
        }

        /// <summary>
        /// Returns a business user instances from the given IWebApiUser.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>The business user instance.</returns>
        public Business.Service.User GetBusinessUser(IWebApiUser user)
        {
            var userCache = GetUserCache(user);
            return new Business.Service.User(userCache.CamPrincipalId);
        }

        /// <summary>
        /// Returns a business user instances from the given IWebApiUser.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>The business user instance.</returns>
        public async Task<Business.Service.User> GetBusinessUserAsync(IWebApiUser user)
        {
            var userCache = await GetUserCacheAsync(user);
            return new Business.Service.User(userCache.CamPrincipalId);
        }

        /// <summary>
        /// Returns the permissions of the given user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>The permissions.</returns>
        public async Task<IEnumerable<IPermission>> GetPermissionsAsync(IWebApiUser user)
        {
            return (await GetUserCacheAsync(user)).Permissions;
        }

        /// <summary>
        /// Returns the permissions of the given user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>The permissions.</returns>
        public IEnumerable<IPermission> GetPermissions(IWebApiUser user)
        {
            return GetUserCache(user).Permissions;
        }

        /// <summary>
        /// Returns the CAM principal Id of the user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>The principal id of the user.</returns>
        public async Task<int> GetPrincipalIdAsync(IWebApiUser user)
        {
            var principalId = (await GetUserCacheAsync(user)).CamPrincipalId;
            logger.Info("User [{0}] has cam principal id:  {1}", user, principalId);
            return principalId;
        }

        /// <summary>
        /// Returns the CAM principal Id of the user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>The principal id of the user.</returns>
        public int GetPrincipalId(IWebApiUser user)
        {
            var principalId = GetUserCache(user).CamPrincipalId;
            logger.Info("User [{0}] has cam principal id:  {1}", user, principalId);
            return principalId;
        }

        /// <summary>
        /// Returns the user cache of the given user.  If the user does not exist in CAM it is added.  If
        /// the user is invalid according to cam no permissions are cached.
        /// </summary>
        /// <param name="user">The user to return cache for.</param>
        /// <returns>The user's cache.</returns>
        public async Task<UserCache> GetUserCacheAsync(IWebApiUser user)
        {
            Contract.Requires(user != null, "The user must not be null.");
            var isUserCached = cacheService.IsUserCached(user);
            logger.Info("User [{0}] is cached:  {1}.", user, isUserCached);
            if (!isUserCached)
            {
                logger.Info("Caching user [{0}] information.", user);
                var camUser = await userService.GetUserByIdAsync(user.Id);
                var isValidUser = false;
                IEnumerable<IPermission> permissions = new List<IPermission>();
                if (camUser != null)
                {
                    isValidUser = await userService.IsUserValidAsync(user.Id);                    
                    if (isValidUser)
                    {
                        permissions = await GetUserPermissionsAsync(camUser.PrincipalId);
                    }
                }
                else
                {
                    camUser = new User();
                }
                cacheService.Add(new UserCache(user, camUser, isValidUser, permissions));                
            }
            return cacheService.GetUserCache(user);
        }

        /// <summary>
        /// Returns the user cache of the given user.  If the user does not exist in CAM it is added.  If
        /// the user is invalid according to cam no permissions are cached.
        /// </summary>
        /// <param name="user">The user to return cache for.</param>
        /// <returns>The user's cache.</returns>
        public UserCache GetUserCache(IWebApiUser user)
        {
            Contract.Requires(user != null, "The user must not be null.");
            var isUserCached = cacheService.IsUserCached(user);
            logger.Info("User [{0}] is cached:  {1}.", user, isUserCached);
            if (!isUserCached)
            {
                logger.Info("Caching user [{0}] information.", user);
                var camUser = userService.GetUserById(user.Id);
                var isValidUser = false;
                IEnumerable<IPermission> permissions = new List<IPermission>();
                if (camUser != null)
                {
                    isValidUser = userService.IsUserValid(user.Id);
                    if (isValidUser)
                    {
                        permissions = GetUserPermissions(camUser.PrincipalId);
                    }
                }
                else
                {
                    camUser = new User();
                }
                cacheService.Add(new UserCache(user, camUser, isValidUser, permissions)); 
            }
            return cacheService.GetUserCache(user);
        }

        private async Task<IEnumerable<IPermission>> GetUserPermissionsAsync(int principalId)
        {
            var permissions = await this.permissionService.GetAllowedPermissionsByPrincipalIdAsync(principalId);
            logger.Trace("Retrieved user with principal id [{0}] permissions.", principalId);
            return permissions;
        }

        private IEnumerable<IPermission> GetUserPermissions(int principalId)
        {
            var permissions = this.permissionService.GetAllowedPermissionsByPrincipalId(principalId);
            logger.Trace("Retrieved user with principal id [{0}] permissions.", principalId);
            return permissions;
        }

        /// <summary>
        /// Removes the given user's cache.
        /// </summary>
        /// <param name="user">the user to clear.</param>
        public void Clear(IWebApiUser user)
        {
            Clear(user.Id);
        }

        /// <summary>
        /// Removes the user with the given id cache.
        /// </summary>
        /// <param name="userId">The id of the user to clear cache.</param>
        public void Clear(Guid userId)
        {
            if (this.cacheService.IsUserCached(userId))
            {
                this.cacheService.Remove(userId);
                logger.Info("Removed user with id [{0}] from cache.", userId);
            }
        }

        /// <summary>
        /// Allows a user to impersonate another user by permissions.
        /// </summary>
        /// <param name="impersonator">The user who will be impersonating another.</param>
        /// <param name="idOfUserToImpersonate">The id of user to impersonate.</param>
        public void Impersonate(IWebApiUser impersonator, Guid idOfUserToImpersonate)
        {
            logger.Info("User [{0}] is going to impersonate user with id [{1}].", impersonator, idOfUserToImpersonate);
            BeforeImpersonateUser(impersonator.Id);
            var impersonatedUser = new ImpersonatedUser(impersonatorUserId: impersonator.Id, impersonatedUserId: idOfUserToImpersonate, impersonatorUserName: impersonator.GetUsername());
            bool isImpersonatedUserValid = userService.IsUserValid(idOfUserToImpersonate);
            bool isImpersonatorValid = userService.IsUserValid(impersonator.Id);
            var impersonatedCamUser = userService.GetUserById(idOfUserToImpersonate);
            var impersonatorCamUser = userService.GetUserById(impersonator.Id);
            var impersonatedUserPermissions = GetPermissions(impersonatedUser);
            CacheImpersonatedUser(impersonator, impersonatorCamUser, isImpersonatedUserValid, impersonatedUserPermissions.ToList());
            logger.Info("User [{0}] is now impersonating user with id [{1}].", impersonator, idOfUserToImpersonate);
        }

        /// <summary>
        /// Allows a user to impersonate another user by permissions.
        /// </summary>
        /// <param name="impersonator">The user who will be impersonating another.</param>
        /// <param name="idOfUserToImpersonate">The id of user to impersonate.</param>
        public async Task ImpersonateAsync(IWebApiUser impersonator, Guid idOfUserToImpersonate)
        {
            logger.Info("User [{0}] is going to impersonate user with id [{1}].", impersonator, idOfUserToImpersonate);
            BeforeImpersonateUser(impersonator.Id);
            var impersonatedUser = new ImpersonatedUser(impersonatorUserId: impersonator.Id, impersonatedUserId: idOfUserToImpersonate, impersonatorUserName: impersonator.GetUsername());
            bool isImpersonatedUserValid = await userService.IsUserValidAsync(idOfUserToImpersonate);
            bool isImpersonatorValid = await userService.IsUserValidAsync(impersonator.Id);
            var impersonatedCamUser = await userService.GetUserByIdAsync(idOfUserToImpersonate);
            var impersonatorCamUser = await userService.GetUserByIdAsync(impersonator.Id);
            var impersonatedUserPermissions = await GetPermissionsAsync(impersonatedUser);
            CacheImpersonatedUser(impersonator, impersonatorCamUser, isImpersonatedUserValid, impersonatedUserPermissions.ToList());
            logger.Info("User [{0}] is now impersonating user with id [{1}].", impersonator, idOfUserToImpersonate);
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
                if (this.userService != null && this.userService is IDisposable)
                {
                    ((IDisposable)this.userService).Dispose();
                    this.userService = null;
                }
                if (this.permissionService != null && this.permissionService is IDisposable)
                {
                    ((IDisposable)this.permissionService).Dispose();
                    this.permissionService = null;
                }
                if (this.cacheService != null && this.cacheService is IDisposable)
                {
                    ((IDisposable)this.cacheService).Dispose();
                    this.cacheService = null;
                }
            }
        }

        #endregion
    }
}