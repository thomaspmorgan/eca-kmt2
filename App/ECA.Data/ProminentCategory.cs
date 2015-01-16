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
        [Key]
        public int ProminentCategoryId { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
