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
    /// Impacts are descriptions of the evidence of effectiveness of a program or project.
    /// </summary>
    public class Impact
    {
        public int ImpactId { get; set; }
        public virtual Program Program { get; set; }
        public int? ProgramId { get; set; }
        public virtual Project Project { get; set; }
        public int? ProjectId { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public virtual ICollection<ImpactType> ImpactTypes { get; set; }
        public virtual ICollection<Artifact> Artifacts { get; set; }
        public virtual ICollection<Person> People { get; set; }

        public History History { get; set; }
    }
}
