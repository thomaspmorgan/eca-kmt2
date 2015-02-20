using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECA.Data
{
    public class Method
    {
        [Key]
        public int MethodId { get; set; }
        [Required]
        [MaxLength(20)]
        public string MethodName { get; set; }
        public History History { get; set; }
    }
}

