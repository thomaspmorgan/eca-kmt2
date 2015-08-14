using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Admin
{
    /// <summary>
    /// An UpdatedLocation represents a business layer's client's request to update an existing location.
    /// </summary>
    public class UpdatedLocation : EcaLocation
    {
        /// <summary>
        /// Creates a new UpdatedLocation and initializes the location properties.
        /// </summary>
        /// <param name="locationName">The name of the location.</param>
        /// <param name="cityId">The id of the city.</param>
        /// <param name="countryId">The id of the country.</param>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitude">The longitude.</param>
        /// <param name="projectId">The project the location belongs to.</param>
        /// <param name="locationId">The existing location id.</param>
        public UpdatedLocation(
            User updator,
            string locationName,
            int? cityId,
            int? countryId,
            int? divisionId,
            float? latitude,
            float? longitude,
            int locationTypeId,
            int locationId
            )
            : base(
                locationName: locationName,
                cityId: cityId,
                countryId: countryId,
                divisionId: divisionId,
                longitude: longitude,
                latitude: latitude,
                locationTypeId: locationTypeId
                )
        {
            this.LocationId = locationId;
            this.Audit = new Update(updator);
        }

        /// <summary>
        /// Gets the location id.
        /// </summary>
        public int LocationId { get; private set; }
    }
}
