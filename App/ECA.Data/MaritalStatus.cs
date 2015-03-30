using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECA.Data
{
    public class MaritalStatus
    {
        [Key]
        public int MaritalStatusId { get; set; }
        [Required]
        [MaxLength(1)]
        public string Status { get; set; }
        [MaxLength(20)]
        public string Description { get; set; }
        public History History { get; set; }
    }
}
