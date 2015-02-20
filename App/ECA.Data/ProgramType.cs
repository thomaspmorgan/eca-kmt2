using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECA.Data
{
    public class ProgramType
    {
        [Key]
        public int ProgramTypeId { get; set; }
        [Required]
        [MaxLength(20)]
        public string ProgramTypeName { get; set; }
        public History History { get; set; }
    }
}
