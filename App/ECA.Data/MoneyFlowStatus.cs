using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECA.Data
{
    /// <summary>
    /// THe model to store/retrieve a Money Flow Status
    /// </summary>
    public partial class MoneyFlowStatus
    {
        /// <summary>
        /// Id of the Money Flow Status
        /// </summary>
        [Key]
        public int MoneyFlowStatusId {get; set;}

        /// <summary>
        /// Name of the Money Flow Status (the status)
        /// </summary>
        [Required]
        [StringLength(80)]
        public string MoneyFlowStatusName { get; set; }

        /// <summary>
        /// create/update history
        /// </summary>
        public History History { get; set; }

        //Relations

        /// <summary>
        /// The money flows associated with this status
        /// </summary>
        public ICollection<MoneyFlow> MoneyFlows { get; set; }
    }
}
