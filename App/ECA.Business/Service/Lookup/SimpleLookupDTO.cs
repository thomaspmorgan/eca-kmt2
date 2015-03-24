namespace ECA.Business.Service.Lookup
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
            var otherType = obj as SimpleLookupDTO;
            if (otherType == null)
            {
                return false;
            }
            return this.Id == otherType.Id && this.Value == otherType.Value;
        }

        /// <summary>
        /// Returns a hash of this object.
        /// </summary>
        /// <returns>A hash of this object.</returns>
        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }
    }
}
