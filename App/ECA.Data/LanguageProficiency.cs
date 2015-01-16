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

    public class LanguageProficiency
    {
        [Key]
        public int LanguageProficiencyId { get; set; }
        [Required]
        public string LanguageName { get; set; }
        // relationship
        public ICollection<Person> People { get; set; }

        public History History { get; set; }
    }
}
