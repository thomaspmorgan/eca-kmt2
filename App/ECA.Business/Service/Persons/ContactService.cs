using ECA.Business.Queries.Models.Persons;
using ECA.Business.Queries.Persons;
using ECA.Core.DynamicLinq;
using ECA.Core.Logging;
using ECA.Core.Query;
using ECA.Core.Service;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Persons
{
    /// <summary>
    /// The ContactService is capable of performing operations on contacts against a DbContext.
    /// </summary>
    public class ContactService : DbContextService<EcaContext>, IContactService
    {

        /// <summary>
        /// Creates a new ContactService with the given context to operate against.
        /// </summary>
        /// <param name="context">The context to operate against.</param>
        public ContactService(EcaContext context, ILogger logger) : base(context, logger)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(logger != null, "The logger must not be null.");
        }

        #region Get

        /// <summary>
        /// Returns the sorted, filtered, and paged contacts in the eca system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The sorted, filtered, and paged contacts.</returns>
        public PagedQueryResults<ContactDTO> GetContacts(QueryableOperator<ContactDTO> queryOperator)
        {
            return ContactQueries.CreateContactDTOQuery(this.Context, queryOperator).ToPagedQueryResults<ContactDTO>(queryOperator.Start, queryOperator.Limit);
        }

        /// <summary>
        /// Returns the sorted, filtered, and paged contacts in the eca system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The sorted, filtered, and paged contacts.</returns>
        public Task<PagedQueryResults<ContactDTO>> GetContactsAsync(QueryableOperator<ContactDTO> queryOperator)
        {
            return ContactQueries.CreateContactDTOQuery(this.Context, queryOperator).ToPagedQueryResultsAsync<ContactDTO>(queryOperator.Start, queryOperator.Limit);
        }
        #endregion

    }
}
