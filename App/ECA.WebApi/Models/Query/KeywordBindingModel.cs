using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Filter;
using ECA.Core.DynamicLinq.Sorter;
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
    /// <summary>
    /// A KeywordBindingModel allows a query to return objects that will be filtered by keyword.  Be sure to set the properties that are filterable.
    /// </summary>
    /// <typeparam name="T">The object that will be filtered on.</typeparam>
    public class KeywordBindingModel<T> : PagingQueryBindingModel where T : class
    {
        private List<Expression<Func<T, object>>> propertySelectors;

        /// <summary>
        /// Creates a new default instance and intializes the Filter and Sort properties.
        /// </summary>
        public KeywordBindingModel()
        {
            this.Keyword = new List<string>();
        }

        /// <summary>
        /// The keywords.
        /// </summary>
        [Keyword(KeywordFilter<T>.MAX_KEYWORDS_COUNT, KeywordFilter<T>.MAX_KEYWORD_CHARACTER_COUNT)]
        public List<string> Keyword { get; set; }

        /// <summary>
        /// Sets the properties that can be filtered for keywords.
        /// </summary>
        /// <param name="propertySelectors">The expressions to select properties that can be filtered on keywords.</param>
        public void SetPropertiesToFilter(params Expression<Func<T, object>>[] propertySelectors)
        {
            var count = propertySelectors.Count();
            if (count == 0)
            {
                throw new NotSupportedException("There must be at least one property to filter on.");
            }
            if (count > KeywordFilter<T>.MAX_PROPERTIES_COUNT)
            {
                throw new NotSupportedException("The number of properties to filter on exceeds the max.");
            }

            this.propertySelectors = propertySelectors.ToList();
        }

        /// <summary>
        /// Returns a formatted string of this model.
        /// </summary>
        /// <returns>A formatted string of this model.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("Start:  {0}, Limit:  {1}:  Keyword:  [{2}], Sorters:  [{3}]",
                this.Start,
                this.Limit,
                String.Join(", ", this.Keyword != null ? this.Keyword : new List<string>()),
                String.Join(", ", this.Sort != null ? this.Sort : new List<string>()));
            return sb.ToString();
        }

        /// <summary>
        /// Returns a keyword filter from the given keywords and properties to filter on.
        /// </summary>
        /// <returns>The keyword filter.</returns>
        public override List<IFilter> GetFilters()
        {
            if (this.propertySelectors == null)
            {
                throw new NotSupportedException("The property selectors have not been initialized, be sure to call SetPropertiesToFilter.");
            }
            var keywordFilter = new SimpleKeywordFilter<T>(this.Keyword, this.propertySelectors.ToArray());
            return new List<IFilter> { keywordFilter };
        }
    }
}