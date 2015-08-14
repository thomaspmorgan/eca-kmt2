using ECA.Core.Exceptions;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Admin
{
    /// <summary>
    /// A EcaLocation is a business layer representation of a new or updated eca data location.
    /// </summary>
    public class EcaLocation : IAuditable
    {
        /// <summary>
        /// A ProjectLocation is a "Place" location that contains an optional city, a country and optional longitude and latitude.
        /// </summary>
        /// <param name="locationName">The name of the location.</param>
        /// <param name="cityId">The id of the city.</param>
        /// <param name="countryId">The id of the country.</param>
        /// <param name="divisionId">The id of the division.</param>
        /// <param name="locationTypeId">The location type id.</param>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitude">The longitude.</param>
        public EcaLocation(
            string locationName,
            int? cityId,
            int? countryId,
            int? divisionId,
            float? latitude,
            float? longitude,
            int locationTypeId
            )
        {
            if (LocationType.GetStaticLookup(locationTypeId) == null)
            {
                throw new UnknownStaticLookupException(String.Format("The location type id [{0}] is not known.", locationTypeId));
            }
            this.LocationName = locationName;
            this.CityId = cityId;
            this.DivisionId = divisionId;
            this.CountryId = countryId;
            this.Latitude = latitude;
            this.Longitude = longitude;
            this.LocationTypeId = locationTypeId;
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
        public int? CountryId { get; private set; }

        /// <summary>
        /// Gets or sets the division id.
        /// </summary>
        public int? DivisionId { get; private set; }

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
