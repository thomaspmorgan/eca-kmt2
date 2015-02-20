using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECA.Data
{
    public class PersonStatus
    {
        [Key]
        public int PersonStatusId { get; set; }
        [Required]
        [MaxLength(20)]
        public string PersonStatusName { get; set; }
        public History History { get; set; }
    }
}
