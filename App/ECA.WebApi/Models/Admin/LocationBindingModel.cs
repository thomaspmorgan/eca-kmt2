using ECA.Business.Service;
using ECA.Business.Service.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Models.Admin
{
    /// <summary>
    /// A LocationBindingModel is used to represent's a physical location to the ECA system.
    /// </summary>
    public class LocationBindingModel
    {
        /// <summary>
        /// The latitude of the location.
        /// </summary>
        public float? Latitude { get; set; }

        /// <summary>
        /// The longitude.
        /// </summary>
        public float? Longitude { get; set; }

        /// <summary>
        /// The id of the city.
        /// </summary>
        public int? CityId { get; set; }

        /// <summary>
        /// The id of the division.
        /// </summary>
        public int? DivisionId { get; set; }

        /// <summary>
        /// The id of the country.
        /// </summary>
        public int? CountryId { get; set; }

        /// <summary>
        /// The id of the region.
        /// </summary>
        public int? RegionId { get; set; }

        /// <summary>
        /// The name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The location type id.
        /// </summary>
        public int LocationTypeId { get; set; }

        /// <summary>
        /// Returns the AdditionalLocation to be used in the business layer.
        /// </summary>
        /// <param name="businessUser">The user creating a location.</param>
        /// <returns>The business entity.</returns>
        public AdditionalLocation ToAdditionalLocation(User businessUser)
        {
            return new AdditionalLocation(
                creator: businessUser,
                locationName: this.Name,
                cityId: this.CityId,
                countryId: this.CountryId,
                divisionId: this.DivisionId,
                regionId: this.RegionId,
                latitude: this.Latitude,
                longitude: this.Longitude,
                locationTypeId: this.LocationTypeId
                );
        }
    }
}