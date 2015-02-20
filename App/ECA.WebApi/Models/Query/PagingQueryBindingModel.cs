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
            this.Filter = new List<FilterBindingModel>();
            this.Sort = new List<SorterBindingModel>();
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
        public List<FilterBindingModel> Filter { get; set; }

        /// <summary>
        /// Gets or sets the Sorters.
        /// </summary>
        public List<SorterBindingModel> Sort { get; set; }

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
                filters: this.Filter.Select(x => x.ToIFilter()).ToList(),
                sorters: this.Sort.Select(x => x.ToISorter()).ToList());
        }
    }

    /// <summary>
    /// A ModelBinder for the PagingQueryBindingModel which can be used for Get actions.
    /// </summary>
    public class PagingQueryBindingModelBinder : System.Web.Http.ModelBinding.IModelBinder
    {

        private const string FILTER_QUERY_KEY = "filter";

        private const string SORTER_QUERY_KEY = "sort";

        private const string START_QUERY_KEY = "start";

        private const string LIMIT_QUERY_KEY = "limit";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionContext"></param>
        /// <param name="bindingContext"></param>
        /// <returns></returns>
        public bool BindModel(System.Web.Http.Controllers.HttpActionContext actionContext, System.Web.Http.ModelBinding.ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelType != typeof(PagingQueryBindingModel))
            {
                return false;
            }
            var model = new PagingQueryBindingModel();
            var queryNameValuePairs = actionContext.Request.GetQueryNameValuePairs().ToDictionary(x => x.Key, x => x.Value);
            if (queryNameValuePairs.ContainsKey(FILTER_QUERY_KEY))
            {
                var filterValue = queryNameValuePairs[FILTER_QUERY_KEY];
                try
                {
                    var filters = ParseFilters(filterValue);
                    model.Filter = filters;
                }
                catch (Exception)
                {
                    bindingContext.ModelState.AddModelError(bindingContext.ModelName, "Unable to parse filters.  The filter must be a valid json string.");
                    return false;
                }
            }

            if (queryNameValuePairs.ContainsKey(SORTER_QUERY_KEY))
            {
                var sorterValue = queryNameValuePairs[SORTER_QUERY_KEY];
                try
                {
                    var sorters = ParseSorters(sorterValue);
                    model.Sort = sorters;
                }
                catch (Exception)
                {
                    bindingContext.ModelState.AddModelError(bindingContext.ModelName, "Unable to parse sorters.  The sort must be a valid json string.");
                    return false;
                }
            }
            if (!queryNameValuePairs.ContainsKey(START_QUERY_KEY))
            {
                bindingContext.ModelState.AddModelError(bindingContext.ModelName, "The start parameter was not given.  It must be a numeric value.");
                return false;
            }
            else
            {
                var success = DoStartValueParse(queryNameValuePairs[START_QUERY_KEY], bindingContext, model);
                if (!success)
                {
                    return false;
                }
            }

            if (!queryNameValuePairs.ContainsKey(LIMIT_QUERY_KEY))
            {
                bindingContext.ModelState.AddModelError(bindingContext.ModelName, "The limit parameter was not given.  It must be a numeric value.");
                return false;
            }
            else
            {
                var success = DoLimitValueParse(queryNameValuePairs[LIMIT_QUERY_KEY], bindingContext, model);
                if (!success)
                {
                    return false;
                }
            }
            bindingContext.Model = model;
            return true;
        }

        private bool DoStartValueParse(string startValueAsString, System.Web.Http.ModelBinding.ModelBindingContext bindingContext, PagingQueryBindingModel model)
        {
            int start;
            bool success = Int32.TryParse(startValueAsString, out start);
            if (success)
            {
                if (start < 0)
                {
                    bindingContext.ModelState.AddModelError(bindingContext.ModelName, "The start parameter must be greater than or equal to zero.");
                    return false;
                }
                model.Start = start;
            }
            else
            {
                bindingContext.ModelState.AddModelError(bindingContext.ModelName, "The start parameter not a valid integer.");
                return false;
            }
            return true;
        }

        private bool DoLimitValueParse(string limitValueAsString, System.Web.Http.ModelBinding.ModelBindingContext bindingContext, PagingQueryBindingModel model)
        {
            int limit;
            bool success = Int32.TryParse(limitValueAsString, out limit);
            if (success)
            {
                if (limit < 0 || limit > PagingQueryBindingModel.MAX_LIMIT)
                {
                    bindingContext.ModelState.AddModelError(bindingContext.ModelName, String.Format("The limit must be between 1 and {0}.", PagingQueryBindingModel.MAX_LIMIT));
                    return false;
                }
                model.Limit = limit;
            }
            else
            {
                bindingContext.ModelState.AddModelError(bindingContext.ModelName, "The start parameter not a valid integer.");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Returns a collection of IFilters from the given filter as a string.  If
        /// the string is null an empty list is returned.
        /// </summary>
        /// <param name="filter">The json array of simple filters.</param>
        /// <returns>A list of filters from the given string.</returns>
        public List<FilterBindingModel> ParseFilters(string filter)
        {
            List<FilterBindingModel> filters = new List<FilterBindingModel>();
            if (filter != null)
            {
                var parsedFilters = Newtonsoft.Json.JsonConvert.DeserializeObject<List<FilterBindingModel>>(filter);
                filters.AddRange(parsedFilters);
            }
            return filters;
        }

        /// <summary>
        /// Returns a collection of ISorters form the given sort as a string.  If
        /// the string is null an empty list is returned.
        /// </summary>
        /// <param name="sort">The json array of sorters.</param>
        /// <returns>The list of sorters.</returns>
        public List<SorterBindingModel> ParseSorters(string sort)
        {
            List<SorterBindingModel> sorters = new List<SorterBindingModel>();
            if (sort != null)
            {
                var parsedSorters = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SorterBindingModel>>(sort);
                sorters.AddRange(parsedSorters);
            }
            return sorters;
        }
    }
}