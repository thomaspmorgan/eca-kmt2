using ECA.Business.Queries.Models.Persons;
using ECA.Business.Queries.Persons;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using ECA.Core.Service;
using ECA.Data;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Creates a new ContactService with the given context to operate against.
        /// </summary>
        /// <param name="saveActions">The save actions.</param>
        /// <param name="context">The context to operate against.</param>
        public ContactService(EcaContext context, List<ISaveAction> saveActions = null) : base(context, saveActions)
        {
            Contract.Requires(context != null, "The context must not be null.");
        }

        #region Get

        /// <summary>
        /// Returns the sorted, filtered, and paged contacts in the eca system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The sorted, filtered, and paged contacts.</returns>
        public PagedQueryResults<ContactDTO> GetContacts(QueryableOperator<ContactDTO> queryOperator)
        {
            var contacts = ContactQueries.CreateContactDTOQuery(this.Context, queryOperator).ToPagedQueryResults<ContactDTO>(queryOperator.Start, queryOperator.Limit);
            this.logger.Trace("Retrieved contacts by query operator [{0}].", queryOperator);
            
            return contacts;
        }

        /// <summary>
        /// Returns the sorted, filtered, and paged contacts in the eca system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The sorted, filtered, and paged contacts.</returns>
        public async Task<PagedQueryResults<ContactDTO>> GetContactsAsync(QueryableOperator<ContactDTO> queryOperator)
        {
            var contacts = await ContactQueries.CreateContactDTOQuery(this.Context, queryOperator).ToPagedQueryResultsAsync<ContactDTO>(queryOperator.Start, queryOperator.Limit);
            this.logger.Trace("Retrieved contacts by query operator [{0}].", queryOperator);
            return contacts;
        }
        #endregion

    }
}
