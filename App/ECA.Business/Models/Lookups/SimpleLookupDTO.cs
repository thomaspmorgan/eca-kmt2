namespace ECA.Business.Models.Lookups
{
    /// <summary>
    /// A SimpleLookupDTO is a class that holds a value and an id.
    /// </summary>
    public class SimpleLookupDTO : ILookup
    {
        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public string Value { get; set; }
    }
}
