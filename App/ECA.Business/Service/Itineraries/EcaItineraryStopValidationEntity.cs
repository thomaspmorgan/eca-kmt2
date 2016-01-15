using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Itineraries
{
    public class EcaItineraryStopValidationEntity
    {
        /// <summary>
        /// Creates a new AddedEcaItineraryStopValidationEntity with the given validation data.
        /// </summary>
        /// <param name="itineraryStartDate">The start date of the itinerary.</param>
        /// <param name="itineraryEndDate">The end date of the itinerary.</param>
        /// <param name="itineraryStopArrivalDate">The itinerary stop arrival date.</param>
        /// <param name="itineraryStopDepartureDate">The itinerary stop departure date.</param>
        public EcaItineraryStopValidationEntity(
            DateTimeOffset itineraryStartDate,
            DateTimeOffset itineraryEndDate,
            DateTimeOffset itineraryStopArrivalDate,
            DateTimeOffset itineraryStopDepartureDate,
            string timezoneId
            )
        {
            this.ItineraryEndDate = itineraryEndDate;
            this.ItineraryStartDate = itineraryStartDate;
            this.ItineraryStopArrivalDate = itineraryStopArrivalDate;
            this.ItineraryStopDepartureDate = itineraryStopDepartureDate;
            this.TimezoneId = timezoneId;
        }

        /// <summary>
        /// Gets or sets the itinerary state date.
        /// </summary>
        public DateTimeOffset ItineraryStartDate { get; private set; }

        /// <summary>
        /// Gets or sets the itinerary end date.
        /// </summary>
        public DateTimeOffset ItineraryEndDate { get; private set; }

        /// <summary>
        /// Gets or sets the itinerary stop arrival date.
        /// </summary>
        public DateTimeOffset ItineraryStopArrivalDate { get; private set; }

        /// <summary>
        /// Gets or sets the itinerary stop end date.
        /// </summary>
        public DateTimeOffset ItineraryStopDepartureDate { get; private set; }

        /// <summary>
        /// Gets the timezone id.
        /// </summary>
        public string TimezoneId { get; private set; }
    }
}
