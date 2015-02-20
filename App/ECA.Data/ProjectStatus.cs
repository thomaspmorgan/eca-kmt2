using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECA.Data
{
    public partial class ProjectStatus
    {
        [Key]
        public int ProjectStatusId { get; set; }
        [Required]
        public string Status { get; set; }

        public History History { get; set; }
    }
}
