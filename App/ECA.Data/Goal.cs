using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECA.Data
{
    public class Goal
    {
        [Key]
        public int GoalId { get; set; }
        [Required]
        public string GoalName { get; set; }

        public History History { get; set; }

        public virtual ICollection<Program> Programs { get; set; }
        public virtual ICollection<Project> Projects { get; set; }
    }
}
