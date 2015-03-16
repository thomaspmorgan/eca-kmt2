using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Filter;
using ECA.Core.DynamicLinq.Sorter;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;

namespace ECA.WebApi.Models.Query
{
    public class PagingQueryBindingModel<T> where T : class
    {
        /// <summary>
        /// Gets the maximum number of results a paged request can have.
        /// </summary>
        public const int MAX_LIMIT = 300;

        private Expression<Func<T, string>>[] propertySelectors;

        /// <summary>
        /// Creates a new PagingQueryBindingModel and initializes keyword and filter properties.
        /// </summary>
        public PagingQueryBindingModel()
        {
            this.Keyword = new List<string>();
            this.Filter = new List<string>();
        }

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
        /// The keywords.
        /// </summary>
        [Keyword(KeywordFilter<T>.MAX_KEYWORDS_COUNT, KeywordFilter<T>.MAX_KEYWORD_LENGTH)]
        public List<string> Keyword { get; set; }

        /// <summary>
        /// Gets or sets the Filters.
        /// </summary>
        public List<string> Filter { get; set; }

        /// <summary>
        /// Returns a QueryableOperator on type T to page, filter, and sort a query.
        /// </summary>
        /// <param name="defaultSorter">The default sorter of the query.</param>
        /// <param name="propertySelectors">The properties to allow keyword searches on.</param>
        /// <returns>The query operator to apply to an IQueryable.</returns>
        public QueryableOperator<T> ToQueryableOperator(ISorter defaultSorter, params Expression<Func<T, string>>[] propertySelectors)
        {
            Contract.Requires(defaultSorter != null, "The default sorter must not be null.");
            if ((propertySelectors == null || propertySelectors.Count() == 0) && this.Keyword != null && this.Keyword.Count > 0)
            {
                throw new ArgumentException("The QueryableOperator must have keyword search properties selected in order to perform a keyword search.");
            }
            if (propertySelectors.Count() > KeywordFilter<T>.MAX_PROPERTIES_COUNT)
            {
                throw new ArgumentException("The number of properties to keyword search on exceeds the max.");
            }
            if (propertySelectors != null && propertySelectors.Count() > 0)
            {
                SetPropertiesToSearch(propertySelectors);
            }
            return new QueryableOperator<T>(
                start: this.Start,
                limit: this.Limit,
                defaultSorter: defaultSorter,
                filters: GetFilters(),
                sorters: ParseSorters(this.Sort).Select(x => x.ToISorter()).ToList());
        }

        /// <summary>
        /// Returns the filters for this model.
        /// </summary>
        /// <returns>The filters for this model.</returns>
        public IList<IFilter> GetFilters()
        {
            var filters = new List<IFilter>();
            if (this.Keyword != null && this.Keyword.Count > 0)
            {
                filters.Add(GetKeywordFilter());
            }
            if (this.Filter != null && this.Filter.Count > 0)
            {
                filters.AddRange(ParseFilters(this.Filter).Select(x => x.ToIFilter()).ToList());
            }
            return filters;
        }

        /// <summary>
        /// Returns a keyword filter from the given keywords and properties to filter on.
        /// </summary>
        /// <returns>The keyword filter.</returns>
        public IFilter GetKeywordFilter()
        {
            if (this.propertySelectors == null)
            {
                throw new NotSupportedException("The property selectors have not been initialized, be sure to call SetPropertiesToFilter.");
            }
            var keywordsHashSet = new HashSet<string>();
            this.Keyword.ForEach(x => keywordsHashSet.Add(x));
            var keywordFilter = new SimpleKeywordFilter<T>(keywordsHashSet, this.propertySelectors);
            return keywordFilter;
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
        /// Returns a formatted string of this model.
        /// </summary>
        /// <returns>A formatted string of this model.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("Start:  {0}, Limit:  {1}:  Filters:  [{2}], Sorters:  [{3}], Keyword:  [{4}]",
                this.Start,
                this.Limit,
                String.Join(", ", this.Filter != null ? this.Filter : new List<string>()),
                String.Join(", ", this.Sort != null ? this.Sort : new List<string>()),
                String.Join(", ", this.Keyword != null ? this.Keyword : new List<string>()));
            return sb.ToString();
        }

        /// <summary>
        /// Sets the string properties that can be filtered for keywords.  The properties must be strings.
        /// </summary>
        /// <param name="propertySelectors">The expressions to select properties that can be filtered on keywords.</param>
        public void SetPropertiesToSearch(params Expression<Func<T, string>>[] propertySelectors)
        {
            if (propertySelectors == null)
            {
                throw new ArgumentNullException("propertySelectors");
            }
            var count = propertySelectors.Count();
            if (count == 0)
            {
                throw new ArgumentException("There must be at least one property to filter on.");
            }
            if (count > KeywordFilter<T>.MAX_PROPERTIES_COUNT)
            {
                throw new ArgumentException("The number of properties to filter on exceeds the max.");
            }

            this.propertySelectors = propertySelectors.Distinct().ToArray();
        }
    }
}