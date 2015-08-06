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
    /// Prominent categories of a participant
    /// Example: Ambassador to/from the U.S., Cabinet Minister, Chief Justice of Supreme Court, Current President of a University/College 
    /// </summary>
    public class ProminentCategory
    {
        /// <summary>
        /// Gets the max length of the first name.
        /// </summary>
        public const int NAME_MAX_LENGTH = 255;

        /// <summary>
        /// Creates a new default instance and initializes the history.
        /// </summary>
        public ProminentCategory()
        {
            this.History = new History();
        }

        /// <summary>
        /// Gets or sets the Key of the ProminentCategory
        /// </summary>
        [Key]
        public int ProminentCategoryId { get; set; }

        /// <summary>
        /// Gets or sets the Name of the ProminentCategory
        /// </summary>
        [Required]
        [MaxLength(NAME_MAX_LENGTH)]
        public string Name { get; set; }


        /// <summary>
        /// Gets or sets the history of the ProminentCategory
        /// </summary>
        public History History { get; set; }

        /// <summary>
        /// Persons that have this category (many to many)
        /// </summary>
        public ICollection<Person> People { get; set; }
    }
}
