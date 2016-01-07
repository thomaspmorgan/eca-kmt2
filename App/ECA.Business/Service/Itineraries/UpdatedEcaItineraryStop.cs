using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Itineraries
{
    public class UpdatedEcaItineraryStop : EcaItineraryStop, IAuditable
    {
        public UpdatedEcaItineraryStop(
            User updator,
            int itineraryStopId,
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
            this.Audit = new Update(updator);
            this.ItineraryStopId = itineraryStopId;
        }

        public Audit Audit { get; private set; }

        public int ItineraryStopId { get; private set; }
    }
}
