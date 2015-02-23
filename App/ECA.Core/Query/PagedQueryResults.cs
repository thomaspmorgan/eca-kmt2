using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Core.Query
{
    /// <summary>
    /// A PagedQueryResults class is a class used to hold to hold a list of objects from a query thtat
    /// been paged.
    /// </summary>
    /// <typeparam name="T">The result type.</typeparam>
    public class PagedQueryResults<T> where T : class
    {
        private IQueryable<T> query;
        private int start;
        private int limit;

        /// <summary>
        /// Creates a new PagedQueryResults object with the total number of objects T, and the paged list of objects T.
        /// </summary>
        /// <param name="total">The total number of objects T.</param>
        /// <param name="results">The paged list of objects T.</param>
        public PagedQueryResults(int total, List<T> results)
        {
            this.Total = total;
            this.Results = results ?? new List<T>();
        }

        /// <summary>
        /// Creates a new PagedQueryResults object with the IQueryable of T and the paging values.
        /// </summary>
        /// <param name="query">The IQueryable of T."/></param>
        /// <param name="start">The number of objects to skip.</param>
        /// <param name="limit">The number of objects to return.</param>
        public PagedQueryResults(IQueryable<T> query, int start, int limit)
        {
            Contract.Requires(query != null, "The query must not be null.");
            this.query = query;
            this.start = start;
            this.limit = limit;
        }

        /// <summary>
        /// Gets the total count of objects T.
        /// </summary>
        public int Total { get; private set; }

        /// <summary>
        /// Gets the paged results of type T.
        /// </summary>
        public List<T> Results { get; private set; }

        /// <summary>
        /// Computes the paged list given the IQueryable of T and the paging values.
        /// </summary>
        /// <returns>This object after being computed.</returns>
        public PagedQueryResults<T> Compute()
        {
            if (this.query == null)
            {
                throw new NotSupportedException("The query is null.  Use the compute operations if you are constructing this object from an IQueryable.");
            }
            this.Total = this.query.Count();
            this.Results = this.query.Skip(start).Take(limit).ToList();
            return this;
        }

        /// <summary>
        /// Computes the paged list given the IQueryable of T and the paging values.
        /// </summary>
        /// <returns>This object after being computed.</returns>
        public async Task<PagedQueryResults<T>> ComputeAsync()
        {
            if (this.query == null)
            {
                throw new NotSupportedException("The query is null.  Use the compute operations if you are constructing this object from an IQueryable.");
            }
            this.Total = await this.query.CountAsync();
            this.Results = await this.query.Skip(start).Take(limit).ToListAsync();
            return this;
        }
    }
}
