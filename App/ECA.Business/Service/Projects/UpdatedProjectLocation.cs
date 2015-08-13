using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Projects
{
    /// <summary>
    /// An UpdatedProjectLocation represents a business layer's client's request to update an existing project location.
    /// </summary>
    public class UpdatedProjectLocation : ProjectLocation
    {
        /// <summary>
        /// A ProjectLocation is a "Place" location that contains an optional city, a country and optional longitude and latitude.
        /// </summary>
        /// <param name="locationName">The name of the location.</param>
        /// <param name="cityId">The id of the city.</param>
        /// <param name="countryId">The id of the country.</param>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitude">The longitude.</param>
        /// <param name="projectId">The project the location belongs to.</param>
        /// <param name="locationId">The existing location id.</param>
        public UpdatedProjectLocation(
            User updator,
            string locationName,
            int? cityId,
            int countryId,
            float? latitude,
            float? longitude,
            int projectId,
            int locationId
            )
            : base(
                locationName: locationName,
                cityId: cityId,
                countryId: countryId,
                longitude: longitude,
                latitude: latitude
                )
        {
            this.LocationId = locationId;
            this.ProjectId = projectId;
            this.Audit = new Update(updator);
        }

        /// <summary>
        /// Gets the location id.
        /// </summary>
        public int LocationId { get; private set; }

        /// <summary>
        /// Gets the project id.
        /// </summary>
        public int ProjectId { get; private set; }

    }
}
