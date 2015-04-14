using ECA.Core.DynamicLinq.Filter;
using ECA.Core.DynamicLinq.Sorter;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Core.DynamicLinq
{
    /// <summary>
    /// A Queryable operator is a class to contain sorting, filtering, and paging details for an IQueryable.
    /// </summary>
    /// <typeparam name="T">The type of object that will be sorted, filtered and paged.</typeparam>
    public class QueryableOperator<T> where T : class
    {
        /// <summary>
        /// Creates a new QueryableOperator with start, limit, default sorter, sorters and filters.
        /// </summary>
        /// <param name="start">The paging start value.</param>
        /// <param name="limit">The paging limit value.</param>
        /// <param name="defaultSorter">The default sorter.</param>
        /// <param name="filters">The filters.</param>
        /// <param name="sorters">The sorters.</param>
        public QueryableOperator(int start, int limit, ISorter defaultSorter, IList<IFilter> filters = null, IList<ISorter> sorters = null)
        {
            Contract.Requires(start >= 0, "The start value must be greater than or equal to zero.");
            Contract.Requires(limit >= 0, "The limit must be greater than or equal to zero.");
            Contract.Requires(defaultSorter != null, "The default sorter must not be null.");
            this.Filters = filters ?? new List<IFilter>();
            this.Sorters = sorters ?? new List<ISorter>();
            this.Start = start;
            this.Limit = limit;
            this.DefaultSorter = defaultSorter;
        }

        /// <summary>
        /// Gets the paging start value.
        /// </summary>
        public int Start { get; private set; }

        /// <summary>
        /// Gets the paging limit value.
        /// </summary>
        public int Limit { get; private set; }

        /// <summary>
        /// Gets the filters.
        /// </summary>
        public IList<IFilter> Filters { get; private set; }

        /// <summary>
        /// Gets the sorters.
        /// </summary>
        public IList<ISorter> Sorters { get; private set; }

        /// <summary>
        /// Gets the default sorter.
        /// </summary>
        public ISorter DefaultSorter { get; private set; }

        /// <summary>
        /// Returns a nicely formatted string of the queryable operator.
        /// </summary>
        /// <returns>A nicely formatted string of the queryable operator.</returns>
        public override string ToString()
        {
            var filterString = "Null";
            if(this.Filters != null)
            {
                filterString = String.Join(", ", this.Filters.Select(x => x.ToString()).ToList());
            }
            var sorterString = "Null";
            if(this.Sorters != null)
            {
                sorterString = String.Join(", ", this.Sorters.Select(x => x.ToString()).ToList());
            }
            var defaultSorterString = "Null";
            if(this.DefaultSorter != null)
            {
                defaultSorterString = this.DefaultSorter.ToString();
            }

            return String.Format("Start:  [{0}], Limit:  [{1}], Filters:[{2}], Sorters:  [{3}], DefaultSorter:  [{4}]",
                this.Start,
                this.Limit,
                filterString,
                sorterString,
                defaultSorterString);
        }
    }

    /// <summary>
    /// Contains additional IQueryable extensions.
    /// </summary>
    public static class QueryableOperatorExtensions
    {
        /// <summary>
        /// Applies the queryableOperator's filters and sorters to the IQueryable source.  This does not have an effect
        /// on paging i.e. start and limit.
        /// </summary>
        /// <typeparam name="TSource">The type of object that will be sorted and filtered.</typeparam>
        /// <param name="source">The query to sort and filter.</param>
        /// <param name="queryableOperator">The queryable operator containing filter and sort information.</param>
        /// <returns>The filtered and sorted query.</returns>
        public static IQueryable<TSource> Apply<TSource>(this IQueryable<TSource> source, QueryableOperator<TSource> queryableOperator) where TSource : class
        {
            Contract.Requires(source != null, "The source must not be null.");
            Contract.Requires(queryableOperator != null, "The queryable operator must not be null.");
            if (queryableOperator.Filters != null && queryableOperator.Filters.Count > 0)
            {
                source = source.Where(queryableOperator.Filters);
            }
            if (queryableOperator.Sorters != null && queryableOperator.Sorters.Count > 0)
            {
                source = source.OrderBy(queryableOperator.Sorters);
            }
            else
            {
                source = source.OrderBy(queryableOperator.DefaultSorter);
            }
            return source;
            
        }
    }
}
