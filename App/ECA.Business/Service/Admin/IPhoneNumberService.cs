using ECA.Business.Queries.Models.Admin;
using ECA.Core.Service;
using ECA.Data;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
namespace ECA.Business.Service.Admin
{
    /// <summary>
    /// An IPhoneNumberService is used to create or update phone numbers for ECA objects.
    /// </summary>
    [ContractClass(typeof(PhoneNumberServiceContract))]
    public interface IPhoneNumberService : ISaveable
    {
        /// <summary>
        /// Creates a new phone number in the ECA system.
        /// </summary>
        /// <typeparam name="T">The IPhoneNumberable entity type.</typeparam>
        /// <param name="phoneNumber">The phoneNumber.</param>
        /// <returns>The created  phone number entity.</returns>
        PhoneNumber Create<T>(NewPhoneNumber<T> phoneNumber) where T : class, IPhoneNumberable;

        /// <summary>
        /// Creates a new phone number in the ECA system.
        /// </summary>
        /// <typeparam name="T">The IPhoneNumberable entity type.</typeparam>
        /// <param name="phoneNumber">The phone number.</param>
        /// <returns>The created phone number entity.</returns>
        Task<PhoneNumber> CreateAsync<T>(NewPhoneNumber<T> phoneNumber) where T : class, IPhoneNumberable;

        /// <summary>
        /// Updates the ECA system's  phone number data with the given updated  phone number.
        /// </summary>
        /// <param name="updatedPhoneNumber">The updated  phone number.</param>
        void Update(UpdatedPhoneNumber updatedPhoneNumber);

        /// <summary>
        /// Updates the ECA system's  phone number with the given updated  phone number.
        /// </summary>
        /// <param name="updatedPhoneNumber">The updated phone number.</param>
        Task UpdateAsync(UpdatedPhoneNumber updatedPhoneNumber);

        /// <summary>
        /// Retrieves the  phone number dto with the given id.
        /// </summary>
        /// <param name="id">The id of the  phone number.</param>
        /// <returns>The  phone number dto.</returns>
        PhoneNumberDTO GetById(int id);

        /// <summary>
        /// Retrieves the phone number dto with the given id.
        /// </summary>
        /// <param name="id">The id of the phone number.</param>
        /// <returns>The phone numbers dto.</returns>
        Task<PhoneNumberDTO> GetByIdAsync(int id);

        /// <summary>
        /// Deletes the phone number entry with the given id.
        /// </summary>
        /// <param name="phoneNumberId">The id of the phone number to delete.</param>
        void Delete(int phoneNumberId);

        /// <summary>
        /// Deletes the phone number entry with the given id.
        /// </summary>
        /// <param name="phoneNumberId">The id of the phone number to delete.</param>
        Task DeleteAsync(int phoneNumberId);
    }

    /// <summary>
    /// 
    /// </summary>
    [ContractClassFor(typeof(IPhoneNumberService))]
    public abstract class PhoneNumberServiceContract : IPhoneNumberService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public PhoneNumber Create<T>(NewPhoneNumber<T> phoneNumber) where T : class, IPhoneNumberable
        {
            Contract.Requires(phoneNumber != null, "The phone number entity must not be null.");
            Contract.Ensures(Contract.Result<PhoneNumber>() != null, "The phone number entity returned must not be null.");
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public Task<PhoneNumber> CreateAsync<T>(NewPhoneNumber<T> phoneNumber) where T : class, IPhoneNumberable
        {
            Contract.Requires(phoneNumber != null, "The phone number entity must not be null.");
            Contract.Ensures(Contract.Result<Task<PhoneNumber>>() != null, "The phone number entity returned must not be null.");
            return Task.FromResult<PhoneNumber>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="updatedPhoneNumber"></param>
        public void Update(UpdatedPhoneNumber updatedPhoneNumber)
        {
            Contract.Requires(updatedPhoneNumber != null, "The updated phone number must not be null.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="updatedPhoneNumber"></param>
        /// <returns></returns>
        public Task UpdateAsync(UpdatedPhoneNumber updatedPhoneNumber)
        {
            Contract.Requires(updatedPhoneNumber != null, "The updated phone number must not be null.");
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
        public PhoneNumberDTO GetById(int id)
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<PhoneNumberDTO> GetByIdAsync(int id)
        {
            return Task.FromResult<PhoneNumberDTO>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="phoneNumberId"></param>
        public void Delete(int phoneNumberId)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="phoneNumberId"></param>
        /// <returns></returns>
        public Task DeleteAsync(int phoneNumberId)
        {
            return Task.FromResult<object>(null);
        }
    }
}
