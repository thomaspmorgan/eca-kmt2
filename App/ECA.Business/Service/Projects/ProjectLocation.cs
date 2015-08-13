using ECA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Projects
{
    /// <summary>
    /// A ProjectLocation represents a client's project location specification.  The location type will be a place.
    /// </summary>
    public class ProjectLocation : IAuditable
    {
        /// <summary>
        /// A ProjectLocation is a "Place" location that contains an optional city, a country and optional longitude and latitude.
        /// </summary>
        /// <param name="locationName">The name of the location.</param>
        /// <param name="cityId">The id of the city.</param>
        /// <param name="countryId">The id of the country.</param>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitude">The longitude.</param>
        public ProjectLocation(
            string locationName,
            int? cityId,
            int countryId,
            float? latitude,
            float? longitude
            )
        {
            this.LocationName = locationName;
            this.CityId = cityId;
            this.CountryId = countryId;
            this.Latitude = latitude;
            this.Longitude = longitude;
            this.LocationTypeId = LocationType.Place.Id;
        }

        /// <summary>
        /// Gets the location name.
        /// </summary>
        public string LocationName { get; private set; }

        /// <summary>
        /// Gets the city id.
        /// </summary>
        public int? CityId { get; private set; }

        /// <summary>
        /// Gets the country id.
        /// </summary>
        public int CountryId { get; private set; }

        /// <summary>
        /// Gets the latitude.
        /// </summary>
        public float? Latitude { get; private set; }

        /// <summary>
        /// Gets the longitude.
        /// </summary>
        public float? Longitude { get; private set; }

        /// <summary>
        /// Gets the location type id.
        /// </summary>
        public int LocationTypeId { get; private set; }

        /// <summary>
        /// Gets the audit.
        /// </summary>
        public Audit Audit { get; protected set; }
    }
}
