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
    /// The types of programs the participant has participated in 
    /// Example: Virtual, Local, Outbound 
    /// </summary>
    public partial class ParticipantType
    {
        [Key]
        public int ParticipantTypeId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public bool IsPerson { get; set; }
    }
}
