using ECA.Business.Queries.Models.Itineraries;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Itineraries
{
    /// <summary>
    /// An AddedEcaItineraryGroupValidationEntity is used to validate a created itinerary group.
    /// </summary>
    public class AddedEcaItineraryGroupValidationEntity
    {
        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="participants">The participants.</param>
        /// <param name="existingItineraryGroups">The collection of itinerary groups that already exist for the added itinerary group.</param>
        public AddedEcaItineraryGroupValidationEntity(IEnumerable<Participant> participants, IEnumerable<ItineraryGroupDTO> existingItineraryGroups)
        {
            this.Participants = participants;
            this.ExistingItineraryGroups = existingItineraryGroups;
        }

        /// <summary>
        /// Gets the added eca itinerary group participants.
        /// </summary>
        public IEnumerable<Participant> Participants { get; private set; }

        /// <summary>
        /// Gets the itinerary groups that already exist for the itinerary.
        /// </summary>
        public IEnumerable<ItineraryGroupDTO> ExistingItineraryGroups { get; private set; }
    }
}
