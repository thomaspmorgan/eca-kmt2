using CAM.Data;
using System.Data.Entity;
using ECA.Core.Service;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.Business.Service
{
    /// <summary>
    /// The UserService uses a CamModel to perform crud and validation operations for users in CAM.
    /// </summary>
    public class UserService : DbContextService<CamModel>, CAM.Business.Service.IUserService
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Creates a new UserService with the CamModel.
        /// </summary>
        /// <param name="context">The context.</param>
        public UserService(CamModel context)
            : base(context)
        {
            Contract.Requires(context != null, "The context must not be null.");
        }

        #region Get

        private void UpdateLastAccessed(Guid id)
        {
            var user = CreateGetUserAccountByIdQuery(id).FirstOrDefault();
            if (user != null)
            {
                DoUpdateLoadAccess(user);
                this.SaveChanges();
            }

        }

        private async Task UpdateLastAccessedAsync(Guid id)
        {
            var user = await CreateGetUserAccountByIdQuery(id).FirstOrDefaultAsync();
            if (user != null)
            {
                DoUpdateLoadAccess(user);
                await this.SaveChangesAsync();
            }
            
        }

        private void DoUpdateLoadAccess(UserAccount userAccount)
        {
            var now = DateTimeOffset.UtcNow;
            userAccount.LastAccessed = now;
        }

        private IQueryable<User> CreateGetUserByIdQuery(Guid id)
        {
            return Context.UserAccounts.Where(x => x.AdGuid == id).
                Select(x => new User
                {
                    AccountStatusId = x.AccountStatusId,
                    AccountStatusText = x.AccountStatus.Status,
                    AdGuid = x.AdGuid,
                    DisplayName = x.DisplayName,                    
                    EmailAddress = x.EmailAddress,
                    ExpiredDate = x.ExpiredDate,
                    FirstName = x.FirstName,
                    LastAccessed = x.LastAccessed,
                    LastName = x.LastName,
                    PrincipalId = x.PrincipalId,
                    RestoredDate = x.RestoredDate,
                    RevokedDate = x.RevokedDate,
                    SuspendedDate = x.SuspendedDate
                });
        }

        /// <summary>
        /// Returns the user with the given id, or null if not found.
        /// </summary>
        /// <param name="id">The id of the user.</param>
        /// <returns>The user or null if not found.</returns>
        public User GetUserById(Guid id)
        {
            UpdateLastAccessed(id);
            var user = CreateGetUserByIdQuery(id).FirstOrDefault();
            if (user != null)
            {
                logger.Trace("Retrieved user {0} by id {1}.", user, id);
            }
            else
            {
                logger.Trace("No user found for id {0}.", id);
            }
            return user;
        }

        /// <summary>
        /// Returns the user with the given id, or null if not found.
        /// </summary>
        /// <param name="id">The id of the user.</param>
        /// <returns>The user or null if not found.</returns>
        public async Task<User> GetUserByIdAsync(Guid id)
        {
            await UpdateLastAccessedAsync(id);
            var user = await CreateGetUserByIdQuery(id).FirstOrDefaultAsync();
            if (user != null)
            {
                logger.Trace("Retrieved user {0} by id {1}.", user, id);
            }
            else
            {
                logger.Trace("No user found for id {0}.", id);
            }
            return user;
        }

        #endregion

        #region Is Valid
        private IQueryable<UserAccount> CreateGetUserAccountByIdQuery(Guid id)
        {
            return Context.UserAccounts.Where(x => x.AdGuid == id);
        }

        /// <summary>
        /// Returns true if the user is valid in CAM, otherwise false.
        /// </summary>
        /// <param name="id">The id of the user to validate.</param>
        /// <returns>Returns true if the user is valid in CAM, otherwise false.</returns>
        public bool IsUserValid(Guid id)
        {
            UpdateLastAccessed(id);
            var userAccount = CreateGetUserAccountByIdQuery(id).FirstOrDefault();
            return IsUserValid(userAccount);
        }


        /// <summary>
        /// Returns true if the user is valid in CAM, otherwise false.
        /// </summary>
        /// <param name="id">The id of the user to validate.</param>
        /// <returns>Returns true if the user is valid in CAM, otherwise false.</returns>
        public async Task<bool> IsUserValidAsync(Guid id)
        {
            await UpdateLastAccessedAsync(id);
            var userAccount = await CreateGetUserAccountByIdQuery(id).FirstOrDefaultAsync();
            return IsUserValid(userAccount);
        }

        private bool IsUserValid(UserAccount userAccount)
        {
            var isValid = userAccount != null;
            if (isValid)
            {
                logger.Trace("User {0} is valid:  {1}", userAccount.AdGuid, isValid);
            }
            else
            {
                logger.Error("Unknown user was checked for validity.");
            }
            return isValid;
        }
        #endregion
    }
}
