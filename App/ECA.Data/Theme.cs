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
        public Theme()
        {
            this.Programs = new HashSet<Program>();
            this.Projects = new HashSet<Project>();
        }

        [Key]
        public int ThemeId { get; set; }
        [Required]
        public string ThemeName { get; set; }

        public History History { get; set; }

        public ICollection<Program> Programs { get; set; }
        public ICollection<Project> Projects { get; set; }

        /// <summary>
        /// Gets or sets is active
        /// </summary>
        public bool IsActive { get; set; }
    }

}

