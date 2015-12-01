using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ECA.Data
{
    /// <summary>
    /// An accommodation is a place of lodging in which a participant stays during the course of an itinerary stop (including hotels, host families and other accommodations).
    /// </summary>
    public class Accommodation
    {
        /// <summary>
        /// The Id of the Accommodation
        /// </summary>
        [Key]
        public int AccommodationId { get; set; }
        /// <summary>
        /// The host of the Accommodation (an Org)
        /// </summary>
        [Required]
        public virtual Organization Host { get; set; }
        /// <summary>
        /// The checkin date and time
        /// </summary>
        [Required]
        public DateTimeOffset CheckIn { get; set; }
        /// <summary>
        /// The checkout date and time
        /// </summary>
        [Required]
        public DateTimeOffset CheckOut { get; set; }
        /// <summary>
        /// The external id of the accomodation record
        /// </summary>
        public string RecordLocator { get; set; }
        /// <summary>
        /// The list of expenses at this accomodation
        /// </summary>
        public virtual ICollection<MoneyFlow> RecipientAccommodationExpenses { get; set; }

        /// <summary>
        /// The list of stops for this accomodation
        /// </summary>
        // relations
        public virtual ICollection<ItineraryStop> ItineraryStops { get; set; }

        /// <summary>
        /// History info
        /// </summary>
        public History History { get; set; }

    }
}
