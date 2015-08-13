using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Projects
{
    /// <summary>
    /// An AdditionalProjectLocation represents a business layer's client's request to add a location to project.
    /// </summary>
    public class AdditionalProjectLocation : ProjectLocation
    {
        /// <summary>
        /// A ProjectLocation is a "Place" location that contains an optional city, a country and optional longitude and latitude.
        /// </summary>
        /// <param name="locationName">The name of the location.</param>
        /// <param name="cityId">The id of the city.</param>
        /// <param name="countryId">The id of the country.</param>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitude">The longitude.</param>
        /// <param name="projectId">The project id.</param>
        public AdditionalProjectLocation(
            User creator,
            string locationName,
            int? cityId,
            int countryId,
            float? latitude,
            float? longitude,
            int projectId
            )
            :
            base(
                locationName: locationName,
                cityId: cityId,
                countryId: countryId,
                longitude: longitude,
                latitude: latitude
                )
        {
            this.ProjectId = projectId;
            this.Audit = new Create(creator);
        }

        /// <summary>
        /// Gets the project id.
        /// </summary>
        public int ProjectId { get; private set; }
    }
}
