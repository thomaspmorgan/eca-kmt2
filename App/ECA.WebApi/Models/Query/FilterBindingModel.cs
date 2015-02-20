using ECA.Core.DynamicLinq.Filter;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Http.ModelBinding;
using System.Web.Http.ValueProviders;

namespace ECA.WebApi.Models.Query
{
    /// <summary>
    /// The FilterBindingModel represents a filter the client can send to a web api action.
    /// </summary>
    public class FilterBindingModel
    {
        /// <summary>
        /// Gets or sets the Property to filter on.
        /// </summary>
        [Required]
        public string Property { get; set; }

        /// <summary>
        /// Gets or sets the value to filter with.
        /// </summary>
        [Required]
        public object Value { get; set; }

        /// <summary>
        /// Gets or sets the Comparison type.
        /// </summary>
        [Required]
        public string Comparison { get; set; }

        /// <summary>
        /// Returns an IFilter.
        /// </summary>
        /// <returns>An IFilter.</returns>
        public IFilter ToIFilter()
        {
            return new SimpleFilter
            {
                Comparison = this.Comparison,
                Property = this.Property,
                Value = this.Value
            };
        }
    }

    
}