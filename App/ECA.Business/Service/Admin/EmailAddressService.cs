using ECA.Business.Exceptions;
using ECA.Business.Queries.Admin;
using ECA.Business.Queries.Models.Admin;
using ECA.Core.Exceptions;
using ECA.Core.Service;
using ECA.Data;
using NLog;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;

namespace ECA.Business.Service.Admin
{
    /// <summary>
    /// The EmailAddressService is capable of handling crud operations on an IEmailAddressable entity
    /// and its email address.
    /// </summary>
    public class EmailAddressService : EcaService, IEmailAddressService
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly Action<IEmailAddressable, int> throwIfEMailAddressableEntityNotFound;
        private readonly Action<EmailAddress, int> throwIfEmailAddressNotFound;
        private Action<Participant> throwValidationErrorIfParticipantSevisInfoIsLocked;
        
        /// <summary>
        /// Creates a new instance and initializes the context..
        /// </summary>
        /// <param name="context">The context to operate against.</param>
        /// <param name="saveActions">The save actions.</param>
        public EmailAddressService(EcaContext context, List<ISaveAction> saveActions = null)
            : base(context, saveActions)
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
            throwValidationErrorIfParticipantSevisInfoIsLocked = (participant) =>
            {
                if (participant.ParticipantPerson != null)
                {
                    var sevisStatusId = participant.ParticipantPerson.ParticipantPersonSevisCommStatuses.OrderByDescending(x => x.AddedOn).Select(x => x.SevisCommStatusId).FirstOrDefault();

                    if (participant != null && IndexOfInt(participant.LOCKED_SEVIS_COMM_STATUSES, sevisStatusId) != -1)
                    {
                        var msg = String.Format("An update was attempted on participant with id [{0}] but should have failed validation.",
                                participant.ParticipantId);

                        throw new EcaBusinessException(msg);
                    }
                }
            };
        }

        static int IndexOfInt(int[] arr, int value)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] == value)
                {
                    return i;
                }
            }
            return -1;
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
            List<EmailAddress> existingEmailAddresses = new List<EmailAddress>();
            if (newEmailAddress.IsPrimary)
            {
                existingEmailAddresses = newEmailAddress.CreateGetEmailAddressesQuery(this.Context).ToList();
            }
            return DoCreate(newEmailAddress, addressable, existingEmailAddresses);
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
            List<EmailAddress> existingEmailAddresses = new List<EmailAddress>();
            if (newEmailAddress.IsPrimary)
            {
                existingEmailAddresses = await newEmailAddress.CreateGetEmailAddressesQuery(this.Context).ToListAsync();
            }
            return DoCreate(newEmailAddress, addressable, existingEmailAddresses);
        }

        private EmailAddress DoCreate<T>(NewEmailAddress<T> newEmailAddress, T emailAddressable, List<EmailAddress> existingEmailAddresses) where T : class, IEmailAddressable
        {
            throwIfEMailAddressableEntityNotFound(emailAddressable, newEmailAddress.GetEmailAddressableEntityId());
            if (newEmailAddress.IsPrimary)
            {
                existingEmailAddresses.ForEach(x => x.IsPrimary = false);
            }
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
            Participant participant = null;
            if (emailAddress != null && emailAddress.PersonId.HasValue)
            {
                participant = GetParticipantByPersonId((int)emailAddress.PersonId);
            }            
            List<EmailAddress> existingEmailAddresses = new List<EmailAddress>();
            if (emailAddress != null && updatedEmailAddress.IsPrimary)
            {
                existingEmailAddresses = CreateGetOtherEmailAddressesQuery(emailAddress).ToList();
            }
            DoUpdate(updatedEmailAddress, emailAddress, existingEmailAddresses, participant);
        }

        /// <summary>
        /// Updates the ECA system's email address data with the given updated email address.
        /// </summary>
        /// <param name="updatedEmailAddress">The updated email address.</param>
        public async Task UpdateAsync(UpdatedEmailAddress updatedEmailAddress)
        {
            var emailAddress = await Context.EmailAddresses.FindAsync(updatedEmailAddress.Id);
            Participant participant = null;
            if (emailAddress != null && emailAddress.PersonId.HasValue)
            {
                participant = await GetParticipantByPersonIdAsync((int)emailAddress.PersonId);
            }            
            List<EmailAddress> existingEmailAddresses = new List<EmailAddress>();
            if (emailAddress != null && updatedEmailAddress.IsPrimary)
            {
                existingEmailAddresses = await CreateGetOtherEmailAddressesQuery(emailAddress).ToListAsync();
            }
            DoUpdate(updatedEmailAddress, emailAddress, existingEmailAddresses, participant);
        }

        private void DoUpdate(UpdatedEmailAddress updatedEmailAddress, EmailAddress modelToUpdate, 
            List<EmailAddress> otherEmailAddresses, Participant participant)
        {
            Contract.Requires(updatedEmailAddress != null, "The updatedEmailAddress must not be null.");
            throwIfEmailAddressNotFound(modelToUpdate, updatedEmailAddress.Id);
            if (participant != null)
            {
                throwValidationErrorIfParticipantSevisInfoIsLocked(participant);
            }
            modelToUpdate.EmailAddressTypeId = updatedEmailAddress.EmailAddressTypeId;
            modelToUpdate.Address = updatedEmailAddress.Address;
            modelToUpdate.IsPrimary = updatedEmailAddress.IsPrimary;
            updatedEmailAddress.Audit.SetHistory(modelToUpdate);
            if (updatedEmailAddress.IsPrimary)
            {
                otherEmailAddresses.ForEach(x => x.IsPrimary = false);
            }
        }

        private IQueryable<EmailAddress> CreateGetOtherEmailAddressesQuery(EmailAddress emailAddressToUpdate)
        {
            var query = this.Context.EmailAddresses.Where(x => x.EmailAddressId != emailAddressToUpdate.EmailAddressId);
            Contract.Assert(emailAddressToUpdate.PersonId.HasValue || emailAddressToUpdate.ContactId.HasValue, "The email address must either be related to a contact or person.");
            if (emailAddressToUpdate.PersonId.HasValue)
            {
                query = query.Where(x => x.PersonId == emailAddressToUpdate.PersonId);
            }
            else if (emailAddressToUpdate.ContactId.HasValue)
            {
                query = query.Where(x => x.ContactId == emailAddressToUpdate.ContactId);
            }
            return query;
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
