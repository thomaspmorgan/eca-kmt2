using ECA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Itineraries
{
    public class AddedEcaItineraryValidationEntity : EcaItineraryValidationEntity
    {
        public AddedEcaItineraryValidationEntity(AddedEcaItinerary addedEcaItinerary, Project project, Location arrivalLocation, Location departureLocation)
            : base(
                  arrivalLocation: arrivalLocation,
                  departureLocation: departureLocation)
        {
            this.Project = project;
            this.AddedItinerary = addedEcaItinerary;
        }

        public AddedEcaItinerary AddedItinerary { get; private set; }

        public Project Project { get; private set; }
    }
}
