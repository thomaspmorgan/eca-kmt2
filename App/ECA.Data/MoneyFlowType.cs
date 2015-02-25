using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECA.Data
{
    public partial class MoneyFlowType
    {
        [Key]
        public int MoneyFlowTypeId { get; set; }
        [Required]
        [StringLength(80)]
        public string MoneyFlowTypeName { get; set; }
        public History History { get; set; }

        // Relations
        public ICollection<MoneyFlow> MoneyFlows { get; set; }
    }
}