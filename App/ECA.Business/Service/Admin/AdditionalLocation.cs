using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Admin
{
    /// <summary>
    /// An AdditionalLocation represents a business layer's client's request to add a location to the eca system.
    /// </summary>
    public class AdditionalLocation : EcaLocation
    {
        /// <summary>
        /// Creates a new AdditionalLocation instance and initializes the properties.
        /// </summary>
        /// <param name="locationName">The name of the location.</param>
        /// <param name="cityId">The id of the city.</param>
        /// <param name="creator">The user creating the location.</param>
        /// <param name="divisionId">The id of the division.</param>
        /// <param name="locationTypeId">The location type id.</param>
        /// <param name="countryId">The id of the country.</param>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitude">The longitude.</param>
        public AdditionalLocation(
            User creator,
            string locationName,
            int? cityId,
            int? countryId,
            int? divisionId,
            float? latitude,
            float? longitude,
            int locationTypeId
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
            this.Audit = new Create(creator);
        }
    }
}
