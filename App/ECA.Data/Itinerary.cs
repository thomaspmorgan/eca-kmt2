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
    /// Itineraries describe the travel participants take during a project.
    /// </summary>
    public class Itinerary
    {
        [Key]
        public int ItineraryId { get; set; }
        public ItineraryStatus ItineraryStatus { get; set; }
        [Required]
        public int ItineraryStatusId { get; set; }
        [Required]
        public virtual ICollection<ItineraryStop> Stops { get; set; }
        [Required]
        public virtual ICollection<ParticipantStatus> Participants { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }

        public History History { get; set; }

    }
}
