using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECA.Data
{
    public class ImpactType
    {
        [Key]
        public int ImpactTypeId { get; set; }
        public string Impact { get; set; }

        public History History { get; set; }
    }
}
