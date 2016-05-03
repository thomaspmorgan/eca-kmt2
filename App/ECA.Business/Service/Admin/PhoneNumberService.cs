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
    /// The PhoneNumberService is capable of handling crud operations on an IPhoneNumberable entity
    /// and its phone number.
    /// </summary>
    public class PhoneNumberService : DbContextService<EcaContext>, IPhoneNumberService
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private EcaService service;
        private readonly Action<IPhoneNumberable, int> throwIfPhoneNumberableEntityNotFound;
        private readonly Action<PhoneNumber, int> throwIfPhoneNumberNotFound;
        private Action<Participant> throwValidationErrorIfParticipantSevisInfoIsLocked;
        public readonly int[] LOCKED_SEVIS_COMM_STATUSES = { SevisCommStatus.QueuedToSubmit.Id, SevisCommStatus.PendingSevisSend.Id, SevisCommStatus.SentByBatch.Id };

        /// <summary>
        /// Creates a new instance and initializes the context..
        /// </summary>
        /// <param name="context">The context to operate against.</param>
        /// <param name="saveActions">The save actions.</param>
        public PhoneNumberService(EcaContext context, List<ISaveAction> saveActions = null)
            : base(context, saveActions)
        {
            service = new EcaService(context);

            Contract.Requires(context != null, "The context must not be null.");
            throwIfPhoneNumberableEntityNotFound = (phoneNumberableEntity, id) =>
            {
                if (phoneNumberableEntity == null)
                {
                    throw new ModelNotFoundException(String.Format("The phoneNumberable entity with id [{0}] was not found.", id));
                }
            };
            throwIfPhoneNumberNotFound = (phoneNumber, id) =>
            {
                if (phoneNumber == null)
                {
                    throw new ModelNotFoundException(String.Format("The phone number with id [{0}] was not found.", id));
                }
            };
            throwValidationErrorIfParticipantSevisInfoIsLocked = (participant) =>
            {
                if (participant.ParticipantPerson != null)
                {
                    var sevisStatusId = participant.ParticipantPerson.ParticipantPersonSevisCommStatuses.OrderByDescending(x => x.AddedOn).Select(x => x.SevisCommStatusId).FirstOrDefault();

                    if (participant != null && IndexOfInt(LOCKED_SEVIS_COMM_STATUSES, sevisStatusId) != -1)
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
        /// Retrieves the phone number dto with the given id.
        /// </summary>
        /// <param name="id">The id of the phone number.</param>
        /// <returns>The phone number dto.</returns>
        public PhoneNumberDTO GetById(int id)
        {
            var dto = PhoneNumberQueries.CreateGetPhoneNumberDTOByIdQuery(this.Context, id).FirstOrDefault();
            logger.Info("Retrieved the phone number dto with the given id [{0}].", id);
            return dto;
        }

        /// <summary>
        /// Retrieves the phone number dto with the given id.
        /// </summary>
        /// <param name="id">The id of the phone number.</param>
        /// <returns>The phone number dto.</returns>
        public async Task<PhoneNumberDTO> GetByIdAsync(int id)
        {
            var dto = await PhoneNumberQueries.CreateGetPhoneNumberDTOByIdQuery(this.Context, id).FirstOrDefaultAsync();
            logger.Info("Retrieved the phone number dto with the given id [{0}].", id);
            return dto;
        }
        #endregion

        #region Create
        /// <summary>
        /// Creates a new phone number in the ECA system.
        /// </summary>
        /// <typeparam name="T">The IPhoneNumberable entity type.</typeparam>
        /// <param name="newPhoneNumber">The phone number.</param>
        /// <returns>The created phone number entity.</returns>
        public PhoneNumber Create<T>(NewPhoneNumber<T> newPhoneNumber) where T : class, IPhoneNumberable
        {
            var phoneNumberable = this.Context.Set<T>().Find(newPhoneNumber.GetPhoneNumberableEntityId());
            List<PhoneNumber> existingPhoneNumbers = new List<PhoneNumber>();
            if (newPhoneNumber.IsPrimary)
            {
                existingPhoneNumbers = newPhoneNumber.CreateGetPhoneNumbersQuery(this.Context).ToList();
            }
            return DoCreate(newPhoneNumber, phoneNumberable, existingPhoneNumbers);
        }

        /// <summary>
        /// Creates a new phone number in the ECA system.
        /// </summary>
        /// <typeparam name="T">The IEmailAddressable entity type.</typeparam>
        /// <param name="newPhoneNumber">The  phone number.</param>
        /// <returns>The created  phone number entity.</returns>
        public async Task<PhoneNumber> CreateAsync<T>(NewPhoneNumber<T> newPhoneNumber) where T : class, IPhoneNumberable
        {
            var phoneNumberable = await this.Context.Set<T>().FindAsync(newPhoneNumber.GetPhoneNumberableEntityId());
            List<PhoneNumber> existingPhoneNumbers = new List<PhoneNumber>();
            if (newPhoneNumber.IsPrimary)
            {
                existingPhoneNumbers = await newPhoneNumber.CreateGetPhoneNumbersQuery(this.Context).ToListAsync();
            }
            return DoCreate(newPhoneNumber, phoneNumberable, existingPhoneNumbers);
        }

        private PhoneNumber DoCreate<T>(NewPhoneNumber<T> newPhoneNumber, T phoneNumberable, List<PhoneNumber> existingPhoneNumbers) where T : class, IPhoneNumberable
        {
            throwIfPhoneNumberableEntityNotFound(phoneNumberable, newPhoneNumber.GetPhoneNumberableEntityId());
            if (newPhoneNumber.IsPrimary)
            {
                existingPhoneNumbers.ForEach(x => x.IsPrimary = false);
            }           
            return newPhoneNumber.AddPhoneNumber(phoneNumberable);
        }
        #endregion

        #region Update

        /// <summary>
        /// Updates the ECA system's phone number with the given updated phone number.
        /// </summary>
        /// <param name="updatedPhoneNumber">The updated phone number.</param>
        public void Update(UpdatedPhoneNumber updatedPhoneNumber)
        {
            var phoneNumber = Context.PhoneNumbers.Find(updatedPhoneNumber.Id);
            List<PhoneNumber> existingPhoneNumbers = new List<PhoneNumber>();
            if (phoneNumber != null && updatedPhoneNumber.IsPrimary)
            {
                existingPhoneNumbers = CreateGetOtherPhoneNumbersQuery(phoneNumber).ToList();
            }
            DoUpdate(updatedPhoneNumber, phoneNumber, existingPhoneNumbers);
        }

        /// <summary>
        /// Updates the ECA system's phone number data with the given updated phone number.
        /// </summary>
        /// <param name="updatedPhoneNumber">The updated phone number.</param>
        public async Task UpdateAsync(UpdatedPhoneNumber updatedPhoneNumber)
        {
            var phoneNumber = await Context.PhoneNumbers.FindAsync(updatedPhoneNumber.Id);
            List<PhoneNumber> existingPhoneNumbers = new List<PhoneNumber>();
            if (phoneNumber != null && updatedPhoneNumber.IsPrimary)
            {
                existingPhoneNumbers = await CreateGetOtherPhoneNumbersQuery(phoneNumber).ToListAsync();
            }
            DoUpdate(updatedPhoneNumber, phoneNumber, existingPhoneNumbers);
        }

        private void DoUpdate(UpdatedPhoneNumber updatedPhoneNumber, PhoneNumber modelToUpdate, List<PhoneNumber> existingPhoneNumbers)
        {
            Contract.Requires(updatedPhoneNumber != null, "The updatedPhoneNumber must not be null.");
            throwIfPhoneNumberNotFound(modelToUpdate, updatedPhoneNumber.Id);
            if (modelToUpdate.PersonId.HasValue)
            {
                var participant = service.GetParticipantByPersonId((int)modelToUpdate.PersonId);
                if (participant != null)
                {
                    throwValidationErrorIfParticipantSevisInfoIsLocked(participant);
                }
            }
            modelToUpdate.PhoneNumberTypeId = updatedPhoneNumber.PhoneNumberTypeId;
            modelToUpdate.Number = updatedPhoneNumber.Number;
            modelToUpdate.IsPrimary = updatedPhoneNumber.IsPrimary;
            updatedPhoneNumber.Audit.SetHistory(modelToUpdate);
            if (updatedPhoneNumber.IsPrimary)
            {
                existingPhoneNumbers.ForEach(x => x.IsPrimary = false);
            }
        }

        private IQueryable<PhoneNumber> CreateGetOtherPhoneNumbersQuery(PhoneNumber phoneNumberToUpdate)
        {
            var query = this.Context.PhoneNumbers.Where(x => x.PhoneNumberId != phoneNumberToUpdate.PhoneNumberId);
            Contract.Assert(phoneNumberToUpdate.PersonId.HasValue || phoneNumberToUpdate.ContactId.HasValue, "The phone number must either be related to a contact or person.");
            if (phoneNumberToUpdate.PersonId.HasValue)
            {
                query = query.Where(x => x.PersonId == phoneNumberToUpdate.PersonId);
            }
            else if (phoneNumberToUpdate.ContactId.HasValue)
            {
                query = query.Where(x => x.ContactId == phoneNumberToUpdate.ContactId);
            }
            return query;
        }
        #endregion

        #region Delete
        /// <summary>
        /// Deletes the phone number entry with the given id.
        /// </summary>
        /// <param name="phoneNumberId">The id of the phone number to delete.</param>
        public void Delete(int phoneNumberId)
        {
            var phoneNumber = Context.PhoneNumbers.Find(phoneNumberId);
            DoDelete(phoneNumber);
        }

        /// <summary>
        /// Deletes the phone number entry with the given id.
        /// </summary>
        /// <param name="phoneNumberId">The id of the phone number to delete.</param>
        public async Task DeleteAsync(int phoneNumberId)
        {
            var phoneNumber = await Context.PhoneNumbers.FindAsync(phoneNumberId);
            DoDelete(phoneNumber);
        }

        private void DoDelete(PhoneNumber phoneNumberToDelete)
        {
            if (phoneNumberToDelete != null)
            {
                Context.PhoneNumbers.Remove(phoneNumberToDelete);
            }
        }
        #endregion
    }
}
