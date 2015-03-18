
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
}
