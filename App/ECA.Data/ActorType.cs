using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECA.Data
{
    public partial class ActorType
    {
        [Key]
        public int ActorTypeId { get; set; }
        [Required]
        [MaxLength(20)]
        public string ActorName { get; set; }
        public History History { get; set; }
    }
}
