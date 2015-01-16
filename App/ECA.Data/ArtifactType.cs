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

    public class ArtifactType
    {
        [Key]
        public int ArtifactTypeId { get; set; }
        [Required]
        public int Name { get; set; }

        public History History { get; set; }
    }
}
