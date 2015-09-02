using ECA.Business.Queries.Models.Admin;
using ECA.Core.Service;
using ECA.Data;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
namespace ECA.Business.Service.Admin
{
    /// <summary>
    /// An ISocialMediaService is used to create or update social media presences for ECA objects.
    /// </summary>
    [ContractClass(typeof(EmailAddressServiceContract))]
    public interface IEmailAddressService : ISaveable
    {
        /// <summary>
        /// Creates a new eMailAddress in the ECA system.
        /// </summary>
        /// <typeparam name="T">The IEmailAddressable entity type.</typeparam>
        /// <param name="emailAddress">The eMailAddress.</param>
        /// <returns>The created email entity.</returns>
        EmailAddress Create<T>(NewEmailAddress<T> emailAddress) where T : class, IEmailAddressable;

        /// <summary>
        /// Creates a new eMail in the ECA system.
        /// </summary>
        /// <typeparam name="T">The IEmailAddressable entity type.</typeparam>
        /// <param name="emailAddress">The email address.</param>
        /// <returns>The created email entity.</returns>
        Task<EmailAddress> CreateAsync<T>(NewEmailAddress<T> emailAddress) where T : class, IEmailAddressable;

        /// <summary>
        /// Updates the ECA system's email data with the given updated email address.
        /// </summary>
        /// <param name="updatedEmailAddress">The updated email.</param>
        void Update(UpdatedEmailAddress updatedEmailAddress);

        /// <summary>
        /// Updates the ECA system's email address with the given updated email address.
        /// </summary>
        /// <param name="updatedEmailAddress">The updated email address.</param>
        Task UpdateAsync(UpdatedEmailAddress updatedEmailAddress);

        /// <summary>
        /// Retrieves the email dto with the given id.
        /// </summary>
        /// <param name="id">The id of the email address.</param>
        /// <returns>The EmailAddress dto.</returns>
        EmailAddressDTO GetById(int id);

        /// <summary>
        /// Retrieves the email address dto with the given id.
        /// </summary>
        /// <param name="id">The id of the email address.</param>
        /// <returns>The email address dto.</returns>
        Task<EmailAddressDTO> GetByIdAsync(int id);

        /// <summary>
        /// Deletes the email address entry with the given id.
        /// </summary>
        /// <param name="emailAddressId">The id of the email address to delete.</param>
        void Delete(int emailAddressId);

        /// <summary>
        /// Deletes the email address entry with the given id.
        /// </summary>
        /// <param name="emailAddressId">The id of the email address to delete.</param>
        Task DeleteAsync(int emailAddressId);
    }

    /// <summary>
    /// 
    /// </summary>
    [ContractClassFor(typeof(IEmailAddressService))]
    public abstract class EmailAddressServiceContract : IEmailAddressService
    { 
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        public EmailAddress Create<T>(NewEmailAddress<T> emailAddress) where T : class, IEmailAddressable
        {
            Contract.Requires(emailAddress != null, "The social media entity must not be null.");
            Contract.Ensures(Contract.Result<EmailAddress>() != null, "The email address entity returned must not be null.");
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        public Task<EmailAddress> CreateAsync<T>(NewEmailAddress<T> emailAddress) where T : class, IEmailAddressable
        {
            Contract.Requires(emailAddress != null, "The social media entity must not be null.");
            Contract.Ensures(Contract.Result<Task<EmailAddress>>() != null, "The email address entity returned must not be null.");
            return Task.FromResult<EmailAddress>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="updatedEmailAddress"></param>
        public void Update(UpdatedEmailAddress updatedEmailAddress)
        {
            Contract.Requires(updatedEmailAddress != null, "The updated email address must not be null.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="updatedEmailAddress"></param>
        /// <returns></returns>
        public Task UpdateAsync(UpdatedEmailAddress updatedEmailAddress)
        {
            Contract.Requires(updatedEmailAddress != null, "The updated email address must not be null.");
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int SaveChanges()
        {
            return 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<int> SaveChangesAsync()
        {
            return Task.FromResult<int>(1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public EmailAddressDTO GetById(int id)
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<EmailAddressDTO> GetByIdAsync(int id)
        {
            return Task.FromResult<EmailAddressDTO>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="emailAddressId"></param>
        public void Delete(int emailAddressId)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="emailAddressId"></param>
        /// <returns></returns>
        public Task DeleteAsync(int emailAddressId)
        {
            return Task.FromResult<object>(null);
        }
    }
}
