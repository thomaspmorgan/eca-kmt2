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
        /// Gets the total count of objects T.
        /// </summary>
        public int Total { get; private set; }

        /// <summary>
        /// Gets the paged results of type T.
        /// </summary>
        public List<T> Results { get; private set; }
    }

    /// <summary>
    /// The PagedQueryResultsExtensions provide additional functional to an IQueryable to return a PagedQueryResults object.
    /// </summary>
    public static class PagedQueryResultsExtensions
    {
        /// <summary>
        /// Returns a PagedQueryResults objects given the IQueryable and start and limit values.
        /// </summary>
        /// <typeparam name="T">The type of objects in the query.</typeparam>
        /// <param name="source">The query to page.</param>
        /// <param name="start">The number of records to skip.</param>
        /// <param name="limit">The number of records to take.</param>
        /// <returns>A PagedQueryResults object containing the total number of records and the paged records.</returns>
        public static PagedQueryResults<T> ToPagedQueryResults<T>(this IQueryable<T> source, int start, int limit) where T : class
        {
            Contract.Requires(source != null, "The source must not be null.");
            var results = source.Skip(() => start).Take(() => limit).ToList();
            var total = source.Count();
            return new PagedQueryResults<T>(total, results);
        }

        /// <summary>
        /// Returns a PagedQueryResults objects given the IQueryable and start and limit values.
        /// </summary>
        /// <typeparam name="T">The type of objects in the query.</typeparam>
        /// <param name="source">The query to page.</param>
        /// <param name="start">The number of records to skip.</param>
        /// <param name="limit">The number of records to take.</param>
        /// <returns>A PagedQueryResults object containing the total number of records and the paged records.</returns>
        public static async Task<PagedQueryResults<T>> ToPagedQueryResultsAsync<T>(this IQueryable<T> source, int start, int limit) where T : class
        {
            Contract.Requires(source != null, "The source must not be null.");
            var results = await source.Skip(() => start).Take(() => limit).ToListAsync();
            var total = await source.CountAsync();
            return new PagedQueryResults<T>(total, results);
        }
    }
}
