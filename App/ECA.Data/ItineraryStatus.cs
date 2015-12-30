using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECA.Data
{
    /// <summary>
    /// Details the status of an itinerary.
    /// </summary>
    public partial class ItineraryStatus
    {
        /// <summary>
        /// Creates a new default instance and initializes a default instance.
        /// </summary>
        public ItineraryStatus()
        {
            this.History = new History();
        }

        /// <summary>
        /// Gets or sets the itinerary status id.
        /// </summary>
        [Key]
        public int ItineraryStatusId { get; set; }

        /// <summary>
        /// Gets or sets the itinerary status name.
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string ItineraryStatusName { get; set; }

        /// <summary>
        /// Gets or sets the history.
        /// </summary>
        public History History { get; set; }
    }
}
