using System;
namespace ECA.Business.Service.Persons
{
    /// <summary>
    /// An IContactService is capable of performing crud operations on eca contacts.
    /// </summary>
    public interface IContactService
    {
        /// <summary>
        /// Returns the contacts currently in the ECA system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The paged, filtered, and sorted contacts in the ECA system.</returns>
        ECA.Core.Query.PagedQueryResults<ECA.Business.Queries.Models.Persons.ContactDTO> GetContacts(ECA.Core.DynamicLinq.QueryableOperator<ECA.Business.Queries.Models.Persons.ContactDTO> queryOperator);

        /// <summary>
        /// Returns the contacts currently in the ECA system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The paged, filtered, and sorted contacts in the ECA system.</returns>
        System.Threading.Tasks.Task<ECA.Core.Query.PagedQueryResults<ECA.Business.Queries.Models.Persons.ContactDTO>> GetContactsAsync(ECA.Core.DynamicLinq.QueryableOperator<ECA.Business.Queries.Models.Persons.ContactDTO> queryOperator);
    }
}
