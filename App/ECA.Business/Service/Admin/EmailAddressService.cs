using ECA.Business.Queries.Admin;
using ECA.Business.Queries.Models.Admin;
using ECA.Core.Exceptions;
using ECA.Core.Service;
using ECA.Data;
using NLog;
using System;
using System.Data.Entity;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;

namespace ECA.Business.Service.Admin
{
    /// <summary>
    /// The EmailAddressService is capable of handling crud operations on an ISocialable entity
    /// and its email address.
    /// </summary>
    public class EmailAddressService : DbContextService<EcaContext>, IEmailAddressService
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly Action<IEmailAddressable, int> throwIfEMailAddressableEntityNotFound;
        private readonly Action<EmailAddress, int> throwIfEmailAddressNotFound;

        /// <summary>
        /// Creates a new instance and initializes the context..
        /// </summary>
        /// <param name="context">The context to operate against.</param>
        public EmailAddressService(EcaContext context)
            : base(context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            throwIfEMailAddressableEntityNotFound = (emailAddressableEntity, id) =>
            {
                if (emailAddressableEntity == null)
                {
                    throw new ModelNotFoundException(String.Format("The sociable entity with id [{0}] was not found.", id));
                }
            };
            throwIfEmailAddressNotFound = (emailAddress, id) =>
            {
                if (emailAddress == null)
                {
                    throw new ModelNotFoundException(String.Format("The email address with id [{0}] was not found.", id));
                }
            };
        }

        #region Get
        /// <summary>
        /// Retrieves the email address dto with the given id.
        /// </summary>
        /// <param name="id">The id of the email Address.</param>
        /// <returns>The email address dto.</returns>
        public EmailAddressDTO GetById(int id)
        {
            var dto = EmailAddressQueries.CreateGetEmailAddressDTOByIdQuery(this.Context, id).FirstOrDefault();
            logger.Info("Retrieved the social media dto with the given id [{0}].", id);
            return dto;
        }

        /// <summary>
        /// Retrieves the social media dto with the given id.
        /// </summary>
        /// <param name="id">The id of the email address.</param>
        /// <returns>The email address dto.</returns>
        public async Task<EmailAddressDTO> GetByIdAsync(int id)
        {
            var dto = await EmailAddressQueries.CreateGetEmailAddressDTOByIdQuery(this.Context, id).FirstOrDefaultAsync();
            logger.Info("Retrieved the email address dto with the given id [{0}].", id);
            return dto;
        }
        #endregion

        #region Create
        /// <summary>
        /// Creates a new email address in the ECA system.
        /// </summary>
        /// <typeparam name="T">The IEmailAddressable entity type.</typeparam>
        /// <param name="newEmailAddress">The email address.</param>
        /// <returns>The created email address entity.</returns>
        public EmailAddress Create<T>(NewEmailAddress<T> newEmailAddress) where T : class, IEmailAddressable
        {
            var addressable = this.Context.Set<T>().Find(newEmailAddress.GetEmailAddressableEntityId());
            return DoCreate(newEmailAddress, addressable);
        }

        /// <summary>
        /// Creates a new email addres in the ECA system.
        /// </summary>
        /// <typeparam name="T">The IEmailAddressable entity type.</typeparam>
        /// <param name="newEmailAddress">The email address.</param>
        /// <returns>The created semail address entity.</returns>
        public async Task<EmailAddress> CreateAsync<T>(NewEmailAddress<T> newEmailAddress) where T : class, IEmailAddressable
        {
            var addressable = await this.Context.Set<T>().FindAsync(newEmailAddress.GetEmailAddressableEntityId());
            return DoCreate(newEmailAddress, addressable);
        }

        private EmailAddress DoCreate<T>(NewEmailAddress<T> newEmailAddress, IEmailAddressable emailAddressable) where T : class, IEmailAddressable
        {
            throwIfEMailAddressableEntityNotFound(emailAddressable, newEmailAddress.GetEmailAddressableEntityId());            
            return newEmailAddress.AddEmailAddress(emailAddressable);
        }
        #endregion

        #region Update

        /// <summary>
        /// Updates the ECA system's social email address with the given updated email address.
        /// </summary>
        /// <param name="updatedEmailAddress">The updated email address.</param>
        public void Update(UpdatedEmailAddress updatedEmailAddress)
        {
            var emailAddress = Context.EmailAddresses.Find(updatedEmailAddress.Id);
            DoUpdate(updatedEmailAddress, emailAddress);
        }

        /// <summary>
        /// Updates the ECA system's email address data with the given updated email address.
        /// </summary>
        /// <param name="updatedEmailAddress">The updated email address.</param>
        public async Task UpdateAsync(UpdatedEmailAddress updatedEmailAddress)
        {
            var emailAddress = await Context.EmailAddresses.FindAsync(updatedEmailAddress.Id);
            DoUpdate(updatedEmailAddress, emailAddress);
        }

        private void DoUpdate(UpdatedEmailAddress updatedEmailAddress, EmailAddress modelToUpdate)
        {
            Contract.Requires(updatedEmailAddress != null, "The updatedEmailAddress must not be null.");
            throwIfEmailAddressNotFound(modelToUpdate, updatedEmailAddress.Id);
            modelToUpdate.EmailAddressTypeId = updatedEmailAddress.EmailAddressTypeId;
            modelToUpdate.Address = updatedEmailAddress.Address;
            updatedEmailAddress.Update.SetHistory(modelToUpdate);
        }
        #endregion

        #region Delete
        /// <summary>
        /// Deletes the email entry with the given id.
        /// </summary>
        /// <param name="emailAddressId">The id of the email address to delete.</param>
        public void Delete(int emailAddressId)
        {
            var emailAddress = Context.EmailAddresses.Find(emailAddressId);
            DoDelete(emailAddress);
        }

        /// <summary>
        /// Deletes the email address entry with the given id.
        /// </summary>
        /// <param name="emailAddressId">The id of the email address to delete.</param>
        public async Task DeleteAsync(int emailAddressId)
        {
            var emailAddress = await Context.EmailAddresses.FindAsync(emailAddressId);
            DoDelete(emailAddress);
        }

        private void DoDelete(EmailAddress emailAddressToDelete)
        {
            if (emailAddressToDelete != null)
            {
                Context.EmailAddresses.Remove(emailAddressToDelete);
            }
        }
        #endregion
    }
}
