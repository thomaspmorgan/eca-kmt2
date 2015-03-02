using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECA.Data
{
    public partial class NameType
    {
        [Key]
        public int NameTypeId {get; set;}
        [Required]
        [MaxLength(20)]
        public string NameTypeName { get; set; }
        public History History { get; set; }
    }
}
