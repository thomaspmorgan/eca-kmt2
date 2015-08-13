using ECA.Business.Service;
using ECA.Business.Service.Projects;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Models.Projects
{
    /// <summary>
    /// A ProjectLocationBindingModel represents a client's request to add a location to a project.
    /// </summary>
    public class ProjectLocationBindingModel
    {
        /// <summary>
        /// The id of the project.
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// Gets the location name.
        /// </summary>
        [MaxLength(Location.NAME_MAX_LENGTH)]
        public string LocationName { get; set; }

        /// <summary>
        /// Gets the city id.
        /// </summary>
        public int? CityId { get; set; }

        /// <summary>
        /// Gets the country id.
        /// </summary>
        public int CountryId { get; set; }

        /// <summary>
        /// Gets the latitude.
        /// </summary>
        public float? Latitude { get; set; }

        /// <summary>
        /// Gets the longitude.
        /// </summary>
        public float? Longitude { get; set; }

        /// <summary>
        /// Returns a business entity to be used when creating the project location.
        /// </summary>
        /// <param name="creator">The user creating the project location.</param>
        /// <returns>The additional project location.</returns>
        public AdditionalProjectLocation ToAdditionalProjectLocation(User creator)
        {
            return new AdditionalProjectLocation(
                creator: creator,
                locationName: this.LocationName,
                cityId: this.CityId,
                countryId: this.CountryId,
                latitude: this.Latitude,
                longitude: this.Longitude,
                projectId: this.ProjectId
                );
        }

    }
}