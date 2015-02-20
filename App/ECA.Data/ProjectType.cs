using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECA.Data
{
    public class ProjectType
    {
        [Key]
        public int ProjectTypeId { get; set; }
        [Required]
        [MaxLength(20)]
        public string ProjectTypeName { get; set; }
        public History History { get; set; }
    }
}