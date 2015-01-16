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
    /// Actors are organizations or participants who must perform specific actions during the course of a project phase.
    /// </summary>
    public class Actor
    {
        [Key]
        public int ActorId { get; set; }
        [Required]
        public virtual ActorType ActorType { get; set; }
        [Required]
        public string ActorName { get; set; }
        public string Status { get; set; }
        public string Action { get; set; }
        // relationships
        public virtual Person Person { get; set; }
        public int? PersonId { get; set; }
        public virtual Organization Organization { get; set; }
        public int? OrganizationId { get; set; }
        public virtual Event Event { get; set; }
        public int? EventId { get; set; }
        public virtual ItineraryStop ItineraryStop { get; set; }
        public int? ItineraryStopId { get; set; }

        public History History { get; set; }
    }
}
