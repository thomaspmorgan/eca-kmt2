using CAM.Business.Model;
using CAM.Business.Queries.Models;
using CAM.Data;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using ECA.Core.Service;
using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
namespace CAM.Business.Service
{
    /// <summary>
    /// An IUserService performs user validation and crud operations on a CAM user.
    /// </summary>
    [ContractClass(typeof(UserServiceContract))]
    public interface IUserService : ISaveable
    {
        /// <summary>
        /// Creates a new user in the system.
        /// </summary>
        /// <param name="newUser">The new user.</param>
        /// <returns>The created user.</returns>
        UserAccount Create(AzureUser newUser);

        /// <summary>
        /// Returns the user with the given id.
        /// </summary>
        /// <param name="id">The id of the user.</param>
        /// <returns>The user with the given id.</returns>
        User GetUserById(Guid id);

        /// <summary>
        /// Returns the user with the given id.
        /// </summary>
        /// <param name="id">The id of the user.</param>
        /// <returns>The user with the given id.</returns>
        System.Threading.Tasks.Task<User> GetUserByIdAsync(Guid id);

        /// <summary>
        /// Returns true if the user with the id is valid in CAM.
        /// </summary>
        /// <param name="id">The id of the user.</param>
        /// <returns>True, if the user is valid in CAM, otherwise false.</returns>
        bool IsUserValid(Guid id);

        /// <summary>
        /// Returns true if the user with the id is valid in CAM.
        /// </summary>
        /// <param name="id">The id of the user.</param>
        /// <returns>True, if the user is valid in CAM, otherwise false.</returns>
        System.Threading.Tasks.Task<bool> IsUserValidAsync(Guid id);

        /// <summary>
        /// Returns the users in the CAM system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The users.</returns>
        PagedQueryResults<UserDTO> GetUsers(QueryableOperator<UserDTO> queryOperator);

        /// <summary>
        /// Returns the users in the CAM system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The users.</returns>
        Task<PagedQueryResults<UserDTO>> GetUsersAsync(QueryableOperator<UserDTO> queryOperator);

        /// <summary>
        /// Returns the user with the given principal id, or null if not found.
        /// </summary>
        /// <param name="id">The principal id of the user.</param>
        /// <returns>The user or null if not found.</returns>
        User GetUserById(int principalId);

        /// <summary>
        /// Returns the user with the given principal id, or null if not found.
        /// </summary>
        /// <param name="id">The principal id of the user.</param>
        /// <returns>The user or null if not found.</returns>
        Task<User> GetUserByIdAsync(int principalId);
    }

    /// <summary>
    /// 
    /// </summary>
    [ContractClassFor(typeof(IUserService))]
    public abstract class UserServiceContract : IUserService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="newUser"></param>
        /// <returns></returns>
        public UserAccount Create(AzureUser newUser)
        {
            Contract.Requires(newUser != null, "The user must not be null.");
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public User GetUserById(Guid id)
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<User> GetUserByIdAsync(Guid id)
        {
            return Task.FromResult<User>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsUserValid(Guid id)
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<bool> IsUserValidAsync(Guid id)
        {
            return Task.FromResult<bool>(false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryOperator"></param>
        /// <returns></returns>
        public PagedQueryResults<UserDTO> GetUsers(QueryableOperator<UserDTO> queryOperator)
        {
            Contract.Requires(queryOperator != null, "The query operator must not be null.");
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryOperator"></param>
        /// <returns></returns>
        public Task<PagedQueryResults<UserDTO>> GetUsersAsync(QueryableOperator<UserDTO> queryOperator)
        {
            Contract.Requires(queryOperator != null, "The query operator must not be null.");
            return Task.FromResult<PagedQueryResults<UserDTO>>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="saveActions"></param>
        /// <returns></returns>
        public int SaveChanges(System.Collections.Generic.IList<ISaveAction> saveActions = null)
        {
            return 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="saveActions"></param>
        /// <returns></returns>
        public Task<int> SaveChangesAsync(System.Collections.Generic.IList<ISaveAction> saveActions = null)
        {
            return Task.FromResult<int>(1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="principalId"></param>
        /// <returns></returns>
        public User GetUserById(int principalId)
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="principalId"></param>
        /// <returns></returns>
        public Task<User> GetUserByIdAsync(int principalId)
        {
            return Task.FromResult<User>(null);
        }
    }
}
