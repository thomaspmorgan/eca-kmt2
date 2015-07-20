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

        /// <summary>
        /// Gets or sets the region id
        /// </summary>
        public int? RegionId { get; set; }

        /// <summary>
        /// Gets or sets the country id
        /// </summary>
        public int? CountryId { get; set; }

        /// <summary>
        /// Gets or sets the Region.
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// Gets or sets the country.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets the division id.
        /// </summary>
        public int? DivisionId { get; set; }
        
        /// <summary>
        /// Gets or sets the division.
        /// </summary>
        public string Division { get; set; }

        /// <summary>
        /// Gets or sets the city id.
        /// </summary>
        public int? CityId { get; set; }

        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        public string City { get; set; }
    }
}
