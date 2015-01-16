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

    public class ExternalId
    {
        [Key]
        public int ExternalIdId { get; set; }
        [Required]
        public string ExternalIdValue { get; set; }

        public History History { get; set; }
    }
}
