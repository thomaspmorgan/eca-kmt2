
using System;
using System.Diagnostics.Contracts;
namespace ECA.Core.Data
{
    /// <summary>
    /// An IConcurrent object that maintains a concurrency token.
    /// </summary>
    public interface IConcurrent
    {
        /// <summary>
        /// Gets or sets the row version.
        /// </summary>
        byte[] RowVersion { get; set; }
    }

    /// <summary>
    /// IConcurrentExtensions provide a simple means of retrieving and setting row version byte arrays to and from strings.
    /// </summary>
    public static class IConcurrentExtensions
    {
        /// <summary>
        /// Returns a string of the source concurrent object to a base 64 string.
        /// </summary>
        /// <param name="source">The concurrent object.</param>
        /// <returns>The row version as a string.</returns>
        public static string GetRowVersionAsString(this IConcurrent source)
        {
            Contract.Requires(source != null, "The source must not be null.");
            return Convert.ToBase64String(source.RowVersion);
        }

        /// <summary>
        /// Sets the row version on the given concurrent object with the given row version as a string.
        /// </summary>
        /// <param name="source">The source object.</param>
        /// <param name="rowVersionAsString">The row version as a string.</param>
        public static void SetRowVersion(this IConcurrent source, string rowVersionAsString)
        {
            Contract.Requires(source != null, "The source must not be null.");
            Contract.Requires(!String.IsNullOrWhiteSpace(rowVersionAsString), "The row version must have a value.");
            source.RowVersion = Convert.FromBase64String(rowVersionAsString);
        }
    }
}
