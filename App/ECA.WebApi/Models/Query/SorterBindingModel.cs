using ECA.Core.DynamicLinq.Sorter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Models.Query
{
    /// <summary>
    /// A SorterBindingModel is used when a client asks to sort a list of objects.
    /// </summary>
    public class SorterBindingModel
    {
        /// <summary>
        /// Gets or sets the property to sort on.
        /// </summary>
        public string Property { get; set; }

        /// <summary>
        /// Gets or sets the direction to sort.
        /// </summary>
        public string Direction { get; set; }

        /// <summary>
        /// Returns an ISorter of this instance.
        /// </summary>
        /// <returns>An ISorter instance.</returns>
        public ISorter ToISorter()
        {
            return new SimpleSorter
            {
                Direction = this.Direction,
                Property = this.Property
            };
        }

        /// <summary>
        /// Returns a formatted string of this object.
        /// </summary>
        /// <returns>Aformatted string of this object.</returns>
        public override string ToString()
        {
            return String.Format("Property:  {0}, Direction: {1}", this.Property, this.Direction);
        }
    }
}