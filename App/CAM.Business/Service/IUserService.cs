using CAM.Business.Model;
using CAM.Data;
using ECA.Core.Service;
using System;
namespace CAM.Business.Service
{
    /// <summary>
    /// An IUserService performs user validation and crud operations on a CAM user.
    /// </summary>
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
    }
}
