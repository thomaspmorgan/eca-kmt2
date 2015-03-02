using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Core.DynamicLinq.Sorter
{
    /// <summary>
    /// A SimpleSorter is an ISorter that tracks the property to sort on by name and the direction to sort on.
    /// </summary>
    public class SimpleSorter : ISorter
    {
        /// <summary>
        /// Gets or sets the direction.
        /// </summary>
        public string Direction { get; set; }

        /// <summary>
        /// Gets or sets the Property.
        /// </summary>
        public string Property { get; set; }

        /// <summary>
        /// Returns a LinqSorter of this simple sorter.
        /// </summary>
        /// <typeparam name="T">The type to sort on.</typeparam>
        /// <returns>The linq sorter.</returns>
        public LinqSorter<T> ToLinqSorter<T>() where T : class
        {
            var direction = SortDirection.ToSortDirection(this.Direction);
            return new LinqSorter<T>(this.Property, direction);
        }

        /// <summary>
        /// Returns a string of this simple sorter.
        /// </summary>
        /// <returns>A string of this simple sorter.</returns>
        public override string ToString()
        {
            return String.Format("{0}:{1}", this.Property, SortDirection.ToSortDirection(this.Direction).Value);
        }
    }
}
