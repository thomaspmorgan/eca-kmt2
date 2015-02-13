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
    /// Itinerary stops describe specific travel locations, logistics and activities during the course of a project.
    /// </summary>
    public class ItineraryStop
    {
        [Key]
        public int ItineraryStopId { get; set; }
        [Required]
        public ItineraryStatus ItineraryStatus { get; set; }
        public virtual Location Origin { get; set; }
        public virtual Location Destination { get; set; }
        public virtual ICollection<Actor> Actors { get; set; }
        public virtual ICollection<Person> Participants { get; set; }
        public DateTimeOffset DateArrive { get; set; }
        public DateTimeOffset DateLeave { get; set; }
        public virtual ICollection<Transportation> Transportations { get; set; }
        public virtual ICollection<Accommodation> Accommodations { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
        public virtual ICollection<Material> Materials { get; set; }
        public ICollection<MoneyFlow> SourceItineraryStopMoneyFlows { get; set; }
        public ICollection<MoneyFlow> RecipientItineraryStopMoneyFlows { get; set; }
        public virtual ICollection<ParticipantStatus> ParticipantStatuses { get; set; }
        public virtual ICollection<Artifact> Artifacts { get; set; }

        // relations

        public virtual Itinerary Itinerary { get; set; }
        public int ItineraryId { get; set; }

        public History History { get; set; }

    }
}
