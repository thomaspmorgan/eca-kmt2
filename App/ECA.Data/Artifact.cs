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
    /// An artifact is a document, photo, file or other digital item.
    /// </summary>
    public class Artifact
    {
        [Key]
        public int ArtifactId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Path { get; set; }
        [Required]
        public int ArtifactTypeId { get; set; }
        [Required]
        public virtual ArtifactType ArtifactType { get; set; }
        public byte[] Data { get; set; }

        //relationships
        public virtual Activity Activity { get; set; }
        public int? ActivityId { get; set; }
        public virtual Project Project { get; set; }
        public int? ProjectId { get; set; }
        public virtual Program Program { get; set; }
        public int? ProgramId { get; set; }
        public virtual Publication Publication { get; set; }
        public int? PublicationId { get; set; }
        public virtual ItineraryStop ItineraryStop { get; set; }
        public int? ItineraryStopId { get; set; }
        public virtual Impact Impact { get; set; }
        public int? ImpactId {get; set;}

        public History History { get; set; }

    }

}
