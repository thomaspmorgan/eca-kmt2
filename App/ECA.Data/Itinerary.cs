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
    public class Itinerary : IHistorical
    {
        /// <summary>
        /// The max length of the description.
        /// </summary>
        public const int NAME_MAX_LENGTH = 100;

        /// <summary>
        /// Creates a new default instance and initializes the history.
        /// </summary>
        public Itinerary()
        {
            this.History = new History();
        }

        /// <summary>
        /// Gets or sets the itinerary id.
        /// </summary>
        [Key]
        public int ItineraryId { get; set; }

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
        /// Gets or sets the intinerary stops.
        /// </summary>
        [Required]
        public virtual ICollection<ItineraryStop> Stops { get; set; }

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        public DateTimeOffset StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        public DateTimeOffset EndDate { get; set; }

        /// <summary>
        /// Gets or sets the history.
        /// </summary>
        public History History { get; set; }

        /// <summary>
        /// Gets or sets the project.
        /// </summary>
        public virtual Project Project { get; set; }

        /// <summary>
        /// Gets or sets the project id.
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// Gets or sets the arrival destination location id of this itinerary.
        /// </summary>
        [Column("Arrival_LocationId")]
        public int? ArrivalLocationId { get; set; }

        /// <summary>
        /// Gets or sets the arrival destination of this itinerary.
        /// </summary>
        [ForeignKey("ArrivalLocationId")]
        public virtual Location Arrival { get; set; }

        /// <summary>
        /// Gets or sets the departure destination location id of this itinerary.
        /// </summary>
        [Column("Departure_LocationId")]
        public int? DepartureLocationId { get; set; }

        /// <summary>
        /// Gets or sets the departure destination of this itinerary.
        /// </summary>
        [ForeignKey("DepartureLocationId")]
        public virtual Location Departure { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        [Required]
        [MaxLength(NAME_MAX_LENGTH)]
        public string Name { get; set; }
    }
}
