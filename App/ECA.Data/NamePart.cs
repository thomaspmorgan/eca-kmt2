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
    /// <summary>
    /// A name part is a segment of a name, such as given name, family name, patronym or middle name.
    /// </summary>
    public class NamePart
    {
        [Key]
        public int NamePartId { get; set; }
        [Required]
        public string Value { get; set; }
        [Required]
        public NameType NameType { get; set; }
        //relationship
        public virtual Person Person { get; set; }
        public int PersonId { get; set; }
    }
}
