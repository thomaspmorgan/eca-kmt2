namespace ECA.Business.Queries.Models.Admin
{
    /// <summary>
    /// A LocationDTO represents a location on the globe.
    /// </summary>
    public class LocationDTO
    {
        /// <summary>
        /// Gets or sets the Id of the location.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the Location Type Id.
        /// </summary>
        public int LocationTypeId { get; set; }

        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the type name.
        /// </summary>
        public string LocationTypeName { get; set; }
    }
}
