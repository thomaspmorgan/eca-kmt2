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
    /// Special Status of a Participant
    /// Examples: Married, Divorced, Deceased, Persona non-grata, Disabled, Alumnus 
    /// </summary>
    public class SpecialStatus
    {
        [Key]
        public int SpecialStatusId { get; set; }
        [Required]
        public string Status { get; set; }
    }
}
