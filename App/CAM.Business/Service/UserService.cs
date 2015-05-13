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
using CAM.Business.Model;
using CAM.Business.Queries.Models;
using ECA.Core.Query;
using ECA.Core.DynamicLinq;
using CAM.Business.Queries;

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

        #region Create

        /// <summary>
        /// Creates a new user in the system.
        /// </summary>
        /// <param name="newUser">The new user.</param>
        /// <returns>The created user.</returns>
        public UserAccount Create(AzureUser newUser)
        {
            return DoCreate(newUser);
        }

        private UserAccount DoCreate(AzureUser newUser)
        {
            var systemUserId = UserAccount.SYSTEM_USER_ACCOUNT_ID;
            var now = DateTimeOffset.UtcNow;
            var userAccount = new UserAccount
            {
                AccountStatusId = AccountStatus.Active.Id,
                AdGuid = newUser.Id,
                CreatedBy = systemUserId,
                CreatedOn = now,
                DisplayName = newUser.DisplayName,
                EmailAddress = newUser.Email,
                FirstName = newUser.FirstName,
                LastAccessed = now,
                LastName = newUser.LastName,
                RevisedBy = systemUserId,
                RevisedOn = now
            };
            var principal = new Principal
            {
                PrincipalTypeId = PrincipalType.Person.Id,
            };
            userAccount.Principal = principal;
            principal.UserAccount = userAccount;
            Context.UserAccounts.Add(userAccount);
            Context.Principals.Add(principal);
            return userAccount;
        }
        #endregion

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

        /// <summary>
        /// Returns the users in the CAM system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The users.</returns>
        public PagedQueryResults<UserDTO> GetUsers(QueryableOperator<UserDTO> queryOperator)
        {
            var users = UserQueries.CreateGetUsersQuery(this.Context, queryOperator).ToPagedQueryResults(queryOperator.Start, queryOperator.Limit);
            logger.Trace("Retrieved users from CAM with query operator [{0}].", queryOperator);
            return users;
        }

        /// <summary>
        /// Returns the users in the CAM system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The users.</returns>
        public async Task<PagedQueryResults<UserDTO>> GetUsersAsync(QueryableOperator<UserDTO> queryOperator)
        {
            var users = await UserQueries.CreateGetUsersQuery(this.Context, queryOperator).ToPagedQueryResultsAsync(queryOperator.Start, queryOperator.Limit);
            logger.Trace("Retrieved users from CAM with query operator [{0}].", queryOperator);
            return users;
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
