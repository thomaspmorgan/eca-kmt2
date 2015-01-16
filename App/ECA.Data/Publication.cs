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
    /// A publication is a published work, film, song or other artifact produced by a person.
    /// </summary>
    public class Publication
    {
        [Key]
        public int PublicationId { get; set; }
        [Required]
        public string PublicationName { get; set; }
        [Required]
        public string Work { get; set; }
        public DateTimeOffset PublicationDate { get; set; }
        public virtual ICollection<Artifact> Artifacts { get; set; }
        public virtual Person Person { get; set; }
        public int? PersonId { get; set; }

        public History History { get; set; }
    }
}
