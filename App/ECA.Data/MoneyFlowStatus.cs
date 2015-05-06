using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECA.Data
{
    public partial class MoneyFlowStatus
    {
        [Key]
        public int MoneyFlowStatusId {get; set;}
        [Required]
        [StringLength(80)]
        public string MoneyFlowStatusName { get; set; }
        public History History { get; set; }

        //Relations
        public ICollection<MoneyFlow> MoneyFlows { get; set; }
    }
}
