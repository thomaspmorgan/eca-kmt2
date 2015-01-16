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
    /// An event is a happening involving participants and/or alumni who have completed a project.
    /// </summary>
    public class Event
    {
        [Key]
        public int EventId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public virtual EventType EventType { get; set; }
        [Required]
        public int EventTypeId { get; set; }
        public DateTimeOffset EventDate { get; set; }
        public virtual Location Location { get; set; }
        public int LocationId { get; set; }
        public string Description { get; set; }
        public string TargetAudience { get; set; }
        public int EsimatedAudienceSize { get; set; }
        public int EsimatedNumberOfAlumni { get; set; }
        public virtual ICollection<Person> Participants { get; set; }
        public virtual ICollection<Project> Projects { get; set; }
        public virtual ICollection<Actor> Actors { get; set; }
        public virtual ICollection<Artifact> Artifacts { get; set; }

        public History History { get; set; }
    }

}
