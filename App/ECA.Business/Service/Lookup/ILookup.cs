
namespace ECA.Business.Service.Lookup
{
    /// <summary>
    /// An ILookup is a simple lookup with an Id and a Value.
    /// </summary>
    public interface ILookup
    {
        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        int Id { get; set; }

        /// <summary>
        /// Gets or sets the Value.
        /// </summary>
        string Value { get; set; }
    }
}
