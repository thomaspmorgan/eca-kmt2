using ECA.Business.Queries.Models.Persons;
using ECA.Business.Queries.Persons;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
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
    public class ContactService : IDisposable, IContactService
    {
        private EcaContext context;

        /// <summary>
        /// Creates a new ContactService with the given context to operate against.
        /// </summary>
        /// <param name="context">The context to operate against.</param>
        public ContactService(EcaContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            this.context = context;
        }

        #region Get

        /// <summary>
        /// Returns the sorted, filtered, and paged contacts in the eca system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The sorted, filtered, and paged contacts.</returns>
        public PagedQueryResults<ContactDTO> GetContacts(QueryableOperator<ContactDTO> queryOperator)
        {
            return ContactQueries.CreateContactDTOQuery(this.context, queryOperator).ToPagedQueryResults<ContactDTO>(queryOperator.Start, queryOperator.Limit);
        }

        /// <summary>
        /// Returns the sorted, filtered, and paged contacts in the eca system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The sorted, filtered, and paged contacts.</returns>
        public Task<PagedQueryResults<ContactDTO>> GetContactsAsync(QueryableOperator<ContactDTO> queryOperator)
        {
            return ContactQueries.CreateContactDTOQuery(this.context, queryOperator).ToPagedQueryResultsAsync<ContactDTO>(queryOperator.Start, queryOperator.Limit);
        }
        #endregion


        #region IDispose

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.context.Dispose();
                this.context = null;
            }
        }

        #endregion
    }
}
