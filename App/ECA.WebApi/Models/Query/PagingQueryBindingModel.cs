using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Filter;
using ECA.Core.DynamicLinq.Sorter;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;
using System.Web.Http.ValueProviders;
using System.Net.Http;
using System.Web.Http.ModelBinding;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Text;

namespace ECA.WebApi.Models.Query
{
    /// <summary>
    /// A PagingQueryBindingModel is used to take in paging, filtering, and sorting parameters in an action
    /// and use those parameters to return a QueryableOperator.
    /// </summary>
    public class PagingQueryBindingModel
    {
        /// <summary>
        /// 
        /// Creates a new default instance and intializes the Filter and Sort properties.
        /// </summary>
        public PagingQueryBindingModel()
        {
            this.Filter = new List<string>();
            this.Sort = new List<string>();
        }

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
        /// Gets or sets the Filters.
        /// </summary>
        public List<string> Filter { get; set; }

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
                filters: ParseFilters(this.Filter).Select(x => x.ToIFilter()).ToList(),
                sorters: ParseSorters(this.Sort).Select(x => x.ToISorter()).ToList());
        }

        /// <summary>
        /// Parses a list of filters given the json encoded filters.
        /// </summary>
        /// <param name="filterStrings">The list of json encoded filters.</param>
        /// <returns>The list of filter binding models.</returns>
        public List<FilterBindingModel> ParseFilters(List<string> filterStrings)
        {
            var list = new List<FilterBindingModel>();
            var converter = new FilterBindingModelConverter();
            filterStrings.ForEach(x =>
            {
                list.Add(JsonConvert.DeserializeObject<FilterBindingModel>(x, converter));
            });
            return list;
        }

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
            sb.AppendFormat("Start:  {0}, Limit:  {1}:  Filters:[{2}], Sorters:  [{3}]",
                this.Start,
                this.Limit,
                String.Join(", ", this.Filter != null ? this.Filter : new List<string>()),
                String.Join(", ", this.Sort != null ? this.Sort : new List<string>()));
            return sb.ToString();
        }
    }
}