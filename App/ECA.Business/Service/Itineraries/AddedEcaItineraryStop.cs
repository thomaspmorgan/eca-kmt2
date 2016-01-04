using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Itineraries
{
    public class AddedEcaItineraryStop : EcaItineraryStop, IAuditable
    {
        public AddedEcaItineraryStop(
            User creator,
            int itineraryId,
            int projectId,
            string name,
            DateTimeOffset arrivalDate,
            DateTimeOffset departureDate,
            int destinationLocationId
            ) : base(
                projectId: projectId,
                name: name,
                arrivalDate: arrivalDate,
                departureDate: departureDate,
                destinationLocationId: destinationLocationId
                )
        {
            this.Audit = new Create(creator);
            this.ItineraryId = itineraryId;
        }

        public Audit Audit { get; private set; }

        public int ItineraryId { get; private set; }
    }
}
