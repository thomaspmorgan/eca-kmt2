using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Data
{
    public partial class ProgramStatus
    {
        /// <summary>
        /// Gets or sets the ProgramStatusId.
        /// </summary>
        [Key]
        public int ProgramStatusId { get; set; }

        /// <summary>
        /// Gets or sets the Status.
        /// </summary>
        [Required]
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the History.
        /// </summary>
        public History History { get; set; }


    }
}
