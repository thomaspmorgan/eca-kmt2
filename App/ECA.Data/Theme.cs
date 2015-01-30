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

    public class Theme
    {
        [Key]
        public int ThemeId { get; set; }
        [Required]
        public string ThemeName { get; set; }

        public History History { get; set; }

        public virtual ICollection<Program> Programs { get; set; }
        public virtual ICollection<Project> Projects { get; set; }

    }

}

