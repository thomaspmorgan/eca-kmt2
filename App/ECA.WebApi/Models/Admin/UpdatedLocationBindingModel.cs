using ECA.Business.Service;
using ECA.Business.Service.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Models.Admin
{
    /// <summary>
    /// An UpdateLocationBindingModel represents a client's request to update a location in the ECA system.
    /// </summary>
    public class UpdatedLocationBindingModel : LocationBindingModel
    {
        /// <summary>
        /// The id of the location.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Returns a UpdatedLocation business entity.
        /// </summary>
        /// <param name="user">The user perforning the update.</param>
        /// <returns>The business entity.</returns>
        public UpdatedLocation ToUpdatedLocation(User user)
        {
            return new UpdatedLocation(
                updator: user,
                locationName: this.Name,
                cityId: this.CityId,
                countryId: this.CountryId,
                divisionId: this.DivisionId,
                regionId: this.RegionId,
                latitude: this.Latitude,
                longitude: this.Longitude,
                locationId: this.Id,
                locationTypeId: this.LocationTypeId
                );
        }
    }
}