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
    public class MultipleFilterBindingModel : PagingQueryBindingModel
    {
        /// <summary>
        /// Creates a new default instance and intializes the Filter and Sort properties.
        /// </summary>
        public MultipleFilterBindingModel()
        {
            this.Filter = new List<string>();
        }

        /// <summary>
        /// Gets or sets the Filters.
        /// </summary>
        public List<string> Filter { get; set; }

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
            sb.AppendFormat("Start:  {0}, Limit:  {1}:  Filters:  [{2}], Sorters:  [{3}]",
                this.Start,
                this.Limit,
                String.Join(", ", this.Filter != null ? this.Filter : new List<string>()),
                String.Join(", ", this.Sort != null ? this.Sort : new List<string>()));
            return sb.ToString();
        }

        /// <summary>
        /// Returns the given filters as IFilters.
        /// </summary>
        /// <returns>The list of parsed IFilters.</returns>
        public override List<IFilter> GetFilters()
        {
            return ParseFilters(this.Filter).Select(x => x.ToIFilter()).ToList();
        }
    }
}