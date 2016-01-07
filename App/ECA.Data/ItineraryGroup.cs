using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Data
{
    /// <summary>
    /// An ItineraryGroup is used to group participants in a logical set to track their movement through an itinerary.
    /// </summary>
    public class ItineraryGroup : IHistorical
    {
        /// <summary>
        /// The max length of an itinerary group name.
        /// </summary>
        public const int NAME_MAX_LENGTH = 255;

        /// <summary>
        /// Creates a new default instance.
        /// </summary>
        public ItineraryGroup()
        {
            this.History = new History();
            this.Participants = new HashSet<Participant>();
        }
        
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int ItineraryGroupId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [Required]
        [MaxLength(NAME_MAX_LENGTH)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the history.
        /// </summary>
        public History History { get; set; }

        /// <summary>
        /// Gets or sets the itinerary id.
        /// </summary>
        [Column("ItineraryId")]
        public int ItineraryId { get; set; }

        /// <summary>
        /// Gets or sets the itinerary.
        /// </summary>
        [ForeignKey("ItineraryId")]
        public virtual Itinerary Itinerary { get; set; }

        /// <summary>
        /// Gets or sets the participants.
        /// </summary>
        public virtual ICollection<Participant> Participants { get; set; }

        /// <summary>
        /// Gets or sets the itinerary stops.
        /// </summary>
        public virtual ICollection<ItineraryStop> Stops { get; set; }
    }
}
