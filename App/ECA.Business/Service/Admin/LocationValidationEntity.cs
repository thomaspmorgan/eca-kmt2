using ECA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Admin
{
    /// <summary>
    /// The LocationValidationEntity is used to validate new or updated locations in the eca system.
    /// </summary>
    public class LocationValidationEntity
    {
        /// <summary>
        /// Creates a new LocationValidationEntity.
        /// </summary>
        /// <param name="location">The location containing new or updated location details.</param>
        /// <param name="contextCountry">The country the location belongs to.</param>
        /// <param name="contextDivision">The division the location belongs to.</param>
        /// <param name="contextCity">The city the location belongs to.</param>
        public LocationValidationEntity(EcaLocation location, Location contextCountry, Location contextDivision, Location contextCity)
        {
            this.LocationName = location.LocationName;
            this.LocationTypeId = location.LocationTypeId;
            this.Latitude = location.Latitude;
            this.Longitude = location.Longitude;
            this.Country = contextCountry;
            this.Division = contextDivision;
            this.City = contextCity;
        }

        /// <summary>
        /// Gets the location name.
        /// </summary>
        public string LocationName { get; private set; }

        /// <summary>
        /// Gets the location type id.
        /// </summary>
        public int LocationTypeId { get; private set; }

        /// <summary>
        /// Gets the latitude.
        /// </summary>
        public float? Latitude { get; private set; }

        /// <summary>
        /// Gets the longitude.
        /// </summary>
        public float? Longitude { get; private set; }

        /// <summary>
        /// Gets the country.
        /// </summary>
        public Location Country { get; private set; }

        /// <summary>
        /// Gets the division.
        /// </summary>
        public Location Division { get; private set; }

        /// <summary>
        /// Gets the city.
        /// </summary>
        public Location City { get; private set; }
    }
}
