using ECA.Data;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using System.Threading.Tasks;
using System.Diagnostics.Contracts;
using ECA.Core.Service;
using ECA.Business.Queries.Models.Persons;

namespace ECA.Business.Service.Persons
{
    /// <summary>
    /// An IContactService is capable of performing crud operations on eca contacts.
    /// </summary>
    [ContractClass(typeof(ContactServiceContract))]
    public interface IContactService : ISaveable
    {
        /// <summary>
        /// Returns the contacts currently in the ECA system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The paged, filtered, and sorted contacts in the ECA system.</returns>
        ECA.Core.Query.PagedQueryResults<ContactDTO> GetContacts(ECA.Core.DynamicLinq.QueryableOperator<ContactDTO> queryOperator);

        /// <summary>
        /// Returns the contacts currently in the ECA system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The paged, filtered, and sorted contacts in the ECA system.</returns>
        System.Threading.Tasks.Task<ECA.Core.Query.PagedQueryResults<ContactDTO>> GetContactsAsync(ECA.Core.DynamicLinq.QueryableOperator<ContactDTO> queryOperator);
        
        /// <summary>
        /// Adds a new point of the contact to the datastore.
        /// </summary>
        /// <param name="pointOfContact">The point of contact.</param>
        /// <returns>The Contact that was added to the context.</returns>
        Contact Create(AdditionalPointOfContact pointOfContact);

        /// <summary>
        /// Adds a new point of the contact to the datastore.
        /// </summary>
        /// <param name="pointOfContact">The point of contact.</param>
        /// <returns>The Contact that was added to the context.</returns>
        System.Threading.Tasks.Task<Contact> CreateAsync(AdditionalPointOfContact pointOfContact);
        
        /// <summary>
        /// Updates a point of contact in the datastore.
        /// </summary>
        /// <param name="updatedPointOfContact"></param>
        /// <returns></returns>
        System.Threading.Tasks.Task<ContactDTO> UpdateContactAsync(UpdatedPointOfContact updatedPointOfContact);

        /// <summary>
        /// Returns the contact with the given id.
        /// </summary>
        /// <param name="contactId">The id of the contact.</param>
        /// <returns>The contact, or null, if it does not exist.</returns>
        ContactDTO GetContactById(int contactId);

        /// <summary>
        /// Returns the contact with the given id.
        /// </summary>
        /// <param name="contactId">The id of the contact.</param>
        /// <returns>The contact, or null, if it does not exist.</returns>
        Task<ContactDTO> GetContactByIdAsync(int contactId);
                
    }

    /// <summary>
    /// 
    /// </summary>
    [ContractClassFor(typeof(IContactService))]
    public abstract class ContactServiceContract : IContactService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pointOfContact"></param>
        /// <returns></returns>
        public Contact Create(AdditionalPointOfContact pointOfContact)
        {
            Contract.Requires(pointOfContact != null, "The point of contact must not be null.");
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pointOfContact"></param>
        /// <returns></returns>
        public Task<Contact> CreateAsync(AdditionalPointOfContact pointOfContact)
        {
            Contract.Requires(pointOfContact != null, "The point of contact must not be null.");
            return Task.FromResult<Contact>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pointOfContact"></param>
        /// <returns></returns>
        public Task<ContactDTO> UpdateContactAsync(UpdatedPointOfContact updatedPointOfContact)
        {
            Contract.Requires(updatedPointOfContact != null, "The point of contact must not be null.");
            return Task.FromResult<ContactDTO>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contactId"></param>
        /// <returns></returns>
        public ContactDTO GetContactById(int contactId)
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contactId"></param>
        /// <returns></returns>
        public Task<ContactDTO> GetContactByIdAsync(int contactId)
        {
            return Task.FromResult<ContactDTO>(null);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryOperator"></param>
        /// <returns></returns>
        public PagedQueryResults<ContactDTO> GetContacts(QueryableOperator<ContactDTO> queryOperator)
        {
            Contract.Requires(queryOperator != null, "The query operator must not be null.");
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryOperator"></param>
        /// <returns></returns>
        public Task<PagedQueryResults<ContactDTO>> GetContactsAsync(QueryableOperator<ContactDTO> queryOperator)
        {
            Contract.Requires(queryOperator != null, "The query operator must not be null.");
            return Task.FromResult<PagedQueryResults<ContactDTO>>(null);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int SaveChanges()
        {
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<int> SaveChangesAsync()
        {
            return Task.FromResult<int>(0);
        }
    }
}
