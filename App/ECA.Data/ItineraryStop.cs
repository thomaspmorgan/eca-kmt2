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
    public class ItineraryStop : IHistorical
    {
        public const int NAME_MAX_LENGTH = 100;

        /// <summary>
        /// Creates a new default instance.
        /// </summary>
        public ItineraryStop()
        {
            this.History = new History();
            this.Participants = new HashSet<Participant>();
            this.Groups = new HashSet<ItineraryGroup>();
            this.SourceItineraryStopMoneyFlows = new HashSet<MoneyFlow>();
            this.RecipientItineraryStopMoneyFlows = new HashSet<MoneyFlow>();
        }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        [Key]
        public int ItineraryStopId { get; set; }

        /// <summary>
        /// Gets or sets the name of the itinerary stop.
        /// </summary>
        [Required]
        [MaxLength(NAME_MAX_LENGTH)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the itinerary status.
        /// </summary>
        public virtual ItineraryStatus ItineraryStatus { get; set; }

        /// <summary>
        /// Gets or sets the itinerary status id.
        /// </summary>
        [Required]
        public int ItineraryStatusId { get; set; }
        
        /// <summary>
        /// Gets or sets the destination lcoation.
        /// </summary>
        [ForeignKey("DestinationId")]
        public virtual Location Destination { get; set; }

        /// <summary>
        /// Gets or sets the destination location id.
        /// </summary>
        [Column("Destination_LocationId")]
        public int? DestinationId { get; set; }
        
        /// <summary>
        /// Gets or sets the arrival date.
        /// </summary>
        public DateTimeOffset DateArrive { get; set; }

        /// <summary>
        /// Gets or sets the departure date.
        /// </summary>
        public DateTimeOffset DateLeave { get; set; }

        /// <summary>
        /// Gets or sets the source money flows.
        /// </summary>
        public virtual ICollection<MoneyFlow> SourceItineraryStopMoneyFlows { get; set; }

        /// <summary>
        /// Gets or sets the recipient money flows.
        /// </summary>
        public virtual ICollection<MoneyFlow> RecipientItineraryStopMoneyFlows { get; set; }
        
        /// <summary>
        /// Gets or sets the itinerary.
        /// </summary>
        public virtual Itinerary Itinerary { get; set; }

        /// <summary>
        /// Gets or sets the itinerary id.
        /// </summary>
        public int ItineraryId { get; set; }

        /// <summary>
        /// Gets or sets the history.
        /// </summary>
        public History History { get; set; }

        /// <summary>
        /// Gets or sets the participants on the travel stop.
        /// </summary>
        public virtual ICollection<Participant> Participants { get; set; }

        /// <summary>
        /// Gets or sets the groups.
        /// </summary>
        public virtual ICollection<ItineraryGroup> Groups { get; set; }

        //public virtual ICollection<Transportation> Transportations { get; set; }
        //public virtual ICollection<Accommodation> Accommodations { get; set; }
        //public virtual ICollection<Course> Courses { get; set; }
        //public virtual ICollection<Material> Materials { get; set; }
        //public virtual ICollection<Actor> Actors { get; set; }

        //public virtual Location Origin { get; set; }
        //public virtual ICollection<Artifact> Artifacts { get; set; }
    }
}
