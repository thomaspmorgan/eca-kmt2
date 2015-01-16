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
    /// Participant interests, specializations and fields of study.
    /// Example: Neuroscience, Economics, Basketball, Archeology, Agriculture 
    /// </summary>
    public class InterestSpecialization
    {
        [Key]
        public int InterestSpecializationId {get; set;}
        [Required]
        public string Name { get; set; }
    }
}
