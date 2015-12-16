using ECA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Itineraries
{
    /// <summary>
    /// An AddedEcaItineraryValidationEntity is a validation entity used when creating a new itinerary.
    /// </summary>
    public class AddedEcaItineraryValidationEntity : EcaItineraryValidationEntity
    {
        /// <summary>
        /// An AddedEcaItineraryValidationEntity is used to validate an itinerary when being created.
        /// </summary>
        /// <param name="addedEcaItinerary">The new itinerary.</param>
        /// <param name="project">The project the itinerary belongs to.</param>
        /// <param name="arrivalLocation">The arrival location.</param>
        /// <param name="departureLocation">The departure destination location.</param>
        public AddedEcaItineraryValidationEntity(AddedEcaItinerary addedEcaItinerary, Project project, Location arrivalLocation, Location departureLocation)
            : base(
                  arrivalLocation: arrivalLocation,
                  departureLocation: departureLocation)
        {
            this.Project = project;
            this.AddedItinerary = addedEcaItinerary;
        }

        /// <summary>
        /// Gets the added itinerary.
        /// </summary>
        public AddedEcaItinerary AddedItinerary { get; private set; }

        /// <summary>
        /// Gets the itinerary project.
        /// </summary>
        public Project Project { get; private set; }
    }
}
