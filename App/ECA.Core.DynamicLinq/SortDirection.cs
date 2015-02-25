using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Core.DynamicLinq
{
    /// <summary>
    /// A sort direction is used for ordering objects.
    /// </summary>
    public class SortDirection
    {
        private const string ASC = "ASC";

        private const string DESC = "DESC";

        /// <summary>
        /// Gets the ascending sort direction.
        /// </summary>
        public static SortDirection Ascending
        {
            get
            {
                return new SortDirection(ASC);
            }
        }

        /// <summary>
        /// Gets the descending sort direction.
        /// </summary>
        public static SortDirection Descending
        {
            get
            {
                return new SortDirection(DESC);
            }
        }

        private SortDirection(string value)
        {
            Debug.Assert(value != null, "The value must not be null.");
            this.Value = value;
        }

        /// <summary>
        /// Gets the value of the sort direction.
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        /// Returns true if the given object equals this object.
        /// </summary>
        /// <param name="obj">The object to test.</param>
        /// <returns>True if the given object equals this object.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            var otherType = obj as SortDirection;
            if (otherType == null)
            {
                return false;
            }
            return this.Value == otherType.Value;

        }

        /// <summary>
        /// Returns a hash of this object.
        /// </summary>
        /// <returns>A hash of this object.</returns>
        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }

        /// <summary>
        /// Returns true if the given instances are equal.
        /// </summary>
        /// <param name="a">The first instance.</param>
        /// <param name="b">The second instance.</param>
        /// <returns>True if the given instances are equal.</returns>
        public static bool operator ==(SortDirection a, SortDirection b)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            return a.Value == b.Value;
        }

        /// <summary>
        /// Returns false if the given instances are equal.
        /// </summary>
        /// <param name="a">The first instance.</param>
        /// <param name="b">The second instance.</param>
        /// <returns>False if the given instances are equal.</returns>
        public static bool operator !=(SortDirection a, SortDirection b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Returns a sort direction whose value is equal to the given string.
        /// </summary>
        /// <param name="direction">The sort direction as a string.</param>
        /// <returns>The sort direction.</returns>
        public static SortDirection ToSortDirection(string direction)
        {
            var dictionary = new Dictionary<string, SortDirection>();
            dictionary.Add(ASC.ToUpper(), SortDirection.Ascending);
            dictionary.Add(DESC.ToUpper(), SortDirection.Descending);
            var dir = direction.ToUpper().Trim();
            Contract.Assert(dictionary.ContainsKey(dir), String.Format("The sort type [{0}] is not recognized.", direction));
            return dictionary[dir];
        }
    }
}
