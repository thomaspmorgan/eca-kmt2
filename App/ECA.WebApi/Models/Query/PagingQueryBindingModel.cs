using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Filter;
using ECA.Core.DynamicLinq.Sorter;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Web;

namespace ECA.WebApi.Models.Query
{
    public abstract class PagingQueryBindingModel
    {
        /// <summary>
        /// Gets the maximum number of results a paged request can have.
        /// </summary>
        public const int MAX_LIMIT = 300;

        /// <summary>
        /// Gets or sets the Start value i.e. the number of records to skip.
        /// </summary>
        [Range(0, Int32.MaxValue)]
        public int Start { get; set; }

        /// <summary>
        /// Gets or sets the Limit value i.e. the number of records to return.
        /// </summary>
        [Range(1, MAX_LIMIT)]
        public int Limit { get; set; }

        /// <summary>
        /// Gets or sets the Sorters.
        /// </summary>
        public List<string> Sort { get; set; }

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
                filters: GetFilters(),
                sorters: ParseSorters(this.Sort).Select(x => x.ToISorter()).ToList());
        }

        /// <summary>
        /// Returns the list of filters for the query operator.
        /// </summary>
        /// <returns>The list of filters for the query operator.</returns>
        public abstract List<IFilter> GetFilters();

        /// <summary>
        /// Parses sorters given the json encoded sorters.
        /// </summary>
        /// <param name="sorterStrings">The json encoded sorters.</param>
        /// <returns>The list of sorter binding models.</returns>
        public List<SorterBindingModel> ParseSorters(List<string> sorterStrings)
        {
            List<SorterBindingModel> sorters = new List<SorterBindingModel>();
            if (sorterStrings != null)
            {
                sorterStrings.ForEach(x =>
                {
                    sorters.Add(JsonConvert.DeserializeObject<SorterBindingModel>(x));
                });
            }
            return sorters;
        }

        /// <summary>
        /// Returns a formatted string of this model.
        /// </summary>
        /// <returns>A formatted string of this model.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("Start:  {0}, Limit:  {1}:  Sorters:  [{2}]",
                this.Start,
                this.Limit,
                String.Join(", ", this.Sort != null ? this.Sort : new List<string>()));
            return sb.ToString();
        }
    }
}