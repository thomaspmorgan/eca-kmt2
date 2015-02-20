using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Filter;
using ECA.Core.DynamicLinq.Sorter;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Models
{
    /// <summary>
    /// A PagingQueryBindingModel is used to take in paging, filtering, and sorting parameters in an action
    /// and use those parameters to return a QueryableOperator.
    /// </summary>
    public class PagingQueryBindingModel
    {
        /// <summary>
        /// Gets or sets the Start value i.e. the number of records to skip.
        /// </summary>
        [Range(0, Int32.MaxValue)]
        public int Start { get; set; }

        /// <summary>
        /// Gets or sets the Limit value i.e. the number of records to return.
        /// </summary>
        [Range(1, 300)]
        public int Limit { get; set; }

        /// <summary>
        /// Gets or sets the filters of the query.
        /// </summary>
        public string Filter { get; set; }

        /// <summary>
        /// Gets or sets the Sort of the query.
        /// </summary>
        public string Sort { get; set; }

        /// <summary>
        /// Returns a QueryableOperator on type T to page, filter, and sort a query.
        /// </summary>
        /// <typeparam name="T">The type to page, filter and sort.</typeparam>
        /// <param name="defaultSorter">The default sorter of the query.</param>
        /// <returns>The query operator to apply to an IQueryable.</returns>
        public virtual QueryableOperator<T> ToQueryableOperator<T>(ISorter defaultSorter) where T : class
        {
            Contract.Requires(defaultSorter != null, "The default sorter must not be null.");
            return new QueryableOperator<T>(
                start: this.Start,
                limit: this.Limit,
                defaultSorter: defaultSorter,
                filters: ParseAsSimpleFilters(this.Filter),
                sorters: ParseAsSimpleSorters(this.Sort));
        }

        /// <summary>
        /// Returns a collection of IFilters from the given filter as a string.  If
        /// the string is null an empty list is returned.
        /// </summary>
        /// <param name="filter">The json array of simple filters.</param>
        /// <returns>A List of SimpleFilters from the given string.</returns>
        public IList<IFilter> ParseAsSimpleFilters(string filter)
        {
            IList<IFilter> filters = new List<IFilter>();
            if (filter != null)
            {
                var parsedFilters = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SimpleFilter>>(filter);
                parsedFilters.ForEach(x => filters.Add(x));
            }
            return filters;
        }

        /// <summary>
        /// Returns a collection of ISorters form the given sort as a string.  If
        /// the string is null an empty list is returned.
        /// </summary>
        /// <param name="sort">The json array of sorters.</param>
        /// <returns>The list of simple sorters.</returns>
        public IList<ISorter> ParseAsSimpleSorters(string sort)
        {
            IList<ISorter> sorters = new List<ISorter>();
            if (sort != null)
            {
                var parsedSorters = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SimpleSorter>>(sort);
                parsedSorters.ForEach(x => sorters.Add(x));
            }
            return sorters;
        }
    }
}