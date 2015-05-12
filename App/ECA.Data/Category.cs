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
    /// A Focus is a concentration that a project encompasses.
    /// </summary>
    public class Category : IHistorical
    {
        /// <summary>
        /// The max length of the focus.
        /// </summary>
        public const int NAME_MAX_LENGTH = 50;

        /// <summary>
        /// Creates a new Focus.
        /// </summary>
        public Category()
        {
            this.History = new History();
            this.Programs = new HashSet<Program>();
            this.Projects = new HashSet<Project>();
        }

        /// <summary>
        /// Gets or set the Id.
        /// </summary>
        [Key]
        public int CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        [MaxLength(NAME_MAX_LENGTH)]
        [Required]
        public string CategoryName { get; set; }

        /// <summary>
        /// Office for the Focus area
        /// </summary>
        public int FocusId { get; set; }

        /// <summary>
        /// Office of the Focus Area
        /// </summary>
        public Focus Focus { get; set; }

        /// <summary>
        /// Programs having this category
        /// </summary>
        public ICollection<Program> Programs { get; set; }

        /// <summary>
        /// Project having this category
        /// </summary>
        public ICollection<Project> Projects { get; set; }

        /// <summary>
        /// Gets or sets the history.
        /// </summary>
        /// 
        public History History { get; set; }
    }
}
