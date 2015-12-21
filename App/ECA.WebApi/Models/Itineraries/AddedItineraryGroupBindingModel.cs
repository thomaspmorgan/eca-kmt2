using ECA.Business.Service;
using ECA.Business.Service.Itineraries;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Models.Itineraries
{
    /// <summary>
    /// An AddedItineraryGroupBindingModel represents a client's request to add an itinerary group to the system.
    /// </summary>
    public class AddedItineraryGroupBindingModel
    {
        /// <summary>
        /// The participants by id to add to the itinerary group.
        /// </summary>
        public IEnumerable<int> ParticipantIds { get; set; }

        /// <summary>
        /// The name of the group.
        /// </summary>
        [Required]
        [MaxLength(ItineraryGroup.NAME_MAX_LENGTH)]
        public string Name { get; set; }

        /// <summary>
        /// Returns a business layer AddedEcaItineraryGroup instance to add a new itinerary group.
        /// </summary>
        /// <param name="creator">The user creating the itinerary group.</param>
        /// <param name="projectId">The project by id.</param>
        /// <param name="itineraryId">The itinerary by id that is having a new group added.</param>
        /// <returns>The business layer model instance.</returns>
        public AddedEcaItineraryGroup ToAddedEcaItinerary(User creator, int projectId, int itineraryId)
        {
            return new AddedEcaItineraryGroup(creator, projectId, itineraryId, this.Name, this.ParticipantIds);
        }
    }
}