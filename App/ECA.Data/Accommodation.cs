using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECA.Data
{
    /// <summary>
    /// An accommodation is a place of lodging in which a participant stays during the course of an itinerary stop (including hotels, host families and other accommodations).
    /// </summary>
    public class Accommodation
    {
        [Key]
        public int AccommodationId { get; set; }
        [Required]
        public virtual Organization Host { get; set; }
        [Required]
        public DateTimeOffset CheckIn { get; set; }
        [Required]
        public DateTimeOffset CheckOut { get; set; }
        public string RecordLocator { get; set; }
        public virtual ICollection<MoneyFlow> RecipientAccommodationExpenses { get; set; }

        // relations
        public virtual ICollection<ItineraryStop> ItineraryStops { get; set; }

        public History History { get; set; }

    }
}
